// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'CodeFlow'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class CodeFlow : PropertyBagHolder, ISarifNode, IRow
    {
        private CodeFlowTable _table;
        private int _index;

        public CodeFlow() : this(SarifLogDatabase.Current.CodeFlow)
        { }

        public CodeFlow(SarifLog root) : this(root.Database.CodeFlow)
        { }

        internal CodeFlow(CodeFlowTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal CodeFlow(CodeFlowTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public CodeFlow(
            Message message,
            IList<ThreadFlow> threadFlows,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.CodeFlow)
        {
            Message = message;
            ThreadFlows = threadFlows;
            Properties = properties;
        }

        public CodeFlow(CodeFlow other) 
            : this(SarifLogDatabase.Current.CodeFlow)
        {
            Message = other.Message;
            ThreadFlows = other.ThreadFlows;
            Properties = other.Properties;
        }

        partial void Init();

        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        public IList<ThreadFlow> ThreadFlows
        {
            get => _table.Database.ThreadFlow.List(_table.ThreadFlows[_index]);
            set => _table.Database.ThreadFlow.List(_table.ThreadFlows[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<CodeFlow>
        public bool Equals(CodeFlow other)
        {
            if (other == null) { return false; }

            if (this.Message != other.Message) { return false; }
            if (this.ThreadFlows != other.ThreadFlows) { return false; }
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

                if (ThreadFlows != default(IList<ThreadFlow>))
                {
                    result = (result * 31) + ThreadFlows.GetHashCode();
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
            return Equals(obj as CodeFlow);
        }

        public static bool operator ==(CodeFlow left, CodeFlow right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(CodeFlow left, CodeFlow right)
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
            _table = (CodeFlowTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.CodeFlow;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public CodeFlow DeepClone()
        {
            return (CodeFlow)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new CodeFlow(this);
        }
        #endregion

        public static IEqualityComparer<CodeFlow> ValueComparer => EqualityComparer<CodeFlow>.Default;
        public bool ValueEquals(CodeFlow other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
