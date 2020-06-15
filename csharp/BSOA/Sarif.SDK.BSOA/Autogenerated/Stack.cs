// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Stack'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Stack : PropertyBagHolder, ISarifNode, IRow
    {
        private StackTable _table;
        private int _index;

        public Stack() : this(SarifLogDatabase.Current.Stack)
        { }

        public Stack(SarifLog root) : this(root.Database.Stack)
        { }

        internal Stack(StackTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Stack(StackTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Stack(
            Message message,
            IList<StackFrame> frames,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Stack)
        {
            Message = message;
            Frames = frames;
            Properties = properties;
        }

        public Stack(Stack other) 
            : this(SarifLogDatabase.Current.Stack)
        {
            Message = other.Message;
            Frames = other.Frames;
            Properties = other.Properties;
        }

        [DataMember(Name = "message", IsRequired = false, EmitDefaultValue = false)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "frames", IsRequired = false, EmitDefaultValue = false)]
        public IList<StackFrame> Frames
        {
            get => _table.Database.StackFrame.List(_table.Frames[_index]);
            set => _table.Database.StackFrame.List(_table.Frames[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Stack>
        public bool Equals(Stack other)
        {
            if (other == null) { return false; }

            if (this.Message != other.Message) { return false; }
            if (this.Frames != other.Frames) { return false; }
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
                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (Frames != default(IList<StackFrame>))
                {
                    result = (result * 31) + Frames.GetHashCode();
                }

                if (Properties != default(IDictionary<string, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Stack);
        }

        public static bool operator ==(Stack left, Stack right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Stack left, Stack right)
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
            _table = (StackTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Stack;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Stack DeepClone()
        {
            return (Stack)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Stack(this);
        }
        #endregion

        public static IEqualityComparer<Stack> ValueComparer => EqualityComparer<Stack>.Default;
        public bool ValueEquals(Stack other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
