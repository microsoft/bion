// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BSOA.GC
{
    /// <summary>
    ///  ConstructorBuilder wraps the nasty reflection code to:
    ///   - Dynamically find a constructor for a particular type with a desired signature
    ///   - Compile it into a performant Lambda to invoke the constructor
    ///   - Cache previously retrieved Lambdas to minimize repeat reflection
    /// </summary>
    internal class ConstructorBuilder
    {
        private static Dictionary<ValueTuple<Type, Type>, Delegate> _cache = new Dictionary<ValueTuple<Type, Type>, Delegate>();

        /// <summary>
        ///  GetConstructor takes a Func signature (arguments and return type) and the type of the real
        ///  object and returns a performant, compiled Func which will construct the desired type using
        ///  the constructor with the required signature.
        /// </summary>
        /// <example>
        ///  If WrappingColumn&lt;T&gt; has a constructor taking (IColumn, IColumn, RowUpdater) and implements IColumn&lt;T&gt; and IColumn,
        ///  call ConstructorBuilder.GetConstructor&lt;Func&lt;IColumn, IColumn, RowUpdater, IColumn&gt;(typeof(WrappingColumn&lt;&gt;).MakeGenericType(
        /// </example>
        /// <typeparam name="FuncType">Delegate or Func type which takes the same arguments as a desired constructor and returns a type the object can cast to</typeparam>
        /// <param name="typeToConstruct">The concrete type to find a constructor for</param>
        /// <returns>Compiled Func matching the requested signature to construct the desired type</returns>
        public static FuncType GetConstructor<FuncType>(Type typeToConstruct)
        {
            ValueTuple<Type, Type> signatureAndType = (typeof(FuncType), typeToConstruct);

            // Check cache to see if constructor already built
            lock (_cache)
            {
                if (_cache.TryGetValue(signatureAndType, out Delegate previous)) { return (FuncType)(object)previous; }
            }

            // Figure out the argument types on the Func or Delegate type requested
            Type delegateOrFuncType = typeof(FuncType);
            MethodInfo withSignatureInfo = delegateOrFuncType.GetMethod("Invoke");
            Type[] arguments = withSignatureInfo.GetParameters().Select((pi) => pi.ParameterType).ToArray();

            // Find a matching constructor on the desired concrete type
            ConstructorInfo constructor = typeToConstruct.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, arguments, null);

            // Compile a lambda to invoke the constructor
            ParameterExpression[] argumentExpressions = arguments.Select((a) => Expression.Parameter(a)).ToArray();
            Delegate compiledDelegate = Expression.Lambda(Expression.New(constructor, argumentExpressions), argumentExpressions).Compile();

            // Add new constructor lambda to cache
            lock (_cache)
            {
                _cache[signatureAndType] = compiledDelegate;
            }

            // Cast delegate to the desired Func type and return
            return (FuncType)(object)compiledDelegate;
        }
    }
}
