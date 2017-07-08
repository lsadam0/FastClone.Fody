using System;

namespace AssemblyToProcess
{
    public class PrivateField : IFastClone<PrivateField>, IEquatable<PrivateField>
    {
        private int _hidden;

        public int A { get; set; }

        public bool Equals(PrivateField other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _hidden == other._hidden && A == other.A;
        }

        public PrivateField FastClone()
        {
            throw new NotImplementedException();
        }

        public static PrivateField BuildTestEntity()
        {
            return new PrivateField
            {
                A = 100,
                _hidden = 200
            };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PrivateField) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_hidden * 397) ^ A;
            }
        }

        public static bool operator ==(PrivateField left, PrivateField right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PrivateField left, PrivateField right)
        {
            return !Equals(left, right);
        }
    }
}