using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Column
{
    internal class ArtifactLocationColumn : ConvertingColumn<ArtifactLocation, int>
    {
        public ArtifactLocationColumn(SarifLogDatabase db)
            : base(
                  new RefColumn(nameof(ArtifactLocation)),
                  (item) => db.ArtifactLocation.LocalIndex(item),
                  (index) => new ArtifactLocation(db.ArtifactLocation, index)
                  )
        { }
    }
}
