using System;

namespace AssemblyToProcess
{
    public class PartialWeave : IFastClone<PartialWeave>, IEquatable<PartialWeave>
    {
        public int CanWeave { get; set; }

        public int CannotWeave { get; }

        public bool Equals(PartialWeave other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CanWeave == other.CanWeave && CannotWeave == other.CannotWeave;
        }

        public PartialWeave FastClone()
        {
            throw new NotImplementedException();
        }

        public static PartialWeave BuildTestEntity()
        {
            return new PartialWeave
            {
                CanWeave = 100
            };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PartialWeave) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CanWeave * 397) ^ CannotWeave;
            }
        }

        public static bool operator ==(PartialWeave left, PartialWeave right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PartialWeave left, PartialWeave right)
        {
            return !Equals(left, right);
        }
    }
}