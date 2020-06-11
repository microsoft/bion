// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'ExceptionData'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ExceptionData : PropertyBagHolder, ISarifNode, IRow
    {
        private ExceptionDataTable _table;
        private int _index;

        public ExceptionData() : this(SarifLogDatabase.Current.ExceptionData)
        { }

        public ExceptionData(SarifLog root) : this(root.Database.ExceptionData)
        { }

        internal ExceptionData(ExceptionDataTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ExceptionData(ExceptionDataTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ExceptionData(
            string kind,
            string message,
            Stack stack,
            IList<ExceptionData> innerExceptions,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ExceptionData)
        {
            Kind = kind;
            Message = message;
            Stack = stack;
            InnerExceptions = innerExceptions;
            Properties = properties;
        }

        public ExceptionData(ExceptionData other) 
            : this(SarifLogDatabase.Current.ExceptionData)
        {
            Kind = other.Kind;
            Message = other.Message;
            Stack = other.Stack;
            InnerExceptions = other.InnerExceptions;
            Properties = other.Properties;
        }

        [DataMember(Name = "kind", IsRequired = false, EmitDefaultValue = false)]
        public string Kind
        {
            get => _table.Kind[_index];
            set => _table.Kind[_index] = value;
        }

        [DataMember(Name = "message", IsRequired = false, EmitDefaultValue = false)]
        public string Message
        {
            get => _table.Message[_index];
            set => _table.Message[_index] = value;
        }

        [DataMember(Name = "stack", IsRequired = false, EmitDefaultValue = false)]
        public Stack Stack
        {
            get => _table.Database.Stack.Get(_table.Stack[_index]);
            set => _table.Stack[_index] = _table.Database.Stack.LocalIndex(value);
        }

        [DataMember(Name = "innerExceptions", IsRequired = false, EmitDefaultValue = false)]
        public IList<ExceptionData> InnerExceptions
        {
            get => _table.Database.ExceptionData.List(_table.InnerExceptions[_index]);
            set => _table.Database.ExceptionData.List(_table.InnerExceptions[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ExceptionData>
        public bool Equals(ExceptionData other)
        {
            if (other == null) { return false; }

            if (this.Kind != other.Kind) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.Stack != other.Stack) { return false; }
            if (this.InnerExceptions != other.InnerExceptions) { return false; }
            if (this.Properties != other.Properties) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Kind != default(string))
                {
                    result = (result * 31) + Kind.GetHashCode();
                }

                if (Message != default(string))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (Stack != default(Stack))
                {
                    result = (result * 31) + Stack.GetHashCode();
                }

                if (InnerExceptions != default(IList<ExceptionData>))
                {
                    result = (result * 31) + InnerExceptions.GetHashCode();
                }

                if (Properties != default(IDictionary<string, string>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExceptionData);
        }

        public static bool operator ==(ExceptionData left, ExceptionData right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ExceptionData left, ExceptionData right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ExceptionDataTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ExceptionData;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ExceptionData DeepClone()
        {
            return (ExceptionData)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ExceptionData(this);
        }
        #endregion

        public static IEqualityComparer<ExceptionData> ValueComparer => EqualityComparer<ExceptionData>.Default;
        public bool ValueEquals(ExceptionData other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
