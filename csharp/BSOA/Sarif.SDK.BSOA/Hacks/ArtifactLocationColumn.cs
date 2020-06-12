using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Column
{
    internal class ArtifactLocationColumn : WrappingColumn<ArtifactLocation, int>
    {
        private ArtifactLocationTable _table;

        public ArtifactLocationColumn(SarifLogDatabase db) : base(new RefColumn(nameof(ArtifactLocation)))
        {
            _table = db.ArtifactLocation;
        }

        public override ArtifactLocation this[int index] 
        { 
            get => new ArtifactLocation(_table, Inner[index]);
            set => Inner[index] = _table.LocalIndex(value);
        }
    }
}
