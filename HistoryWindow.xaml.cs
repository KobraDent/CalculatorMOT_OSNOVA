using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorMOT
{
    public partial class HistoryWindow : Window
    {
        // Коллекция для хранения элементов истории вычислений
        public ObservableCollection<HistoryItem> HistoryItems { get; set; }

        public HistoryWindow()
        {
            // Инициализация коллекции и загрузка истории
            InitializeComponent();
            HistoryItems = new ObservableCollection<HistoryItem>();
            LoadHistory();
        }

        // Метод для загрузки истории из базы данных
        private void LoadHistory()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=calculator.db;Version=3;"))
            {
                connection.Open();
                // Запрос к базе данных для получения записей из таблицы Operations
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT * FROM Operations";
                    // Использование SQLiteDataReader для чтения результатов запроса
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта HistoryItem и добавление его в коллекцию
                            HistoryItems.Add(new HistoryItem
                            {
                                Expression = reader["Expression"].ToString(),
                                Result = Convert.ToDouble(reader["Result"]),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                            });
                        }
                    }
                }
            }

            // Устанавливаем источник данных для ListView
            historyListView.ItemsSource = HistoryItems;
        }
        // Обработчик события изменения выбора в ListView(нажатие на выражение из истории)
        private void HistoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (historyListView.SelectedItem != null)
            {
                // Получаем выбранный элемент истории
                var selectedOperation = (HistoryItem)historyListView.SelectedItem;
                // Отправить выбранное выражение в MainWindow
                (Application.Current.MainWindow as MainWindow)?.InsertExpression(selectedOperation.Expression);
                // Закрываем окно истории
                Close();
            }
        }
    }
}
