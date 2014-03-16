using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace SerializationProviders.Providers
{
    /// <summary>
    /// The Newtonsoft.Json Bson implementation
    /// </summary>
    public class NewtonsoftBsonSerializationProvider: SerializationProviderBase
    {
        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="destination">The destination.</param>
        public override void Serialize(Type type, object value, Stream destination)
        {
            var serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var bsonWriter = new BsonWriter(destination);
            serializer.Serialize(bsonWriter, value, type);
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
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public override T Deserialize<T>(Stream source)
        {
            var serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var bsonReader = new BsonReader(source);
            return serializer.Deserialize<T>(bsonReader);
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
            var serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var bsonReader = new BsonReader(source);
            return serializer.Deserialize(bsonReader);
        }
    }
}
