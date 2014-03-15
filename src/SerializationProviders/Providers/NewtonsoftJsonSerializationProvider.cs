using System;
using System.IO;
using Newtonsoft.Json;

namespace SerializationProviders.Providers
{
    /// <summary>
    /// The Newtonsoft.Json implementation
    /// </summary>
    public class NewtonsoftJsonSerializationProvider : SerializationProviderBase
    {
        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="destination">The destination.</param>
        public override void Serialize(Type type, object value, Stream destination)
        {
            string json = JsonConvert.SerializeObject(
                value,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var writer = new StreamWriter(destination);
            writer.Write(json);
            writer.Flush();
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the vlaue</typeparam>
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
        /// <returns>The deserialized instance</returns>
        public override T Deserialize<T>(Stream source)
        {
            var reader = new StreamReader(source);
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type to deserialize.</param>
        /// <param name="source">The source.</param>
        /// <returns>The deserialized instance</returns>
        public override object Deserialize(Type type, Stream source)
        {
            var reader = new StreamReader(source);
            return JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
        }
    }
}
