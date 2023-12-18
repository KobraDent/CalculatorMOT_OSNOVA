using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;

namespace CalculatorMOT
{
    public partial class HistoryWindow : Window
    {
        public ObservableCollection<HistoryItem> HistoryItems { get; set; }

        public HistoryWindow()
        {
            InitializeComponent();
            HistoryItems = new ObservableCollection<HistoryItem>();
            LoadHistory();
        }

        private void LoadHistory()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=calculator.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "SELECT * FROM Operations";
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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
    }
}
