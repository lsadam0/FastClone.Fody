using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace FastClone.Tests
{
    [TestFixture]
    public class DoesInject
    {
        private Assembly _weavedAssembly;

        [OneTimeSetUp]
        public void Weave()
        {
            _weavedAssembly = Weaver.Weave();
        }

        

        private dynamic ExecuteFastCloneMethod(Tuple<dynamic, Type> target)
        {
            var cloneMethod = target.Item2.GetMethod(TypeManifest.FastCloneMethod);
            Assert.NotNull(cloneMethod, $"Failed to find clone method: {target.Item2.Name}");
            var res = cloneMethod.Invoke(target.Item1, new object[] { });
            Assert.NotNull(res, $"Clone method returned null {target.Item2.Name}");
            return res;
        }

        private Tuple<dynamic, Type> GetInstanceOf(string className)
        {
            var type = _weavedAssembly.GetType(string.Format(TypeManifest.NamespaceMask, className));
            Assert.NotNull(type, $"Unable to find type {className}");

            
            var buildMethod = type.GetMethod(TypeManifest.BuildTestEntityMethod, BindingFlags.Public | BindingFlags.Static);
            Assert.NotNull(buildMethod, $"{className} lacks a BuildTestEntityMethod");
            var res  = buildMethod.Invoke(null, null);
            Assert.IsNotNull(res, $"Failed to create instance of ${className}");
            return new Tuple<dynamic, Type>(res, type);
        }

        private void EnsureThrowsException(string name)
        {
            var target = GetInstanceOf(name);
            Assert.NotNull(target);
            Assert.NotNull(target.Item1);
            Assert.NotNull(target.Item2);
            bool res = false;
            try
            {
                ExecuteFastCloneMethod(target);
                res = true;
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf<NotImplementedException>(e.InnerException);
            }

            Assert.False(res, "Expexted NotImplementedException");
        }

        private void DoesClone(string name)
        {
            var instance = GetInstanceOf(name);
            var clone = ExecuteFastCloneMethod(instance);

            Assert.IsTrue(object.Equals(instance.Item1, clone));
            ExecEquals(instance, clone, true);
            Assert.IsFalse(object.ReferenceEquals(instance.Item1, clone));
        }

        private void ExecEquals(Tuple<dynamic, Type> original, dynamic clone, bool expect)
        {
            var method = original.Item2.GetMethod("Equals", new Type[] { original.Item2 }, null);

            var res = (bool)method.Invoke(original.Item1, new object[] { clone });



            Assert.AreEqual(res, expect);
        }

        [Test]
        public void DoesClone_BasicTest()
        {
            var instance = GetInstanceOf(TypeManifest.BasicTest);
            var clone = ExecuteFastCloneMethod(instance);

            Assert.NotNull(clone);
            Assert.IsTrue(object.Equals(instance.Item1, clone));
            Assert.IsFalse(object.ReferenceEquals(instance.Item1, clone));
            Assert.IsTrue(object.ReferenceEquals(instance.Item1.M, clone.M));
        }

        [Test]
        public void DoesClone_WeaveImmutableProperties()
        {
            DoesClone(TypeManifest.PartialWeave);            
        }

        [Test]
        public void DoesClone_WeaveMissingCtor()
        {
            DoesClone(TypeManifest.LacksParameterlessCtor);
        }

        [Test]
        public void DoesNotClone_WeaveMissingInterface()
        {
            EnsureThrowsException(TypeManifest.MissingInterface);
        }

        [Test]
        public void DoesClone_InternalCtor()
        {
            DoesClone(TypeManifest.InternalCtor);
        }

        [Test]
        public void DoesClone_PrivateCtor()
        {
            DoesClone(TypeManifest.PrivateCtor);
        }

        [Test]
        public void DoesClone_PrivateField()
        {
            DoesClone(TypeManifest.PrivateField);
        }

        [Test]
        public void DoesClone_InternalProperties()
        {
            DoesClone(TypeManifest.InternalProperties);
        }
    }
}