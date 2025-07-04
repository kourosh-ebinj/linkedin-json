using System.Text.Json.Serialization;
using Json.Path;
using System.Text.Json;
using Json.More;

namespace JSONPath.Helpers
{
    public static class JsonHelper
    {
        private static Newtonsoft.Json.JsonSerializerSettings _newtonSoftSettings => new Newtonsoft.Json.JsonSerializerSettings
        {
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
            DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,
            MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Default,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
            //ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            //MaxDepth = 2,

        };

        private static JsonSerializerOptions _SystemTextSettings => new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            PropertyNameCaseInsensitive = true,
            MaxDepth = 2,
            ReferenceHandler = ReferenceHandler.Preserve,
            //PropertyNamingPolicy = JsonNamingPolicy.()
        };

        public static string ToJsonString(object obj, params JsonConverter[] jsonConverters)
        {
            if (obj == null) return string.Empty;

            try
            {
                if (jsonConverters.Any())
                {
                    var systemTextSettings = _SystemTextSettings;
                    foreach (var converter in jsonConverters)
                        systemTextSettings.Converters.Add(converter);

                    return JsonSerializer.Serialize(obj, obj.GetType(), systemTextSettings);
                }

                return JsonSerializer.Serialize(obj, obj.GetType(), _SystemTextSettings);
            }
            catch (Exception ex)
            {
                // log exception
                // note that converters are not considered in fallback.
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, _newtonSoftSettings);
            }
        }

        public static T? FromJsonString<T>(string json, params JsonConverter[] jsonConverters)
        {
            try
            {
                if (jsonConverters.Any())
                {
                    var systemTextSettings = _SystemTextSettings;
                    foreach (var converter in jsonConverters)
                        systemTextSettings.Converters.Add(converter);

                    return JsonSerializer.Deserialize<T>(json, systemTextSettings);
                }

                return JsonSerializer.Deserialize<T>(json, _SystemTextSettings);
            }
            catch (Exception ex)
            {
                // log exception
                // note that converters are not considered in fallback.
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, _newtonSoftSettings);
            }
        }

        public static TResult? GetPrimitiveValueByPath<TResult>(string jsonData, string jsonPath) where TResult : struct
        {
            ArgumentNullException.ThrowIfNull(jsonData, nameof(jsonData));

            using var doc = JsonDocument.Parse(jsonData);
            return GetNodes(doc, jsonPath,
            nodes => nodes.FirstOrDefault()?.Value.GetValue<TResult>());
        }

        public static TResult? GetItemByPath<TResult>(string jsonData, string jsonPath)
        {
            var result = GetItemsByPath<TResult>(jsonData, jsonPath);
            if (!result.Any()) return default;

            return result.ElementAt(0);
        }

        public static IEnumerable<TResult?> GetItemsByPath<TResult>(string jsonData, string jsonPath)
        {
            ArgumentNullException.ThrowIfNull(jsonData, nameof(jsonData));

            using var doc = JsonDocument.Parse(jsonData);

            return GetNodes(doc, jsonPath,
                nodes => nodes.Select(match => match.Value.Deserialize<TResult>()).ToList()
            );
        }

        private static TResult GetNodes<TResult>(JsonDocument jsonDoc, string jsonPath, Func<NodeList, TResult> valueFunc)
        {
            if (!JsonPath.TryParse(jsonPath, out var _jsonPath))
                throw new ArgumentException("Invalid JsonPath", nameof(jsonPath));

            var result = _jsonPath.Evaluate(jsonDoc.RootElement.AsNode());
            //if (result is null || !result.Matches.Any())
            //    return default;
            
            return valueFunc(result.Matches);
        }
    }

}
