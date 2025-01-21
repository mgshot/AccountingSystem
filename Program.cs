using System;
using System.Data.SQLite;

namespace AccountingSystem
{
    class Program
    {
        private const string ConnectionString = "Data Source=accounting.db;";

        static void Main(string[] args)
        {
            InitializeDatabase();
            while (true)
            {
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View Transactions");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTransaction();
                        break;
                    case "2":
                        ViewTransactions();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Transactions (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT NOT NULL, Description TEXT NOT NULL, Amount REAL NOT NULL, Type TEXT NOT NULL);", connection);
                command.ExecuteNonQuery();
            }
        }

        private static void AddTransaction()
        {
            Console.Write("Enter Date (YYYY-MM-DD): ");
            var date = Console.ReadLine();
            Console.Write("Enter Description: ");
            var description = Console.ReadLine();
            Console.Write("Enter Amount: ");
            var amount = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter Type (Income/Expense): ");
            var type = Console.ReadLine();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Transactions (Date, Description, Amount, Type) VALUES (@date, @description, @amount, @type);", connection);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@type", type);
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Transaction added successfully.");
        }

        private static void ViewTransactions()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Transactions;", connection);
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("Transactions:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Date: {reader["Date"]}, Description: {reader["Description"]}, Amount: {reader["Amount"]}, Type: {reader["Type"]}");
                    }
                }
            }
        }
    }
}
