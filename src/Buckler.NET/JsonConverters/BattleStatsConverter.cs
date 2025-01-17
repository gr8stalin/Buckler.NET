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
            using var document = JsonDocument.ParseValue(ref reader);

            var rootElement = document.RootElement;
            var battleStats = BuildBattleStatsContainer(rootElement, options);

            foreach (var property in document.RootElement.EnumerateObject())
            {
                if (property.Name.Contains("gauge_rate") && !property.Name.Contains("drive_other"))
                {
                    var superGaugeNameMap = BuildDataObjectNameMap(typeof(SuperGaugeUsage));
                    var driveGaugeNameMap = BuildDataObjectNameMap(typeof(DriveGaugeUsage));

                    if (property.Name.Contains("_sa_lv") || property.Name == "gauge_rate_ca")
                    {
                        var dtoPropName = superGaugeNameMap.Where(x => x.JsonAttributeName == property.Name).Select(y => y.PropertyName).FirstOrDefault();
                        var jsonProp = typeof(SuperGaugeUsage).GetProperty(dtoPropName);
                        jsonProp?.SetValue(battleStats.SuperGaugeUsage, property.Value.GetDouble());
                    }
                    else
                    {
                        var dtoPropName = driveGaugeNameMap.Where(x => x.JsonAttributeName == property.Name).Select(y => y.PropertyName).FirstOrDefault();
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
        
        private record JsonToPropertyNameMap(string? PropertyName, string? JsonAttributeName);

        private static Battle BuildBattleStatsContainer(JsonElement root, JsonSerializerOptions options)
        {
            var dto = JsonSerializer.Deserialize<Battle>(root.GetRawText(), options);
            dto.SuperGaugeUsage = new SuperGaugeUsage();
            dto.DriveGaugeUsage = new DriveGaugeUsage();

            return dto;
        }

        private static IEnumerable<JsonToPropertyNameMap> BuildDataObjectNameMap(Type dtoType)
        {
            return dtoType.GetProperties().Select(prop => new JsonToPropertyNameMap(prop.Name, prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name));
        }
    }
}
