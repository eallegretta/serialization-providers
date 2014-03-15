using System;
using System.Configuration.Provider;
using System.IO;

namespace SerializationProviders
{
    /// <summary>
    /// Defines the contract for a serliazer
    /// </summary>
    public abstract class SerializationProviderBase: ProviderBase
    {
        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="destination">The destination.</param>
        public abstract void Serialize(Type type, object value, Stream destination);

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="destination">The destination.</param>
        public abstract void Serialize<T>(T value, Stream destination);
        
        /// <summary>
        /// Deserializes the specified source.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The deserialized object</returns>
        public abstract T Deserialize<T>(Stream source);

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="source">The source.</param>
        /// <returns>The deserialized object</returns>
        public abstract object Deserialize(Type type, Stream source);
    }
}
