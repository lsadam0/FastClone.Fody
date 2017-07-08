using System.IO;
using System.Reflection;
using FastClone.Fody;
using Mono.Cecil;
using NUnit.Framework;

namespace FastClone.Tests
{
    public static class Weaver
    {
        private static string _originalAssemblyPath;
        private static string _weavedAssemblyPath;

        private static Assembly _weavedAssembly;


        public static Assembly Weave()
        {
            if (_weavedAssembly != null)
                return _weavedAssembly;

            var projectPath = Path.GetFullPath(
                Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));

            Assert.IsTrue(File.Exists(projectPath));

            _originalAssemblyPath = Path.Combine(
                Path.GetDirectoryName(projectPath),
                @"bin\Debug\AssemblyToProcess.dll");

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
            return _weavedAssembly;
        }
    }
}