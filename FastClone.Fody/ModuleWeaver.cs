using System;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FastClone.Fody
{
    public class ModuleWeaver
    {
        private const string FastCloneInterfaceName = @"IFastClone`1";
        private const string StaticCloneMethodName = @"CloneMethod";
        private const string InstanceCloneMethodName = @"FastClone";
        private const string SourceParamName = @"Source";

        public ModuleWeaver()
        {
            LogInfo = m => { };
        }

        public Action<string> LogInfo { get; set; }

        public ModuleDefinition ModuleDefinition { get; set; }

        private static void BuildInstanceMethod(TypeDefinition def, MethodDefinition mDef)
        {
            var method = def.Methods.FirstOrDefault(x => x.Name == InstanceCloneMethodName);
            /*
            var method = new MethodDefinition(
                InstanceCloneMethodName,
                MethodAttributes.Public,
                def);*/

            var proc = method.Body.GetILProcessor();
            var existing = proc.Body.Instructions.ToList();
            // proc.Body.Instructions.Clear();

            foreach (var pending in existing)
                proc.Remove(pending);

            proc.Emit(OpCodes.Ldarg_0);
            proc.Emit(OpCodes.Call, mDef);
            proc.Emit(OpCodes.Ret);

            // def.Methods.Add(method);
        }

        private MethodDefinition BuildStaticCloneMethod(TypeDefinition target)
        {
            var method = new MethodDefinition(
                StaticCloneMethodName,
                MethodAttributes.Public | MethodAttributes.Static,
                target);

            method.Parameters.Add(new ParameterDefinition(
                SourceParamName,
                ParameterAttributes.None,
                target));

            var constructor = TypeInspector.GetParameterlessConstructor(target);

            var processor = method.Body.GetILProcessor();

            processor.Emit(OpCodes.Newobj, constructor); // invoke constructor
            SetFields(processor, target);
            // SetProperties(processor, target); // Set Fields
            processor.Emit(OpCodes.Ret); // Return

            target.Methods.Add(method);

            return method;
        }

        private static void SetFields(ILProcessor processor, TypeDefinition def)
        {
            foreach (var field in def.Fields)
            {
                processor.Emit(OpCodes.Dup);
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Ldfld, field);
                processor.Emit(OpCodes.Stfld, field);


                /*    IL_0005: dup          
    IL_0006: ldarg.0      // source
    IL_0007: ldfld        int32 AssemblyToProcess.BasicTest::ValueE
    IL_000c: stfld        int32 AssemblyToProcess.BasicTest::ValueE*/

                // processor.Emit(OpCodes.g, prop.GetMethod);
                //  processor.Emit(OpCodes.Callvirt, prop.SetMethod);

                /*                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, field);
                generator.Emit(OpCodes.Stfld, field);*/
            }
        }
        /*
        private static void SetProperties(ILProcessor processor, TypeDefinition def)
        {
            foreach (var prop in def.Properties)
            {
                processor.Emit(OpCodes.Dup);
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Callvirt, prop.GetMethod);
                processor.Emit(OpCodes.Callvirt, prop.SetMethod);
            }
        }*/

        private static bool ImplementsIFastClone(TypeDefinition def)
        {
            return def
                       .Interfaces
                       .FirstOrDefault(x => x.InterfaceType.Name == FastCloneInterfaceName) != null;
        }


        public void Execute()
        {
            Trace.WriteLine("Applying FastClone...");
            foreach (var definition in ModuleDefinition.Types.Where(ImplementsIFastClone))
            {
                if (!TypeInspector.HasParameterlessConstructor(definition))
                {
                    Trace.WriteLine($"Type {definition.Name} lacks a parameterless constructor, skipping");
                    continue;
                }
                Trace.WriteLine($"Extending {definition.Name}");

                // Add Static Clone
                var staticMethod = BuildStaticCloneMethod(definition);

                // Add Instance Method
                BuildInstanceMethod(definition, staticMethod);
            }
            Trace.WriteLine("Done");
        }
    }
}