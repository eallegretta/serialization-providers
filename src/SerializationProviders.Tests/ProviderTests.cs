using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Xunit;

namespace SerializationProviders.Tests
{
    public class ProviderTests
    {
        [Fact]
        public void Should_serialize_type_with_nested_types()
        {
            var mock = new Mock { PropertyA = 1, Nested = new Mock.NestedMock { NestedPropertyA = 2 } };

            Mock binaryMock;
            Mock jsonMock;
            Mock bsonMock;
            Mock xmlMock;
            
            SerializeValidateAndDeserialize(mock, out binaryMock, out jsonMock, out bsonMock, out xmlMock);

            Assert.True(binaryMock.PropertyA == jsonMock.PropertyA 
                && binaryMock.PropertyA == bsonMock.PropertyA
                && binaryMock.PropertyA == xmlMock.PropertyA);
            
            Assert.True(binaryMock.Nested.NestedPropertyA == jsonMock.Nested.NestedPropertyA 
                && binaryMock.Nested.NestedPropertyA == bsonMock.Nested.NestedPropertyA 
                && binaryMock.Nested.NestedPropertyA == xmlMock.Nested.NestedPropertyA);
        }

        [Fact]
        public void Should_serialize_type_with_other_class_types_as_properties()
        {
            var mock = new Mock1 { Mock2 = new Mock2 { PropertyA = 1 } };

            Mock1 binaryMock;
            Mock1 jsonMock;
            Mock1 bsonMock;
            Mock1 xmlMock;

            SerializeValidateAndDeserialize(mock, out binaryMock, out jsonMock, out bsonMock, out xmlMock);


            Assert.True(binaryMock.Mock2.PropertyA == jsonMock.Mock2.PropertyA 
                && binaryMock.Mock2.PropertyA == bsonMock.Mock2.PropertyA
                && binaryMock.Mock2.PropertyA == xmlMock.Mock2.PropertyA);
        }

        [Fact]
        public void Should_serialize_circular_references_binary()
        {
            var mock = new MockWithCircularReferences { Name = "the root" };
            mock.Add(new MockWithCircularReferences.Child { ChildName = "first child" });
            mock.Add(new MockWithCircularReferences.Child { ChildName = "second child" });

            var type = typeof(MockWithCircularReferences);
            SerializationHelper.SerializeAsBase64String(type, mock, "binary");
            SerializationHelper.SerializeAsBase64String(type, mock, "json");
            SerializationHelper.SerializeAsBase64String(type, mock, "bson");
            SerializationHelper.SerializeAsBase64String(type, mock, "xml");
        }

        private static void SerializeValidateAndDeserialize<TMock>(TMock inputMock, out TMock binaryMock, out TMock jsonMock, out TMock bsonMock, out TMock xmlMock)
        {
            var binarySerialized = SerializationHelper.SerializeAsBase64String(typeof(TMock), inputMock, "binary");
            var jsonSerialized = SerializationHelper.SerializeAsBase64String(typeof(TMock), inputMock, "json");
            var bsonSerialized = SerializationHelper.SerializeAsBase64String(typeof(TMock), inputMock, "bson");
            var xmlSerialized = SerializationHelper.SerializeAsBase64String(typeof(TMock), inputMock, "xml");

            Assert.False(string.IsNullOrWhiteSpace(binarySerialized));
            Assert.False(string.IsNullOrWhiteSpace(jsonSerialized));
            Assert.False(string.IsNullOrWhiteSpace(bsonSerialized));
            Assert.False(string.IsNullOrWhiteSpace(xmlSerialized));

            Debug.WriteLine("Binary: " + binarySerialized);
            Debug.WriteLine("JSON: " + jsonSerialized);
            Debug.WriteLine("BSON: " + bsonSerialized);
            Debug.WriteLine("XML: " + xmlSerialized);

            binaryMock = SerializationHelper.DeserializeFromBase64String<TMock>(binarySerialized, "binary");
            jsonMock = SerializationHelper.DeserializeFromBase64String<TMock>(jsonSerialized, "json");
            bsonMock = SerializationHelper.DeserializeFromBase64String<TMock>(bsonSerialized, "bson");
            xmlMock = SerializationHelper.DeserializeFromBase64String<TMock>(xmlSerialized, "xml");
        }

        public class Mock
        {
            public int PropertyA { get; set; }

            public NestedMock Nested { get; set; }

            public class NestedMock
            {
                public int NestedPropertyA { get; set; }
            }
        }

        [XmlInclude(typeof(InheritedMock))]
        public class Mock1
        {
            public Mock2 Mock2 { get; set; }
        }

        public class Mock2
        {
            public int PropertyA { get; set; }
        }

        public class InheritedMock : Mock1
        {
            public string PropertyB { get; set; }
        }

        public class InheritedInheritedMock : InheritedMock
        {
            public string PropertyC { get; set; }
        }

        public class InheritedInheritedMock2 : InheritedMock
        {
            public string PropertyInMock2 { get; set; }
        }

        public class MockWithCircularReferences
        {
            public MockWithCircularReferences()
            {
                Children = new List<Child>();
            }

            public string Name { get; set; }
            public List<Child> Children { get; set; }

            public void Add(Child child)
            {
                Children.Add(child);
                child.Parent = this;
            }

            public class Child
            {
                public string ChildName { get; set; }
                [XmlIgnore]
                public MockWithCircularReferences Parent { get; set; }
            }
        }
    }
}
