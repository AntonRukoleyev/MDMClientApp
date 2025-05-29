using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDMClientApp.Models;
using SQLite;

namespace MDMClientApp.Repositories
{
    public class CompanyRepository
    {
        private SQLiteConnection _database;

        public CompanyRepository(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<Company>();
        }

        public List<Company> GetCompanies() => _database.Table<Company>().ToList();
        public Company GetCompany(int id) => _database.Table<Company>().FirstOrDefault(c => c.Id == id);
        public int SaveCompany(Company company) => company.Id != 0 ? _database.Update(company) : _database.Insert(company);
        public int DeleteCompany(int id) => _database.Delete<Company>(id);
    }
}
