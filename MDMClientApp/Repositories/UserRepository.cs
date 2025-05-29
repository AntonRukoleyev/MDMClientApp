using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDMClientApp.Models;
using SQLite;

namespace MDMClientApp.Repositories
{
    public class UserRepository
    {
        private SQLiteConnection _database;

        public UserRepository(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<User>();
        }

        public List<User> GetUsers() => _database.Table<User>().ToList();
        public User GetUser(int id) => _database.Table<User>().FirstOrDefault(u => u.Id == id);
        public User GetUserByCredentials(string firstName, string password) =>
            _database.Table<User>().FirstOrDefault(u => u.FirstName == firstName && u.Password == password);
        public int SaveUser(User user) => user.Id != 0 ? _database.Update(user) : _database.Insert(user);
        public int DeleteUser(int id) => _database.Delete<User>(id);
    } 
}
