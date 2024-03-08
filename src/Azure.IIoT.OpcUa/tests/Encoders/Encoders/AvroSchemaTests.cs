﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Encoders
{
    using Azure.IIoT.OpcUa.Publisher.Models;
    using Furly.Extensions.Serializers;
    using Furly.Extensions.Serializers.Newtonsoft;
    using Opc.Ua;
    using System;
    using System.Linq;
    using System.Text.Json;
    using Xunit;

    public class AvroSchemaTests
    {
        [Fact]
        public void CreateSchema1Test()
        {
            var group = CreateModel();
            var schema = new AvroSchema(group.DataSetWriters[0],
                new ServiceMessageContext());

            var json = schema.ToString();
            var document = JsonDocument.Parse(json);
            json = JsonSerializer.Serialize(document, kIndented);
            Assert.NotNull(json);
        }

        private static readonly JsonSerializerOptions kIndented = new()
        {
            WriteIndented = true
        };

        private static WriterGroupModel CreateModel()
        {
            IJsonSerializer serializer = new NewtonsoftJsonSerializer();
            return serializer.Deserialize<WriterGroupModel>(
            """
            {
              "id": "da39a3ee5e6b4b0d3255bfef95601890afd80709",
              "messageType": "Json",
              "dataSetWriters": [
                {
                  "id": "cb0a88f5c176801edf4154608ec5abd8b6bab607",
                  "dataSet": {
                    "dataSetSource": {
                      "connection": {
                        "endpoint": {
                          "url": "opc.tcp://localhost:61457/UA/SampleServer",
                          "securityMode": "Best"
                        }
                      },
                      "subscriptionSettings": {},
                      "publishedVariables": {
                        "publishedData": [
                          {
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=Special_\"!§$%&/()=?`´\\+~*'#_-:.;,<>|@^°€µ{[]}",
                            "dataSetFieldName": "\"!§$%&/()=?`´\\+~*'#_-:.;,<>|@^°€µ{[]}",
                            "attribute": "Value",
                            "id": "9823977aebff2147c78aef72e70d759691002164",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 1,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=AlternatingBoolean",
                            "dataSetFieldName": "AlternatingBoolean",
                            "attribute": "Value",
                            "id": "afa0c915b3423c2c102a233f61025286e82a0b8f",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Alternating boolean value",
                              "builtInType": 1,
                              "dataType": "i=1",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 2,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadFastRandomUIntScalar1",
                            "dataSetFieldName": "BadFastRandomUIntScalar1",
                            "attribute": "Value",
                            "id": "06bd52e14188e70226ebf7737c28317c6fb90fc7",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 3,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadFastUIntScalar1",
                            "dataSetFieldName": "BadFastUIntScalar1",
                            "attribute": "Value",
                            "id": "e2e0ae049dd818264228e1ad15772aba6739d768",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 4,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadSlowRandomUIntScalar1",
                            "dataSetFieldName": "BadSlowRandomUIntScalar1",
                            "attribute": "Value",
                            "id": "d77f5659ff844ccbf3484065ead9daa85d48229d",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 5,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadSlowUIntScalar1",
                            "dataSetFieldName": "BadSlowUIntScalar1",
                            "attribute": "Value",
                            "id": "93f821f7117e619be95a96d36e3d9c89cde00040",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 6,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=DipData",
                            "dataSetFieldName": "DipData",
                            "attribute": "Value",
                            "id": "c941a7cc672033c42759b1c49aeec699a7034ea7",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Value with random dips",
                              "builtInType": 11,
                              "dataType": "i=11",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 7,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastRandomUIntScalar1",
                            "dataSetFieldName": "FastRandomUIntScalar1",
                            "attribute": "Value",
                            "id": "5cbf056223e7048dd4e51bd8f0147a9061df2c5d",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 8,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastRandomUIntScalar2",
                            "dataSetFieldName": "FastRandomUIntScalar2",
                            "attribute": "Value",
                            "id": "829313eacd41abed50272256b38528c958768432",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 9,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastRandomUIntScalar3",
                            "dataSetFieldName": "FastRandomUIntScalar3",
                            "attribute": "Value",
                            "id": "564cdea1f56b564a3f077865d7f6af473cfa3118",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 10,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastUIntScalar1",
                            "dataSetFieldName": "FastUIntScalar1",
                            "attribute": "Value",
                            "id": "41b361c0b073ac26613808d98d51648febbf372d",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 11,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastUIntScalar2",
                            "dataSetFieldName": "FastUIntScalar2",
                            "attribute": "Value",
                            "id": "6e1d61f8021a94a6a381e130c8ee3bb9c6f7e296",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 12,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastUIntScalar3",
                            "dataSetFieldName": "FastUIntScalar3",
                            "attribute": "Value",
                            "id": "85f716ac20b3fa35249738e1684b39942bba807b",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 13,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX",
                            "dataSetFieldName": "LongId3950",
                            "attribute": "Value",
                            "id": "775023945cfcf8879a1a11230ecd9e02106362b2",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 14,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString100kB",
                            "dataSetFieldName": "LongString100kB",
                            "attribute": "Value",
                            "id": "43fe61b2834f77f05da2f81f2e7792e3686bdb41",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Long string",
                              "builtInType": 15,
                              "dataType": "i=15",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 15,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString10kB",
                            "dataSetFieldName": "LongString10kB",
                            "attribute": "Value",
                            "id": "75c186c31da63f3f46d5caf73d72c056252b60ca",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Long string",
                              "builtInType": 12,
                              "dataType": "i=12",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 16,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString200kB",
                            "dataSetFieldName": "LongString200kB",
                            "attribute": "Value",
                            "id": "b309d3154d832b688f5fc1f77e44b66f9876ae27",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Long string",
                              "builtInType": 3,
                              "dataType": "i=3",
                              "valueRank": 1,
                              "arrayDimensions": [
                                0
                              ]
                            }
                          },
                          {
                            "fieldIndex": 17,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString50kB",
                            "dataSetFieldName": "LongString50kB",
                            "attribute": "Value",
                            "id": "4806fa2819eb47382831654bcc5cf1da41b27e61",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Long string",
                              "builtInType": 12,
                              "dataType": "i=12",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 18,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=NegativeTrendData",
                            "dataSetFieldName": "NegativeTrendData",
                            "attribute": "Value",
                            "id": "e799f63265f972ccd93e69ac746bad5cbb051d27",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Value with a slow negative trend",
                              "builtInType": 11,
                              "dataType": "i=11",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 19,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=PositiveTrendData",
                            "dataSetFieldName": "PositiveTrendData",
                            "attribute": "Value",
                            "id": "20f134510f1fcb939061fa3718dc32e894fe3cf6",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Value with a slow positive trend",
                              "builtInType": 11,
                              "dataType": "i=11",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 20,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=RandomSignedInt32",
                            "dataSetFieldName": "RandomSignedInt32",
                            "attribute": "Value",
                            "id": "c31f5403fe22c7cdaab570a1dc6e4db51abfa4c0",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Random signed 32 bit integer value",
                              "builtInType": 6,
                              "dataType": "i=6",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 21,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=RandomUnsignedInt32",
                            "dataSetFieldName": "RandomUnsignedInt32",
                            "attribute": "Value",
                            "id": "6bab2c4b1b621e072b5a9e041eb555ecdc6a0e03",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Random unsigned 32 bit integer value",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 22,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowRandomUIntScalar1",
                            "dataSetFieldName": "SlowRandomUIntScalar1",
                            "attribute": "Value",
                            "id": "80ed13e75d257da71e72abce8c127bf1ae258dea",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 23,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowRandomUIntScalar2",
                            "dataSetFieldName": "SlowRandomUIntScalar2",
                            "attribute": "Value",
                            "id": "8e83a3242d07010552b6cbc54f632dfcefff15a9",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 24,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowRandomUIntScalar3",
                            "dataSetFieldName": "SlowRandomUIntScalar3",
                            "attribute": "Value",
                            "id": "49981c7e6cb25ef5a9faf8a412fbb9259ec7068f",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 25,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowUIntScalar1",
                            "dataSetFieldName": "SlowUIntScalar1",
                            "attribute": "Value",
                            "id": "9ad72842c5a86e3eef4da4473f8aaa4cfc91fb51",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 26,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowUIntScalar2",
                            "dataSetFieldName": "SlowUIntScalar2",
                            "attribute": "Value",
                            "id": "7beba67f1be0e4fcf6571aefb0799b805e8e83bf",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 27,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowUIntScalar3",
                            "dataSetFieldName": "SlowUIntScalar3",
                            "attribute": "Value",
                            "id": "12bd9199eafce754f23b63f89a405e9d5fa1fe1a",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value(s)",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 28,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SpikeData",
                            "dataSetFieldName": "SpikeData",
                            "attribute": "Value",
                            "id": "6d4de3ab4d22c953af78baaba4745c7a65adac9f",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Value with random spikes",
                              "builtInType": 11,
                              "dataType": "i=11",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 29,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=StepUp",
                            "dataSetFieldName": "StepUp",
                            "attribute": "Value",
                            "id": "6184c571855dfa67741e79ffe80dae2dd6ed7a9e",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          },
                          {
                            "fieldIndex": 30,
                            "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=f24dabfd-ad9f-033a-55ea-425b1c2a2862",
                            "dataSetFieldName": "f24dabfd-ad9f-033a-55ea-425b1c2a2862",
                            "attribute": "Value",
                            "id": "1533e39295763c2b269bc6d68291b351570f3f5b",
                            "metaData": {
                              "minorVersion": 1,
                              "description": "Constantly increasing value",
                              "builtInType": 7,
                              "dataType": "i=7",
                              "valueRank": -1
                            }
                          }
                        ]
                      }
                    },
                    "dataSetMetaData": {
                      "majorVersion": 1
                    },
                    "sendKeepAlive": false,
                    "Routing": "None"
                  },
                  "dataSetFieldContentMask": "StatusCode, SourceTimestamp, NodeId, DisplayName, EndpointUrl",
                  "messageSettings": {
                    "dataSetMessageContentMask": "MetaDataVersion, MajorVersion, MinorVersion, MessageType, DataSetWriterName",
                    "namespaceFormat": "Expanded"
                  },
                  "dataSetWriterName": null,
                  "metaData": {
                    "queueName": ""
                  },
                  "publishing": {
                    "queueName": "DESKTOP-BGFJS91/messages/<<UnknownWriterGroup>>"
                  }
                },
                {
                  "id": "cb0a88f5c176801edf4154608ec5abd8b6bab607_1",
                  "dataSet": {
                    "name": "Telemetry",
                    "dataSetSource": {
                      "connection": {
                        "endpoint": {
                          "url": "opc.tcp://localhost:61457/UA/SampleServer",
                          "securityMode": "Best"
                        }
                      },
                      "subscriptionSettings": {},
                      "publishedObjects": {
                        "publishedData": [
                          {
                            "Id": "608deca1919bee3253d157094803e6a64fd6ef25",
                            "Name": "Telemetry",
                            "PublishedNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=Telemetry",
                            "BrowsePath": null,
                            "Template": {},
                            "PublishedVariables": {
                              "publishedData": [
                                {
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=Special_\"!§$%&/()=?`´\\+~*'#_-:.;,<>|@^°€µ{[]}",
                                  "dataSetFieldName": "\"!§$%&/()=?`´\\+~*'#_-:.;,<>|@^°€µ{[]}",
                                  "attribute": "Value",
                                  "id": "9823977aebff2147c78aef72e70d759691002164",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 1,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=AlternatingBoolean",
                                  "dataSetFieldName": "AlternatingBoolean",
                                  "attribute": "Value",
                                  "id": "afa0c915b3423c2c102a233f61025286e82a0b8f",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Alternating boolean value",
                                    "builtInType": 1,
                                    "dataType": "i=1",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 2,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadFastRandomUIntScalar1",
                                  "dataSetFieldName": "BadFastRandomUIntScalar1",
                                  "attribute": "Value",
                                  "id": "06bd52e14188e70226ebf7737c28317c6fb90fc7",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 3,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadFastUIntScalar1",
                                  "dataSetFieldName": "BadFastUIntScalar1",
                                  "attribute": "Value",
                                  "id": "e2e0ae049dd818264228e1ad15772aba6739d768",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 4,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadSlowRandomUIntScalar1",
                                  "dataSetFieldName": "BadSlowRandomUIntScalar1",
                                  "attribute": "Value",
                                  "id": "d77f5659ff844ccbf3484065ead9daa85d48229d",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 5,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=BadSlowUIntScalar1",
                                  "dataSetFieldName": "BadSlowUIntScalar1",
                                  "attribute": "Value",
                                  "id": "93f821f7117e619be95a96d36e3d9c89cde00040",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 6,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=DipData",
                                  "dataSetFieldName": "DipData",
                                  "attribute": "Value",
                                  "id": "c941a7cc672033c42759b1c49aeec699a7034ea7",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Value with random dips",
                                    "builtInType": 11,
                                    "dataType": "i=11",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 7,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastRandomUIntScalar1",
                                  "dataSetFieldName": "FastRandomUIntScalar1",
                                  "attribute": "Value",
                                  "id": "5cbf056223e7048dd4e51bd8f0147a9061df2c5d",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 8,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastRandomUIntScalar2",
                                  "dataSetFieldName": "FastRandomUIntScalar2",
                                  "attribute": "Value",
                                  "id": "829313eacd41abed50272256b38528c958768432",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 9,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastRandomUIntScalar3",
                                  "dataSetFieldName": "FastRandomUIntScalar3",
                                  "attribute": "Value",
                                  "id": "564cdea1f56b564a3f077865d7f6af473cfa3118",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 10,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastUIntScalar1",
                                  "dataSetFieldName": "FastUIntScalar1",
                                  "attribute": "Value",
                                  "id": "41b361c0b073ac26613808d98d51648febbf372d",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 11,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastUIntScalar2",
                                  "dataSetFieldName": "FastUIntScalar2",
                                  "attribute": "Value",
                                  "id": "6e1d61f8021a94a6a381e130c8ee3bb9c6f7e296",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 12,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=FastUIntScalar3",
                                  "dataSetFieldName": "FastUIntScalar3",
                                  "attribute": "Value",
                                  "id": "85f716ac20b3fa35249738e1684b39942bba807b",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 13,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWX",
                                  "dataSetFieldName": "LongId3950",
                                  "attribute": "Value",
                                  "id": "775023945cfcf8879a1a11230ecd9e02106362b2",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 14,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString100kB",
                                  "dataSetFieldName": "LongString100kB",
                                  "attribute": "Value",
                                  "id": "43fe61b2834f77f05da2f81f2e7792e3686bdb41",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Long string",
                                    "builtInType": 15,
                                    "dataType": "i=15",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 15,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString10kB",
                                  "dataSetFieldName": "LongString10kB",
                                  "attribute": "Value",
                                  "id": "75c186c31da63f3f46d5caf73d72c056252b60ca",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Long string",
                                    "builtInType": 12,
                                    "dataType": "i=12",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 16,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString200kB",
                                  "dataSetFieldName": "LongString200kB",
                                  "attribute": "Value",
                                  "id": "b309d3154d832b688f5fc1f77e44b66f9876ae27",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Long string",
                                    "builtInType": 3,
                                    "dataType": "i=3",
                                    "valueRank": 1,
                                    "arrayDimensions": [
                                      0
                                    ]
                                  }
                                },
                                {
                                  "fieldIndex": 17,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=LongString50kB",
                                  "dataSetFieldName": "LongString50kB",
                                  "attribute": "Value",
                                  "id": "4806fa2819eb47382831654bcc5cf1da41b27e61",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Long string",
                                    "builtInType": 12,
                                    "dataType": "i=12",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 18,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=NegativeTrendData",
                                  "dataSetFieldName": "NegativeTrendData",
                                  "attribute": "Value",
                                  "id": "e799f63265f972ccd93e69ac746bad5cbb051d27",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Value with a slow negative trend",
                                    "builtInType": 11,
                                    "dataType": "i=11",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 19,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=PositiveTrendData",
                                  "dataSetFieldName": "PositiveTrendData",
                                  "attribute": "Value",
                                  "id": "20f134510f1fcb939061fa3718dc32e894fe3cf6",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Value with a slow positive trend",
                                    "builtInType": 11,
                                    "dataType": "i=11",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 20,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=RandomSignedInt32",
                                  "dataSetFieldName": "RandomSignedInt32",
                                  "attribute": "Value",
                                  "id": "c31f5403fe22c7cdaab570a1dc6e4db51abfa4c0",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Random signed 32 bit integer value",
                                    "builtInType": 6,
                                    "dataType": "i=6",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 21,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=RandomUnsignedInt32",
                                  "dataSetFieldName": "RandomUnsignedInt32",
                                  "attribute": "Value",
                                  "id": "6bab2c4b1b621e072b5a9e041eb555ecdc6a0e03",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Random unsigned 32 bit integer value",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 22,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowRandomUIntScalar1",
                                  "dataSetFieldName": "SlowRandomUIntScalar1",
                                  "attribute": "Value",
                                  "id": "80ed13e75d257da71e72abce8c127bf1ae258dea",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 23,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowRandomUIntScalar2",
                                  "dataSetFieldName": "SlowRandomUIntScalar2",
                                  "attribute": "Value",
                                  "id": "8e83a3242d07010552b6cbc54f632dfcefff15a9",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 24,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowRandomUIntScalar3",
                                  "dataSetFieldName": "SlowRandomUIntScalar3",
                                  "attribute": "Value",
                                  "id": "49981c7e6cb25ef5a9faf8a412fbb9259ec7068f",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 25,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowUIntScalar1",
                                  "dataSetFieldName": "SlowUIntScalar1",
                                  "attribute": "Value",
                                  "id": "9ad72842c5a86e3eef4da4473f8aaa4cfc91fb51",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 26,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowUIntScalar2",
                                  "dataSetFieldName": "SlowUIntScalar2",
                                  "attribute": "Value",
                                  "id": "7beba67f1be0e4fcf6571aefb0799b805e8e83bf",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 27,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SlowUIntScalar3",
                                  "dataSetFieldName": "SlowUIntScalar3",
                                  "attribute": "Value",
                                  "id": "12bd9199eafce754f23b63f89a405e9d5fa1fe1a",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value(s)",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 28,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=SpikeData",
                                  "dataSetFieldName": "SpikeData",
                                  "attribute": "Value",
                                  "id": "6d4de3ab4d22c953af78baaba4745c7a65adac9f",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Value with random spikes",
                                    "builtInType": 11,
                                    "dataType": "i=11",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 29,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=StepUp",
                                  "dataSetFieldName": "StepUp",
                                  "attribute": "Value",
                                  "id": "6184c571855dfa67741e79ffe80dae2dd6ed7a9e",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                },
                                {
                                  "fieldIndex": 30,
                                  "publishedVariableNodeId": "nsu=http://opcfoundation.org/UA/Plc/Applications;s=f24dabfd-ad9f-033a-55ea-425b1c2a2862",
                                  "dataSetFieldName": "f24dabfd-ad9f-033a-55ea-425b1c2a2862",
                                  "attribute": "Value",
                                  "id": "1533e39295763c2b269bc6d68291b351570f3f5b",
                                  "metaData": {
                                    "minorVersion": 1,
                                    "description": "Constantly increasing value",
                                    "builtInType": 7,
                                    "dataType": "i=7",
                                    "valueRank": -1
                                  }
                                }
                              ]
                            },
                            "Flags": "recursive",
                            "State": null
                          }
                        ]
                      }
                    },
                    "dataSetMetaData": {
                      "majorVersion": 1
                    },
                    "sendKeepAlive": false,
                    "Routing": "None"
                  },
                  "dataSetFieldContentMask": "StatusCode, SourceTimestamp, NodeId, DisplayName, EndpointUrl",
                  "messageSettings": {
                    "dataSetMessageContentMask": "MetaDataVersion, MajorVersion, MinorVersion, MessageType, DataSetWriterName",
                    "namespaceFormat": "Expanded"
                  },
                  "dataSetWriterName": null,
                  "metaData": {
                    "queueName": ""
                  },
                  "publishing": {
                    "queueName": "DESKTOP-BGFJS91/messages/<<UnknownWriterGroup>>"
                  }
                }
              ],
              "messageSettings": {
                "networkMessageContentMask": "PublisherId, WriterGroupId, NetworkMessageNumber, SequenceNumber, PayloadHeader, Timestamp, DataSetClassId, NetworkMessageHeader, DataSetMessageHeader",
                "groupVersion": 1
              }
            }
            """)
            ;
        }
    }
}
