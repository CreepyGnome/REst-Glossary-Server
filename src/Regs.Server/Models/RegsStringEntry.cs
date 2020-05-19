namespace Regs.Server.Models
{
    public class RegsStringEntry : RegsEntryAdapter
    {
        private readonly string _data;

        public RegsStringEntry(string value) => _data = value;

        public override string GetString() => _data;
    }

}