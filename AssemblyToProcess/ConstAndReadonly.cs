using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyToProcess
{
    class ConstAndReadonly : IFastClone<ConstAndReadonly>, IEquatable<ConstAndReadonly>
    {

        public ConstAndReadonly() { }

        public ConstAndReadonly(int readonlyField, int valueA)
        {
            ReadonlyField = readonlyField;
            ValueA = valueA;
        }

        public static ConstAndReadonly BuildTestEntity()
        {
            return new ConstAndReadonly(200, 300);
        }
        public const int Constant = 100;

        public readonly int ReadonlyField = 200;

        public int ValueA { get; set; }

        public bool Equals(ConstAndReadonly other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ReadonlyField == other.ReadonlyField && ValueA == other.ValueA;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConstAndReadonly) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ReadonlyField * 397) ^ ValueA;
            }
        }

        public static bool operator ==(ConstAndReadonly left, ConstAndReadonly right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConstAndReadonly left, ConstAndReadonly right)
        {
            return !Equals(left, right);
        }

        public ConstAndReadonly FastClone()
        {
            throw new NotImplementedException();
        }
    }
}
