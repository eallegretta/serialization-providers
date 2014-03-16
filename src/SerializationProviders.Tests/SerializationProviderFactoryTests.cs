using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerializationProviders.Providers;
using Xunit;

namespace SerializationProviders.Tests
{
    public class SerializationProviderFactoryTests
    {
        [Fact]
        public void Should_validate_providers_exist_when_no_configuration_is_set()
        {
            var factory = new SerializationProviderFactory();

            var providers = factory.GetProviders();

            Assert.NotEmpty(providers);

            Assert.Equal("json", factory.GetDefaultProvider().Name);
            Assert.Equal("json", providers[0].Name);
            Assert.Equal("xml", providers[1].Name);
            Assert.Equal("bson", providers[2].Name);
            Assert.Equal("binary", providers[3].Name);


            Assert.IsType<NewtonsoftJsonSerializationProvider>(providers[0]);
            Assert.IsType<DotNetXmlSerializationProvider>(providers[1]);
            Assert.IsType<NewtonsoftBsonSerializationProvider>(providers[2]);
            Assert.IsType<ProtobufSerializationProvider>(providers[3]);
        }
    }
}
