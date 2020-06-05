using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class MessageConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Message expected, Model.Message actual)
        {
            if (expected == null) { return actual == null; }

            if (expected.Text != actual.Text) { return false; }
            if (expected.Markdown != actual.Markdown) { return false; }
            if (expected.Id != actual.Id) { return false; }

            return true;
        }
    }
}
