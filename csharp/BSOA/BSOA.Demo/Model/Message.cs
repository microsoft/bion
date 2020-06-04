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
    ///  GENERATED: BSOA Entity for 'Message'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Message : PropertyBagHolder, ISarifNode, IRow
    {
        private MessageTable _table;
        private int _index;

        internal Message(MessageTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Message(MessageTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Message(SarifLogBsoa database) : this(database.Message)
        { }

        public Message() : this(SarifLogBsoa.Current)
        { }

        public Message(
            string text,
            string markdown,
            string id
        ) : this(SarifLogBsoa.Current)
        {
            Text = text;
            Markdown = markdown;
            Id = id;
        }

        public Message(Message other)
        {
            Text = other.Text;
            Markdown = other.Markdown;
            Id = other.Id;
        }

        [DataMember(Name = "text", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        [DataMember(Name = "markdown", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Markdown
        {
            get => _table.Markdown[_index];
            set => _table.Markdown[_index] = value;
        }

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        #region IEquatable<Message>
        public bool Equals(Message other)
        {
            if (other == null) { return false; }

            if (this.Text != other.Text) { return false; }
            if (this.Markdown != other.Markdown) { return false; }
            if (this.Id != other.Id) { return false; }

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

                if (Id != default(string))
                {
                    result = (result * 31) + Id.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Message);
        }

        public static bool operator ==(Message left, Message right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Message left, Message right)
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
            _table = (MessageTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Message;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Message DeepClone()
        {
            return (Message)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Message(this);
        }
        #endregion

        public static IEqualityComparer<Message> ValueComparer => EqualityComparer<Message>.Default;
        public bool ValueEquals(Message other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
