using Newtonsoft.Json;
using System;
using System.IO;

namespace HuntflowAPI.Data.Huntflow
{
    internal class Deserializer
    {
        public T Deserialize<T>(string fileName)
        {
            string filePath = @"..\..\Data\Huntflow\Cache\" + fileName + ".json";
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            string jsonContent = File.ReadAllText(filePath);

            T deserializedObject = JsonConvert.DeserializeObject<T>(jsonContent, NullableConverter.GetSettings());

            Console.WriteLine($"Извлечен объект {deserializedObject.GetType()}: {filePath}");
            return deserializedObject;
        }
    }
}
