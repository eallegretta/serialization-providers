using System;
using System.IO;
using System.Xml.Serialization;

namespace SerializationProviders.Providers
{
    /// <summary>
    /// The System.Xml.Serialization.XmlSerializer implementation
    /// </summary>
    public class DotNetXmlSerializationProvider : SerializationProviderBase
    {
        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="destination">The destination.</param>
        public override void Serialize(Type type, object value, Stream destination)
        {
            if (value == null)
                return;

            var serializer = new XmlSerializer(type);

            serializer.Serialize(destination, value);
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="destination">The destination.</param>
        public override void Serialize<T>(T value, Stream destination)
        {
            Serialize(typeof(T), value, destination);
        }

        /// <summary>
        /// Deserializes the specified source.
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The deserialized object</returns>
        public override T Deserialize<T>(Stream source)
        {
            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(source);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public override object Deserialize(Type type, Stream source)
        {
            var serializer = new XmlSerializer(type);

            return serializer.Deserialize(source);
        }
    }
}
