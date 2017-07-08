using System;

namespace AssemblyToProcess
{
    public class InternalProperties : IFastClone<InternalProperties>, IEquatable<InternalProperties>
    {
        public InternalProperties()
        {

        }

        public static InternalProperties BuildTestEntity()
        {
            return new InternalProperties()
            {
                A = 100,
                B = 200
            };

        }
        public int A { get; set; }

        internal int B { get; set; }

        public bool Equals(InternalProperties other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return A == other.A && B == other.B;
        }

        public InternalProperties FastClone()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InternalProperties) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (A * 397) ^ B;
            }
        }

        public static bool operator ==(InternalProperties left, InternalProperties right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InternalProperties left, InternalProperties right)
        {
            return !Equals(left, right);
        }
    }
}