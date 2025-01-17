﻿using System;
using System.IO;
#if !WINDOWS_PHONE && !NETSTANDARD
using System.Runtime.Serialization;
#endif
using System.Runtime.Serialization.Json;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Serializers
{
    public class JsonSerializer : TextSerializer, IJsonSerializer
    {
#if !WINDOWS_PHONE && !NETSTANDARD && !WINDOWS_UWP
        protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
        {
            return new DataContractJsonSerializer(type, GetKnownTypes());
        }
#else
        private DataContractJsonSerializer CreateDataContractJsonSerializer(Type type)
        {
            return new DataContractJsonSerializer(type, GetKnownTypes());
        }

        public override void Serialize<T>(Stream stream, T obj)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var serializer = CreateDataContractJsonSerializer(typeof(T));
            serializer.WriteObject(stream, obj);
        }

        public override T Deserialize<T>(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var serializer = CreateDataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }
#endif
    }
}