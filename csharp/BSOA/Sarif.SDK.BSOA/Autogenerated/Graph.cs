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
    ///  GENERATED: BSOA Entity for 'Graph'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Graph : PropertyBagHolder, ISarifNode, IRow
    {
        private GraphTable _table;
        private int _index;

        public Graph() : this(SarifLogDatabase.Current.Graph)
        { }

        public Graph(SarifLog root) : this(root.Database.Graph)
        { }

        internal Graph(GraphTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Graph(GraphTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Graph(
            Message description,
            IList<Node> nodes,
            IList<Edge> edges,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.Graph)
        {
            Description = description;
            Nodes = nodes;
            Edges = edges;
            Properties = properties;
        }

        public Graph(Graph other) 
            : this(SarifLogDatabase.Current.Graph)
        {
            Description = other.Description;
            Nodes = other.Nodes;
            Edges = other.Edges;
            Properties = other.Properties;
        }

        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "nodes", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<Node> Nodes
        {
            get => _table.Database.Node.List(_table.Nodes[_index]);
            set => _table.Database.Node.List(_table.Nodes[_index]).SetTo(value);
        }

        [DataMember(Name = "edges", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<Edge> Edges
        {
            get => _table.Database.Edge.List(_table.Edges[_index]);
            set => _table.Database.Edge.List(_table.Edges[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Graph>
        public bool Equals(Graph other)
        {
            if (other == null) { return false; }

            if (this.Description != other.Description) { return false; }
            if (this.Nodes != other.Nodes) { return false; }
            if (this.Edges != other.Edges) { return false; }
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
                if (Description != default(Message))
                {
                    result = (result * 31) + Description.GetHashCode();
                }

                if (Nodes != default(IList<Node>))
                {
                    result = (result * 31) + Nodes.GetHashCode();
                }

                if (Edges != default(IList<Edge>))
                {
                    result = (result * 31) + Edges.GetHashCode();
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
            return Equals(obj as Graph);
        }

        public static bool operator ==(Graph left, Graph right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Graph left, Graph right)
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
            _table = (GraphTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Graph;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Graph DeepClone()
        {
            return (Graph)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Graph(this);
        }
        #endregion

        public static IEqualityComparer<Graph> ValueComparer => EqualityComparer<Graph>.Default;
        public bool ValueEquals(Graph other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
