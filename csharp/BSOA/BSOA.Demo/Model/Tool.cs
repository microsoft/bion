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
    ///  GENERATED: BSOA Entity for 'Tool'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Tool : PropertyBagHolder, ISarifNode, IRow
    {
        private ToolTable _table;
        private int _index;

        internal Tool(ToolTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Tool(ToolTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Tool(SarifLogBsoa database) : this(database.Tool)
        { }

        public Tool() : this(SarifLogBsoa.Current)
        { }

        public Tool(Tool other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ToolComponent Driver
        {
            get => _table.Database.ToolComponent.Get(_table.Driver[_index]);
            set => _table.Driver[_index] = _table.Database.ToolComponent.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<ToolComponent> Extensions
        {
            get => _table.Database.ToolComponent.List(_table.Extensions[_index]);
        }

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

        //public static IEqualityComparer<Tool> ValueComparer => ToolEqualityComparer.Instance;
        //public bool ValueEquals(Tool other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
