using System.Collections.Generic;
using System.Collections.Immutable;

namespace Regs.Server.Models
{
    public static class RegsEntryDefaults
    {
        public static readonly string DefaultString = string.Empty;
        public static readonly IList<string> DefaultList = new List<string>().AsReadOnly();
        public static readonly ISet<string> DefaultSet = ImmutableHashSet<string>.Empty;
        public static readonly ISet<string> DefaultSortedSet = ImmutableHashSet<string>.Empty;
    }
}