using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDMClientApp.Models;
using SQLite;

namespace MDMClientApp.Services
{
    public static class DatabaseService
    {
        private static string _dbPath => Path.Combine(FileSystem.AppDataDirectory, "mdm.db");

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_dbPath);
        }

        public static void InitializeDatabase()
        {
            using var conn = GetConnection();
            conn.CreateTable<User>();
            conn.CreateTable<Company>();

            // Добавляем тестовую компанию
            if (!conn.Table<Company>().Any())
            {
                conn.Insert(new Company { Name = "Тестовая Компания" });
            }

            // Добавляем администратора
            if (!conn.Table<User>().Any(u => u.FirstName == "admin"))
            {
                conn.Insert(new User
                {
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@example.com",
                    Password = "admin",
                    Department = "IT",
                    Position = "Администратор",
                    CompanyId = 1
                });
            }
        }
    }
}
