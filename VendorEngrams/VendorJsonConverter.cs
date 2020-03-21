using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VendorEngrams
{
    class VendorJsonConverter : JsonConverter<Vendor>
    {
        private static IDictionary<string, Action<Vendor, string>> _converters =
            new Dictionary<string, Action<Vendor, string>>
            {
                ["vendorID"] = (vendor, value) => vendor.Hash = uint.Parse(value),
                ["display"] = (vendor, value) => vendor.Display = int.Parse(value) != 0,
                ["drop"] = (vendor, value) => vendor.Drop = (DropStatus)int.Parse(value),
                ["shorthand"] = (vendor, value) => vendor.Shorthand = value,
                ["interval"] = (vendor, value) => vendor.Interval = uint.Parse(value),
                ["nextRefresh"] = (vendor, value) => vendor.NextRefresh = DateTime.Parse(value)
            };
        
        public override Vendor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var vendor = new Vendor();

            if(reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidOperationException($"Unexpectd TokenType. Expected JsonTokenType.StartObject, got {reader.TokenType}");
            }

            while(reader.Read())
            {
                if(reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if(reader.TokenType == JsonTokenType.PropertyName)
                {
                    var name = reader.GetString();
                    if(!_converters.TryGetValue(name, out var converter))
                    {
                        continue;
                    }

                    reader.Read();

                    var value = reader.GetString();
                    converter(vendor, value);
                }
            }

            return vendor;
        }

        public override void Write(Utf8JsonWriter writer, Vendor value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}