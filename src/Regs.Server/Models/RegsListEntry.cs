using System.Collections.Generic;

namespace Regs.Server.Models
{
    public class RegsListEntry : RegsEntryAdapter
    {
        private readonly List<string> _data;

        public RegsListEntry(List<string> value) => _data = value;

        public override IList<string> GetList() => _data;
    }
}