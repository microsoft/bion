using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BSOA.Generator.Schema
{
    public class Column
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ColumnTypeCategory Category { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Default { get; set; }
        
        public string UnderlyingType { get; set; }
        public string ReferencedTableName { get; set; }

        internal Column()
        { }

        private Column(ColumnTypeCategory category, string name, string type, string defaultValue = null, string underlyingType = null, string referencedTableName = null)
        {
            Category = category;
            Name = name;
            Type = type;
            UnderlyingType = underlyingType;
            Default = defaultValue;
            ReferencedTableName = referencedTableName;
        }

        public static Column Simple(string name, string type, string defaultValue = null)
        {
            return new Column(ColumnTypeCategory.Simple, name, type, defaultValue);
        }

        public static Column Enum(string name, string type, string underlyingType, string defaultValue = null)
        {
            return new Column(ColumnTypeCategory.Enum, name, type, defaultValue, underlyingType);
        }

        public static Column Ref(string name, string targetTable)
        {
            return new Column(ColumnTypeCategory.Ref, name, targetTable, referencedTableName: targetTable);
        }

        public static Column RefList(string name, string targetTable)
        {
            return new Column(ColumnTypeCategory.RefList, name, $"IList<{targetTable}>", referencedTableName: targetTable);
        }
    }
}
