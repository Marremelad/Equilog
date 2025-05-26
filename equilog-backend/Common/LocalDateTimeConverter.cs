using System.Text.Json;
using System.Text.Json.Serialization;

namespace equilog_backend.Common;

// Custom JSON converter for DateTime objects to ensure a consistent serialization format.
public class LocalDateTimeConverter : JsonConverter<DateTime>
{
    // Deserializes JSON string value to DateTime object.
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Get the string value from the JSON reader.
        var value = reader.GetString();
        
        // Attempt to parse the string as a DateTime.
        if (DateTime.TryParse(value, out var dateTime))
        {
            return dateTime;
        }
        
        // Throw an exception if the string cannot be parsed as a valid DateTime.
        throw new JsonException($"Cannot parse {value} as DateTime");
    }

    // Serializes DateTime object to JSON string with a specific format.
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
    }
}