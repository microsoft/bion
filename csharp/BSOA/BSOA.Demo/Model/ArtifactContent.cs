using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  SoA Item for 'ArtifactContent' type.
    /// </summary>
    public struct ArtifactContent
    {
        internal ArtifactContentTable Table { get; }
        internal int Index { get; }

        internal ArtifactContent(ArtifactContentTable table, int index)
        {
            this.Table = table;
            this.Index = index;
        }

        public ArtifactContent(ArtifactContentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ArtifactContent(SarifLogBsoa database) : this(database.ArtifactContent)
        { }

        public string Text
        {
            get => Table.Text[Index];
            set => Table.Text[Index] = value;
        }

        public string Binary
        {
            get => Table.Binary[Index];
            set => Table.Binary[Index] = value;
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
