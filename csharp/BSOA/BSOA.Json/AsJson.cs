using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BSOA.Json
{
    public class AsJson
    {
        private static JsonSerializer _jsonSerializer;

        public static void Save<T>(string filePath, T item, bool verbose = false)
        {
            BuildSerializer();

            using (JsonTextWriter writer = new JsonTextWriter(System.IO.File.CreateText(filePath)))
            {
                if (verbose) { writer.Formatting = Formatting.Indented; }
                _jsonSerializer.Serialize(writer, item);
            }
        }

        public static T Load<T>(string filePath)
        {
            BuildSerializer();

            using (JsonTextReader reader = new JsonTextReader(System.IO.File.OpenText(filePath)))
            {
                return _jsonSerializer.Deserialize<T>(reader);
            }
        }

        private static void BuildSerializer()
        {
            if (_jsonSerializer != null) { return; }

            _jsonSerializer = new JsonSerializer();

            _jsonSerializer.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            // Omit null values
            _jsonSerializer.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
        }
    }
}
