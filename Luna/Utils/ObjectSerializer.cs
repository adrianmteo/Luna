using System.Xml;
using System.Xml.Serialization;

namespace Luna.Utils
{
    public static class ObjectSerializer
    {
        public static void SerializeObjectToFile<T>(string path, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (XmlWriter writer = XmlWriter.Create(path))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T DeserializeObjectFromFile<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (XmlReader reader = XmlReader.Create(path))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
