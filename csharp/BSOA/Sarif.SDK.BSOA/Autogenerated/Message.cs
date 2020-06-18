// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Message'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Message : PropertyBagHolder, ISarifNode, IRow
    {
        private MessageTable _table;
        private int _index;

        public Message() : this(SarifLogDatabase.Current.Message)
        { }

        public Message(SarifLog root) : this(root.Database.Message)
        { }

        internal Message(MessageTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal Message(MessageTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Message(
            string text,
            string markdown,
            string id,
            IList<string> arguments,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Message)
        {
            Text = text;
            Markdown = markdown;
            Id = id;
            Arguments = arguments;
            Properties = properties;
        }

        public Message(Message other) 
            : this(SarifLogDatabase.Current.Message)
        {
            Text = other.Text;
            Markdown = other.Markdown;
            Id = other.Id;
            Arguments = other.Arguments;
            Properties = other.Properties;
        }

        partial void Init();

        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        public string Markdown
        {
            get => _table.Markdown[_index];
            set => _table.Markdown[_index] = value;
        }

        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        public IList<string> Arguments
        {
            get => _table.Arguments[_index];
            set => _table.Arguments[_index] = value;
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Message>
        public bool Equals(Message other)
        {
            if (other == null) { return false; }

            if (this.Text != other.Text) { return false; }
            if (this.Markdown != other.Markdown) { return false; }
            if (this.Id != other.Id) { return false; }
            if (this.Arguments != other.Arguments) { return false; }
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

                if (Id != default(string))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                if (Arguments != default(IList<string>))
                {
                    result = (result * 31) + Arguments.GetHashCode();
                }

                if (Properties != default(IDictionary<string, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
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
