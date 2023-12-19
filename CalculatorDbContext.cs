using System.Collections.Generic;
using System;
using System.Data.SQLite;

// Класс CalculatorDbContext предоставляет доступ к базе данных для сохранения и получения операций
public class CalculatorDbContext
{
    private readonly string connectionString;

    // Конструктор класса, принимающий путь к файлу базы данных
    public CalculatorDbContext(string dbFilePath)
    {
        connectionString = $"Data Source={dbFilePath};Version=3;";
    }
    // Метод для сохранения операции в базе данных
    public void SaveOperation(string expression, double result)
    {
        // SQL-запрос для вставки новой операции в таблицу Operations
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "INSERT INTO Operations (Expression, Result) VALUES (@Expression, @Result)";
                cmd.Parameters.AddWithValue("@Expression", expression);
                cmd.Parameters.AddWithValue("@Result", result);
                cmd.ExecuteNonQuery();
            }
        }
    }
    // Метод для получения списка всех операций из базы данных
    public List<Operation> GetOperations()
    {
        List<Operation> operations = new List<Operation>();

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // SQL-запрос для выборки всех записей из таблицы Operations
            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "SELECT * FROM Operations";

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Создание объекта Operation и добавление его в список операций
                        Operation operation = new Operation
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Expression = Convert.ToString(reader["Expression"]),
                            Result = Convert.ToDouble(reader["Result"]),
                            Timestamp = Convert.ToDateTime(reader["Timestamp"])
                        };

                        operations.Add(operation);
                    }
                }
            }
        }

        return operations;
    }
}