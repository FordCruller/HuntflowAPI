using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI.Data.Huntflow
{
    public class RejectionReason
    {
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }

        public RejectionReason(HuntflowRejectionReasons.HuntflowRejectionReason reason)
        {
            this.id = reason.id;
            this.name = reason.name;
            this.order = reason.order;
        }

        public static RejectionReason[] Convert(HuntflowRejectionReasons reasons)
        {
            return HuntflowData.Converter.ConvertArray<RejectionReason>(reasons.items);
        }

        public static Dictionary<int, RejectionReason> GetDictionary(RejectionReason[] reasons)
        {
            var result = new Dictionary<int, RejectionReason>();
            foreach (var reason in reasons)
            {
                result[reason.id] = reason;
            }
            return result;
        }
    }
}
