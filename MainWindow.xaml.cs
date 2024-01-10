using Calculator_Parser;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorMOT
{
    public partial class MainWindow : Window
    {
        // Переменные для отслеживания состояния калькулятора
        private string currentInput = ""; // Текущий ввод пользователя
        private string operation = "";    // Текущая операция (если выбрана)
        private decimal firstNumber = 0;  // Первое число для выполнения операции
        private int cursorPositionIndex = 0; // Индекс курсора в строке ввода
        private SQLiteConnection dbConnection; // Соединение с базой данных

        public MainWindow()
        {
            InitializeComponent();
            // Инициализация соединения с базой данных
            dbConnection = new SQLiteConnection("Data Source=calculator.db;Version=3;");
            dbConnection.Open();
            // Очистка данных из таблицы при каждом новом запуске
            ClearHistory();
            // Создание таблицы в базе данных, если она не существует
            InitializeDatabase();
        }

        // Инициализация базы данных: создание таблицы, если её нет
        private void InitializeDatabase()
        {
            using (SQLiteCommand cmd = new SQLiteCommand(dbConnection))
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Operations (Id INTEGER PRIMARY KEY AUTOINCREMENT, Expression TEXT, Result REAL, Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
                cmd.ExecuteNonQuery();
            }
        }

        // Очистка истории операций в базе данных
        private void ClearHistory()
        {
            using (SQLiteCommand cmd = new SQLiteCommand(dbConnection))
            {
                cmd.CommandText = "DELETE FROM Operations";
                cmd.ExecuteNonQuery();
            }
        }

        // Обработчик события для кнопок с цифрами, операциями и функциями
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Content.ToString();

            if (buttonText == "CE")
            {
                // Очистка текущего ввода
                currentInput = "";
                cursorPositionIndex = 0;
            }
            else if (buttonText == "Del")
            {
                // Удаление символа слева от курсора
                if (cursorPositionIndex > 0 && cursorPositionIndex <= currentInput.Length)
                {
                    currentInput = currentInput.Remove(cursorPositionIndex - 1, 1);
                    cursorPositionIndex--;
                }
            }
            else if (buttonText == "=")
            {
                // Обработка нажатия кнопки "=" (вычисление результата)
                try
                {
                    Parser parser = new Parser(currentInput);
                    decimal result = parser.Parse();
                    firstNumber = result;
                    SaveToHistory(currentInput, result);
                    currentInput = result.ToString();
                    cursorPositionIndex = currentInput.Length;
                }
                catch (Exception ex)
                {
                    currentInput = "Error: " + ex.Message;
                    cursorPositionIndex = currentInput.Length;
                }
            }
            else if (buttonText == "π")
            {
                // Сохранение символа π в истории и добавление к текущему вводу
                SaveToHistory(currentInput, null);
                currentInput += "3.14";
                cursorPositionIndex = currentInput.Length;
            }
            else if (buttonText == "(")
            {
                // Добавление открывающей скобки к текущему вводу
                currentInput = currentInput.Insert(cursorPositionIndex, "(");
                cursorPositionIndex++;
            }
            else if (buttonText == ")")
            {
                // Добавление закрывающей скобки к текущему вводу
                currentInput = currentInput.Insert(cursorPositionIndex, ")");
                cursorPositionIndex++;
            }
            else
            {
                // Добавление символа к текущему вводу
                currentInput = currentInput.Insert(cursorPositionIndex, buttonText);
                cursorPositionIndex++;
            }

            // Обновление отображения текущего ввода
            display.Content = currentInput;
        }

        // Обработчик события для кнопок операций (+, -, *, /)
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Content.ToString();

            if (currentInput != "")
            {
                // Попытка преобразовать текущий ввод в десятичное число
                if (decimal.TryParse(currentInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out firstNumber))
                {
                    operation = buttonText;
                    currentInput = "";
                    cursorPositionIndex = 0;
                }
                else
                {
                    currentInput = "Error: Invalid input";
                    cursorPositionIndex = currentInput.Length;
                }
            }
        }

        // Обработчик события для кнопки "История"
        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            // Отображение окна истории операций
            ShowHistoryWindow();
        }

        // Метод для отображения окна истории операций
        private void ShowHistoryWindow()
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.ShowDialog();
        }

        // Метод для сохранения выражения и результата в базу данных
        private void SaveToHistory(string expression, decimal? result)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(dbConnection))
            {
                cmd.CommandText = "INSERT INTO Operations (Expression, Result) VALUES (@expression, @result)";
                cmd.Parameters.AddWithValue("@expression", expression);
                cmd.Parameters.AddWithValue("@result", result.HasValue ? (object)result.Value : DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        // Метод для вставки выражения в строку ввода из внешнего источника (например, истории)
        public void InsertExpression(string expression)
        {
            currentInput = expression;
            cursorPositionIndex = expression.Length;
            display.Content = currentInput;
        }
    }
}

