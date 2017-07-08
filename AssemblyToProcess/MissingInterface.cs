using System;

namespace AssemblyToProcess
{
    public class MissingInterface : IEquatable<MissingInterface>
    {
        public int A { get; set; }

        public bool Equals(MissingInterface other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return A == other.A;
        }


        public static MissingInterface BuildTestEntity()
        {
            return new MissingInterface
            {
                A = 200
            };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MissingInterface) obj);
        }

        public override int GetHashCode()
        {
            return A;
        }

        public static bool operator ==(MissingInterface left, MissingInterface right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MissingInterface left, MissingInterface right)
        {
            return !Equals(left, right);
        }

        public MissingInterface FastClone()
        {
            throw new NotImplementedException();
        }
    }
}