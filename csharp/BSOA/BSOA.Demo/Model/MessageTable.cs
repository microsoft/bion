using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Message' entity.
    /// </summary>
    public partial class MessageTable : Table<Message>
    {
        internal SarifLogBsoa Database;

        internal IColumn<string> Text;
        internal IColumn<string> Markdown;
        internal IColumn<string> Id;

        public MessageTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>(null));
            Markdown = AddColumn(nameof(Markdown), ColumnFactory.Build<string>(null));
            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(null));
        }

        public override Message Get(int index)
        {
            return (index == -1 ? null : new Message(this, index));
        }
    }
}
