using System.Linq;

namespace BSOA.Demo.Model
{
    public partial class SarifLog
    {
        public override string ToString()
        {
            return $"{Runs.Sum((run) => run?.Results?.Count ?? 0):n0} {nameof(Result)}s";
        }
    }
}
