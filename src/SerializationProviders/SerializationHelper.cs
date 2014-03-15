using System;
using System.IO;

namespace SerializationProviders
{
    /// <summary>
    /// Provides helper method for serialization and deserialization
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Serializes as string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The string representation of the value
        /// </returns>
        public static string SerializeAsString(Type type, object value, string providerName)
        {
            if (value == null)
                return null;

            using (var ms = new MemoryStream())
            {
                GetSerializationProvider(providerName).Serialize(type, value, ms);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Serializes as byte array.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The byte array representation of the value
        /// </returns>
        public static byte[] SerializeAsByteArray(Type type, object value, string providerName)
        {
            if (value == null)
                return null;

            using (var ms = new MemoryStream())
            {
                GetSerializationProvider(providerName).Serialize(type, value, ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Serializes as base64 string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The base64 string representation of the value
        /// </returns>
        public static string SerializeAsBase64String(Type type, object value, string providerName)
        {
            if (value == null)
                return null;

            using (var ms = new MemoryStream())
            {
                GetSerializationProvider(providerName).Serialize(type, value, ms);

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="serialized">The serialized data.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public static T DeserializeFromString<T>(string serialized, string providerName)
        {
            if (string.IsNullOrWhiteSpace(serialized))
                return default(T);

            using (var ms = new MemoryStream())
            {
                var sr = new StreamWriter(ms);
                sr.Write(serialized);
                ms.Position = 0;
                return GetSerializationProvider(providerName).Deserialize<T>(ms);
            }
        }

        /// <summary>
        /// Deserializes from byte array.
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="serialized">The serialized data.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public static T DeserializeFromByteArray<T>(byte[] serialized, string providerName)
        {
            if (serialized == null)
                return default(T);

            using (var ms = new MemoryStream(serialized))
            {
                return GetSerializationProvider(providerName).Deserialize<T>(ms);
            }
        }

        /// <summary>
        /// Deserializes from base64 string.
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="serialized">The serialized data.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public static T DeserializeFromBase64String<T>(string serialized, string providerName)
        {
            if (string.IsNullOrWhiteSpace(serialized))
                return default(T);

            var byteAfter64 = Convert.FromBase64String(serialized);

            using (var ms = new MemoryStream(byteAfter64))
            {
                ms.Position = 0;
                return GetSerializationProvider(providerName).Deserialize<T>(ms);
            }
        }

        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <param name="type">The type to deserialize.</param>
        /// <param name="serialized">The serialized data.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public static object DeserializeFromString(Type type, string serialized, string providerName)
        {
            if (string.IsNullOrWhiteSpace(serialized))
                return null;

            using (var ms = new MemoryStream())
            {
                var sr = new StreamWriter(ms);
                sr.Write(serialized);
                ms.Position = 0;
                return GetSerializationProvider(providerName).Deserialize(type, ms);
            }
        }

        /// <summary>
        /// Deserializes from byte array.
        /// </summary>
        /// <param name="type">The type to deserialize.</param>
        /// <param name="serialized">The serialized data.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public static object DeserializeFromByteArray(Type type, byte[] serialized, string providerName)
        {
            if (serialized == null)
                return null;

            using (var ms = new MemoryStream(serialized))
            {
                return GetSerializationProvider(providerName).Deserialize(type, ms);
            }
        }

        /// <summary>
        /// Deserializes from a base64 string.
        /// </summary>
        /// <param name="type">The type to deserialize.</param>
        /// <param name="serialized">The serialized data.</param>
        /// <param name="providerName">Name of the serialization provider.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public static object DeserializeFromBase64String(Type type, string serialized, string providerName)
        {
            if (string.IsNullOrWhiteSpace(serialized))
                return null;

            var byteAfter64 = Convert.FromBase64String(serialized);

            using (var ms = new MemoryStream(byteAfter64))
            {
                ms.Position = 0;
                return GetSerializationProvider(providerName).Deserialize(type, ms);
            }
        }

        private static SerializationProviderBase GetSerializationProvider(string providerName)
        {
            return SerializationProviderFactory.Default.GetProvider(providerName);
        }
    }
}
