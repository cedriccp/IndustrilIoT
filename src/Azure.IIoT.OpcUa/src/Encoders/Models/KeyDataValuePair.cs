﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Encoders.Models
{
    using Opc.Ua;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Encodable Key DataValue Pair
    /// </summary>
    public class KeyDataValuePair : IEncodeable
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public KeyDataValuePair() { }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public KeyDataValuePair(string key, DataValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "Key", IsRequired = true, Order = 1)]
        public string Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [DataMember(Name = "Value", IsRequired = true, Order = 2)]
        public DataValue Value { get; set; }

        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId { get; }

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId { get; }

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId { get; }

        /// <param name="encoder"></param>
        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.WriteString("Key", Key);
            encoder.WriteDataValue(Key, Value);
        }

        /// <param name="decoder"></param>
        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            Key = decoder.ReadString("Key");
            Value = decoder.ReadDataValue(Key);
        }

        /// <param name="encodeable"></param>
        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (ReferenceEquals(this, encodeable))
            {
                return true;
            }
            if (encodeable is not KeyDataValuePair value)
            {
                return false;
            }
            if (!Utils.IsEqual(Key, value.Key)) return false;
            if (!Utils.IsEqual(Value, value.Value)) return false;
            return true;
        }

        /// <summary cref="object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            var clone = (KeyDataValuePair)base.MemberwiseClone();
            clone.Key = (string)Utils.Clone(Key);
            clone.Value = (DataValue)Utils.Clone(Value);
            return clone;
        }
    }

    /// <summary>
    /// A collection of KeyDataValuePair objects.
    /// </summary>
    public partial class KeyDataValuePairCollection : List<KeyDataValuePair>
    {
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public KeyDataValuePairCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public KeyDataValuePairCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        /// <param name="collection"></param>
        public KeyDataValuePairCollection(IEnumerable<KeyDataValuePair> collection) : base(collection) { }
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        /// <param name="values"></param>
        public static implicit operator KeyDataValuePairCollection(KeyDataValuePair[] values)
        {
            if (values != null)
            {
                return new KeyDataValuePairCollection(values);
            }
            return new KeyDataValuePairCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        /// <param name="values"></param>
        public static explicit operator KeyDataValuePair[](KeyDataValuePairCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }
            return null;
        }

        /// <summary cref="object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            var clone = new KeyDataValuePairCollection(Count);
            for (var ii = 0; ii < Count; ii++)
            {
                clone.Add((KeyDataValuePair)Utils.Clone(this[ii]));
            }
            return clone;
        }
    }
}
