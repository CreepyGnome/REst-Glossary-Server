using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Regs.Server.Models;
using Regs.Server.Settings;

namespace Regs.Server.Services 
{
    public class RegsDatabaseManager
    {
        private readonly ILogger<RegsDatabaseManager> _logger;
        private readonly Lazy<ConcurrentDictionary<RegsKey, IRegsEntry>>[] _databases;

        /// <summary>
        /// The number of databases available.
        /// </summary>
        public ushort Count { get;}

        public RegsDatabaseManager(IOptionsMonitor<RegsSettings> settingsMonitor, ILogger<RegsDatabaseManager> logger)
        {
            if (settingsMonitor == null)
                throw new ArgumentNullException(nameof(settingsMonitor));
            
            _logger = logger ?? NullLogger<RegsDatabaseManager>.Instance;
            Count = settingsMonitor.CurrentValue.NumberOfDatabases;

            _databases = new Lazy<ConcurrentDictionary<RegsKey, IRegsEntry>>[Count];
            
            for (int i = 0; i < _databases.Length; i++)
                _databases[i] = new Lazy<ConcurrentDictionary<RegsKey, IRegsEntry>>(LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private void ValidateDatabaseOrThrow(ushort db)
        {
            if (db >= Count)
                throw new InvalidOperationException($"Database index [{db}] is not within range of 0 to {Count - 1}.");
        }

        private void ValidateEntryOrThrow(IRegsEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
        }

        /// <summary>
        /// Inserts a the provided entry for the specified key if the key does not already exist.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry to insert.</param>
        /// <param name="entry">The value of the entry.</param>
        /// <returns>true if the key/entry pair was inserted successfully; otherwise false if the key already exists</returns>
        public bool Insert(ushort db, RegsKey key, IRegsEntry entry)
        {
            ValidateDatabaseOrThrow(db);
            ValidateEntryOrThrow(entry);

            return _databases[db].Value.TryAdd(key, entry);
        }

        /// <summary>
        /// Updates the specified key to the newly provided entry if they key already exists.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry to insert.</param>
        /// <param name="entry">The value of the entry.</param>
        /// <returns>true if the entry was updated successfully; otherwise false</returns>
        public bool Update(ushort db, RegsKey key, IRegsEntry entry)
        {
            ValidateDatabaseOrThrow(db);
            ValidateEntryOrThrow(entry);

            if (_databases[db].Value.TryGetValue(key, out IRegsEntry existingEntry))
                return _databases[db].Value.TryUpdate(key, entry, existingEntry);
            return false;
        }

        /// <summary>
        /// Insert a new key/entry pair if the key does not already exist, or updates a key/entry pair if the key already exits.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <param name="entry">The value of the entry.</param>m>
        /// <returns>The new entry for the key.</returns>
        public IRegsEntry Upsert(ushort db, RegsKey key, IRegsEntry entry)
        {
            ValidateDatabaseOrThrow(db);
            ValidateEntryOrThrow(entry);

            return _databases[db].Value.AddOrUpdate(key, entry, (k, existingEntry) =>
            {
                entry.CreatedOn = existingEntry.CreatedOn;
                entry.ModifiedOn = DateTimeOffset.UtcNow;
                return entry;
            });
        }

        /// <summary>
        /// Gets the entry for the specified key.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <returns>The entry if the key exists; otherwise null.</returns>
        public IRegsEntry Get(ushort db, RegsKey key)
        {
            ValidateDatabaseOrThrow(db);

            if (_databases[db].Value.TryGetValue(key, out IRegsEntry entry))
                return entry;

            return null;
        }

        /// <summary>
        /// Deletes key/entry pair.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <returns>true if the key/entry pair was deleted successfully; otherwise false.</returns>
        public bool Delete(ushort db, RegsKey key)
        {
            ValidateDatabaseOrThrow(db);

            return _databases[db].Value.TryRemove(key, out IRegsEntry _);
        }
    }
}
