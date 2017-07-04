using System;

namespace AssemblyToProcess
{
    public class BasicTest : IFastClone<BasicTest>
    {
        public int ValueE;

        public BasicTest(int valueA, int valueB)
        {
            ValueA = valueA;
            ValueB = valueB;
        }

        public BasicTest()
        {
        }

        public int ValueA { get; set; }

        public int ValueB { get; set; }

        // public int ValueC { get; }

        public int ValueD { get; private set; }
        
        
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
        
        public static BasicTest Testng(BasicTest source)
        {
            return new BasicTest()
            {
                ValueE = source.ValueE,
                ValueA = source.ValueA
            };
        }

        public static void Testing()
        {
        }

        public BasicTest FastClone()
        {
            throw new NotImplementedException();
        }
    }
}