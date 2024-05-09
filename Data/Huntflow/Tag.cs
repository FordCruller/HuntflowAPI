using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI.Data.Huntflow
{
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }

        public Tag(HuntflowTags.HuntflowTag huntflowTag)
        {
            if (huntflowTag != null) 
            {
                id = huntflowTag.id;
                name = huntflowTag.name;
                color = huntflowTag.color;
            }
        }

        public static Tag[] Convert(HuntflowTags tags)
        {
            var result = new Tag[tags.items.Length];
            if (tags != null)
            {
                for (int i = 0; i < tags.items.Length; i++)
                {
                    result[i] = new Tag(tags.items[i]);
                }
            }
            return result;
        }

        public static Dictionary<int, Tag> GetDictionary(Tag[] tags)
        {
            if (tags != null)
            {
                Dictionary<int, Tag> result = new Dictionary<int, Tag>();
                foreach (Tag tag in tags)
                {
                    result[tag.id] = tag;
                }
                return result;
            }
            return null;
        }
    }
}
