using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class MessageConverter
    {
        public static Model.Message Convert(Microsoft.CodeAnalysis.Sarif.Message source, SarifLogBsoa toDatabase)
        {
            Model.Message result = new Model.Message(toDatabase);

            result.Text = source.Text;
            result.Markdown = source.Markdown;
            result.Id = source.Id;

            // Arguments, Properties

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Message expected, Model.Message actual)
        {
            if (expected == null) { return actual.IsNull; }

            if (expected.Text != actual.Text) { return false; }
            if (expected.Markdown != actual.Markdown) { return false; }
            if (expected.Id != actual.Id) { return false; }

            return true;
        }
    }
}
