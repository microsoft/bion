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

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'ToolComponent'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ToolComponent : PropertyBagHolder, ISarifNode, IRow
    {
        private ToolComponentTable _table;
        private int _index;

        public ToolComponent() : this(SarifLogDatabase.Current.ToolComponent)
        { }

        public ToolComponent(SarifLog root) : this(root.Database.ToolComponent)
        { }

        internal ToolComponent(ToolComponentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ToolComponent(ToolComponentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ToolComponent(
            string name
        ) 
            : this(SarifLogDatabase.Current.ToolComponent)
        {
            Name = name;
        }

        public ToolComponent(ToolComponent other) 
            : this(SarifLogDatabase.Current.ToolComponent)
        {
            Name = other.Name;
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        #region IEquatable<ToolComponent>
        public bool Equals(ToolComponent other)
        {
            if (other == null) { return false; }

            if (this.Name != other.Name) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ToolComponent);
        }

        public static bool operator ==(ToolComponent left, ToolComponent right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ToolComponent left, ToolComponent right)
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
            _table = (ToolComponentTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ToolComponent;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ToolComponent DeepClone()
        {
            return (ToolComponent)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ToolComponent(this);
        }
        #endregion

        public static IEqualityComparer<ToolComponent> ValueComparer => EqualityComparer<ToolComponent>.Default;
        public bool ValueEquals(ToolComponent other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
