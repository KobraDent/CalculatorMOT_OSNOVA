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
        private string currentInput = "";// Текущий ввод пользователя
        private string operation = "";// Текущая операция (если выбрана)
        private double firstNumber = 0;// Первое число для выполнения операции
        private int cursorPositionIndex = 0;// Индекс курсора в строке ввода
        private SQLiteConnection dbConnection;// Соединение с базой данных

        public MainWindow()
        {
            InitializeComponent();

            // Создание и открытие соединения с базой данных
            dbConnection = new SQLiteConnection("Data Source=calculator.db;Version=3;");
            dbConnection.Open();

            // Очистка данных из таблицы при каждом новом запуске
            ClearHistory();

            // Создание таблицы в базе данных
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // Инициализация базы данных: создание таблицы, если её нет
            using (SQLiteCommand cmd = new SQLiteCommand(dbConnection))
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Operations (Id INTEGER PRIMARY KEY AUTOINCREMENT, Expression TEXT, Result REAL, Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
                cmd.ExecuteNonQuery();
            }
        }

        private void ClearHistory()
        {
            // Очистка истории операций в базе данных
            using (SQLiteCommand cmd = new SQLiteCommand(dbConnection))
            {
                cmd.CommandText = "DELETE FROM Operations";
                cmd.ExecuteNonQuery();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик события для кнопок с цифрами, операциями и функциями
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
                    double result = parser.Parse();
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
                SaveToHistory(currentInput, double.NaN);
                currentInput += "3.14";
                cursorPositionIndex = currentInput.Length;
            }
            else if (buttonText == "(")
            {
                currentInput = currentInput.Insert(cursorPositionIndex, "(");
                cursorPositionIndex++;
            }
            else if (buttonText == ")")
            {
                currentInput = currentInput.Insert(cursorPositionIndex, ")");
                cursorPositionIndex++;
            }
            else
            {
                currentInput = currentInput.Insert(cursorPositionIndex, buttonText);
                cursorPositionIndex++;
            }

            display.Content = currentInput;// Обновление отображения текущего ввода
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик события для кнопок операций (+, -, *, /)
            Button button = (Button)sender;
            string buttonText = button.Content.ToString();

            if (currentInput != "")
            {
                if (double.TryParse(currentInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out firstNumber))
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

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик события для кнопки "History" (показать историю операций)
            ShowHistoryWindow();
        }

        private void ShowHistoryWindow()
        {
            // Отображение окна с историей операций
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.ShowDialog();
        }

        private void SaveToHistory(string expression, double result)
        {
            // Сохранение операции в базе данных
            using (SQLiteCommand cmd = new SQLiteCommand(dbConnection))
            {
                cmd.CommandText = "INSERT INTO Operations (Expression, Result) VALUES (@expression, @result)";
                cmd.Parameters.AddWithValue("@expression", expression);
                cmd.Parameters.AddWithValue("@result", result);
                cmd.ExecuteNonQuery();
            }
        }
        public void InsertExpression(string expression)
        {
            // Вставка выражения из истории в поле ввода
            currentInput = expression;
            cursorPositionIndex = expression.Length;
            display.Content = currentInput;
        }
    }
}


