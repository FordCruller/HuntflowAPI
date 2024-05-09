using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI.Data.Huntflow
{
    public class Organization
    {
        public int id { get; set; }
        public string name { get; set; }
        public string nick { get; set; }
        public string memberType { get; set; }
        public int productionCalendar { get; set; }

        public Organization(HuntflowOrganization huntflowOrganization)
        {
            id = huntflowOrganization.id;
            name = huntflowOrganization.name;
            nick = huntflowOrganization.nick;
            memberType = huntflowOrganization.member_type;
            productionCalendar = huntflowOrganization.production_calendar;
        }

        public static Organization[] Convert(HuntflowOrganizations huntflowOrganizations)
        {
            var organizations = new Organization[huntflowOrganizations.items.Length];
            for(int i = 0; i < organizations.Length; i++)
            {
                organizations[i] = new Organization(huntflowOrganizations.items[i]);
            }
            return organizations;
        }
    }
}
