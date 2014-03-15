using System;
using System.Collections.Generic;
using ProviderModel;
using SerializationProviders.Providers;

namespace SerializationProviders
{
    /// <summary>
    /// The default serializer factory
    /// </summary>
    public class SerializationProviderFactory : ProviderFactory<SerializationProviderBase>
    {
        /// <summary>
        /// The default implementation of the SerializationProvider
        /// </summary>
        public static readonly SerializationProviderFactory Default = new SerializationProviderFactory();

        /// <summary>
        /// Gets the name of the configuration section.
        /// </summary>
        /// <value>
        /// The name of the configuration section.
        /// </value>
        protected override string ConfigurationSectionName
        {
            get { return "serialization-providers"; }
        }

        /// <summary>
        /// Gets the default providers without configuration.
        /// </summary>
        /// <returns>A list of serialization providers when no configuration is set</returns>
        protected override IEnumerable<KeyValuePair<string, Lazy<SerializationProviderBase>>> GetDefaultProvidersWithoutConfiguration()
        {
            yield return new KeyValuePair<string, Lazy<SerializationProviderBase>>("json", new Lazy<SerializationProviderBase>(() => new NewtonsoftJsonSerializationProvider()));
            yield return new KeyValuePair<string, Lazy<SerializationProviderBase>>("xml", new Lazy<SerializationProviderBase>(() => new DotNetXmlSerializationProvider()));
            yield return new KeyValuePair<string, Lazy<SerializationProviderBase>>("bson", new Lazy<SerializationProviderBase>(() => new NewtonsoftBsonSerializationProvider()));
            yield return new KeyValuePair<string, Lazy<SerializationProviderBase>>("binary", new Lazy<SerializationProviderBase>(() => new DotNetXmlSerializationProvider()));
        }
    }
}
