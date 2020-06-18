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
    ///  GENERATED: BSOA Entity for 'Node'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Node : PropertyBagHolder, ISarifNode, IRow
    {
        private NodeTable _table;
        private int _index;

        public Node() : this(SarifLogDatabase.Current.Node)
        { }

        public Node(SarifLog root) : this(root.Database.Node)
        { }

        internal Node(NodeTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Node(NodeTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Node(
            string id,
            Message label,
            Location location,
            IList<Node> children,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Node)
        {
            Id = id;
            Label = label;
            Location = location;
            Children = children;
            Properties = properties;
        }

        public Node(Node other) 
            : this(SarifLogDatabase.Current.Node)
        {
            Id = other.Id;
            Label = other.Label;
            Location = other.Location;
            Children = other.Children;
            Properties = other.Properties;
        }

        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        public Message Label
        {
            get => _table.Database.Message.Get(_table.Label[_index]);
            set => _table.Label[_index] = _table.Database.Message.LocalIndex(value);
        }

        public Location Location
        {
            get => _table.Database.Location.Get(_table.Location[_index]);
            set => _table.Location[_index] = _table.Database.Location.LocalIndex(value);
        }

        public IList<Node> Children
        {
            get => _table.Database.Node.List(_table.Children[_index]);
            set => _table.Database.Node.List(_table.Children[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Node>
        public bool Equals(Node other)
        {
            if (other == null) { return false; }

            if (this.Id != other.Id) { return false; }
            if (this.Label != other.Label) { return false; }
            if (this.Location != other.Location) { return false; }
            if (this.Children != other.Children) { return false; }
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
                if (Id != default(string))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                if (Label != default(Message))
                {
                    result = (result * 31) + Label.GetHashCode();
                }

                if (Location != default(Location))
                {
                    result = (result * 31) + Location.GetHashCode();
                }

                if (Children != default(IList<Node>))
                {
                    result = (result * 31) + Children.GetHashCode();
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
            return Equals(obj as Node);
        }

        public static bool operator ==(Node left, Node right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Node left, Node right)
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
            _table = (NodeTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Node;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Node DeepClone()
        {
            return (Node)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Node(this);
        }
        #endregion

        public static IEqualityComparer<Node> ValueComparer => EqualityComparer<Node>.Default;
        public bool ValueEquals(Node other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
