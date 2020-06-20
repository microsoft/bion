// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Column
{
    public class ColumnFactory
    {
        private static Dictionary<Type, Func<object, IColumn>> Builders = new Dictionary<Type, Func<object, IColumn>>()
        {
            [typeof(string)] = (defaultValue) => new StringColumn(),
            [typeof(bool)] = (defaultValue) => new BooleanColumn((bool)(defaultValue ?? default(bool))),
            [typeof(Uri)] = (defaultValue) => new UriColumn(),
            [typeof(DateTime)] = (defaultValue) => new DateTimeColumn((DateTime)(defaultValue ?? default(DateTime))),
            [typeof(byte)] = (defaultValue) => new NumberColumn<byte>((byte)(defaultValue ?? default(byte))),
            [typeof(sbyte)] = (defaultValue) => new NumberColumn<sbyte>((sbyte)(defaultValue ?? default(sbyte))),
            [typeof(short)] = (defaultValue) => new NumberColumn<short>((short)(defaultValue ?? default(short))),
            [typeof(ushort)] = (defaultValue) => new NumberColumn<ushort>((ushort)(defaultValue ?? default(ushort))),
            [typeof(int)] = (defaultValue) => new NumberColumn<int>((int)(defaultValue ?? default(int))),
            [typeof(uint)] = (defaultValue) => new NumberColumn<uint>((uint)(defaultValue ?? default(uint))),
            [typeof(long)] = (defaultValue) => new NumberColumn<long>((long)(defaultValue ?? default(long))),
            [typeof(ulong)] = (defaultValue) => new NumberColumn<ulong>((ulong)(defaultValue ?? default(ulong))),
            [typeof(float)] = (defaultValue) => new NumberColumn<float>((float)(defaultValue ?? default(float))),
            [typeof(double)] = (defaultValue) => new NumberColumn<double>((double)(defaultValue ?? default(double))),
            [typeof(char)] = (defaultValue) => new NumberColumn<char>((char)(defaultValue ?? default(char))),
        };

        public static IColumn<T> BuildTyped<T>(T defaultValue = default, Func<Type, object, IColumn> recurseTo = null)
        {
            return (IColumn<T>)Build(typeof(T), defaultValue, recurseTo);
        }

        public static IColumn Build(Type type)
        {
            return Build(type, defaultValue: null, recurseTo: null);
        }

        public static IColumn Build(Type type, object defaultValue)
        {
            return Build(type, defaultValue: defaultValue, recurseTo: null);
        }

        public static IColumn Build(Type type, object defaultValue, Func<Type, object, IColumn> recurseTo)
        {
            if (recurseTo == null) { recurseTo = Build; }

            if (Builders.TryGetValue(type, out Func<object, IColumn> creator))
            {
                return creator(defaultValue);
            }

            if (type.IsEnum)
            {
                throw new NotImplementedException($"Can't create EnumColumn via builder because can't generically define converting Funcs.");
            }

            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                Type[] typeArguments = type.GetGenericArguments();

                if (genericType == typeof(IList<>))
                {
                    return BuildList(typeArguments[0], recurseTo);
                }
                else if (genericType == typeof(IDictionary<,>))
                {
                    return BuildDictionary(typeArguments[0], typeArguments[1], recurseTo);
                }
            }

            throw new NotImplementedException($"ColumnFactory doesn't know how to build an IColumn<{type.Name}>.");
        }

        private static IColumn BuildList(Type itemType, Func<Type, object, IColumn> recurseTo)
        {
            IColumn innerColumn = recurseTo(itemType, null);
            return (IColumn)(typeof(ListColumn<>).MakeGenericType(itemType).GetConstructor(new[] { typeof(IColumn) }).Invoke(new[] { innerColumn }));
        }

        private static IColumn BuildDictionary(Type keyType, Type valueType, Func<Type, object, IColumn> recurseTo)
        {
            IColumn keyColumn = WrapInDistinct(recurseTo(keyType, null), null);
            IColumn valueColumn = recurseTo(valueType, null);
            return (IColumn)(typeof(DictionaryColumn<,>).MakeGenericType(keyType, valueType).GetConstructor(new[] { typeof(IColumn), typeof(IColumn) }).Invoke(new[] { keyColumn, valueColumn }));
        }

        private static IColumn WrapInDistinct(IColumn inner, object defaultValue)
        {
            return (IColumn)(typeof(DistinctColumn<>).MakeGenericType(inner.Type).GetConstructor(new[] { typeof(IColumn), typeof(object) }).Invoke(new[] { inner, defaultValue }));
        }
    }
}
