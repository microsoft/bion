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
    ///  GENERATED: BSOA Entity for 'Tool'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Tool : PropertyBagHolder, ISarifNode, IRow
    {
        private ToolTable _table;
        private int _index;

        public Tool() : this(SarifLogDatabase.Current.Tool)
        { }

        public Tool(SarifLog root) : this(root.Database.Tool)
        { }

        internal Tool(ToolTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal Tool(ToolTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Tool(
            ToolComponent driver,
            IList<ToolComponent> extensions,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Tool)
        {
            Driver = driver;
            Extensions = extensions;
            Properties = properties;
        }

        public Tool(Tool other) 
            : this(SarifLogDatabase.Current.Tool)
        {
            Driver = other.Driver;
            Extensions = other.Extensions;
            Properties = other.Properties;
        }

        partial void Init();

        public ToolComponent Driver
        {
            get => _table.Database.ToolComponent.Get(_table.Driver[_index]);
            set => _table.Driver[_index] = _table.Database.ToolComponent.LocalIndex(value);
        }

        public IList<ToolComponent> Extensions
        {
            get => _table.Database.ToolComponent.List(_table.Extensions[_index]);
            set => _table.Database.ToolComponent.List(_table.Extensions[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Tool>
        public bool Equals(Tool other)
        {
            if (other == null) { return false; }

            if (this.Driver != other.Driver) { return false; }
            if (this.Extensions != other.Extensions) { return false; }
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
                if (Driver != default(ToolComponent))
                {
                    result = (result * 31) + Driver.GetHashCode();
                }

                if (Extensions != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Extensions.GetHashCode();
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
            return Equals(obj as Tool);
        }

        public static bool operator ==(Tool left, Tool right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Tool left, Tool right)
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
            _table = (ToolTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Tool;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Tool DeepClone()
        {
            return (Tool)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Tool(this);
        }
        #endregion

        public static IEqualityComparer<Tool> ValueComparer => EqualityComparer<Tool>.Default;
        public bool ValueEquals(Tool other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
