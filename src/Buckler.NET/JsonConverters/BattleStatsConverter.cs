using Buckler.NET.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Buckler.NET.JsonConverters
{
    public class BattleStatsConverter : JsonConverter<Battle>
    {
        public override Battle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);

            var root = document.RootElement;
            var battleStats = JsonSerializer.Deserialize<Battle>(root.GetRawText(), options);
            battleStats.SuperGaugeUsage = new SuperGaugeUsage();
            battleStats.DriveGaugeUsage = new DriveGaugeUsage();

            foreach (var property in root.EnumerateObject())
            {
                if (property.Name.Contains("gauge_rate"))
                {
                    var jsonProp = typeof(Battle).GetProperty(property.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);

                    if (property.Name.Contains("_sa_lv") || property.Name == "gauge_rate_ca")
                    {
                        jsonProp?.SetValue(battleStats.SuperGaugeUsage, property.Value);
                    }
                    else
                    {
                        jsonProp?.SetValue(battleStats.DriveGaugeUsage, property.Value);
                    }
                }
            }

            return battleStats;
        }

        public override void Write(Utf8JsonWriter writer, Battle value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
