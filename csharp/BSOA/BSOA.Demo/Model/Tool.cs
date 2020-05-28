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

        public Tool(
            ToolComponent driver,
            IList<ToolComponent> extensions
        ) : this(SarifLogBsoa.Current)
        {
            Driver = driver;
            Extensions = extensions;
        }

        public Tool(Tool other)
        {
            Driver = other.Driver;
            Extensions = other.Extensions;
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
            set => _table.Database.ToolComponent.List(_table.Extensions[_index]).SetTo(value);
        }

        #region IEquatable<Tool>
        public bool Equals(Tool other)
        {
            if (other == null) { return false; }

            if (this.Driver != other.Driver) { return false; }
            if (this.Extensions != other.Extensions) { return false; }
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

            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tool);
        }

        public static bool operator ==(Tool left, Tool right)
        {
            return (left == null ? right == null : left.Equals(right));
        }

        public static bool operator !=(Tool left, Tool right)
        {
            return (left == null ? right != null : !(left.Equals(right)));
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
