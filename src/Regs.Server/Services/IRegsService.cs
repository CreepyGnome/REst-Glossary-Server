using Regs.Server.Models;

namespace Regs.Server.Services
{
    public interface IRegsService
    {
        /// <summary>
        /// Insert a new key/entry pair if the key does not already exist, or updates a key/entry pair if the key already exits.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <param name="entry">The value of the entry to insert. The entry can be null.</param>m>
        /// <returns>The new entry for the key.</returns>
        IRegsEntry Set(ushort db, RegsKey key, IRegsEntry entry);

        /// <summary>
        /// Gets the entry for the specified key.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <returns>The entry if the key exists; otherwise null.</returns>
        IRegsEntry Get(ushort db, RegsKey key);

        /// <summary>
        /// Deletes key/entry pair.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <returns>true if the key/entry pair was deleted successfully; otherwise false.</returns>
        bool Delete(ushort db, RegsKey key);
    }
}