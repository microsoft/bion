using BSOA.Generator.Schema;
using System;
using System.IO;
using System.Text;

namespace BSOA.Generator.Generation
{
    /// <summary>
    ///  Generator for Entity classes.
    /// </summary>
    /// <remarks>
    ///  Built on Team.cs, this code uses the known:
    ///   - Type name
    ///   - DB type name
    ///   - Column names
    ///   - Column types
    ///   - Namespace
    /// </remarks>
    public class EntityModel
    {
        public static string Code;
        public static string SimpleColumn;
        public static string RefColumn;
        public static string RefListColumn;

        static EntityModel()
        {
            Code = File.ReadAllText(@"Templates\\Team.cs");
            SimpleColumn = CodeSection.Extract(Code, "SimpleColumn");
            RefColumn = CodeSection.Extract(Code, "RefColumn");
            RefListColumn = CodeSection.Extract(Code, "RefListColumn");
        }

        public static string Generate(Table table, Database database)
        {
            StringBuilder properties = new StringBuilder();
            foreach (Schema.Column column in table)
            {
                properties.AppendLine(ColumnProperty(column));
            }

            string renamedCode = Code
                .Replace("Team", table.Name)
                .Replace("CompanyDatabase", database.Name)
                .Replace("BSOA.Generator.Templates", database.Namespace);

            string finalCode = CodeSection.Replace(renamedCode, "Columns", properties.ToString());

            return finalCode;
        }

        public static string ColumnProperty(Schema.Column column)
        {
            switch (column.Category)
            {
                case ColumnTypeCategory.Simple:
                    return SimpleColumn
                        .Replace("WhenFormed", column.Name)
                        .Replace("DateTime", column.Type);
                case ColumnTypeCategory.Ref:
                    return RefColumn
                        .Replace("Manager", column.Name)
                        .Replace("Employee", column.Type);
                case ColumnTypeCategory.RefList:
                    return RefListColumn
                        .Replace("Members", column.Name)
                        .Replace("Employee", column.Type);
                default:
                    throw new NotImplementedException($"{nameof(EntityModel)} unable to generate Column of Category {column.Category} ({column.Name})");
            }
        }
    }
}
