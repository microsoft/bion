using BSOA.Model;

namespace BSOA.Demo.Model
{
    public partial class SarifLogBsoa
    {
        public ILimitedList<Run> Runs => Run;

        public override string ToString()
        {
            return $"{Result.Count:n0} {nameof(Result)}s";
        }
    }
}
