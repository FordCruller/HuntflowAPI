using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using static HuntflowAPI.GetRequest;

namespace HuntflowAPI.Data.Huntflow
{
    public class Applicant
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string money { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string skype { get; set; }
        public string position { get; set; }
        public string company { get; set; }
        public int photo { get; set; }
        public int id { get; set; }
        public int account { get; set; }
        public string photoUrl { get; set; }
        public DateTime birthday { get; set; }
        public DateTime created { get; set; }
        public Tag[] tags { get; set; }
        public Link[] links { get; set; }
        public External[] external { get; set; }
        public Agreement agreement { get; set; }
        public Double[] doubles { get; set; }
        public Social[] social { get; set; }

        private bool? isHot;

        private bool? isReferal;

        private static ManualSettings settings = ManualSettings.GetInstance();

        public bool IsReferal()
        {
            if (isReferal == null)
            {
                foreach (var tag in tags)
                {
                    if (tag.tag == settings.referal_tag_id)
                    {
                        isReferal = true;
                        return true;
                    }
                }
                isReferal = false;
                return false;
            }
            else
            {
                return (bool)isReferal;
            }
        }

        public bool IsHot(Worklog[] logs)
        {
            if (isHot == null)
            {
                bool sourceFlag = false;
                int hhIndex = GetHHExternalIndex();

                int hotHHid = settings.hot_sources.hh_id;
                bool haveHotSource = false;

                if (hhIndex != -1 && external[hhIndex].accountSource == hotHHid)
                {
                    haveHotSource = true;
                }

                bool haveResponse = false;
                foreach (var log in logs)
                {
                    if (log.type.Equals("RESPONSE"))
                    {
                        haveHotSource = true;
                        break;
                    }
                }

                sourceFlag = haveHotSource || haveResponse;

                bool haveHotTag = false;
                foreach (var tag in tags)
                {
                    if (tag.tag == settings.hot_tag_id)
                    {
                        haveHotTag = true;
                        break;
                    }
                }

                return sourceFlag || haveHotTag;
            }
            else
            {
                return (bool)isHot;
            }
        }

        public int GetHHExternalIndex()
        {
            int hhIndex = -1;
            for (int i = 0; i < this.external.Length; i++)
            {
                if (external[i].authType.Equals("HH"))
                {
                    hhIndex = i;
                    break;
                }
            }
            return hhIndex;
        }

        public static int[] GetApplicantIdsArray(Applicant[] applicants)
        {
            var result = new int[applicants.Length];
            for (int i = 0; i < applicants.Length; i++)
            {
                result[i] = applicants[i].id;
            }
            return result;
        }

        public Applicant(HuntflowApplicant applicant)
        {
            firstName = applicant.first_name;
            lastName = applicant.last_name;
            middleName = applicant.middle_name;
            money = applicant.money;
            phone = applicant.phone;
            email = applicant.email;
            skype = applicant.skype;
            position = applicant.position;
            company = applicant.company;
            photo = applicant.photo;
            id = applicant.id;
            account = applicant.account;
            photoUrl = applicant.photo_url;
            birthday = applicant.birthday;
            created = applicant.created;

            var tagArray = new Tag[applicant.tags.Length];
            for (int i = 0; i < applicant.tags.Length; i++)
            {
                tagArray[i] = new Tag(applicant.tags[i]);
            }
            tags = tagArray;

            var linkArray = new Link[applicant.links.Length];
            for (int i = 0; i < applicant.links.Length; i++)
            {
                linkArray[i] = new Link(applicant.links[i]);
            }
            links = linkArray;

            var externalArray = new External[applicant.external.Length];
            for (int i = 0; i < applicant.external.Length; i++)
            {
                externalArray[i] = new External(applicant.external[i]);
            }
            external = externalArray;


            if (applicant.agreement != null)
            {
                agreement = new Agreement(applicant.agreement);
            }

            var doublesArray = new Double[applicant.doubles.Length];
            for (int i = 0; i < applicant.doubles.Length; i++)
            {
                doublesArray[i] = new Double(applicant.doubles[i]);
            }
            doubles = doublesArray;

            var socialArray = new Social[applicant.social.Length];
            for (int i = 0; i < applicant.social.Length; i++)
            {
                socialArray[i] = new Social(applicant.social[i]);
            }
            social = socialArray;
        }

        public static Applicant[] Convert(HuntflowApplicants huntflowApplicants)
        {
            var applicants = new Applicant[huntflowApplicants.items.Length];
            for (int i = 0; i < applicants.Length; i++)
            {
                applicants[i] = new Applicant(huntflowApplicants.items[i]);
            }
            return applicants;
        }

        public class Tag
        {
            public string v { get; set; }
            public int tag { get; set; }
            public int id { get; set; }

            public Tag(HuntflowApplicant.Tag huntflowTag)
            {
                v = huntflowTag.v;
                tag = huntflowTag.tag;
                id = huntflowTag.id;
            }
        }

        public class Link
        {
            public int id { get; set; }
            public int status { get; set; }
            public DateTime updated { get; set; }
            public DateTime changed { get; set; }
            public int vacancy { get; set; }

            public Link(HuntflowApplicant.Link link)
            {
                id = link.id;
                status = link.status;
                updated = link.updated;
                changed = link.changed;
                vacancy = link.vacancy;
            }
        }

        public class External
        {
            public int id { get; set; }
            public string authType { get; set; }
            public int accountSource { get; set; }
            public DateTime updated { get; set; }

            public External(HuntflowApplicant.External external)
            {
                id = external.id;
                authType = external.auth_type;
                accountSource = external.account_source;
                updated = external.updated;
            }
        }

        public class Agreement
        {
            public string state { get; set; }
            public DateTime decisionDate { get; set; }

            public Agreement(HuntflowApplicant.Agreement agreement)
            {
                state = agreement.state;
                decisionDate = agreement.decision_date;
            }
        }

        public class Double
        {
            public int doubleVar { get; set; }

            public Double(HuntflowApplicant.Double d)
            {
                doubleVar = d.double_var;
            }
        }

        public class Social
        {
            public int id { get; set; }
            public string socialType { get; set; }
            public string value { get; set; }
            public bool verified { get; set; }
            public DateTime verificationDate { get; set; }

            public Social(HuntflowApplicant.Social social)
            {
                id = social.id;
                socialType = social.social_type;
                value = social.value;
                verified = social.verified;
                verificationDate = social.verification_date;
            }
        }
    }
}
