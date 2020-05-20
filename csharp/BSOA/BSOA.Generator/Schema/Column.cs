namespace BSOA.Generator.Schema
{
    public class Column
    {
        public ColumnTypeCategory Category { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }

        private Column(ColumnTypeCategory category, string name, string type, string defaultValue)
        {
            Category = category;
            Name = name;
            Type = type;
            Default = defaultValue;
        }

        public static Column Simple(string name, string type, string defaultValue = null)
        {
            return new Column(ColumnTypeCategory.Simple, name, type, defaultValue);
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
