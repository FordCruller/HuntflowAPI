using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace HuntflowAPI
{
    public class HuntflowApplicantsResume
    {
        public string id { get; set; }
        public string auth_type { get; set; }
        public int account_source { get; set; }
        public DateTime updated { get; set; }
        public DateTime created { get; set; }
        public ResumeFile[] files { get; set; }
        public string source_url { get; set; }
        public string foreign { get; set; }
        public string key { get; set; }
        public PortfolioImage[] portfolio { get; set; }
        public ResumeData data { get; set; }
        public ResumeInfo resume { get; set; }

        public class ResumeFile
        {
            public string id { get; set; }
            public string url { get; set; }
            public string content_type { get; set; }
            public string name { get; set; }
        }

        public class PortfolioImage
        {
            public string small { get; set; }
            public string large { get; set; }
            public string description { get; set; }
        }

        public class ResumeData
        {
            public string body { get; set; }
            public Area area { get; set; }

            public class Area
            {
                public string id { get; set; }
                public string name { get; set; }
                public string url { get; set; }
            }
        }

        public class ResumeInfo
        {
            public PersonalInfo personal_info { get; set; }
            public string source_url { get; set; }
            public string position { get; set; }
            public Specialization[] specialization { get; set; }
            public string[] skill_set { get; set; }
            public Gender gender { get; set; }
            public Experience[] experience { get; set; }
            public Education education { get; set; }
            public Contact[] contact { get; set; }
            public Area area { get; set; }
            public Relocation relocation { get; set; }
            public Citizenship[] citizenship { get; set; }
            public Language[] language { get; set; }
            public WantedSalary wanted_salary { get; set; }
            public WorkSchedule[] work_schedule { get; set; }
            public bool has_vehicle { get; set; }
            public string[] driver_license_types { get; set; }
            public MilitaryService[] military { get; set; }
            public SocialRating[] social_ratings { get; set; }
            public Photo[] photos { get; set; }
            public AdditionalInfo[] additionals { get; set; }
            public string wanted_place_of_work { get; set; }
            public DateTime updated_on_source { get; set; }
            public TravelTime travel_time { get; set; }
        }

        public class WantedSalary
        {
            public int amount {  get; set; }
            public string currency { get; set; }
        }

        public class PersonalInfo
        {
            public PhotoInfo photo { get; set; }
            public string first_name { get; set; }
            public string middle_name { get; set; }
            public string last_name { get; set; }
            public BirthDate birth_date { get; set; }
            public TextBlock text_block { get; set; }
        }

        public class PhotoInfo
        {
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
            public string external_id { get; set; }
            public string description { get; set; }
            public string source { get; set; }
            public string id { get; set; }
        }

        public class BirthDate
        {
            public int year { get; set; }
            public int month { get; set; }
            public int day { get; set; }
            public PrecisionType precision { get; set; }
        }

        public enum PrecisionType
        {
            year,
            month,
            day
        }

        public class TextBlock
        {
            public string header { get; set; }
            public string body { get; set; }
        }

        public class Specialization
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
            public string profarea_id { get; set; }
            public string external_profarea_id { get; set; }
            public string prefarea_name { get; set; }
        }

        public class Relocation
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class Gender
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class Experience
        {
            public string position { get; set; }
            public DateInfo date_from { get; set; }
            public DateInfo date_to { get; set; }
            public string company { get; set; }
            public Area experience_area { get; set; }
            public Industry[] industries { get; set; }
            public string description { get; set; }
            public Skill[] skills { get; set; }
        }

        public class DateInfo
        {
            public int year { get; set; }
            public int month { get; set; }
            public int day { get; set; }
            public PrecisionType precision { get; set; }
        }

        public class Industry
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class Skill
        {
            public string title { get; set; }
        }

        public class Education
        {
            public EducationLevel level { get; set; }
            public HigherEducation[] higher { get; set; }
            public VocationalEducation[] vocational { get; set; }
            public ElementaryEducation[] elementary { get; set; }
            public AdditionalEducation[] additional { get; set; }
            public Attestation[] attestation { get; set; }
            public Certificate[] certificate { get; set; }
        }

        public class EducationLevel
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class HigherEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo date_from { get; set; }
            public DateInfo date_to { get; set; }
            public Area education_area { get; set; }
            public string faculty { get; set; }
            public Form form { get; set; }
        }

        public class VocationalEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo date_from { get; set; }
            public DateInfo date_to { get; set; }
            public Area education_area { get; set; }
            public string faculty { get; set; }
            public Form form { get; set; }
        }

        public class ElementaryEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo date_from { get; set; }
            public DateInfo date_to { get; set; }
            public Area education_area { get; set; }
        }

        public class AdditionalEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo date_from { get; set; }
            public DateInfo date_to { get; set; }
            public Area education_area { get; set; }
            public string result { get; set; }
        }

        public class Attestation
        {
            public DateInfo date { get; set; }
            public string name { get; set; }
            public string organization { get; set; }
            public string description { get; set; }
            public string result { get; set; }
        }

        public class Certificate
        {
            public string name { get; set; }
            public string organization { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public Area area { get; set; }
            public DateInfo date { get; set; }
        }

        public class Area
        {
            public Country country { get; set; }
            public City city { get; set; }
            public Metro metro { get; set; }
            public string address { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Country
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class City
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class Metro
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class Form
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class Language
        {
            public string id { get; set; }
            public string name { get; set; }
            public Level level { get; set; }
        }

        public class Level
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class WorkSchedule
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class MilitaryService
        {
            public DateInfo date_from { get; set; }
            public DateInfo date_to { get; set; }
            public Area area { get; set; }
            public string unit { get; set; }
        }

        public class SocialRating
        {
            public string kind { get; set; }
            public string stats { get; set; }
            public string[] tags { get; set; }
            public string url { get; set; }
            public string login { get; set; }
            public DateTime registered_at { get; set; }
        }

        public class Photo
        {
            public string url { get; set; }
            public string original { get; set; }
        }

        public class AdditionalInfo
        {
            public string name { get; set; }
            public string description { get; set; }
        }

        public class Contact
        {
            public ContactType type { get; set; }
            public string value { get; set; }
            public bool preferred { get; set; }
            public PhoneInfo full_value { get; set; }
        }

        public class ContactType
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class PhoneInfo
        {
            public string country { get; set; }
            public string city { get; set; }
            public string number { get; set; }
            public string formatted { get; set; }
        }

        public class Citizenship
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public class TravelTime
        {
            public string id { get; set; }
            public string name { get; set; }
            public string external_id { get; set; }
        }

        public HuntflowApplicantsResume() { }

        public HuntflowApplicantsResume(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}