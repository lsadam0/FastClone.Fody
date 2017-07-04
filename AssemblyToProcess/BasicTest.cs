using System;

namespace AssemblyToProcess
{
    public class BasicTest : IFastClone<BasicTest>
    {
        public BasicTest(int valueA, int valueB)
        {
            ValueA = valueA;
            ValueB = valueB;
        }

        public BasicTest()
        {
        }

        public static BasicTest BuildTest()
        {
            return new BasicTest()
            {
                ValueE = 10,
                ValueD = 9,
                ValueB = 7,
                ValueA = 6,
            };
        }
        public int ValueA { get; set; }

        public int ValueB { get; set; }

        // public int ValueC { get; }

        public int ValueD { get; private set; }

        public int ValueE;

        

      //  public BasicTest FastShallowClone() => CloneMethod(this);

        public static void Testing()
        {
            
        }

        /*
        public static BasicTest CloneMethod(BasicTest source)
        { return null; }
        public BasicTest FastClone() => CloneMethod(this); */
        public BasicTest FastCloneZ()
        {
            throw new NotImplementedException();
        }
    }
}