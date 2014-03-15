using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

namespace SerializationProviders.Providers
{
    /// <summary>
    /// Protobuf serialization provider for fast binary serialization and deserialization
    /// </summary>
    public class ProtobufSerializationProvider : SerializationProviderBase
    {
        private static object _protobufMappingSyncRoot = new object();
        private static IDictionary<Type, MetaType> _protobufMappedTypes = new Dictionary<Type, MetaType>();
        private static IDictionary<Type, int> _protobufMappedTypesFieldNumber = new Dictionary<Type, int>();

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

            EnsureSerialization(type);
            Serializer.NonGeneric.Serialize(destination, value);
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
            EnsureSerialization(typeof(T));
            return Serializer.Deserialize<T>(source);
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
            EnsureSerialization(type);
            return Serializer.NonGeneric.Deserialize(type, source);
        }

        /// <summary>
        /// Creates a protobuf-net mapping for type if it's not serializable yet.
        /// Ensures types of all public properties have mappings too.
        /// </summary>
        /// <param name="type">the type that will be used when de/serializing</param>
        private static void EnsureSerialization(Type type)
        {
            if (!_protobufMappedTypes.ContainsKey(type))
            {
                lock (_protobufMappingSyncRoot)
                {
                    EnsureSerializationRecursive(type);
                }
            }
        }

        private static MetaType EnsureSerializationRecursive(Type type)
        {
            MetaType metaType = null;
            if (_protobufMappedTypes.TryGetValue(type, out metaType))
            {
                return metaType;
            }

            if (type.IsArray)
            {
                return EnsureSerializationRecursive(type.GetElementType());
            }

            if (type.IsGenericType)
            {
                foreach (var subtype in type.GetGenericArguments())
                {
                    EnsureSerializationRecursive(subtype);
                }
            }

            if (!RuntimeTypeModel.Default.CanSerialize(type))
            {
                // type is not configured for protobuf serialization
                // create a mapping automatically
                metaType = RuntimeTypeModel.Default.Add(type, true);
                AddMappingsForAllPublicProperties(type, metaType);

                // if base class is in same assembly, ensure its mapped first
                if (type.BaseType != null && type.BaseType.Assembly == type.Assembly && !_protobufMappedTypes.ContainsKey(type.BaseType))
                {
                    EnsureSerializationRecursive(type.BaseType);
                }

                // map all children classes
                var subtypes = type.Assembly.GetTypes().Where(t => t.BaseType == type);
                int fieldNumber = _protobufMappedTypesFieldNumber[type];
                foreach (var subtype in subtypes)
                {
                    EnsureSerializationRecursive(subtype);
                    metaType.AddSubType(fieldNumber, subtype);
                    fieldNumber++;
                }
            }

            return _protobufMappedTypes[type] = metaType;
        }

        private static void AddMappingsForAllPublicProperties(Type type, MetaType metaType)
        {
            int fieldNumber = 1;
            foreach (var property in TypeDescriptor.GetProperties(type).OfType<PropertyDescriptor>())
            {
                if (!property.IsReadOnly)
                {
                    var propertyType = property.PropertyType;
                    var field = metaType.AddField(fieldNumber, property.Name);
                    fieldNumber++;
                    if (!propertyType.IsValueType && propertyType != typeof(string) &&
                        !propertyType.IsArray && !typeof(IEnumerable).IsAssignableFrom(propertyType))
                    {
                        field.AsReference = true;
                    }

                    // recursively check nested property types
                    EnsureSerializationRecursive(propertyType);
                }
            }

            _protobufMappedTypesFieldNumber[type] = fieldNumber;
        }
    }
}
