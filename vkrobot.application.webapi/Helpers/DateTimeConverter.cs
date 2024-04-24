using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace vkrobot.application.webapi.Helpers
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            string? dateTimeString = reader.GetString();
            DateTimeOffset value = DateTimeOffset.Parse(dateTimeString ?? string.Empty);
            return value.DateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            string dateTimeString = value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            writer.WriteStringValue(dateTimeString);
        }
    }
}
