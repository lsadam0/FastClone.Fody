using System;
using AssemblyToProcess;
using NUnit.Framework;

namespace PostWeaving.Tests
{
    [TestFixture]
    public class BasicTests
    {
        private static readonly Random rNum = new Random();

        private BasicTest GetTestEntity()
        {
            return new BasicTest(
                rNum.Next(),
                rNum.Next(),
                rNum.Next(),
                rNum.Next(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                (byte) rNum.Next(0, 255),
                (short) rNum.Next(0, 32000),
                (ushort) rNum.Next(0, 64000),
                (uint) rNum.Next(),
                rNum.Next(0, 2000000000),
                (ulong) rNum.Next(0, 2000000000),
                rNum.NextDouble(),
                (float) rNum.NextDouble(),
                new object(),
                DateTime.Now,
                rNum.Next()
            );
        }

        [Test]
        public void BasicTestClass_DoesClone()
        {
            var original = GetTestEntity();

            var clone = original.FastClone();

            Assert.IsFalse(object.ReferenceEquals(original, clone));
            Assert.IsTrue(original.Equals(clone));
            Assert.IsTrue(object.ReferenceEquals(original.M, clone.M));
        }
    }
}