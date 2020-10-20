using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoughBench.Attributes
{
    /// <summary>
    ///  [Benchmark] attribute for methods, in case Benchmark.net isn't referenced.
    /// </summary>
    public class BenchmarkAttribute : Attribute
    { }
}

namespace RoughBench
{
    internal static class BenchmarkReflector
    {
        /// <summary>
        ///  Reflection magic to get public methods with the [Benchmark] attribute
        ///  matching the desired signature.
        /// </summary>
        /// <remarks>
        ///  See https://github.com/microsoft/elfie-arriba/blob/master/XForm/XForm/Core/NativeAccelerator.cs for the
        ///  closest related craziness.
        /// </remarks>
        /// <typeparam name="WithSignature">Action or Func with desired parameters and return type</typeparam>
        /// <param name="fromType">Type to search for matching methods</param>
        /// <returns>Dictionary with method names and Action or Func to invoke for each matching method</returns>
        internal static Dictionary<string, WithSignature> BenchmarkMethods<WithSignature>(Type fromType)
        {
            Dictionary<string, WithSignature> methods = new Dictionary<string, WithSignature>();

            // Identify the return type and argument types on the desired method signature
            Type delegateOrFuncType = typeof(WithSignature);
            MethodInfo withSignatureInfo = delegateOrFuncType.GetMethod("Invoke");
            Type returnType = withSignatureInfo.ReturnType;
            List<Type> arguments = new List<Type>(withSignatureInfo.GetParameters().Select((pi) => pi.ParameterType));

            // Create an instance of the desired class (triggering any initialization)
            object instance = null;

            // Find all public methods with 'Benchmark' attribute and correct signature
            foreach (MethodInfo method in fromType.GetMethods())
            {
                if (!method.IsPublic) { continue; }
                if (!method.GetCustomAttributes().Where((a) => a.GetType().Name == "BenchmarkAttribute").Any()) { continue; }
                if (!method.ReturnType.Equals(returnType)) { continue; }
                if (!arguments.SequenceEqual(method.GetParameters().Select((pi) => pi.ParameterType))) { continue; }

                if (!method.IsStatic && instance == null)
                {
                    instance = fromType.GetConstructor(new Type[0]).Invoke(null);
                }

                methods[method.Name] = (WithSignature)(object)method.CreateDelegate(delegateOrFuncType, instance);
            }

            return methods;
        }
    }
}
