using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    public readonly struct Message
    {
        internal readonly MessageTable _table;
        internal readonly int _index;

        public Message(MessageTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public Message(MessageTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Message(SarifLogBsoa database) : this(database.Message)
        { }

        public bool IsNull => (_table == null || _index < 0);

        // Add via 'bprop' snippet
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
    }

    public class MessageTable : Table<Message>
    {
        internal SarifLogBsoa Database;
        
        internal StringColumn Text;
        internal StringColumn Markdown;
        internal StringColumn Id;

        // StringListColumn Arguments;
        // Properties

        public MessageTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.Text = AddColumn(nameof(Text), new StringColumn());
            this.Markdown = AddColumn(nameof(Markdown), new StringColumn());
            this.Id = AddColumn(nameof(Id), new StringColumn());
        }

        public override Message this[int index] => new Message(this, index);
    }

}
