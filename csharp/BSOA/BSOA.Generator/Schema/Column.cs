namespace BSOA.Generator.Schema
{
    public class Column
    {
        public ColumnTypeCategory Category { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public string UnderlyingType { get; set; }

        private Column(ColumnTypeCategory category, string name, string type, string defaultValue, string underlyingType = null)
        {
            Category = category;
            Name = name;
            Type = type;
            UnderlyingType = underlyingType;
            Default = defaultValue ?? "null";
        }

        public static Column Simple(string name, string type, string defaultValue = null)
        {
            return new Column(ColumnTypeCategory.Simple, name, type, defaultValue);
        }

        public static Column DateTime(string name, string defaultValue = null)
        {
            return new Column(ColumnTypeCategory.DateTime, name, "DateTime", defaultValue);
        }

        public static Column Enum(string name, string type, string underlyingType, string defaultValue = null)
        {
            return new Column(ColumnTypeCategory.Enum, name, type, defaultValue, underlyingType);
        }

        public static Column FlagsEnum(string name, string type, string underlyingType, string defaultValue = null)
        {
            return new Column(ColumnTypeCategory.FlagsEnum, name, type, defaultValue, underlyingType);
        }

        public static Column Ref(string name, string targetTable)
        {
            return new Column(ColumnTypeCategory.Ref, name, targetTable, null);
        }

        public static Column RefList(string name, string targetTable)
        {
            return new Column(ColumnTypeCategory.RefList, name, targetTable, null);
        }
    }
}
