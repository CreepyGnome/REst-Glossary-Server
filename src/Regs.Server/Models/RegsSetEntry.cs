using System.Collections.Generic;

namespace Regs.Server.Models
{
    public class RegsSetEntry : RegsEntryAdapter
    {
        private readonly HashSet<string> _data;

        public RegsSetEntry(HashSet<string> value) => _data = value;

        public override ISet<string> GetSet() => _data;
    }
}