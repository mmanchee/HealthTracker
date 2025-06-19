using SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using HealthTracker.Models;
using System;

namespace HealthTracker.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;
        private bool _initialized = false;

        // Path to the database file.
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "HealthTracker.db3"
        );

        public DatabaseService()
        {
            _database = new SQLiteAsyncConnection(DbPath);
        }

        // Initialize the database and create tables if they don't exist.
        private async Task InitializeAsync()
        {
            if (_initialized)
                return;

            // Create tables for each model.
            // CreateFlags.None ensures the table is only created if it doesn't already exist.
            await _database.CreateTableAsync<MentalHealthEntry>();
            await _database.CreateTableAsync<PhysicalHealthEntry>();
            await _database.CreateTableAsync<EmotionalHealthEntry>();
            await _database.CreateTableAsync<JournalEntry>();
            await _database.CreateTableAsync<TodoItem>();
            _initialized = true;
        }

        // --- Generic Health Entry Methods ---

        // Checks if a health entry for a specific type was made today.
        public async Task<bool> HasEntryForToday<T>() where T : BaseModel, new()
        {
            await InitializeAsync();
            var todayString = DateTime.Today.ToString("yyyy-MM-dd");
            var tableName = _database.GetConnection().GetMapping<T>().TableName;
            var query = $"SELECT COUNT(*) FROM {tableName} WHERE DATE(CreateDate) = DATE(?)";
            var count = await _database.ExecuteScalarAsync<int>(query, todayString);

            return count > 0;
        }

        // --- Mental Health Methods ---
        public async Task<int> SaveMentalHealthEntryAsync(MentalHealthEntry entry)
        {
            await InitializeAsync();
            return await _database.InsertAsync(entry);
        }

        // --- Physical Health Methods ---
        public async Task<int> SavePhysicalHealthEntryAsync(PhysicalHealthEntry entry)
        {
            await InitializeAsync();
            return await _database.InsertAsync(entry);
        }

        // --- Emotional Health Methods ---
        public async Task<int> SaveEmotionalHealthEntryAsync(EmotionalHealthEntry entry)
        {
            await InitializeAsync();
            return await _database.InsertAsync(entry);
        }

        // --- Journal Methods ---
        public async Task<int> SaveJournalEntryAsync(JournalEntry entry)
        {
            await InitializeAsync();
            if (entry.Id != 0)
            {
                return await _database.UpdateAsync(entry);
            }
            else
            {
                // Find if an entry for today already exists
                var todayEntry = await GetJournalForDateAsync(DateTime.Today);
                if (todayEntry != null)
                {
                    todayEntry.Content = entry.Content;
                    return await _database.UpdateAsync(todayEntry);
                }
                else
                {
                    return await _database.InsertAsync(entry);
                }
            }
        }

        public async Task<JournalEntry> GetJournalForDateAsync(DateTime date)
        {
            await InitializeAsync();
            var dateString = date.ToString("yyyy-MM-dd");
            var tableName = _database.GetConnection().GetMapping<JournalEntry>().TableName;

            var query = $"SELECT * FROM {tableName} WHERE DATE(CreateDate) = DATE(?) LIMIT 1";
            var results = await _database.QueryAsync<JournalEntry>(query, dateString);

            return results.FirstOrDefault();
        }

        public async Task<List<JournalEntry>> GetAllJournalEntriesAsync()
        {
            await InitializeAsync();
            // Order by most recent first
            return await _database.Table<JournalEntry>().OrderByDescending(j => j.CreateDate).ToListAsync();
        }


        // --- Todo List Methods ---
        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            await InitializeAsync();
            // Oldest to newest, but incomplete items first, then by priority
            return await _database.Table<TodoItem>().OrderBy(t => t.IsCompleted).ThenBy(t => t.Priority).ThenBy(t => t.CreateDate).ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemAsync(int id)
        {
            await InitializeAsync();
            return await _database.Table<TodoItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveTodoItemAsync(TodoItem item)
        {
            await InitializeAsync();
            if (item.Id != 0)
            {
                return await _database.UpdateAsync(item);
            }
            else
            {
                return await _database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteTodoItemAsync(TodoItem item)
        {
            await InitializeAsync();
            return await _database.DeleteAsync(item);
        }
    }
}
