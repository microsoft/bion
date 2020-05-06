using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  SoA Item for 'ArtifactContent' type.
    /// </summary>
    public readonly struct ArtifactContent
    {
        internal readonly ArtifactContentTable _table;
        internal readonly int _index;

        internal ArtifactContent(ArtifactContentTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public ArtifactContent(ArtifactContentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ArtifactContent(SarifLogBsoa database) : this(database.ArtifactContent)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        public string Binary
        {
            get => _table.Binary[_index];
            set => _table.Binary[_index] = value;
        }
    }

    /// <summary>
    ///  SoA Table for 'ArtifactContent' type.
    /// </summary>
    public class ArtifactContentTable : Table<ArtifactContent>
    {
        internal SarifLogBsoa Database;

        internal StringColumn Text;
        internal StringColumn Binary;
        
        // MultiFormatMessageString Rendered
        // Properties

        public ArtifactContentTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.Text = AddColumn(nameof(Text), new StringColumn());
            this.Binary = AddColumn(nameof(Binary), new StringColumn());
        }

        public override ArtifactContent this[int index] => new ArtifactContent(this, index);
    }
}
