using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FastClone.Fody
{
    public class ModuleWeaver
    {
        public Action<string> LogInfo { get; set; }

        // An instance of Mono.Cecil.ModuleDefinition for processing
        public ModuleDefinition ModuleDefinition { get; set; }

        // TypeSystem typeSystem;

        // Init logging delegates to make testing easier
        public ModuleWeaver()
        {
            LogInfo = m => { };
        }

        private MethodDefinition GetCtor(TypeDefinition def)
        {
            var ctor = def.Methods.FirstOrDefault(x => x.IsConstructor && !x.HasParameters);
            { }
            return ctor;
        }
        private void BuildInstanceMethod(TypeDefinition def, MethodDefinition mDef)
        {
            /*    IL_0000: ldarg.0      // this
    IL_0001: call         class AssemblyToProcess.BasicTest AssemblyToProcess.BasicTest::CloneMethod(class AssemblyToProcess.BasicTest)
    IL_0006: ret          */

            var method = new MethodDefinition(
                "FastClone",
                MethodAttributes.Public,
                def);

            var proc = method.Body.GetILProcessor();
            proc.Emit(OpCodes.Ldarg_0); // this
            proc.Emit(OpCodes.Call, mDef);
            proc.Emit(OpCodes.Ret);

            def.Methods.Add(method);
        }
        private void BuildStaticCloneMethod(TypeDefinition def)
        {
            var method = new MethodDefinition(
                "CloneMethod",
                MethodAttributes.Public | MethodAttributes.Static,
                def);

            if (method.HasBody)
            {
                { }
                
            }
            method.Parameters.Add(new ParameterDefinition("Source", ParameterAttributes.None, def));
            var ctor = GetCtor(def);
            // var res = new VariableDefinition(def);
            var processor = method.Body.GetILProcessor();
            
            //processor.Create(OpCodes.Newobj, ctor);
            processor.Emit(OpCodes.Newobj, ctor);
            SetFields(processor, def);
            processor.Emit(OpCodes.Ret);
    
            def.Methods.Add(method);

            BuildInstanceMethod(def, method);
        }
  
           private void SetFields(ILProcessor processor, TypeDefinition def)
           {
    
            foreach (var prop in def.Properties)
            {
                processor.Emit(OpCodes.Dup);
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Callvirt, prop.GetMethod);
                processor.Emit(OpCodes.Callvirt, prop.SetMethod);                
            }

        }

        private const string FastCloneName = @"IFastClone`1";
        private bool ImplementsIFastClone(TypeDefinition def)
        {
            return def.Interfaces.FirstOrDefault(x => x.InterfaceType.Name == FastCloneName) != null;
        }
     
        private void FindCloneables()
        {
            var iface = ModuleDefinition.Types.FirstOrDefault(x => x.Name == "IFastClone`1");

            if (iface == null)
                return;

            foreach (var m in ModuleDefinition.Types.Where(ImplementsIFastClone))
            {
                InjectMethod(m);
                BuildStaticCloneMethod(m);
            }
        }

        
        public void Execute()
        {
            FindCloneables();
            /*
            foreach (var m in ModuleDefinition.Types.Where(x => x.IsPublic && x.IsClass))
            {
                InjectMethod(m);
            }*/
        }


        private void MakeStaticMethod(TypeDefinition target)
        {
            var del = new FieldDefinition(
                "CloneMethod",
                FieldAttributes.Public | FieldAttributes.Static,
                target);

            
            // var mType = typeof(Func<,>).MakeGenericType(target.)
        }
        private void InjectMethod(TypeDefinition target)
        {
            MakeStaticMethod(target);
            var method = new MethodDefinition(
                "HelloWorld",
                MethodAttributes.Public,
                ModuleDefinition.TypeSystem.String);

            var processor = method.Body.GetILProcessor();
            processor.Emit(OpCodes.Ldstr, "Hello World");
            processor.Emit(OpCodes.Ret);

            target.Methods.Add(method);

            

            /*  .method public hidebysig instance string 
    HelloWorld() cil managed 
  {
    .maxstack 8

    // [19 39 - 19 52]
    IL_0000: ldstr        "Hello World"
    IL_0005: ret          

  } // end of method BasicTest::HelloWorld*/
        }
    }
}
