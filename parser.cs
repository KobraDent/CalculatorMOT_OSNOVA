using System;

namespace Calculator_Parser
{
    public class Parser
    {
        private string InputString;
        private bool isInvalid = false;
        private int symbol, index = 0;

        public Parser(string? sourceString)
        {
            InputString = sourceString ?? "0";
        }

        // Получение следующего символа из входной строки
        private void GetSymbol()
        {
            if (index < InputString.Length)
            {
                symbol = InputString[index];
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
                symbol = '\0';
        }


        // Метод для разбора входной строки и выполнения вычислений
        public double Parse()
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
            double result = MethodE();
            // Проверка наличия ошибок в выражении
            if (isInvalid)
                throw new InvalidOperationException("Invalid input");

            return result;
        }

        private double MethodE()
        {
            double x = MethodT();
            while (symbol == '+' || symbol == '-')
            {
                char p = (char)symbol;
                GetSymbol();
                if (symbol == '\0')
                {
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

        private double MethodT()
        {
            double x = MethodM();
            while (symbol == '*' || symbol == '/')
            {
                char p = (char)symbol;
                GetSymbol();
                double y = MethodM();

                if (p == '*')
                {
                    x *= y;
                }
                else if (p == '/')
                {
                    if (y == 0)
                    {
                        isInvalid = true;
                        return 0; // Деление на ноль
                    }
                    x /= y;
                }
            }
            return x;
        }


        private double MethodM()
        {
            double x;
            if (symbol == '(')
            {
                GetSymbol();
                x = MethodE();
                if (symbol != ')')
                {
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
                    x = -MethodM();
                }
                else if (symbol >= '0' && symbol <= '9')
                {
                    x = MethodC();
                }
                else
                {
                    isInvalid = true;
                    return 0;
                }
            }
            return x;
        }

        private double MethodC()
        {
            string x = "";
            bool isDot = false;
            while (symbol >= '0' && symbol <= '9' || symbol == '.' || symbol == ',')
            {
                if (symbol == '.' || symbol == ',')
                {
                    if (isDot)
                    {
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
            return double.Parse(x, System.Globalization.CultureInfo.InvariantCulture);
        }

    }
}

