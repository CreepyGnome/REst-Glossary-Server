using System;
using System.Collections.Generic;

namespace Regs.Server.Models
{
    public abstract class RegsEntryAdapter : IRegsEntry
    {
        protected RegsEntryAdapter()
        {
            CreatedOn = DateTimeOffset.UtcNow;
            ModifiedOn = DateTimeOffset.UtcNow;
        }

        public virtual string GetString() => RegsEntryDefaults.DefaultString;

        public virtual IList<string> GetList() => RegsEntryDefaults.DefaultList;

        public virtual ISet<string> GetSet() => RegsEntryDefaults.DefaultSet;

        public virtual ISet<string> GetSortedSet() => RegsEntryDefaults.DefaultSortedSet;

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }
    }
}