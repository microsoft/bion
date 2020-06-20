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
    ///  GENERATED: BSOA Entity for 'EdgeTraversal'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class EdgeTraversal : PropertyBagHolder, ISarifNode, IRow
    {
        private EdgeTraversalTable _table;
        private int _index;

        public EdgeTraversal() : this(SarifLogDatabase.Current.EdgeTraversal)
        { }

        public EdgeTraversal(SarifLog root) : this(root.Database.EdgeTraversal)
        { }

        internal EdgeTraversal(EdgeTraversalTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal EdgeTraversal(EdgeTraversalTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public EdgeTraversal(
            String edgeId,
            Message message,
            IDictionary<String, MultiformatMessageString> finalState,
            int stepOverEdgeCount,
            IDictionary<String, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.EdgeTraversal)
        {
            EdgeId = edgeId;
            Message = message;
            FinalState = finalState;
            StepOverEdgeCount = stepOverEdgeCount;
            Properties = properties;
        }

        public EdgeTraversal(EdgeTraversal other) 
            : this(SarifLogDatabase.Current.EdgeTraversal)
        {
            EdgeId = other.EdgeId;
            Message = other.Message;
            FinalState = other.FinalState;
            StepOverEdgeCount = other.StepOverEdgeCount;
            Properties = other.Properties;
        }

        partial void Init();

        public String EdgeId
        {
            get => _table.EdgeId[_index];
            set => _table.EdgeId[_index] = value;
        }

        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        public IDictionary<String, MultiformatMessageString> FinalState
        {
            get => _table.FinalState[_index];
            set => _table.FinalState[_index] = value;
        }

        public int StepOverEdgeCount
        {
            get => _table.StepOverEdgeCount[_index];
            set => _table.StepOverEdgeCount[_index] = value;
        }

        internal override IDictionary<String, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<EdgeTraversal>
        public bool Equals(EdgeTraversal other)
        {
            if (other == null) { return false; }

            if (this.EdgeId != other.EdgeId) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.FinalState != other.FinalState) { return false; }
            if (this.StepOverEdgeCount != other.StepOverEdgeCount) { return false; }
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
                if (EdgeId != default(String))
                {
                    result = (result * 31) + EdgeId.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (FinalState != default(IDictionary<String, MultiformatMessageString>))
                {
                    result = (result * 31) + FinalState.GetHashCode();
                }

                if (StepOverEdgeCount != default(int))
                {
                    result = (result * 31) + StepOverEdgeCount.GetHashCode();
                }

                if (Properties != default(IDictionary<String, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EdgeTraversal);
        }

        public static bool operator ==(EdgeTraversal left, EdgeTraversal right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(EdgeTraversal left, EdgeTraversal right)
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

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.EdgeTraversal;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public EdgeTraversal DeepClone()
        {
            return (EdgeTraversal)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new EdgeTraversal(this);
        }
        #endregion

        public static IEqualityComparer<EdgeTraversal> ValueComparer => EqualityComparer<EdgeTraversal>.Default;
        public bool ValueEquals(EdgeTraversal other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
