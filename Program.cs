using Google.Apis.Sheets.v4.Data;
using HuntflowAPI.Data;
using HuntflowAPI.Order;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using static HuntflowAPI.GetRequest;
using static HuntflowAPI.HuntflowResumeSources;

namespace HuntflowAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HuntflowData data = HuntflowData.GetInstance();
            int accountId = data.GetAllOrganizations(Data.Source.CACHE)[0].id;

            //HuntflowData.UpdateCache(new HuntflowEntity[] { HuntflowEntity.VACANCIES, HuntflowEntity.TAGS, HuntflowEntity.STEPS }, accountId);

            CreateExcelReports(Data.Source.HUNTFLOW, data, accountId);
            //CreateExcelReport(Data.Source.HUNTFLOW, accountId, 3606543);

            //HuntflowRequest request = new HuntflowRequest(data.GetToken());

            //var companies = request.GetAllAvailableOrganizations();

            /*
            foreach (var company in companies.items)
            {
                Console.WriteLine(company.id);
            }
            */
        }

        private static void CreateExcelReports(Data.Source source, HuntflowData data, int accountId)
        {
            var vacancies = data.GetAllVacancies(Data.Source.CACHE, accountId, false, new GetRequest.VacancyState[] { GetRequest.VacancyState.OPEN });

            foreach (var vacancy in vacancies)
            {
                //Console.WriteLine(vacancy.position);
                CreateExcelReport(source, accountId, vacancy.id);
            }
        }

        private static void CreateExcelReport(Data.Source source, int accountId, int vacancyId)
        {
            var orderData = ApplicantsOrderData.FromHuntflow(source, accountId, vacancyId);

            ApplicantsReport applicantsReport = new ApplicantsReport(orderData);
            PipeReport pipeReport = new PipeReport(orderData);
            DashboardReport dashboardReport = new DashboardReport(orderData);

            Office.ExcelReport excelReport = new Office.ExcelReport(applicantsReport, pipeReport, dashboardReport);
            excelReport.Write(@"C:\Users\eldas\Downloads\" + SanitizeFilename(orderData.vacancy.position) + " Report.xlsx");
        }

        private static string SanitizeFilename(string filename)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, "_");
        }

        private static void PrintVacancies(HuntflowData data, int accountId, VacancyState[] states)
        {
            var vacancies = data.GetAllVacancies(Data.Source.HUNTFLOW, accountId, false, new VacancyState[]{ VacancyState.CLOSED });
            foreach (var vacancy in vacancies)
            {
                Console.WriteLine($"[{vacancy.id}]: {vacancy.position}");
            }
        }
    }
}
