using System;

namespace AssemblyToProcess
{
    public class BasicTest : IFastClone<BasicTest>, IEquatable<BasicTest>
    {
        public int ValueE;

        public BasicTest()
        {
        }

        public BasicTest(int valueE, int valueA, int valueB, int valueD, string a, string b, string c, string d, byte e,
            short f, ushort g, uint h, long x, ulong j, double k, float l, object m, DateTime n, int o)
        {
            ValueE = valueE;
            ValueA = valueA;
            ValueB = valueB;
            ValueD = valueD;
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
            G = g;
            H = h;
            I = x;
            J = j;
            K = k;
            L = l;
            M = m;
            N = n;
            O = o;
        }

        public int ValueA { get; set; }

        public int ValueB { get; set; }

        public int ValueD { get; private set; }


        public string A { get; set; }

        public string B { get; set; }


        public string C { get; set; }

        public string D { get; set; }

        public byte E { get; set; }

        public short F { get; set; }

        public ushort G { get; set; }

        public uint H { get; set; }

        public long I { get; set; }

        public ulong J { get; set; }

        public double K { get; set; }

        public float L { get; set; }

        public object M { get; set; }

        public DateTime N { get; set; }

        public int O { get; set; }

        public bool Equals(BasicTest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ValueE == other.ValueE && ValueA == other.ValueA && ValueB == other.ValueB &&
                   ValueD == other.ValueD && string.Equals(A, other.A) && string.Equals(B, other.B) &&
                   string.Equals(C, other.C) && string.Equals(D, other.D) && E == other.E && F == other.F &&
                   G == other.G && H == other.H && I == other.I && J == other.J && K.Equals(other.K) &&
                   L.Equals(other.L) && Equals(M, other.M) && N.Equals(other.N) && O == other.O;
        }

        public BasicTest FastClone()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BasicTest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ValueE;
                hashCode = (hashCode * 397) ^ ValueA;
                hashCode = (hashCode * 397) ^ ValueB;
                hashCode = (hashCode * 397) ^ ValueD;
                hashCode = (hashCode * 397) ^ (A != null ? A.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (B != null ? B.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (C != null ? C.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (D != null ? D.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ E.GetHashCode();
                hashCode = (hashCode * 397) ^ F.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) H;
                hashCode = (hashCode * 397) ^ I.GetHashCode();
                hashCode = (hashCode * 397) ^ J.GetHashCode();
                hashCode = (hashCode * 397) ^ K.GetHashCode();
                hashCode = (hashCode * 397) ^ L.GetHashCode();
                hashCode = (hashCode * 397) ^ (M != null ? M.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ N.GetHashCode();
                hashCode = (hashCode * 397) ^ O;
                return hashCode;
            }
        }

        public static bool operator ==(BasicTest left, BasicTest right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BasicTest left, BasicTest right)
        {
            return !Equals(left, right);
        }

        public BasicTest ReflectionMethod()
        {
            return null;
        }

        public BasicTest BinarySerializationMethod()
        {
            return null;
        }

        public BasicTest SerializationMethod()
        {
            return null;
        }

        public BasicTest MemberWiseMethod()
        {
            return (BasicTest) MemberwiseClone();
        }

        public static BasicTest BuildTest()
        {
            return new BasicTest
            {
                ValueE = 10,
                ValueD = 9,
                ValueB = 7,
                ValueA = 6
            };
        }
    }
}