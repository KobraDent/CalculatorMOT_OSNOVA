using System.Collections.Generic;
using System;
using System.Data.SQLite;

public class CalculatorDbContext
{
    private readonly string connectionString;

    public CalculatorDbContext(string dbFilePath)
    {
        connectionString = $"Data Source={dbFilePath};Version=3;";
    }

    public void SaveOperation(string expression, double result)
    {
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

    public List<Operation> GetOperations()
    {
        List<Operation> operations = new List<Operation>();

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "SELECT * FROM Operations";

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
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