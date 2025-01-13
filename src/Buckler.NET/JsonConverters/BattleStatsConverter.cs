using Buckler.NET.Models;
using System.Reflection;
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

            var superGaugeJsonPropNames = typeof(SuperGaugeUsage).GetProperties().Select(prop => new { PropertyName = prop.Name, JsonAttributeName = prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name });
            var driveGaugeJsonPropNames = typeof(DriveGaugeUsage).GetProperties().Select(prop => new { PropertyName = prop.Name, JsonAttributeName = prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name });

            foreach (var property in root.EnumerateObject())
            {
                if (property.Name.Contains("gauge_rate") && !property.Name.Contains("drive_other"))
                {

                    if (property.Name.Contains("_sa_lv") || property.Name == "gauge_rate_ca")
                    {
                        var dtoPropName = superGaugeJsonPropNames.Where(x => x.JsonAttributeName == property.Name).Select(y => y.PropertyName).FirstOrDefault();
                        var jsonProp = typeof(SuperGaugeUsage).GetProperty(dtoPropName);
                        jsonProp?.SetValue(battleStats.SuperGaugeUsage, property.Value.GetDouble());
                    }
                    else
                    {
                        var dtoPropName = driveGaugeJsonPropNames.Where(x => x.JsonAttributeName == property.Name).Select(y => y.PropertyName).FirstOrDefault();
                        var jsonProp = typeof(DriveGaugeUsage).GetProperty(dtoPropName);
                        jsonProp?.SetValue(battleStats.DriveGaugeUsage, property.Value.GetDouble());
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
