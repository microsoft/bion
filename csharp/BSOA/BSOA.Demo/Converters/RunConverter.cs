using BSOA.Demo.Converters;
using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class RunConverter
    {
        public static Model.Run Convert(Microsoft.CodeAnalysis.Sarif.Run source, SarifLogBsoa toDatabase)
        {
            Model.Run run = new Model.Run(toDatabase.Run);

            run.Artifacts.ConvertList(source.Artifacts, toDatabase, ArtifactConverter.Convert);
            run.Results.ConvertList(source.Results, toDatabase, ResultConverter.Convert);

            return run;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Run expected, Model.Run actual)
        {
            if (expected == null) { return actual == null; }

            if (!expected.Artifacts.CompareList(actual.Artifacts, ArtifactConverter.Compare)) { return false; }
            if (!expected.Results.CompareList(actual.Results, ResultConverter.Compare)) { return false; }

            return true;
        }
    }
}
