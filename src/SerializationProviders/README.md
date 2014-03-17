#Serialization Providers

This library provides different bundled serialization providers using the [ProviderModel](http://github.com/eallegretta/providermodel) library

## Bundled serializers

The library provides four different bunled providers

* JSON: Uses the mighty [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) library
* BSON: Uses the mighty [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) library
* XML: Uses the .NET System.Xml.XmlSerializer
* Binary: Uses the blazing fast and powerful [protobuf-net](https://code.google.com/p/protobuf-net/) library

## Configuration

Since we rely on the power of the ProviderModel library you can use this library without any configuraiton at all, because we provide the four providers out of the box when no configuration is set.

In case you want to configure the available providers just do the following

Add the section to the configSections tag in the configuration file

    <section name="serialization-providers" type="ProviderModel.Configuration.ProviderSectionHandler, ProviderModel"></section>

And then add the serialization-providers section to the configuration file

    <serialization-providers defaultProvider="json">
        <add name="json" type="SerializationProviders.Providers.NewtonsoftJsonSerializationProvider, SerializationProviders" />
        <add name="binary" type="SerializationProviders.Providers.ProtobufSerializationProvider, SerializationProviders" />
    </serialization-providers>

## Usage

You can use the providers using the SerializationProviderFactory as in:

    SerializationProviderFactory.Instance.GetProvider("json").Serialize(typeof(int), 2, fileStream);

But we know that doing that of managing streams can be annoying so we provided a really helpful class called SerializationHelper that provides out of the box serialization as string, byte array and base64.

    SerializationHelper.SerializeAsString(typeof(int), 2, "json");
    SerializationHelper.SerializeAsByteArray(typeof(int), 2, "binary");
    SerializationHelper.SerializeAsBase64String(typeof(int), 2, "xml");
