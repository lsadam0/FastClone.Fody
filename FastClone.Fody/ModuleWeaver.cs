using System;
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

            var proc = method.Body.GetILProcessor();
            var existing = proc.Body.Instructions.ToList();

            foreach (var pending in existing)
                proc.Remove(pending);

            proc.Emit(OpCodes.Ldarg_0);
            proc.Emit(OpCodes.Call, mDef);
            proc.Emit(OpCodes.Ret);

        }

        private MethodDefinition BuildStaticCloneMethod(TypeDefinition target)
        {
            var method = new MethodDefinition(
                StaticCloneMethodName,
                MethodAttributes.Private | MethodAttributes.Static,
                target);

            method.Parameters.Add(new ParameterDefinition(
                SourceParamName,
                ParameterAttributes.None,
                target));

            var constructor = TypeInspector.GetParameterlessConstructor(target);

            var processor = method.Body.GetILProcessor();

            processor.Emit(OpCodes.Newobj, constructor); // invoke constructor
            SetFields(processor, target);
            processor.Emit(OpCodes.Ret); // Return

            target.Methods.Add(method);

            return method;
        }

        private static void SetFields(ILProcessor processor, TypeDefinition def)
        {
            foreach (var field in def.Fields)
            {
                if (field.HasConstant || field.IsInitOnly || field.IsStatic)
                    continue;

                processor.Emit(OpCodes.Dup);
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Ldfld, field);
                processor.Emit(OpCodes.Stfld, field);

            }
        }
        
        private static bool ImplementsIFastClone(TypeDefinition def)
        {
            return def
                       .Interfaces
                       .FirstOrDefault(x => x.InterfaceType.Name == FastCloneInterfaceName) != null;
        }


        public void Execute()
        {
            LogInfo("Applying FastClone...");
            foreach (var definition in ModuleDefinition.Types.Where(ImplementsIFastClone))
            {
                if (!TypeInspector.HasParameterlessConstructor(definition))
                {
                    LogInfo($"Type {definition.Name} lacks a parameterless constructor, skipping");
                    continue;
                }
                LogInfo($"Extending {definition.Name}");

                // Add Static Clone
                var staticMethod = BuildStaticCloneMethod(definition);

                // Add Instance Method
                BuildInstanceMethod(definition, staticMethod);
            }
            LogInfo("Done");
        }
    }
}