using System.Collections.Generic;

namespace Regs.Server.Models
{
    public class RegsSortedSetEntry : RegsEntryAdapter
    {
        private readonly SortedSet<string> _data;

        public RegsSortedSetEntry(SortedSet<string> value) => _data = value;

        public override ISet<string> GetSortedSet() => _data;
    }
}