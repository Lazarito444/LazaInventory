using System.Text.Json;
using System.Text.Json.Serialization;

namespace LazaInventory.Presentation.Api.Converters;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateString = reader.GetString()!;
        return DateOnly.ParseExact(dateString, "yyyy-MM-dd");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}