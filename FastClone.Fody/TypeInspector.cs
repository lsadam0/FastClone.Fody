using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace FastClone.Fody
{
    internal static class TypeInspector
    {
        internal static MethodDefinition GetParameterlessConstructor(TypeDefinition def)
        {
            return def.Methods.FirstOrDefault(x => x.IsConstructor && !x.HasParameters);
        }

        internal static bool HasParameterlessConstructor(TypeDefinition def)
        {
            return GetParameterlessConstructor(def) != null;
        }
    }
}
