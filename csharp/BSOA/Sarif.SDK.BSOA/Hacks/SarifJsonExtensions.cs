using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Newtonsoft.Json
{
    public static class SerializedPropertyInfoJsonExtensions
    {
        public static SerializedPropertyInfo ReadSerializedPropertyInfo<TRoot>(this JsonReader reader, TRoot root)
        {
            return (SerializedPropertyInfo)SerializedPropertyInfoConverter.Instance.ReadJson(reader, typeof(SerializedPropertyInfo), null, null);
        }

        public static void Write(this JsonWriter writer, string propertyName, SerializedPropertyInfo item, SerializedPropertyInfo defaultValue = null)
        {
            SerializedPropertyInfoConverter.Instance.WriteJson(writer, item, null);
        }
    }

    public static class SarifJsonExtensions
    {
        //public static SarifVersion ReadSarifVersion<TRoot>(this JsonReader reader, TRoot root)
        //{
        //    return (SarifVersion)SarifVersionConverter.Instance.ReadJson(reader, typeof(SarifVersion), null, null);
        //}
    }
}
