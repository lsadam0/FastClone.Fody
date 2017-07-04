using System;
using System.IO;
using System.Reflection;

using FastClone.Fody;
using Mono.Cecil;
using NUnit.Framework;

namespace FastClone.Tests
{
    [TestFixture]
    public class DoesInject
    {
        private string _originalAssemblyPath;
        private string _weavedAssemblyPath;

        private Assembly _weavedAssembly;
        [OneTimeSetUp]
        public void Weave()
        {
            var projectPath = Path.GetFullPath(
                Path.Combine(
                    TestContext.CurrentContext.TestDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));

            Assert.IsTrue(File.Exists(projectPath));

            _originalAssemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
            Assert.IsTrue(File.Exists(_originalAssemblyPath));
            _weavedAssemblyPath = _originalAssemblyPath.Replace(".dll", ".Weaved.dll");
            File.Copy(_originalAssemblyPath, _weavedAssemblyPath, true);

            using (var moduleDef = ModuleDefinition.ReadModule(_originalAssemblyPath))
            {
                var weave = new ModuleWeaver
                {
                    ModuleDefinition = moduleDef
                };

                weave.Execute();
                moduleDef.Write(_weavedAssemblyPath);
            }

            _weavedAssembly = Assembly.LoadFile(_weavedAssemblyPath);
            Assert.NotNull(_weavedAssembly);
        }


        private dynamic GetInstance(string name)
        {
            var type = _weavedAssembly.GetType(name);
            Assert.NotNull(type);

            var m = type.GetMethod("BuildTest", BindingFlags.Public | BindingFlags.Static);
            // var instance = (dynamic)Activator.CreateInstance(type);
            var instance = m.Invoke(null, null);
            return instance;
        }

        
        [Test]
        public void DoesInjectStaticClone()
        {
            var type = _weavedAssembly.GetType("AssemblyToProcess.BasicTest");
            var test = GetInstance("AssemblyToProcess.BasicTest");

            var method = type.GetMethod("CloneMethod", BindingFlags.Static | BindingFlags.Public);
            var weavedMethod = type.GetMethod("CloneMethod", BindingFlags.Static | BindingFlags.Public);


            var resA = method.Invoke(test, new object[] { test });
            var resB = weavedMethod.Invoke(test, new object[] { test });

        }

        [Test]
        public void DoesInject_HelloWorld()
        {
            Assert.NotNull(_weavedAssembly);

            var type = _weavedAssembly.GetType("AssemblyToProcess.BasicTest");
            Assert.NotNull(type);

            var instance = (dynamic)Activator.CreateInstance(type);
            Assert.NotNull(instance);

            var method = type.GetMethod("HelloWorld");
            Assert.NotNull(method);

            var res = (string)method.Invoke(instance, null);
            Assert.AreEqual("Hello World", res);

        }
    }
}