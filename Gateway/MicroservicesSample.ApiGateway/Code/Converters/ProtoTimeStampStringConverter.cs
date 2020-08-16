using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf.WellKnownTypes;
using Type = System.Type;

namespace MicroservicesSample.ApiGateway.Code.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class ProtoTimeStampStringConverter : JsonConverter<Timestamp>
    {
        /// <inheritdoc />
        public override Timestamp Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Timestamp value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteStringValue(string.Empty);
            }
            else
            {
                var res = value.ToDateTime();
                writer.WriteStringValue(res);
            }
        }
    }
}
