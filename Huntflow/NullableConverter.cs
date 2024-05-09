using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

public class NullableConverter : JsonConverter
{
    public static JsonSerializerSettings settings;

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(int) || objectType == typeof(DateTime);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);

        if (objectType == typeof(int))
        {
            return token.Type == JTokenType.Null ? -1 : token.ToObject<int>();
        }
        else if (objectType == typeof(DateTime))
        {
            try
            {
                return token.Type == JTokenType.Null ? DateTime.MinValue : token.ToObject<DateTime>();
            } catch(ArgumentException e) 
            {
                Console.WriteLine(e.Message);
                return DateTime.MinValue;
            }
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public static JsonSerializerSettings GetSettings()
    {
        if (settings == null)
        {
            settings = new JsonSerializerSettings();
            settings.Converters.Add(new NullableConverter());
        }
        return settings;
    }
}