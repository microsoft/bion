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
    ///  GENERATED: BSOA Entity for 'MultiformatMessageString'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class MultiformatMessageString : PropertyBagHolder, ISarifNode, IRow
    {
        private MultiformatMessageStringTable _table;
        private int _index;

        public MultiformatMessageString() : this(SarifLogDatabase.Current.MultiformatMessageString)
        { }

        public MultiformatMessageString(SarifLog root) : this(root.Database.MultiformatMessageString)
        { }

        internal MultiformatMessageString(MultiformatMessageStringTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal MultiformatMessageString(MultiformatMessageStringTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public MultiformatMessageString(
            string text,
            string markdown,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.MultiformatMessageString)
        {
            Text = text;
            Markdown = markdown;
            Properties = properties;
        }

        public MultiformatMessageString(MultiformatMessageString other) 
            : this(SarifLogDatabase.Current.MultiformatMessageString)
        {
            Text = other.Text;
            Markdown = other.Markdown;
            Properties = other.Properties;
        }

        [DataMember(Name = "text", IsRequired = false, EmitDefaultValue = false)]
        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        [DataMember(Name = "markdown", IsRequired = false, EmitDefaultValue = false)]
        public string Markdown
        {
            get => _table.Markdown[_index];
            set => _table.Markdown[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<MultiformatMessageString>
        public bool Equals(MultiformatMessageString other)
        {
            if (other == null) { return false; }

            if (this.Text != other.Text) { return false; }
            if (this.Markdown != other.Markdown) { return false; }
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
                if (Text != default(string))
                {
                    result = (result * 31) + Text.GetHashCode();
                }

                if (Markdown != default(string))
                {
                    result = (result * 31) + Markdown.GetHashCode();
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
            return Equals(obj as MultiformatMessageString);
        }

        public static bool operator ==(MultiformatMessageString left, MultiformatMessageString right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(MultiformatMessageString left, MultiformatMessageString right)
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
            _table = (MultiformatMessageStringTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.MultiformatMessageString;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public MultiformatMessageString DeepClone()
        {
            return (MultiformatMessageString)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new MultiformatMessageString(this);
        }
        #endregion

        public static IEqualityComparer<MultiformatMessageString> ValueComparer => EqualityComparer<MultiformatMessageString>.Default;
        public bool ValueEquals(MultiformatMessageString other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
