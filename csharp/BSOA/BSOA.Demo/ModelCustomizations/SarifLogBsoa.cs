using Microsoft.CodeAnalysis.Sarif;

using System.Linq;

namespace BSOA.Demo.Model
{
    public static class SarifLogExtensions
    {
        public static string Summary(this SarifLog log)
        {
            return $"{log.Runs.Sum((run) => run?.Results?.Count ?? 0):n0} {nameof(Result)}s";
        }
    }
}
