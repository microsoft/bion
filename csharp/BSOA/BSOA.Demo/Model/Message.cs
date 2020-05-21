using BSOA.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Message'
    /// </summary>
    public partial class Message : IRow
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
