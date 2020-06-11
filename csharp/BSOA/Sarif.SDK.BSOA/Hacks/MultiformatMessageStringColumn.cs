using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Column
{
    internal class MultiformatMessageStringColumn : ConvertingColumn<MultiformatMessageString, int>
    {
        public MultiformatMessageStringColumn(SarifLogDatabase db)
            : base(
                  new RefColumn(nameof(MultiformatMessageString)),
                  (item) => db.MultiformatMessageString.LocalIndex(item),
                  (index) => new MultiformatMessageString(db.MultiformatMessageString, index)
                  )
        { }
    }
}
