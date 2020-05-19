using System;
using System.Collections.Generic;

namespace Regs.Server.Models
{
    public interface IRegsEntry
    {
        string GetString();

        IList<string> GetList();

        ISet<string> GetSet();

        ISet<string> GetSortedSet();

        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset ModifiedOn { get; set; }
    }
}