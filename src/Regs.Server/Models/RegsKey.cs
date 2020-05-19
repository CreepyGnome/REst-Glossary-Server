using System;

namespace Regs.Server.Models
{
    public readonly struct RegsKey : IEquatable<RegsKey>
    {
        public string Key { get; }
        public RegsDataType Type { get; }

        public RegsKey(string key, RegsDataType type = default)
        {
            Key = key;
            Type = type;
        }

        public override int GetHashCode() => (Key != null ? StringComparer.InvariantCulture.GetHashCode(Key) : 0);
        public override bool Equals(object obj) => obj is RegsKey regsKey && Equals(regsKey);
        public static bool operator ==(RegsKey left, RegsKey right) => left.Equals(right);
        public static bool operator !=(RegsKey left, RegsKey right) => !left.Equals(right);
        public bool Equals(RegsKey regsKey) => string.Equals(Key, regsKey.Key, StringComparison.InvariantCulture);

    }
}
