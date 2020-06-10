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
    ///  GENERATED: BSOA Entity for 'Replacement'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Replacement : PropertyBagHolder, ISarifNode, IRow
    {
        private ReplacementTable _table;
        private int _index;

        public Replacement() : this(SarifLogDatabase.Current.Replacement)
        { }

        public Replacement(SarifLog root) : this(root.Database.Replacement)
        { }

        internal Replacement(ReplacementTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Replacement(ReplacementTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Replacement(
            Region deletedRegion,
            ArtifactContent insertedContent,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.Replacement)
        {
            DeletedRegion = deletedRegion;
            InsertedContent = insertedContent;
            Properties = properties;
        }

        public Replacement(Replacement other) 
            : this(SarifLogDatabase.Current.Replacement)
        {
            DeletedRegion = other.DeletedRegion;
            InsertedContent = other.InsertedContent;
            Properties = other.Properties;
        }

        [DataMember(Name = "deletedRegion", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Region DeletedRegion
        {
            get => _table.Database.Region.Get(_table.DeletedRegion[_index]);
            set => _table.DeletedRegion[_index] = _table.Database.Region.LocalIndex(value);
        }

        [DataMember(Name = "insertedContent", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactContent InsertedContent
        {
            get => _table.Database.ArtifactContent.Get(_table.InsertedContent[_index]);
            set => _table.InsertedContent[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Replacement>
        public bool Equals(Replacement other)
        {
            if (other == null) { return false; }

            if (this.DeletedRegion != other.DeletedRegion) { return false; }
            if (this.InsertedContent != other.InsertedContent) { return false; }
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
                if (DeletedRegion != default(Region))
                {
                    result = (result * 31) + DeletedRegion.GetHashCode();
                }

                if (InsertedContent != default(ArtifactContent))
                {
                    result = (result * 31) + InsertedContent.GetHashCode();
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
            return Equals(obj as Replacement);
        }

        public static bool operator ==(Replacement left, Replacement right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Replacement left, Replacement right)
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
            _table = (ReplacementTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Replacement;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Replacement DeepClone()
        {
            return (Replacement)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Replacement(this);
        }
        #endregion

        public static IEqualityComparer<Replacement> ValueComparer => EqualityComparer<Replacement>.Default;
        public bool ValueEquals(Replacement other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
