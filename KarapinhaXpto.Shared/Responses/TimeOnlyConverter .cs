using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.Responses
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {

        private const string TimeFormat = "HH:mm:ss"; // Você pode ajustar o formato conforme necessário

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string timeString = reader.GetString();
            return TimeOnly.ParseExact(timeString, TimeFormat);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeFormat));
        }

    }

    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy-MM-dd"; // Você pode ajustar o formato conforme necessário

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            return DateOnly.ParseExact(dateString, DateFormat);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}

