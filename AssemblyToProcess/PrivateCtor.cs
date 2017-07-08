using System;

namespace AssemblyToProcess
{
    public class PrivateCtor : IFastClone<PrivateCtor>, IEquatable<PrivateCtor>
    {
        private PrivateCtor()
        {
        }

        public int A { get; set; }

        public bool Equals(PrivateCtor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return A == other.A;
        }

        public PrivateCtor FastClone()
        {
            throw new NotImplementedException();
        }

        public static PrivateCtor BuildTestEntity()
        {
            return new PrivateCtor
            {
                A = 200
            };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PrivateCtor) obj);
        }

        public override int GetHashCode()
        {
            return A;
        }

        public static bool operator ==(PrivateCtor left, PrivateCtor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PrivateCtor left, PrivateCtor right)
        {
            return !Equals(left, right);
        }
    }
}