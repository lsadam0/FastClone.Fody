using System;

namespace AssemblyToProcess
{
    public class LacksParameterlessCtor : IFastClone<LacksParameterlessCtor>, IEquatable<LacksParameterlessCtor>
    {
        public int A { get; set; }

        public bool Equals(LacksParameterlessCtor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return A == other.A;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LacksParameterlessCtor) obj);
        }

        public override int GetHashCode()
        {
            return A;
        }

        public static bool operator ==(LacksParameterlessCtor left, LacksParameterlessCtor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LacksParameterlessCtor left, LacksParameterlessCtor right)
        {
            return !Equals(left, right);
        }

        public static LacksParameterlessCtor BuildTestEntity()
        {
            return new LacksParameterlessCtor()
            {
                A = 200
            };

        }

        public LacksParameterlessCtor FastClone()
        {
            throw new NotImplementedException();
        }
    }
}