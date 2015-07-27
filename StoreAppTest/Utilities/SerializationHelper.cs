
namespace StoreAppTest.Utilities
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public static class SerializationHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string SerializeObject<T>(T obj, Encoding enc)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings()
                {
                    // If set to true XmlWriter would close MemoryStream automatically and using would then do double dispose
                    // Code analysis does not understand that. That's why there is a suppress message.
                    CloseOutput = false,
                    Encoding = enc,
                    OmitXmlDeclaration = false,
                    Indent = true
                };
                using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(ms, xmlWriterSettings))
                {
                    XmlSerializer s = new XmlSerializer(typeof(T));
                    s.Serialize(xw, obj);
                }

                return enc.GetString(ms.ToArray(), 0, (int) ms.Length);
            }
        }
    }
}
