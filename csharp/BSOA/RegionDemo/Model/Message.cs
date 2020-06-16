// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Message'
    /// </summary>
    public partial class Message : IRow, IEquatable<Message>
    {
        private MessageTable _table;
        private int _index;

        public Message() : this(TinyDatabase.Current.Message)
        { }

        public Message(TinyLog root) : this(root.Database.Message)
        { }

        internal Message(MessageTable table) : this(table, table.Count)
        {
            table.Add();
        }
        
        internal Message(MessageTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Message(
            string text,
            string markdown,
            string id
        ) 
            : this(TinyDatabase.Current.Message)
        {
            Text = text;
            Markdown = markdown;
            Id = id;
        }

        public Message(Message other) 
            : this(TinyDatabase.Current.Message)
        {
            Text = other.Text;
            Markdown = other.Markdown;
            Id = other.Id;
        }

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
    }
}
