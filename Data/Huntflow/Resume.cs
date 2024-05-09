using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI.Data.Huntflow
{
    public class Resume
    {
        public string id { get; set; }
        public string authType { get; set; }
        public int accountSource { get; set; }
        public DateTime updated { get; set; }
        public DateTime created { get; set; }
        public ResumeFile[] files { get; set; }
        public string sourceUrl { get; set; }
        public string foreign { get; set; }
        public string key { get; set; }
        public PortfolioImage[] portfolio { get; set; }
        public ResumeData data { get; set; }
        public ResumeInfo resume { get; set; }

        public class ResumeFile
        {
            public string id { get; set; }
            public string url { get; set; }
            public string contentType { get; set; }
            public string name { get; set; }

            public ResumeFile(HuntflowApplicantsResume.ResumeFile resumeFile)
            {
                this.id = resumeFile.id;
                this.url = resumeFile.url;
                this.contentType = resumeFile.content_type;
                this.name = resumeFile.name;
            }
        }

        public class PortfolioImage
        {
            public string small { get; set; }
            public string large { get; set; }
            public string description { get; set; }

            public PortfolioImage(HuntflowApplicantsResume.PortfolioImage portfolioImage)
            {
                this.small = portfolioImage.small;
                this.large = portfolioImage.large;
                this.description = portfolioImage.description;
            }
        }

        public class ResumeData
        {
            public ResumeData(HuntflowApplicantsResume.ResumeData data)
            {
                if (data.area != null)
                {
                    this.area = new Area(data.area);
                }
                this.body = data.body;
            }

            public string body { get; set; }
            public Area area { get; set; }
            public class Area
            {
                public string id { get; set; }
                public string name { get; set; }
                public string url { get; set; }

                public Area(HuntflowApplicantsResume.ResumeData.Area area)
                {
                    this.id = area.id;
                    this.name = area.name;
                    this.url = area.url;
                }
            }
        }

        public class ResumeInfo
        {
            public PersonalInfo personalInfo { get; set; }
            public string sourceUrl { get; set; }
            public string position { get; set; }
            public Specialization[] specialization { get; set; }
            public string[] skillSet { get; set; }
            public Gender gender { get; set; }
            public Experience[] experience { get; set; }
            public Education education { get; set; }
            public Contact[] contact { get; set; }
            public Area area { get; set; }
            public Relocation relocation { get; set; }
            public Citizenship[] citizenship { get; set; }
            public Language[] language { get; set; }
            public WantedSalary wantedSalary { get; set; }
            public WorkSchedule[] workSchedule { get; set; }
            public bool hasVehicle { get; set; }
            public string[] driverLicenseTypes { get; set; }
            public MilitaryService[] military { get; set; }
            public SocialRating[] socialRatings { get; set; }
            public Photo[] photos { get; set; }
            public AdditionalInfo[] additionals { get; set; }
            public string wantedPlaceOfWork { get; set; }
            public DateTime updatedOnSource { get; set; }
            public TravelTime travelTime { get; set; }

            public ResumeInfo(HuntflowApplicantsResume.ResumeInfo resumeInfo)
            {
                if (resumeInfo.personal_info != null)
                {
                    this.personalInfo = new PersonalInfo(resumeInfo.personal_info);
                }
                this.sourceUrl = resumeInfo.source_url;
                this.position = resumeInfo.position;
                if (resumeInfo.specialization != null)
                {
                    this.specialization = HuntflowData.Converter.ConvertArray<Specialization>(resumeInfo.specialization);
                }
                this.skillSet = resumeInfo.skill_set;
                if (resumeInfo.gender != null)
                {
                    this.gender = new Gender(resumeInfo.gender);
                }
                if (resumeInfo.experience != null)
                {
                    this.experience = HuntflowData.Converter.ConvertArray<Experience>(resumeInfo.experience);
                }
                if (resumeInfo.education != null)
                {
                    this.education = new Education(resumeInfo.education);
                }
                if (resumeInfo.contact != null)
                {
                    this.contact = HuntflowData.Converter.ConvertArray<Contact>(resumeInfo.contact);
                }
                if (resumeInfo.area != null)
                {
                    this.area = new Area(resumeInfo.area);
                }
                if (resumeInfo.relocation != null)
                {
                    this.relocation = new Relocation(resumeInfo.relocation);
                }
                if (resumeInfo.citizenship != null)
                {
                    this.citizenship = HuntflowData.Converter.ConvertArray<Citizenship>(resumeInfo.citizenship);
                }
                if (resumeInfo.language != null)
                {
                    this.language = HuntflowData.Converter.ConvertArray<Language>(resumeInfo.language);
                }
                if (resumeInfo.wanted_salary != null)
                {
                    this.wantedSalary = new WantedSalary(resumeInfo.wanted_salary);
                }
                if (resumeInfo.work_schedule != null)
                {
                    this.workSchedule = HuntflowData.Converter.ConvertArray<WorkSchedule>(resumeInfo.work_schedule);
                }
                this.hasVehicle = resumeInfo.has_vehicle;
                this.driverLicenseTypes = resumeInfo.driver_license_types;
                if (resumeInfo.military != null)
                {
                    this.military = HuntflowData.Converter.ConvertArray<MilitaryService>(resumeInfo.military);
                }
                if (resumeInfo.social_ratings != null)
                {
                    this.socialRatings = HuntflowData.Converter.ConvertArray<SocialRating>(resumeInfo.social_ratings);
                }
                if(resumeInfo.photos != null)
                {
                    this.photos = HuntflowData.Converter.ConvertArray<Photo>(resumeInfo.photos);
                }
                if (resumeInfo.additionals != null)
                {
                    this.additionals = HuntflowData.Converter.ConvertArray<AdditionalInfo>(resumeInfo.additionals);
                }
                this.wantedPlaceOfWork = resumeInfo.wanted_place_of_work;
                this.updatedOnSource = resumeInfo.updated_on_source;
                if (resumeInfo.travel_time != null)
                {
                    this.travelTime = new TravelTime(resumeInfo.travel_time);
                }
            }
        }

        public class WantedSalary
        {
            public int amount { get; set; }
            public string currency { get; set; }

            public WantedSalary(HuntflowApplicantsResume.WantedSalary wantedSalary)
            {
                this.amount = wantedSalary.amount;
                this.currency = wantedSalary.currency;
            }
        }

        public class PersonalInfo
        {
            public PhotoInfo photo { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string lastName { get; set; }
            public BirthDate birthDate { get; set; }
            public TextBlock textBlock { get; set; }

            public PersonalInfo(HuntflowApplicantsResume.PersonalInfo personalInfo)
            {
                if(personalInfo.photo != null)
                {
                    this.photo = new PhotoInfo(personalInfo.photo);
                }
                this.firstName = personalInfo.first_name;
                this.middleName = personalInfo.middle_name;
                this.lastName = personalInfo.last_name;
                if (personalInfo.birth_date != null)
                {
                    this.birthDate = new BirthDate(personalInfo.birth_date);
                }
                if (personalInfo.text_block != null)
                {
                    this.textBlock = new TextBlock(personalInfo.text_block);
                }
            }
        }

        public class PhotoInfo
        {
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
            public string externalId { get; set; }
            public string description { get; set; }
            public string source { get; set; }
            public string id { get; set; }

            public PhotoInfo(HuntflowApplicantsResume.PhotoInfo photoInfo)
            {
                this.small = photoInfo.small;
                this.medium = photoInfo.medium;
                this.large = photoInfo.large;
                this.externalId = photoInfo.external_id;
                this.description = photoInfo.description;
                this.source = photoInfo.source;
                this.id = photoInfo.id;
            }
        }

        public class BirthDate
        {
            public int year { get; set; }
            public int month { get; set; }
            public int day { get; set; }
            public HuntflowApplicantsResume.PrecisionType precision { get; set; }

            public BirthDate(HuntflowApplicantsResume.BirthDate birthDate)
            {
                this.year = birthDate.year;
                this.month = birthDate.month;
                this.day = birthDate.day;
                this.precision = birthDate.precision;
            }
        }

        public class TextBlock
        {
            public string header { get; set; }
            public string body { get; set; }

            public TextBlock(HuntflowApplicantsResume.TextBlock textBlock)
            {
                this.header = textBlock.header;
                this.body = textBlock.body;
            }
        }

        public class Specialization
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }
            public string profareaId { get; set; }
            public string externalProfareaId { get; set; }
            public string prefareaName { get; set; }

            public Specialization()
            {
            }

            public Specialization(HuntflowApplicantsResume.Specialization specialization)
            {
                this.id = specialization.id;
                this.name = specialization.name;
                this.externalId = specialization.external_id;
                this.profareaId = specialization.profarea_id;
                this.externalProfareaId = specialization.external_profarea_id;
                this.prefareaName = specialization.prefarea_name;
            }
        }

        public class Relocation
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Relocation(HuntflowApplicantsResume.Relocation relocation)
            {
                this.id = relocation.id;
                this.name = relocation.name;
                this.externalId = relocation.external_id;
            }
        }

        public class Gender
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Gender(HuntflowApplicantsResume.Gender gender)
            {
                this.id = gender.id;
                this.name = gender.name;
                this.externalId = gender.external_id;
            }
        }

        public class Experience
        {
            public string position { get; set; }
            public DateInfo dateFrom { get; set; }
            public DateInfo dateTo { get; set; }
            public string company { get; set; }
            public Area experienceArea { get; set; }
            public Industry[] industries { get; set; }
            public string description { get; set; }
            public Skill[] skills { get; set; }

            public Experience(HuntflowApplicantsResume.Experience experience)
            {
                this.position = experience.position;
                if(experience.date_from != null)
                {
                    this.dateFrom = new DateInfo(experience.date_from);
                }
                if (experience.date_to != null)
                {
                    this.dateTo = new DateInfo(experience.date_to);
                }
                this.company = experience.company;
                if (experience.experience_area != null)
                {
                    this.experienceArea = new Area(experience.experience_area);
                }
                if (experience.industries != null)
                {
                    this.industries = HuntflowData.Converter.ConvertArray<Industry>(experience.industries);
                }
                this.description = experience.description;
                if (experience.skills != null)
                {
                    this.skills = HuntflowData.Converter.ConvertArray<Skill>(experience.skills);
                }
            }
        }

        public class DateInfo
        {
            public int year { get; set; }
            public int month { get; set; }
            public int day { get; set; }
            public HuntflowApplicantsResume.PrecisionType precision { get; set; }

            public DateInfo(HuntflowApplicantsResume.DateInfo dateInfo)
            {
                this.year = dateInfo.year;
                this.month = dateInfo.month;
                this.day = dateInfo.day;
                this.precision = dateInfo.precision;
            }
        }

        public class Industry
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Industry(HuntflowApplicantsResume.Industry industry)
            {
                this.id = industry.id;
                this.name = industry.name;
                this.externalId = industry.external_id;
            }
        }

        public class Skill
        {
            public string title { get; set; }

            public Skill(HuntflowApplicantsResume.Skill skill)
            {
                this.title = skill.title;
            }
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

            public Education(HuntflowApplicantsResume.Education education)
            {
                if (education.level != null)
                {
                    this.level = new EducationLevel(education.level);
                }
                if(education.higher != null)
                {
                    this.higher = HuntflowData.Converter.ConvertArray<HigherEducation>(education.higher);
                }
                if (education.vocational != null)
                {
                    this.vocational = HuntflowData.Converter.ConvertArray<VocationalEducation>(education.vocational);
                }
                if (education.elementary != null)
                {
                    this.elementary = HuntflowData.Converter.ConvertArray<ElementaryEducation>(education.elementary);
                }
                if(education.additional != null)
                {
                    this.additional = HuntflowData.Converter.ConvertArray<AdditionalEducation>(education.additional);
                }
                if(education.attestation != null)
                {
                    this.attestation = HuntflowData.Converter.ConvertArray<Attestation>(education.attestation);
                }
                if(education.certificate != null)
                {
                    this.certificate = HuntflowData.Converter.ConvertArray<Certificate>(education.certificate);
                }
            }
        }

        public class EducationLevel
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public EducationLevel(HuntflowApplicantsResume.EducationLevel educationLevel)
            {
                this.id = educationLevel.id;
                this.name = educationLevel.name;
                this.externalId = educationLevel.external_id;
            }
        }

        public class HigherEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo dateFrom { get; set; }
            public DateInfo dateTo { get; set; }
            public Area educationArea { get; set; }
            public string faculty { get; set; }
            public Form form { get; set; }

            public HigherEducation(HuntflowApplicantsResume.HigherEducation higherEducation)
            {
                this.name = higherEducation.name;
                this.description = higherEducation.description;
                if (higherEducation.date_from != null)
                {
                    this.dateFrom = new DateInfo(higherEducation.date_from);
                }
                if(higherEducation.date_to != null)
                {
                    this.dateTo = new DateInfo(higherEducation.date_to);
                }
                if (higherEducation.education_area != null)
                {
                    this.educationArea = new Area(higherEducation.education_area);
                }
                if (higherEducation.faculty != null)
                {
                    this.faculty = higherEducation.faculty;
                }
                if (higherEducation.form != null)
                {
                    this.form = new Form(higherEducation.form);
                }
            }
        }

        public class VocationalEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo dateFrom { get; set; }
            public DateInfo dateTo { get; set; }
            public Area educationArea { get; set; }
            public string faculty { get; set; }
            public Form form { get; set; }

            public VocationalEducation(HuntflowApplicantsResume.VocationalEducation vocationalEducation)
            {
                this.name = vocationalEducation.name;
                this.description = vocationalEducation.description;
                if(vocationalEducation.date_from != null)
                {
                    this.dateFrom = new DateInfo(vocationalEducation.date_from);
                }
                if(vocationalEducation.date_to != null)
                {
                    this.dateTo = new DateInfo(vocationalEducation.date_to);
                }
                if(vocationalEducation.education_area != null)
                {
                    this.educationArea = new Area(vocationalEducation.education_area);
                }
                this.faculty = vocationalEducation.faculty;
                if (vocationalEducation.faculty != null)
                {
                    this.form = new Form(vocationalEducation.form);
                }
            }
        }

        public class ElementaryEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo dateFrom { get; set; }
            public DateInfo dateTo { get; set; }
            public Area educationArea { get; set; }

            public ElementaryEducation(HuntflowApplicantsResume.ElementaryEducation elementaryEducation)
            {
                this.name = elementaryEducation.name;
                this.description = elementaryEducation.description;
                if(elementaryEducation.date_from != null)
                {
                    this.dateFrom = new DateInfo(elementaryEducation.date_from);
                }
                if (elementaryEducation.date_to != null)
                {
                    this.dateTo = new DateInfo(elementaryEducation.date_to);
                }
                if(elementaryEducation.education_area != null)
                {
                    this.educationArea = new Area(elementaryEducation.education_area);
                }
            }
        }

        public class AdditionalEducation
        {
            public string name { get; set; }
            public string description { get; set; }
            public DateInfo dateFrom { get; set; }
            public DateInfo dateTo { get; set; }
            public Area educationArea { get; set; }
            public string result { get; set; }

            public AdditionalEducation()
            {
            }

            public AdditionalEducation(HuntflowApplicantsResume.AdditionalEducation additionalEducation)
            {
                this.name = additionalEducation.name;
                this.description = additionalEducation.description;
                if(additionalEducation.date_from != null)
                {
                    this.dateFrom = new DateInfo(additionalEducation.date_from);
                }
                if(additionalEducation.date_to != null)
                {
                    this.dateTo = new DateInfo(additionalEducation.date_to);
                }
                if(additionalEducation.education_area != null)
                {
                    this.educationArea = new Area(additionalEducation.education_area);
                }
                this.result = additionalEducation.result;
            }
        }

        public class Attestation
        {
            public DateInfo date { get; set; }
            public string name { get; set; }
            public string organization { get; set; }
            public string description { get; set; }
            public string result { get; set; }

            public Attestation(HuntflowApplicantsResume.Attestation attestation)
            {
                if(attestation.date != null)
                {
                    this.date = new DateInfo(attestation.date);
                }
                this.name = attestation.name;
                this.organization = attestation.organization;
                this.description = attestation.description;
                this.result = attestation.result;
            }
        }

        public class Certificate
        {
            public string name { get; set; }
            public string organization { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public Area area { get; set; }
            public DateInfo date { get; set; }

            public Certificate(HuntflowApplicantsResume.Certificate certificate)
            {
                this.name = certificate.name;
                this.organization = certificate.organization;
                this.description = certificate.description;
                this.url = certificate.url;
                if(certificate.area != null)
                {
                    this.area = new Area(certificate.area);
                }
                if(certificate.date != null)
                {
                    this.date = new DateInfo(certificate.date);
                }
            }
        }

        public class Area
        {
            public Country country { get; set; }
            public City city { get; set; }
            public Metro metro { get; set; }
            public string address { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }

            public Area(HuntflowApplicantsResume.Area area)
            {
                if(area.country != null)
                {
                    this.country = new Country(area.country);
                }
                if(area.city != null)
                {
                    this.city = new City(area.city);
                }
                if(area.metro != null)
                {
                    this.metro = new Metro(area.metro);
                }
                this.address = area.address;
                this.lat = area.lat;
                this.lng = area.lng;
            }
        }

        public class Country
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Country(HuntflowApplicantsResume.Country country)
            {
                this.id = country.id;
                this.name = country.name;
                this.externalId = country.external_id;
            }
        }

        public class City
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public City(HuntflowApplicantsResume.City city)
            {
                this.id = city.id;
                this.name = city.name;
                this.externalId = city.external_id;
            }
        }

        public class Metro
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Metro(HuntflowApplicantsResume.Metro metro)
            {
                this.id = metro.id;
                this.name = metro.name;
                this.externalId = metro.external_id;
            }
        }

        public class Form
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Form(HuntflowApplicantsResume.Form form)
            {
                this.id = form.id;
                this.name = form.name;
                this.externalId = form.external_id;
            }
        }

        public class Language
        {
            public string id { get; set; }
            public string name { get; set; }
            public Level level { get; set; }

            public Language(HuntflowApplicantsResume.Language language)
            {
                this.id = language.id;
                this.name = language.name;
                if(language.level != null)
                {
                    this.level = new Level(language.level);
                }
            }
        }

        public class Level
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Level(HuntflowApplicantsResume.Level level)
            {
                this.id = level.id;
                this.name = level.name;
                this.externalId = level.external_id;
            }
        }

        public class WorkSchedule
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public WorkSchedule(HuntflowApplicantsResume.WorkSchedule workSchedule)
            {
                this.id = workSchedule.id;
                this.name = workSchedule.name;
                this.externalId = workSchedule.external_id;
            }
        }

        public class MilitaryService
        {
            public DateInfo dateFrom { get; set; }
            public DateInfo dateTo { get; set; }
            public Area area { get; set; }
            public string unit { get; set; }

            public MilitaryService(HuntflowApplicantsResume.MilitaryService militaryService)
            {
                if (militaryService.date_from != null)
                {
                    this.dateFrom = new DateInfo(militaryService.date_from);
                }
                if(militaryService.date_to != null)
                {
                    this.dateTo = new DateInfo(militaryService.date_to);
                }
                if(militaryService.area != null)
                {
                    this.area = new Area(militaryService.area);
                }
                this.unit = militaryService.unit;
            }
        }

        public class SocialRating
        {
            public string kind { get; set; }
            public string stats { get; set; }
            public string[] tags { get; set; }
            public string url { get; set; }
            public string login { get; set; }
            public DateTime registeredAt { get; set; }

            public SocialRating(HuntflowApplicantsResume.SocialRating socialRating)
            {
                this.kind = socialRating.kind;
                this.stats = socialRating.stats;
                this.tags = socialRating.tags;
                this.url = socialRating.url;
                this.login = socialRating.login;
                this.registeredAt = socialRating.registered_at;
            }
        }

        public class Photo
        {
            public string url { get; set; }
            public string original { get; set; }

            public Photo(HuntflowApplicantsResume.Photo photo)
            {
                this.url = photo.url;
                this.original = photo.original;
            }
        }

        public class AdditionalInfo
        {
            public string name { get; set; }
            public string description { get; set; }

            public AdditionalInfo(HuntflowApplicantsResume.AdditionalInfo additionalInfo)
            {
                this.name = additionalInfo.name;
                this.description = additionalInfo.description;
            }
        }

        public class Contact
        {
            public ContactType type { get; set; }
            public string value { get; set; }
            public bool preferred { get; set; }
            public PhoneInfo fullValue { get; set; }

            public Contact(HuntflowApplicantsResume.Contact contact)
            {
                if(contact.type != null)
                {
                    this.type = new ContactType(contact.type);
                }
                this.value = contact.value;
                this.preferred = contact.preferred;
                if(contact.full_value != null)
                {
                    this.fullValue = new PhoneInfo(contact.full_value);
                }
            }
        }

        public class ContactType
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public ContactType(HuntflowApplicantsResume.ContactType contactType)
            {
                this.id = contactType.id;
                this.name = contactType.name;
                this.externalId = contactType.external_id;
            }
        }

        public class PhoneInfo
        {
            public string country { get; set; }
            public string city { get; set; }
            public string number { get; set; }
            public string formatted { get; set; }

            public PhoneInfo(HuntflowApplicantsResume.PhoneInfo phoneInfo)
            {
                this.country = phoneInfo.country;
                this.city = phoneInfo.city;
                this.number = phoneInfo.number;
                this.formatted = phoneInfo.formatted;
            }
        }

        public class Citizenship
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public Citizenship(HuntflowApplicantsResume.Citizenship citizenship)
            {
                this.id = citizenship.id;
                this.name = citizenship.name;
                this.externalId = citizenship.external_id;
            }
        }

        public class TravelTime
        {
            public string id { get; set; }
            public string name { get; set; }
            public string externalId { get; set; }

            public TravelTime(HuntflowApplicantsResume.TravelTime travelTime)
            {
                this.id = travelTime.id;
                this.name = travelTime.name;
                this.externalId = travelTime.external_id;
            }
        }

        public Resume() { }

        public Resume(HuntflowApplicantsResume huntflowApplicantsResume)
        {
            this.id = huntflowApplicantsResume.id;
            this.authType = huntflowApplicantsResume.auth_type;
            this.accountSource = huntflowApplicantsResume.account_source;
            this.updated = huntflowApplicantsResume.updated;
            this.created = huntflowApplicantsResume.created;
            if (huntflowApplicantsResume.files != null)
            {
                this.files = HuntflowData.Converter.ConvertArray<ResumeFile>(huntflowApplicantsResume.files);
            }
            this.sourceUrl = huntflowApplicantsResume.source_url;
            this.foreign = huntflowApplicantsResume.foreign;
            this.key = huntflowApplicantsResume.key;
            if(huntflowApplicantsResume.portfolio != null)
            {
                this.portfolio = HuntflowData.Converter.ConvertArray<PortfolioImage>(huntflowApplicantsResume.portfolio);
            }
            if (huntflowApplicantsResume.data != null)
            {
                this.data = new ResumeData(huntflowApplicantsResume.data);
            }
            if (huntflowApplicantsResume.resume != null)
            {
                this.resume = new ResumeInfo(huntflowApplicantsResume.resume);
            }
        }
    }
}
