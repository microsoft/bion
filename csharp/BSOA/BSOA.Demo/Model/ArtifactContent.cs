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
    ///  GENERATED: BSOA Entity for 'ArtifactContent'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ArtifactContent : PropertyBagHolder, ISarifNode, IRow
    {
        private ArtifactContentTable _table;
        private int _index;

        internal ArtifactContent(ArtifactContentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactContent(ArtifactContentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ArtifactContent(SarifLogBsoa database) : this(database.ArtifactContent)
        { }

        public ArtifactContent() : this(SarifLogBsoa.Current)
        { }

        public ArtifactContent(
			string text,
			string binary
        ) : this(SarifLogBsoa.Current)
        {
			Text = text;
			Binary = binary;
        }

        public ArtifactContent(ArtifactContent other)
        {
			Text = other.Text;
			Binary = other.Binary;
        }

        [DataMember(Name = "text", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        [DataMember(Name = "binary", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Binary
        {
            get => _table.Binary[_index];
            set => _table.Binary[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ArtifactContentTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ArtifactContent;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ArtifactContent DeepClone()
        {
            return (ArtifactContent)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ArtifactContent(this);
        }
        #endregion

        //public static IEqualityComparer<ArtifactContent> ValueComparer => ArtifactContentEqualityComparer.Instance;
        //public bool ValueEquals(ArtifactContent other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
