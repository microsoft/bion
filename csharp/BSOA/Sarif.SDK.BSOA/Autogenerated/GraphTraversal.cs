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
    ///  GENERATED: BSOA Entity for 'GraphTraversal'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class GraphTraversal : PropertyBagHolder, ISarifNode, IRow
    {
        private GraphTraversalTable _table;
        private int _index;

        public GraphTraversal() : this(SarifLogDatabase.Current.GraphTraversal)
        { }

        public GraphTraversal(SarifLog root) : this(root.Database.GraphTraversal)
        { }

        internal GraphTraversal(GraphTraversalTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal GraphTraversal(GraphTraversalTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public GraphTraversal(
            int runGraphIndex,
            int resultGraphIndex,
            Message description,
            IDictionary<string, MultiformatMessageString> initialState,
            IDictionary<string, MultiformatMessageString> immutableState,
            IList<EdgeTraversal> edgeTraversals,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.GraphTraversal)
        {
            RunGraphIndex = runGraphIndex;
            ResultGraphIndex = resultGraphIndex;
            Description = description;
            InitialState = initialState;
            ImmutableState = immutableState;
            EdgeTraversals = edgeTraversals;
            Properties = properties;
        }

        public GraphTraversal(GraphTraversal other) 
            : this(SarifLogDatabase.Current.GraphTraversal)
        {
            RunGraphIndex = other.RunGraphIndex;
            ResultGraphIndex = other.ResultGraphIndex;
            Description = other.Description;
            InitialState = other.InitialState;
            ImmutableState = other.ImmutableState;
            EdgeTraversals = other.EdgeTraversals;
            Properties = other.Properties;
        }

        public int RunGraphIndex
        {
            get => _table.RunGraphIndex[_index];
            set => _table.RunGraphIndex[_index] = value;
        }

        public int ResultGraphIndex
        {
            get => _table.ResultGraphIndex[_index];
            set => _table.ResultGraphIndex[_index] = value;
        }

        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        public IDictionary<string, MultiformatMessageString> InitialState
        {
            get => _table.InitialState[_index];
            set => _table.InitialState[_index] = value;
        }

        public IDictionary<string, MultiformatMessageString> ImmutableState
        {
            get => _table.ImmutableState[_index];
            set => _table.ImmutableState[_index] = value;
        }

        public IList<EdgeTraversal> EdgeTraversals
        {
            get => _table.Database.EdgeTraversal.List(_table.EdgeTraversals[_index]);
            set => _table.Database.EdgeTraversal.List(_table.EdgeTraversals[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<GraphTraversal>
        public bool Equals(GraphTraversal other)
        {
            if (other == null) { return false; }

            if (this.RunGraphIndex != other.RunGraphIndex) { return false; }
            if (this.ResultGraphIndex != other.ResultGraphIndex) { return false; }
            if (this.Description != other.Description) { return false; }
            if (this.InitialState != other.InitialState) { return false; }
            if (this.ImmutableState != other.ImmutableState) { return false; }
            if (this.EdgeTraversals != other.EdgeTraversals) { return false; }
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
                if (RunGraphIndex != default(int))
                {
                    result = (result * 31) + RunGraphIndex.GetHashCode();
                }

                if (ResultGraphIndex != default(int))
                {
                    result = (result * 31) + ResultGraphIndex.GetHashCode();
                }

                if (Description != default(Message))
                {
                    result = (result * 31) + Description.GetHashCode();
                }

                if (InitialState != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + InitialState.GetHashCode();
                }

                if (ImmutableState != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + ImmutableState.GetHashCode();
                }

                if (EdgeTraversals != default(IList<EdgeTraversal>))
                {
                    result = (result * 31) + EdgeTraversals.GetHashCode();
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
            return Equals(obj as GraphTraversal);
        }

        public static bool operator ==(GraphTraversal left, GraphTraversal right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(GraphTraversal left, GraphTraversal right)
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
            _table = (GraphTraversalTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.GraphTraversal;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public GraphTraversal DeepClone()
        {
            return (GraphTraversal)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new GraphTraversal(this);
        }
        #endregion

        public static IEqualityComparer<GraphTraversal> ValueComparer => EqualityComparer<GraphTraversal>.Default;
        public bool ValueEquals(GraphTraversal other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
