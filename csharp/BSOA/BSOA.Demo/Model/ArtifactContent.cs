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
    ///  GENERATED: BSOA Entity for 'ArtifactContent'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ArtifactContent : PropertyBagHolder, ISarifNode, IRow
    {
        private ArtifactContentTable _table;
        private int _index;

        public ArtifactContent() : this(SarifLogDatabase.Current.ArtifactContent)
        { }

        public ArtifactContent(SarifLog root) : this(root.Database.ArtifactContent)
        { }

        internal ArtifactContent(ArtifactContentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ArtifactContent(ArtifactContentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactContent(
            string text,
            string binary
        ) 
            : this(SarifLogDatabase.Current.ArtifactContent)
        {
            Text = text;
            Binary = binary;
        }

        public ArtifactContent(ArtifactContent other) 
            : this(SarifLogDatabase.Current.ArtifactContent)
        {
            Text = other.Text;
            Binary = other.Binary;
        }

        [DataMember(Name = "text", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        [DataMember(Name = "binary", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Binary
        {
            get => _table.Binary[_index];
            set => _table.Binary[_index] = value;
        }

        #region IEquatable<ArtifactContent>
        public bool Equals(ArtifactContent other)
        {
            if (other == null) { return false; }

            if (this.Text != other.Text) { return false; }
            if (this.Binary != other.Binary) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Text != default(string))
                {
                    result = (result * 31) + Text.GetHashCode();
                }

                if (Binary != default(string))
                {
                    result = (result * 31) + Binary.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArtifactContent);
        }

        public static bool operator ==(ArtifactContent left, ArtifactContent right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ArtifactContent left, ArtifactContent right)
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

        public static IEqualityComparer<ArtifactContent> ValueComparer => EqualityComparer<ArtifactContent>.Default;
        public bool ValueEquals(ArtifactContent other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
