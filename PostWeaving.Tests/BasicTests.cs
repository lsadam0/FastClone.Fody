using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AssemblyToProcess;


namespace PostWeaving.Tests
{
    [TestFixture]
    public class BasicTests
    {

        [Test]
        public void BasicTestClass_DoesClone()
        {
            var original = new BasicTest()
            {
                ValueA = 1,
                ValueB = 2,
                ValueE = 3,
              //  ValueD = 4
            };

            var clone = original.FastClone();
        }
    }
}
