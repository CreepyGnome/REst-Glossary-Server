using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Regs.Server.Models;

namespace Regs.Server.Services
{
    public class RegsService : IRegsService
    {
        private readonly RegsDatabaseManager _dbManager;
        private readonly ILogger<RegsService> _logger;

        public RegsService(RegsDatabaseManager dbManager, ILogger<RegsService> logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? NullLogger<RegsService>.Instance;
        }

        public IRegsEntry Set(ushort db, RegsKey key, IRegsEntry entry) => _dbManager.Upsert(db, key, entry);

        public IRegsEntry Get(ushort db, RegsKey key) => _dbManager.Get(db, key);

        public bool Delete(ushort db, RegsKey key) => _dbManager.Delete(db, key);
    }
}
