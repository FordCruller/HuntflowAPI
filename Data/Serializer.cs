using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static HuntflowAPI.HuntflowVacancyList;

namespace HuntflowAPI.Data.Huntflow
{
    internal class Serializer
    {
        public void Serialize(Object obj, string nameIdentificator)
        {
            string json = JsonConvert.SerializeObject(obj);
            WriteJson($"{nameIdentificator}.json", json);
        }

        private void WriteJson(string fileName, string json)
        {
            string executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.GetFullPath(Path.Combine(executingPath, @"..\..\Data\Huntflow\Cache\" + fileName));
            File.WriteAllText(filePath, json);
        }
    }
}
