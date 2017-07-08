using System;

namespace AssemblyToProcess
{
    public class InternalCtor : IFastClone<InternalCtor>, IEquatable<InternalCtor>
    {
        internal InternalCtor()
        {
        }

        public int A { get; set; }

        public bool Equals(InternalCtor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return A == other.A;
        }

        public InternalCtor FastClone()
        {
            throw new NotImplementedException();
        }

        public static InternalCtor BuildTestEntity()
        {
            return new InternalCtor
            {
                A = 100
            };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InternalCtor) obj);
        }

        public override int GetHashCode()
        {
            return A;
        }

        public static bool operator ==(InternalCtor left, InternalCtor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InternalCtor left, InternalCtor right)
        {
            return !Equals(left, right);
        }
    }
}