// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

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

        internal ToolComponent(ToolComponentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ToolComponent(ToolComponentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ToolComponent(SarifLogBsoa database) : this(database.ToolComponent)
        { }

        public ToolComponent() : this(SarifLogBsoa.Current)
        { }

        public ToolComponent(ToolComponent other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

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

        //public static IEqualityComparer<ToolComponent> ValueComparer => ToolComponentEqualityComparer.Instance;
        //public bool ValueEquals(ToolComponent other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
