using BSOA.Column;
using BSOA.Model;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    public struct Run
    {
        internal RunTable _table;
        internal int _index;

        public Run(RunTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public Run(RunTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Run(SarifLogBsoa database) : this(database.Run)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public IList<Result> Results
        {
            get => new NumberListConverter<Result, ResultTable>(_table.Results[_index], _table.Database.Result, (table, index) => new Result(table, index), (table, item) => item._index);
            set => new NumberListConverter<Result, ResultTable>(_table.Results[_index], _table.Database.Result, (table, index) => new Result(table, index), (table, item) => item._index).SetTo(value);
        }

        public IList<Artifact> Artifacts
        {
            get => new NumberListConverter<Artifact, ArtifactTable>(_table.Artifacts[_index], _table.Database.Artifact, (table, index) => new Artifact(table, index), (table, item) => item._index);
            set => new NumberListConverter<Artifact, ArtifactTable>(_table.Artifacts[_index], _table.Database.Artifact, (table, index) => new Artifact(table, index), (table, item) => item._index).SetTo(value);
        }
    }

    public class RunTable : Table<Run>
    {
        internal SarifLogBsoa Database;

        internal RefListColumn Results;
        internal RefListColumn Artifacts;
        
        // Missing many unused in Spam SARIF
        // Used but missing: Invocations, 

        public RunTable(SarifLogBsoa database) : base()
        {
            this.Database = database;
         
            this.Results = AddColumn(nameof(Results), new RefListColumn(nameof(database.Result)));
            this.Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(database.Artifact)));
        }

        public override Run this[int index] => new Run(this, index);
    }
}
