using System;
using System.Diagnostics;
using System.IO;
using FastClone.Fody;
using Mono.Cecil;

namespace BuildIntermediate
{
    internal class Program
    {
        private static readonly string Filename = @"AssemblyToProcess.dll";
        private static readonly string WeavedFilename = @"AssemblyToProcess.Weaved.dll";

        private static void Weave()
        {
            Trace.WriteLine(Environment.CurrentDirectory);

            if (!File.Exists(Filename))
                throw new InvalidOperationException($"Unable to find {Filename}");
            /*
            var_originalAssemblyPath = Path.Combine(Path.GetDirectoryName(projectPath),
                @"bin\Debug\AssemblyToProcess.dll");
            Assert.IsTrue(File.Exists(_originalAssemblyPath));
            _weavedAssemblyPath = _originalAssemblyPath.Replace(".dll", ".Weaved.dll");
            File.Copy(_originalAssemblyPath, _weavedAssemblyPath, true);*/

            using (var moduleDef = ModuleDefinition.ReadModule(Filename))
            {
                var weave = new ModuleWeaver
                {
                    ModuleDefinition = moduleDef
                };

                weave.Execute();
                moduleDef.Write(WeavedFilename); //_weavedAssemblyPath);
            }
        }

        private static void Main(string[] args)
        {
            Weave();
        }
    }
}