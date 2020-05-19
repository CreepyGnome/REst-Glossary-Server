namespace Regs.Server.Settings
{
    public class RegsSettings
    {
        private const ushort DefaultNumberOfDatabases = 4;

        private ushort _numberOfDatabases = DefaultNumberOfDatabases;

        public ushort NumberOfDatabases
        {
            get => _numberOfDatabases;
            set => _numberOfDatabases = value > 1 ? value : DefaultNumberOfDatabases;
        }
    }
}
