using System;

namespace Calculator_Parser
{
    public class Parser
    {
        private string InputString;  // Входная строка для парсера
        private bool isInvalid = false;  // Флаг для отслеживания ошибок при парсинге
        private int symbol, index = 0;  // Переменные для отслеживания текущего символа и индекса входной строки

        public Parser(string? sourceString)
        {
            // Инициализация входной строки, если она не пуста, иначе устанавливаем значение "0"
            InputString = sourceString ?? "0";
        }

        // Получение следующего символа из входной строки
        private void GetSymbol()
        {
            if (index < InputString.Length)
            {
                // Получение символа из строки
                symbol = InputString[index];
                // Замена символов "×" на "*" и "÷" на "/"
                if (symbol == '×')
                {
                    symbol = '*';
                }
                else if (symbol == '÷')
                {
                    symbol = '/';
                }
                index++;
            }
            else
                // Если достигнут конец строки, устанавливаем символ в '\0'
                symbol = '\0';
        }

        // Метод для разбора входной строки и выполнения вычислений
        public decimal Parse()
        {
            // Проверка наличия входной строки
            if (string.IsNullOrWhiteSpace(InputString))
                throw new ArgumentNullException("String is null or contains whitespace");

            // Если входная строка равна "0", вернуть 0
            if (InputString == "0")
                return 0;

            // Инициализация индекса и первого символа
            index = 0;
            GetSymbol();
            // Вызов метода для разбора выражения
            decimal result = MethodE();
            // Проверка наличия ошибок в выражении
            if (isInvalid)
                throw new InvalidOperationException("Invalid input");

            return result;
        }

        private decimal MethodE()
        {
            // Метод для разбора выражения
            decimal x = MethodT();
            while (symbol == '+' || symbol == '-')
            {
                char p = (char)symbol;
                GetSymbol();
                if (symbol == '\0')
                {
                    // Если достигнут конец строки, выполнить операцию с предыдущим результатом
                    if (p == '+')
                        x += MethodT();
                    else
                        x -= MethodT();
                }
                else if (p == '+')
                    x += MethodT();
                else
                    x -= MethodT();
            }
            return x;
        }

        private decimal MethodT()
        {
            // Метод для разбора терма
            decimal x = MethodM();
            while (symbol == '*' || symbol == '/')
            {
                char p = (char)symbol;
                GetSymbol();
                decimal y = MethodM();

                if (p == '*')
                {
                    x *= y;
                }
                else if (p == '/')
                {
                    if (y == 0)
                    {
                        // Обработка деления на ноль
                        isInvalid = true;
                        return 0;
                    }
                    x /= y;
                }
            }
            return x;
        }

        private decimal MethodM()
        {
            // Метод для разбора множителя
            decimal x;
            if (symbol == '(')
            {
                GetSymbol();
                // Рекурсивный вызов для разбора выражения в скобках
                x = MethodE();
                if (symbol != ')')
                {
                    // Если закрывающая скобка не найдена, установить флаг ошибки
                    isInvalid = true;
                    return 0;
                }
                GetSymbol();
            }
            else
            {
                if (symbol == '-')
                {
                    GetSymbol();
                    // Унарный минус
                    x = -MethodM();
                }
                else if (symbol >= '0' && symbol <= '9')
                {
                    // Если символ - цифра, вызвать метод для разбора числа
                    x = MethodC();
                }
                else
                {
                    // Если символ не является цифрой, установить флаг ошибки
                    isInvalid = true;
                    return 0;
                }
            }
            return x;
        }

        private decimal MethodC()
        {
            // Метод для разбора числа
            string x = "";
            bool isDot = false;
            while (symbol >= '0' && symbol <= '9' || symbol == '.' || symbol == ',')
            {
                if (symbol == '.' || symbol == ',')
                {
                    if (isDot)
                    {
                        // Если уже встречена точка, установить флаг ошибки
                        isInvalid = true;
                        return 0;
                    }
                    x += '.';
                    isDot = true;
                }
                else
                {
                    x += (char)symbol;
                }
                GetSymbol();
            }
            // Преобразование строки в десятичное число, игнорируя культуру
            return decimal.Parse(x, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

