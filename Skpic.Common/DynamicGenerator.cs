using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Skpic.Common
{
    public class DynamicGenerator
    {
        /// <summary>
        /// dynamic object
        /// </summary>
        /// <param name="fieldParam">object field param. exp:{typeof(string),typeof(string)}</param>
        /// <returns></returns>
        public static Type DynamicObject(List<Type> fieldParam)
        {
            var count = fieldParam.Count;

            var myDomain = Thread.GetDomain();
            var myAsmName = new AssemblyName("ParamVector");

            var myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.Run);

            var paramVectorModule = myAsmBuilder.DefineDynamicModule(myAsmName.Name);

            var paramVector = paramVectorModule.DefineType("ParamVectorModule", TypeAttributes.Public);

            //Create a default constructor
            var ivCtorDefault = paramVector.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            var ctorIlDefault = ivCtorDefault.GetILGenerator();

            ctorIlDefault.Emit(OpCodes.Ldarg_0);
            ctorIlDefault.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ctorIlDefault.Emit(OpCodes.Ret);

            //Creating an argument constructor by fieldParam
            var ivCtor = paramVector.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, fieldParam.ToArray());

            var ctorIl = ivCtor.GetILGenerator();

            for (int i = 0; i < count; i++)
            {
                //define field into the class
                FieldBuilder field = paramVector.DefineField("m_p" + i, typeof(string), FieldAttributes.Private);
                ctorIl.Emit(OpCodes.Ldarg_0);
                ctorIl.Emit(OpCodes.Ldarg, i + 1);
                ctorIl.Emit(OpCodes.Stfld, field);
                ctorIl.Emit(OpCodes.Ret);

                var pbNumber = paramVector.DefineProperty("P" + i, PropertyAttributes.HasDefault, typeof(string), null);

                // The property "set" and property "get" methods require a special set of attributes.
                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                // Define the "get" accessor method for Number. The method returns
                // an integer and has no arguments. (Note that null could be
                // used instead of Types.EmptyTypes)
                var mbNumberGetAccessor = paramVector.DefineMethod("get_p" + i, getSetAttr, typeof(string), Type.EmptyTypes);

                var numberGetIl = mbNumberGetAccessor.GetILGenerator();
                // For an instance property, argument zero is the instance. Load the
                // instance, then load the private field and return, leaving the
                // field value on the stack.
                numberGetIl.Emit(OpCodes.Ldarg_0);
                numberGetIl.Emit(OpCodes.Ldfld, field);
                numberGetIl.Emit(OpCodes.Ret);

                // Define the "set" accessor method for Number, which has no return
                // type and takes one argument of type int (Int32).
                var mbNumberSetAccessor = paramVector.DefineMethod("set_p" + i, getSetAttr, null, new[] { typeof(string) });

                var numberSetIl = mbNumberSetAccessor.GetILGenerator();
                // Load the instance and then the numeric argument, then store the
                // argument in the field.
                numberSetIl.Emit(OpCodes.Ldarg_0);
                numberSetIl.Emit(OpCodes.Ldarg_1);
                numberSetIl.Emit(OpCodes.Stfld, field);
                numberSetIl.Emit(OpCodes.Ret);

                // Last, map the "get" and "set" accessor methods to the
                // PropertyBuilder. The property is now complete.
                pbNumber.SetGetMethod(mbNumberGetAccessor);
                pbNumber.SetSetMethod(mbNumberSetAccessor);
            }

            return paramVector.CreateType();
        }
    }
}