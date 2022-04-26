using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Quiz_Vlajky.Models;
using SQLite;

namespace Quiz_Vlajky.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _connection;
        
        public async Task Initialize()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(appData, "Database.db3");
            await ExtractDatabase(path);

            _connection = new SQLiteAsyncConnection(path);
            await _connection.CreateTableAsync<Country>();
            await _connection.CreateTableAsync<Question>();
        }

        public Task<List<Country>> GetAllCountries()
        {
            return _connection.Table<Country>().ToListAsync();
        }

        public Task<List<Country>> GetCountriesByCategory(CountryCategory category)
        {
            return _connection.Table<Country>().Where(c => c.Category == category).ToListAsync();
        }

        public Task<List<Question>> GetAllQuestions()
        {
            return _connection.Table<Question>().ToListAsync();
        }

        public Task<List<Question>> GetQuestionsByCategory(QuestionCategory category)
        {
            return _connection.Table<Question>().Where(q => q.Category == category).ToListAsync();
        }

        private async Task ExtractDatabase(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var assembly = Assembly.GetExecutingAssembly();
            using var resourceStream = assembly.GetManifestResourceStream("Quiz_Vlajky.Database.db3");
            using var fileStream = File.Open(path, FileMode.Create, FileAccess.Write);

            await resourceStream.CopyToAsync(fileStream);
        }
    }
}