using System.Xml.Linq;
using System.Xml.Serialization;

namespace Isogeo.Models.Configuration
{
    public static class SerializationUtil
    {
        public static T Deserialize<T>(XDocument doc)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var reader = doc.Root.CreateReader();
            return (T)xmlSerializer.Deserialize(reader);
        }

        public static XDocument Serialize<T>(T value)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var doc = new XDocument();

            using var writer = doc.CreateWriter();
            xmlSerializer.Serialize(writer, value);

            return doc;
        }
    }
}