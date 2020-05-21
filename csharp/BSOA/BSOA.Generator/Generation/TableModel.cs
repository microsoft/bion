using BSOA.Generator.Schema;
using System;
using System.IO;
using System.Text;

namespace BSOA.Generator.Generation
{
    /// <summary>
    ///  Generator for Table classes.
    /// </summary>
    /// <remarks>
    ///  Built on TeamTable.cs, this code uses the known:
    ///   - Type name
    ///   - DB type name
    ///   - Column names
    ///   - Column types
    ///   - Namespace
    /// </remarks>
    public class TableModel
    {
        public static string Code;

        public static string SimpleColumnMember;
        public static string EnumColumnMember;
        public static string RefColumnMember;
        public static string RefListColumnMember;

        public static string SimpleColumnConstructor;
        public static string EnumColumnConstructor;
        public static string RefColumnConstructor;
        public static string RefListColumnConstructor;

        static TableModel()
        {
            Code = File.ReadAllText(@"Templates\\TeamTable.cs");

            SimpleColumnMember = CodeSection.Extract(Code, nameof(SimpleColumnMember));
            EnumColumnMember = CodeSection.Extract(Code, nameof(EnumColumnMember));
            RefColumnMember = CodeSection.Extract(Code, nameof(RefColumnMember));
            RefListColumnMember = CodeSection.Extract(Code, nameof(RefListColumnMember));

            SimpleColumnConstructor = CodeSection.Extract(Code, nameof(SimpleColumnConstructor));
            EnumColumnConstructor = CodeSection.Extract(Code, nameof(EnumColumnConstructor));
            RefColumnConstructor = CodeSection.Extract(Code, nameof(RefColumnConstructor));
            RefListColumnConstructor = CodeSection.Extract(Code, nameof(RefListColumnConstructor));
        }

        public static string Generate(Table table, Database database)
        {
            StringBuilder members = new StringBuilder();
            StringBuilder constructors = new StringBuilder();

            foreach (Schema.Column column in table)
            {
                members.Append(ColumnMember(column));
                constructors.Append(ColumnConstructor(column, database));
            }

            // Empty line between column properties and constructor
            members.AppendLine();

            string resultCode = Code
                .Replace("Team", table.Name)
                .Replace("CompanyDatabase", database.Name)
                .Replace("BSOA.Generator.Templates", database.Namespace);

            resultCode = CodeSection.Replace(resultCode, "ColumnMembers", members.ToString());
            resultCode = CodeSection.Replace(resultCode, "ColumnConstructors", constructors.ToString());

            return resultCode;
        }

        public static string ColumnMember(Schema.Column column)
        {
            switch (column.Category)
            {
                case ColumnTypeCategory.Simple:
                    return SimpleColumnMember
                        .Replace("WhenFormed", column.Name)
                        .Replace("DateTime", column.Type);
                case ColumnTypeCategory.Enum:
                    return EnumColumnMember
                        .Replace("JoinPolicy", column.Name)
                        .Replace("byte", column.UnderlyingType);
                case ColumnTypeCategory.Ref:
                    return RefColumnMember
                        .Replace("Manager", column.Name);
                case ColumnTypeCategory.RefList:
                    return RefListColumnMember
                        .Replace("Members", column.Name);
                default:
                    throw new NotImplementedException($"{nameof(TableModel)} unable to generate ColumnMember of Category {column.Category} ({column.Name})");
            }
        }

        public static string ColumnConstructor(Schema.Column column, Database database)
        {
            switch (column.Category)
            {
                case ColumnTypeCategory.Simple:
                    return SimpleColumnConstructor
                        .Replace("WhenFormed", column.Name)
                        .Replace("DateTime.MinValue", column.Default)
                        .Replace("DateTime", column.Type);
                case ColumnTypeCategory.Enum:
                    return EnumColumnConstructor
                        .Replace("JoinPolicy", column.Name)
                        .Replace("SecurityPolicy.Open", column.Default)
                        .Replace("SecurityPolicy", column.Type)
                        .Replace("byte", column.UnderlyingType);
                case ColumnTypeCategory.Ref:
                    return RefColumnConstructor
                        .Replace("Manager", column.Name)
                        .Replace("Employee", column.Type)
                        .Replace("CompanyDatabase", database.Name);
                case ColumnTypeCategory.RefList:
                    return RefListColumnConstructor
                        .Replace("Members", column.Name)
                        .Replace("Employee", column.Type)
                        .Replace("CompanyDatabase", database.Name);
                default:
                    throw new NotImplementedException($"{nameof(TableModel)} unable to generate ColumnConstructor of Category {column.Category} ({column.Name})");
            }
        }
    }
}
