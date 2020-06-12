using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Column
{
    internal class MultiformatMessageStringColumn : WrappingColumn<MultiformatMessageString, int>
    {
        private MultiformatMessageStringTable _table;

        public MultiformatMessageStringColumn(SarifLogDatabase db) : base(new RefColumn(nameof(MultiformatMessageString)))
        {
            _table = db.MultiformatMessageString;
        }

        public override MultiformatMessageString this[int index] 
        {
            get => new MultiformatMessageString(_table, Inner[index]);
            set => Inner[index] = _table.LocalIndex(value);
        }
    }
}
