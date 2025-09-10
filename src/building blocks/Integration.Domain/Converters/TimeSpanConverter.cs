using System.Text.Json;
using System.Text.Json.Serialization;

namespace Integration.Domain.Converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (TimeSpan.TryParse(value, out var timeSpan))
                    return timeSpan;
                throw new JsonException($"Unable to convert \"{value}\" to TimeSpan.");
            }
            
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                long? ticks = null;
                
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                        break;
                        
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var propertyName = reader.GetString();
                        reader.Read();
                        
                        if (propertyName?.Equals("ticks", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            ticks = reader.GetInt64();
                        }
                    }
                }
                
                if (ticks.HasValue)
                    return new TimeSpan(ticks.Value);
            }
            
            if (reader.TokenType == JsonTokenType.Number)
            {
                var ticks = reader.GetInt64();
                return new TimeSpan(ticks);
            }
            
            throw new JsonException($"Unable to convert token type {reader.TokenType} to TimeSpan.");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"hh\:mm\:ss"));
        }
    }
}
