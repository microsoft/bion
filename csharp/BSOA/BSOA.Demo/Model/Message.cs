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
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        [DataMember(Name = "markdown", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Markdown
        {
            get => _table.Markdown[_index];
            set => _table.Markdown[_index] = value;
        }

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

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

        //public static IEqualityComparer<Message> ValueComparer => MessageEqualityComparer.Instance;
        //public bool ValueEquals(Message other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
