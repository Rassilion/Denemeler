using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ForgeBot
{
	/// <summary>
	/// XML Serialization helper class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class XmlSerializerHelper<T>
	{
		public Type _type;

		public XmlSerializerHelper()
		{
			_type = typeof(T);
		}


		public string Save(T obj)
		{
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");

			XmlWriterSettings writerSettings = new XmlWriterSettings();
			writerSettings.OmitXmlDeclaration = true;
			writerSettings.Indent = false;
			StringBuilder sb = new StringBuilder();
			using (TextWriter textWriter = new StringWriter(sb))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, writerSettings))
				{
					XmlSerializer serializer = new XmlSerializer(_type);
					serializer.Serialize(xmlWriter, obj, ns);
				}
			}
			return sb.ToString();
		}

        public Stream SaveToStream(T obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            XmlSerializer xs = new XmlSerializer(_type);
            xs.Serialize(xmlTextWriter, obj);
            return memoryStream;

        }

	    public T Read(string message)
		{
			T result;
			using (TextReader textReader = new StringReader(message))
			{
				XmlSerializer deserializer = new XmlSerializer(_type);
				result = (T)deserializer.Deserialize(textReader);
			}
			return result;
		}

        public T Read(Stream stream)
        {
            T result;
            using (TextReader textReader = new StreamReader(stream))
            {
                XmlSerializer deserializer = new XmlSerializer(_type);
                result = (T)deserializer.Deserialize(textReader);
            }
            return result;
        }
	}


}
