using HuntflowAPI.Data;
using HuntflowAPI.Data.Huntflow;
using HuntflowAPI.Order;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace HuntflowAPI.Office
{
    internal class ExcelReport
    {
        ApplicantsReport applicantsReport;
        PipeReport pipeReport;
        DashboardReport dashboardReport;

        private static ManualSettings settings;

        public ExcelReport(ApplicantsReport applicantsReport, PipeReport pipeReport, DashboardReport dashboardReport)
        {
            this.applicantsReport = applicantsReport;
            this.dashboardReport = dashboardReport;
            this.pipeReport = pipeReport;
        }

        public void Write(string filePath)
        {
            settings = ManualSettings.GetInstance();
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet applicantsWorksheet = package.Workbook.Worksheets.Add("Кандидаты");
                WriteApplicantsWorksheet(applicantsWorksheet);
                AddApplicantsWorksheetStyles(applicantsWorksheet);
                ExcelWorksheet pipeWorksheet = package.Workbook.Worksheets.Add("Воронка");
                WritePipeWorksheet(pipeWorksheet);
                AddPipeWorksheetStyles(pipeWorksheet);
                ExcelWorksheet dashboardWorksheet = package.Workbook.Worksheets.Add("Общий отчет");
                WriteDashboardWorksheet(dashboardWorksheet);

                package.SaveAs(new FileInfo(filePath));
            }

            Console.WriteLine("Данные успешно записаны в Excel файл.");
        }

        public void WriteApplicantsWorksheet(ExcelWorksheet worksheet)
        {
            var stepNames = GetStepNames(applicantsReport.steps);

            //Write criterian
            MergeColumns(worksheet, "A1", 2, 2);
            worksheet.Cells["C1:I1"].Merge = true;
            worksheet.Cells["J1:L1"].Merge = true;
            MergeColumns(worksheet, "M1", 2, stepNames.Length);

            WriteValuesRow(worksheet, "A1", new string[] { "№", "ФИО", "Общая информация о кандидате" });
            worksheet.Cells["J1"].Value = "Активный этап";
            WriteValuesRow(worksheet, "C2", new string[] { "Зарплатные ожидания", "Город", "Теплый/\nхолодный", "Площадка", "Взят в работу", "Канал коммуникации", "Теги", "Результат", "Причина отказа", "Комментарий" });
            WriteValuesRow(worksheet, "M1", stepNames);

            //Write values
            for (int i = 0; i < applicantsReport.elements.Length; i++)
            {
                var element = applicantsReport.elements[i];
                WriteValuesRow(worksheet, $"A{i + 4}", new string[] { element.id.ToString(), element.name, element.wantedSalary, element.city, element.isHot ? "теплый" : "холодный", element.source, element.createDate.ToString("dd.MM.yyyy"), element.communicationChannels, element.tags, (element.currentStep == null || element.currentStep.name == null) ? "" : element.currentStep.name, element.rejectionReason, element.commentsLog });

                string[] stepDates = new string[applicantsReport.steps.Length];
                for (int s = 0; s < applicantsReport.steps.Length; s++)
                {
                    try
                    {
                        stepDates[s] = applicantsReport.elements[i].steps[applicantsReport.steps[s].id].date.ToString("dd.MM.yyyy");
                    }
                    catch (KeyNotFoundException ex)
                    {
                        stepDates[s] = "";
                    }
                }

                WriteValuesRow(worksheet, $"M{i + 4}", stepDates);
            }
        }

        public void AddApplicantsWorksheetStyles(ExcelWorksheet worksheet)
        {
            var criteriansWidth = new List<int> { 5, 25, 12, 15, 10, 10, 15, 20, 20, 15, 25, 35 };
            for (int i = 0; i < applicantsReport.steps.Length; i++)
            {
                criteriansWidth.Add(15);
            }
            ExcelStyler.SetColumnsWidth(worksheet, 1, criteriansWidth.ToArray());

            worksheet.Cells[1, 1, 2, criteriansWidth.Count].Style.WrapText = true;
            worksheet.Cells[1, 1, 2, criteriansWidth.Count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(2, 3), new Color[] { ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE1 }, 10);
            ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(1, 1), new Color[] { ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE1 }, 12 + applicantsReport.steps.Length);

            for (int i = 0; i < applicantsReport.elements.Length; i++)
            {
                var cells = worksheet.Cells[4 + i, 1, 4 + i, 12 + applicantsReport.steps.Length];
                if (i % 2 == 0)
                {
                    ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(4 + i, 1), new Color[] { ExcelStyler.ColorSchema.GREY1, ExcelStyler.ColorSchema.WHITE2 }, 12 + applicantsReport.steps.Length);
                }
                else
                {
                    ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(4 + i, 1), new Color[] { ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE1 }, 12 + applicantsReport.steps.Length);
                }
                
                cells.Style.WrapText = false;
                worksheet.Rows[i + 4].Height = 15;
            }

            worksheet.Cells[3, 1, 3, applicantsReport.steps.Length + 12].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY3);

            ExcelStyler.ApplyBorderToCell(worksheet.Cells[1, 1, applicantsReport.elements.Length + 3, applicantsReport.steps.Length + 12], OfficeOpenXml.Style.ExcelBorderStyle.Thin, ExcelStyler.BorderPosition.ALL);

            //var gradienColors = ExcelStyler.GetEvenlyDistributedColors(ExcelStyler.ColorSchema.YELLOW1_COLOR, ExcelStyler.ColorSchema.GREEN2_COLOR, settings.step_by_step.Length);
            var gradienColors = ExcelStyler.GetLightRainbowColors(settings.step_by_step.Length);

            int[] allStepsId = applicantsReport.steps.Select(p => p.id).ToArray();
            int c = 0;
            foreach (var stepId in settings.step_by_step)
            {
                worksheet.Cells[1, Array.IndexOf(allStepsId, stepId) + 13].Style.Fill.SetBackground(gradienColors[c]);
                c++;
            }

            for(int i = 0; i < applicantsReport.elements.Length; i++)
            {
                var element = applicantsReport.elements[i];
                try
                {
                    if (element.currentStep != null)
                    {
                        int index = Array.IndexOf(settings.step_by_step, element.currentStep.id);
                        if (index >= 0)
                        {
                            worksheet.Cells[i + 4, 1, i + 4, 12 + applicantsReport.steps.Length].Style.Fill.SetBackground(gradienColors[index]);
                        }
                    }
                }
                catch(KeyNotFoundException ex) { Console.WriteLine(ex); }
            }

            worksheet.View.FreezePanes(4, 1);
            worksheet.Cells[3, 1, applicantsReport.elements.Length + 4, applicantsReport.steps.Length + 12].AutoFilter = true;
        }

        public void AddPipeWorksheetStyles(ExcelWorksheet worksheet)
        {
            var criteriansWidth = new List<int> { 35, 7 };
            for (int i = 0; i < pipeReport.weeks.Length * 2; i++)
            {
                criteriansWidth.Add(7);
            }
            ExcelStyler.SetColumnsWidth(worksheet, 1, criteriansWidth.ToArray());

            var allRowsCount = pipeReport.migrationElements.Length + pipeReport.rejectReasonElements.Length + settings.step_by_step.Length * 2 + 5;
            var allColumnsCount = criteriansWidth.Count;

            worksheet.Cells[1, 1, 2, allColumnsCount].Style.WrapText = true;
            worksheet.Cells[1, 1, 2, allColumnsCount].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            for (int i = 0; i < allRowsCount; i++)
            {
                if (i % 2 == 0)
                {
                    ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(i + 1, 1), new Color[] { ExcelStyler.ColorSchema.WHITE1, ExcelStyler.ColorSchema.WHITE1, ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE2 }, pipeReport.weeks.Length * 2 + 2);
                }
                else
                {
                    ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(i + 1, 1), new Color[] { ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.GREY1, ExcelStyler.ColorSchema.GREY1 }, pipeReport.weeks.Length * 2 + 2);
                }
            }

            var stepsArray = pipeReport.stepDictinioary.Values.ToArray();
            for (int i = 0; i < pipeReport.migrationElements.Length; i++)
            {
                if (!settings.step_by_step.Contains(stepsArray[i].id))
                {
                    //ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(i + 4, 1), new Color[] { ExcelStyler.ColorSchema.GREY2, ExcelStyler.ColorSchema.GREY2, ExcelStyler.ColorSchema.GREY3, ExcelStyler.ColorSchema.GREY3 }, pipeReport.weeks.Length * 2 + 2);
                    worksheet.Cells[i + 4, 1].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);
                }
            }

            ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(1, 3), new Color[] { ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE1, ExcelStyler.ColorSchema.WHITE1 }, criteriansWidth.Count - 2);
            ExcelStyler.SetRowColors(worksheet, ExcelCellBase.GetAddress(2, 3), new Color[] { ExcelStyler.ColorSchema.RED2, ExcelStyler.ColorSchema.BLUE2 }, allColumnsCount - 2);

            for (int i = 2; i < allRowsCount; i++)
            {
                double num = 0.0;
                if (worksheet.Cells[i + 1, 2].Value != null && (double.TryParse(worksheet.Cells[i + 1, 2].Value.ToString(), out num) && num > 0))
                {
                    worksheet.Cells[i + 1, 2].Style.Fill.SetBackground(ExcelStyler.ColorSchema.YELLOW2);
                }
                else
                {
                    worksheet.Cells[i + 1, 2].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY1);
                }
            }

            for (int r = 3; r <= allRowsCount; r++)
            {
                for (int c = 3; c <= allColumnsCount; c++)
                {
                    double num = 0.0;
                    if (worksheet.Cells[r, c].Value != null && (double.TryParse(worksheet.Cells[r, c].Value.ToString(), out num) && num > 0))
                    {
                        if (r % 2 == 0)
                        {
                            if (c % 2 == 0)
                            {
                                worksheet.Cells[r, c].Style.Fill.SetBackground(ExcelStyler.ColorSchema.BLUE2);
                            }
                            else
                            {
                                worksheet.Cells[r, c].Style.Fill.SetBackground(ExcelStyler.ColorSchema.RED2);
                            }
                        }
                        else
                        {
                            if (c % 2 == 0)
                            {
                                worksheet.Cells[r, c].Style.Fill.SetBackground(ExcelStyler.ColorSchema.BLUE1);
                            }
                            else
                            {
                                worksheet.Cells[r, c].Style.Fill.SetBackground(ExcelStyler.ColorSchema.RED1);
                            }
                        }
                    }
                }
            }

            worksheet.Cells[pipeReport.migrationElements.Length + pipeReport.rejectReasonElements.Length + 5, 1, allRowsCount, allColumnsCount].Style.Numberformat.Format = "0.0";

            ExcelStyler.ApplyBorderToCell(worksheet.Cells[1, 1, pipeReport.migrationElements.Length + pipeReport.rejectReasonElements.Length + settings.step_by_step.Length * 2 + 5, pipeReport.weeks.Length * 2 + 2], OfficeOpenXml.Style.ExcelBorderStyle.Thin, ExcelStyler.BorderPosition.ALL);

            int[] a1RowIndexes = new int[] { 3, pipeReport.migrationElements.Length + 4, pipeReport.migrationElements.Length + pipeReport.rejectReasonElements.Length + 5, pipeReport.migrationElements.Length + pipeReport.rejectReasonElements.Length + settings.step_by_step.Length + 5 };
            
            int a = 0;
            foreach(var index in a1RowIndexes)
            {
                worksheet.Cells[index, 1].Style.Fill.SetBackground(ExcelStyler.ColorSchema.WHITE1);
                worksheet.Cells[index, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                a++;
            }

            worksheet.Cells[3, 3, pipeReport.migrationElements.Length + pipeReport.rejectReasonElements.Length + 5, pipeReport.weeks.Length * 2 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            for (int r = pipeReport.migrationElements.Length + pipeReport.rejectReasonElements.Length + 5; r < allRowsCount + 1; r++)
            {
                for (int c = 2; c < pipeReport.weeks.Length * 2 + 3; c++)
                {
                    var cell = worksheet.Cells[r, c];
                    double num = 0.0;
                    if (cell.Value != null)
                    {
                        if (cell.Value.ToString().Equals("-"))
                        {
                            cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);
                            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        else if (double.TryParse(cell.Value.ToString(), out num) && num == 0.0)
                        {
                            cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY1);
                        }
                    }
                }
            }

            worksheet.View.FreezePanes(3, 2);
        }

        private void WritePipeWorksheet(ExcelWorksheet worksheet)
        {
            //Write criterions
            var conversionNames = new string[settings.step_by_step.Length - 1];
            for (int i = 1; i < settings.step_by_step.Length; i++)
            {
                var aStepName = pipeReport.stepDictinioary[settings.step_by_step[i - 1]].name;
                var bStepName = pipeReport.stepDictinioary[settings.step_by_step[i]].name;
                conversionNames[i - 1] = $"{aStepName} -> {bStepName}";
            }

            MergeColumns(worksheet, "A1", 2, 2);
            MergeRows(worksheet, "C1", 2, pipeReport.weeks.Length);
            worksheet.Cells[3, 1, 3, pipeReport.weeks.Length * 2 + 2].Merge = true;
            worksheet.Cells[pipeReport.stepNames.Length + 4, 1, pipeReport.stepNames.Length + 4, pipeReport.weeks.Length * 2 + 2].Merge = true;
            worksheet.Cells[pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length + 5, 1, pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length + 5, pipeReport.weeks.Length * 2 + 2].Merge = true;
            worksheet.Cells[pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length + conversionNames.Length + 6, 1, pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length + conversionNames.Length + 6, pipeReport.weeks.Length * 2 + 2].Merge = true;

            WriteValuesRow(worksheet, "A1", new string[] { "Название", "Итого" });

            string[] weeksCriterions = new string[pipeReport.weeks.Length];
            for (int i = 0; i < pipeReport.weeks.Length; i++)
            {
                weeksCriterions[i] = $"{pipeReport.weeks[i].startDay.ToString("dd.MM.yyyy")} - \n{pipeReport.weeks[i].endDay.ToString("dd.MM.yyyy")}";
            }
            WriteValuesRow(worksheet, "C1", weeksCriterions, 2);

            string[] hotAndCold = new string[pipeReport.weeks.Length * 2];
            for (int i = 0; i < pipeReport.weeks.Length * 2; i += 2)
            {
                hotAndCold[i] = "тепл.";
                hotAndCold[i + 1] = "холод.";
            }
            WriteValuesRow(worksheet, "C2", hotAndCold);
            worksheet.Cells["A3"].Value = "Миграция кандидатов, чел.";
            WriteValuesColumn(worksheet, "A4", pipeReport.stepNames);

            worksheet.Cells[pipeReport.stepNames.Length + 4, 1].Value = "Причины отказа, ед.";
            WriteValuesColumn(worksheet, worksheet.Cells[5 + pipeReport.stepNames.Length, 1].Address, pipeReport.rejectReasonNames);
            worksheet.Cells[pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length + 5, 1].Value = "Конверсии из этапа в этап, %";

            WriteValuesColumn(worksheet, worksheet.Cells[6 + pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length, 1].Address, conversionNames);
            worksheet.Cells[pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length + conversionNames.Length + 6, 1].Value = "Воронка, %";

            var stepByStepNames = new string[settings.step_by_step.Length];
            for (int i = 0; i < stepByStepNames.Length; i++)
            {
                stepByStepNames[i] = pipeReport.stepDictinioary[settings.step_by_step[i]].name;
            }
            WriteValuesColumn(worksheet, worksheet.Cells[7 + pipeReport.stepNames.Length + pipeReport.rejectReasonNames.Length + conversionNames.Length, 1].Address, stepByStepNames);

            //Write values
            for (int s = 0; s < pipeReport.migrationElements.Length; s++)
            {
                for (int c = 0; c < pipeReport.migrationElements[s].periods.Length; c++)
                {
                    int hotCount = pipeReport.migrationElements[s].periods[c].hotApplicantsCount;
                    int coldCount = pipeReport.migrationElements[s].periods[c].coldApplicantsCount;
                    worksheet.Cells[s + 4, c * 2 + 3].Value = hotCount;
                    worksheet.Cells[s + 4, c * 2 + 4].Value = coldCount;
                }
            }

            for (int i = 0; i < pipeReport.migrationElements.Length; i++)
            {
                worksheet.Cells[i + 4, 2].Formula = $"=SUM({ExcelCellBase.GetAddress(i + 4, 3)}:{ExcelCellBase.GetAddress(i + 4, 2 + pipeReport.weeks.Length * 2)})";
            }

            var stepsCount = pipeReport.stepNames.Length;
            for (int s = 0; s < pipeReport.rejectReasonElements.Length; s++)
            {
                for (int c = 0; c < pipeReport.rejectReasonElements[s].periods.Length; c++)
                {
                    int hotCount = pipeReport.rejectReasonElements[s].periods[c].hotRejectionReasons;
                    int coldCount = pipeReport.rejectReasonElements[s].periods[c].coldRejectionReasons;
                    worksheet.Cells[s + 5 + stepsCount, c * 2 - 1 + 4].Value = hotCount;
                    worksheet.Cells[s + 5 + stepsCount, c * 2 - 1 + 5].Value = coldCount;
                }
            }

            for (int i = 0; i < pipeReport.rejectReasonElements.Length; i++)
            {
                worksheet.Cells[i + stepsCount + 5, 2].Formula = $"=SUM({ExcelCellBase.GetAddress(i + stepsCount + 5, 3)}:{ExcelCellBase.GetAddress(i + stepsCount + 5, 2 + pipeReport.weeks.Length * 2)})";
            }

            var stepInds = new int[pipeReport.stepDictinioary.Count];
            var d = 0;
            foreach (var step in pipeReport.stepDictinioary)
            {
                stepInds[d] = step.Value.id;
                d++;
            }


            for (int i = 1; i < settings.step_by_step.Length; i++)
            {
                string aColdFormula = "";
                string bColdFolrmula = "";
                string aHotFormula = "";
                string bHotFormula = "";
                int rowA = Array.IndexOf(stepInds, settings.step_by_step[i - 1]) + 4;
                int rowB = Array.IndexOf(stepInds, settings.step_by_step[i]) + 4;
                var aHotList = new List<string>();
                var aColdList = new List<string>();

                for (int p = 0; p < pipeReport.weeks.Length; p++)
                {
                    if (p != 0)
                    {
                        aHotFormula += "+"; aColdFormula += "+";
                        bHotFormula += "+"; bColdFolrmula += "+";
                    }
                    aHotFormula += ExcelCellBase.GetAddress(rowA, p * 2 + 3);  aColdFormula += ExcelCellBase.GetAddress(rowA, p * 2 + 4);
                    bHotFormula += ExcelCellBase.GetAddress(rowB, p * 2 + 3);  bColdFolrmula += ExcelCellBase.GetAddress(rowB, p * 2 + 4);
                    aHotList.Add(ExcelCellBase.GetAddress(rowA, p * 2 + 3));
                    aColdList.Add(ExcelCellBase.GetAddress(rowA, p * 2 + 4));

                    var hotCell = worksheet.Cells[6 + stepsCount + pipeReport.rejectReasonElements.Length + i - 1, p * 2 + 3];
                    var coldCell = worksheet.Cells[6 + stepsCount + pipeReport.rejectReasonElements.Length + i - 1, p * 2 + 4];

                    if (allValuesIsZero(worksheet, aHotList))
                    {
                        hotCell.Value = "-";
                    }
                    else
                    {
                        hotCell.Formula = $"=({bHotFormula})/({aHotFormula})*100";
                    }
                    if (allValuesIsZero(worksheet, aColdList))
                    {
                        coldCell.Value = "-";
                    }
                    else
                    {
                        coldCell.Formula = $"=({bColdFolrmula})/({aColdFormula})*100";
                    }
                }
            }

            worksheet.Calculate();

            for (int i = 1; i < settings.step_by_step.Length; i++)
            {
                int rowA = Array.IndexOf(stepInds, settings.step_by_step[i - 1]) + 4;
                int rowB = Array.IndexOf(stepInds, settings.step_by_step[i]) + 4;
                int num = 0;
                if (int.TryParse(worksheet.Cells[rowA, 2].Value.ToString(), out num) && num != 0)
                {
                    worksheet.Cells[6 + stepsCount + pipeReport.rejectReasonElements.Length + i - 1, 2].Formula = $"=({ExcelCellBase.GetAddress(rowB, 2)})/({ExcelCellBase.GetAddress(rowA, 2)})*100";
                }
                else
                {
                    worksheet.Cells[6 + stepsCount + pipeReport.rejectReasonElements.Length + i - 1, 2].Value = "-";
                }
            }

            for (int i = 0; i < settings.step_by_step.Length; i++)
            {
                string aColdFormula = "";
                string bColdFolrmula = "";
                string aHotFormula = "";
                string bHotFormula = "";
                int rowA = Array.IndexOf(stepInds, settings.step_by_step[0]) + 4;
                int rowB = Array.IndexOf(stepInds, settings.step_by_step[i]) + 4;
                var aHotList = new List<string>();
                var aColdList = new List<string>();

                for (int p = 0; p < pipeReport.weeks.Length; p++)
                {
                    if (p != 0)
                    {
                        aHotFormula += "+"; aColdFormula += "+";
                        bHotFormula += "+"; bColdFolrmula += "+";
                    }
                    aHotFormula += ExcelCellBase.GetAddress(rowA, p * 2 + 3); aColdFormula += ExcelCellBase.GetAddress(rowA, p * 2 + 4);
                    bHotFormula += ExcelCellBase.GetAddress(rowB, p * 2 + 3); bColdFolrmula += ExcelCellBase.GetAddress(rowB, p * 2 + 4);
                    aHotList.Add(ExcelCellBase.GetAddress(rowA, p * 2 + 3));
                    aColdList.Add(ExcelCellBase.GetAddress(rowA, p * 2 + 4));

                    var hotCell = worksheet.Cells[7 + stepsCount + settings.step_by_step.Length + pipeReport.rejectReasonElements.Length + i - 1, p * 2 + 3];
                    var coldCell = worksheet.Cells[7 + stepsCount + settings.step_by_step.Length + pipeReport.rejectReasonElements.Length + i - 1, p * 2 + 4];

                    if (allValuesIsZero(worksheet, aHotList))
                    {
                        hotCell.Value = "-";
                    }
                    else
                    {
                        hotCell.Formula = $"=({bHotFormula})/({aHotFormula})*100";
                    }
                    if (allValuesIsZero(worksheet, aColdList))
                    {
                        coldCell.Value = "-";
                    }
                    else
                    {
                        coldCell.Formula = $"=({bColdFolrmula})/({aColdFormula})*100";
                    }
                }
            }

            for (int i = 0; i < settings.step_by_step.Length; i++)
            {
                int rowA = Array.IndexOf(stepInds, settings.step_by_step[0]) + 4;
                int rowB = Array.IndexOf(stepInds, settings.step_by_step[i]) + 4;
                int num = 0;
                if (int.TryParse(worksheet.Cells[rowA, 2].Value.ToString(), out num) && num != 0)
                {
                    worksheet.Cells[7 + stepsCount + settings.step_by_step.Length + pipeReport.rejectReasonElements.Length + i - 1, 2].Formula = $"=({ExcelCellBase.GetAddress(rowB, 2)})/({ExcelCellBase.GetAddress(rowA, 2)})*100";
                }
                else
                {
                    worksheet.Cells[7 + stepsCount + settings.step_by_step.Length + pipeReport.rejectReasonElements.Length + i - 1, 2].Value = "-";
                }
            }

            worksheet.Calculate();

            for (int i = 0; i < settings.step_by_step.Length; i++)
            {
                for (int p = 0; p < pipeReport.weeks.Length * 2 + 1; p++)
                {
                    var cell = worksheet.Cells[7 + stepsCount + settings.step_by_step.Length + pipeReport.rejectReasonElements.Length + i - 1, p + 2];
                    double num = 0;
                    if (double.TryParse(cell.Value.ToString(), out num) && num == 0.0)
                    {
                        cell.Value = "-";
                    }
                }
            }
        }

        private void WriteDashboardWorksheet(ExcelWorksheet worksheet)
        {
            var today = DateTime.Today;
            WriteApplicantSegmentsTable(worksheet, "A1", today);
            WriteRejectReasonsTable(worksheet, "F1", today);
            WriteInterviewTable(worksheet, "L1", today);
            WriteOfferTable(worksheet, "P1", today);

            worksheet.Calculate();

            AddSegmentsTableStyle(worksheet, "A1");
            AddRejectionReasonsTableStyle(worksheet, "F1");
            AddInterwievTableStyle(worksheet, "L1");
            AddOfferTableStyle(worksheet, "P1");

            ExcelStyler.SetColumnsWidth(worksheet, 1, new int[] { 10, 10, 10, 10, 10, 20, 20, 20, 20, 20, 20, 25, 7, 7, 7, 35, 7 });
        }

        private void WriteOfferTable(ExcelWorksheet worksheet, string cellAdress, DateTime date)
        {
            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            worksheet.Cells[startRow, startColumn, startRow, startColumn + 1].Merge = true;
            worksheet.Cells[startRow + 1, startColumn, startRow + 1, startColumn + 1].Merge = true;

            WriteValuesColumn(worksheet, cellAdress, new string[] { $"[{date.ToString("dd.MM.yyyy")}] {dashboardReport.vacancyName}", "Офферы" });
            WriteValuesColumn(worksheet, ExcelCellBase.GetAddress(startRow + 2, startColumn), new string[] { "офферов выставлено", "отказов от оффера", "офферов отозвали" });
            WriteValuesColumn(worksheet, ExcelCellBase.GetAddress(startRow + 2, startColumn + 1), new string[] { dashboardReport.offerCount.ToString(), dashboardReport.applicantRejectOfOfferCount.ToString(), dashboardReport.companyRejectOfOfferCount.ToString() });
        }

        private void WriteInterviewTable(ExcelWorksheet worksheet, string cellAdress, DateTime date)
        {
            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            worksheet.Cells[startRow, startColumn, startRow, startColumn + 3].Merge = true;
            worksheet.Cells[startRow + 1, startColumn, startRow + 1, startColumn + 3].Merge = true;

            WriteValuesColumn(worksheet, cellAdress, new string[] { $"[{date.ToString("dd.MM.yyyy")}] {dashboardReport.vacancyName}", "Интервью" });
            WriteValuesRow(worksheet, ExcelCellBase.GetAddress(startRow + 2, startColumn), new string[] { "тип", "кол-во, холод.", "кол-во, тепл.", "всего" });

            for (int i = 0; i < dashboardReport.oldInterviewCounts.Length; i++)
            {
                var count = dashboardReport.oldInterviewCounts[i];
                WriteValuesRow(worksheet, ExcelCellBase.GetAddress(startRow + 3 + i, startColumn), new string[] { count.name, count.cold.ToString(), count.hot.ToString() });
                worksheet.Cells[startRow + 3 + i, startColumn + 3].Formula = $"=SUM({ExcelCellBase.GetAddress(startRow + 3 + i, startColumn + 1)}:{ExcelCellBase.GetAddress(startRow + 3 + i, startColumn + 2)})";
            }
        }

        private void WriteApplicantSegmentsTable(ExcelWorksheet worksheet, string cellAdress, DateTime date)
        {
            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            worksheet.Cells[startRow, startColumn, startRow, startColumn + 4].Merge = true;
            worksheet.Cells[startRow + 1, startColumn, startRow + 1, startColumn + 4].Merge = true;

            WriteValuesColumn(worksheet, cellAdress, new string[] { $"[{date.ToString("dd.MM.yyyy")}] {dashboardReport.vacancyName}", "Кандидаты" });
            WriteValuesRow(worksheet, worksheet.Cells[startRow + 2, startColumn].Address, new string[] { "", "всего", "холодные", "теплые", "рефералы" });
            WriteValuesRow(worksheet, worksheet.Cells[startRow + 3, startColumn].Address, new string[] { "чел.", "", dashboardReport.coldApplicantsCount.ToString(), dashboardReport.hotApplicantsCount.ToString(), dashboardReport.referalApplicantCount.ToString() });
            worksheet.Cells[startRow + 3, startColumn + 1].Formula = $"=SUM({ExcelCellBase.GetAddress(startRow + 3, startColumn + 2)}:{ExcelCellBase.GetAddress(startRow + 3, startColumn + 4)})";
            WriteValuesRow(worksheet, worksheet.Cells[startRow + 4, startColumn].Address, new string[] { "%" });
            var allAdress = ExcelCellBase.GetAddress(startRow + 3, startColumn + 1);
            var formulas = new string[4];
            for (int i = 0; i < formulas.Length; i++)
            {
                formulas[i] = $"={ExcelCellBase.GetAddress(startRow + 3, startColumn + i + 1)}/{allAdress}*100";
            }
            WriteFormulaRow(worksheet, worksheet.Cells[startRow + 4, startColumn + 1].Address, formulas);

        }

        private void AddSegmentsTableStyle(ExcelWorksheet worksheet, string cellAdress)
        {
            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            ExcelStyler.ApplyBorderToCell(worksheet.Cells[startRow, startColumn, startRow + 4, startColumn + 4], OfficeOpenXml.Style.ExcelBorderStyle.Thin, ExcelStyler.BorderPosition.ALL);

            for (int i = 0; i < 5; i++)
            {
                ExcelStyler.SetRowColors(worksheet, ExcelAddressBase.GetAddress(startRow + i, startColumn), new Color[] { ExcelStyler.ColorSchema.WHITE1, ExcelStyler.ColorSchema.WHITE2 }, 5);
            }

            worksheet.Cells[startRow, startColumn].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);
            worksheet.Cells[startRow + 4, startColumn + 2, startRow + 4, startColumn + 4].Style.Numberformat.Format = "0.0";

            for (int r = startRow + 3; r <= 5; r++)
            {
                var cell = worksheet.Cells[r, startColumn + 1];
                double num = 0;
                if (cell.Value != null && double.TryParse(cell.Value.ToString(), out num) && num > 0)
                {
                    if (r % 2 == 0)
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.YELLOW2);
                    }
                    else
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.YELLOW1);
                    }
                }
            }

            for (int r = startRow + 3; r <= 5; r++)
            {
                var cell = worksheet.Cells[r, startColumn + 2];
                double num = 0;
                if (cell.Value != null && double.TryParse(cell.Value.ToString(), out num) && num > 0)
                {
                    if (r % 2 == 0)
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.BLUE2);
                    }
                    else
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.BLUE1);
                    }
                }
            }

            for (int r = startRow + 3; r <= 5; r++)
            {
                var cell = worksheet.Cells[r, startColumn + 3];
                double num = 0;
                if (cell.Value != null && double.TryParse(cell.Value.ToString(), out num) && num > 0)
                {
                    if (r % 2 == 0)
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.RED2);
                    }
                    else
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.RED1);
                    }
                }
            }

            for (int r = startRow + 3; r <= 5; r++)
            {
                var cell = worksheet.Cells[r, startColumn + 4];
                double num = 0;
                if (cell.Value != null && double.TryParse(cell.Value.ToString(), out num) && num > 0)
                {
                    if (r % 2 == 0)
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREEN2);
                    }
                    else
                    {
                        cell.Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREEN1);
                    }
                }
            }
        }

        private void AddRejectionReasonsTableStyle(ExcelWorksheet worksheet, string cellAdress)
        {
            var companyCount = 0;
            var applicantCount = 0;

            foreach (var reason in dashboardReport.applicantRejectReasonCounts)
            {
                if (reason.count != 0)
                {
                    applicantCount++;
                }
            }

            foreach (var reason in dashboardReport.companyRejectReasonCounts)
            {
                if (reason.count != 0)
                {
                    companyCount++;
                }
            }

            int maxReasonsCount = companyCount;
            int minReasonsCount = applicantCount;
            bool applicantIsMax = false;
            if (maxReasonsCount < applicantCount)
            {
                maxReasonsCount = applicantCount;
                minReasonsCount = companyCount;
                applicantIsMax = true;
            }
            maxReasonsCount++;
            minReasonsCount++;

            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            ExcelStyler.ApplyBorderToCell(worksheet.Cells[startRow, startColumn, startRow + maxReasonsCount + 7, startColumn + 5], OfficeOpenXml.Style.ExcelBorderStyle.Thin, ExcelStyler.BorderPosition.ALL);

            for (int i = 0; i < maxReasonsCount + 8; i++)
            {
                if (i % 2 == 0)
                {
                    worksheet.Cells[startRow + i, startColumn, startRow + i, startColumn + 5].Style.Fill.SetBackground(ExcelStyler.ColorSchema.WHITE2);
                }
                else
                {
                    worksheet.Cells[startRow + i, startColumn, startRow + i, startColumn + 5].Style.Fill.SetBackground(ExcelStyler.ColorSchema.WHITE1);
                }
                //ExcelStyler.SetRowColors(worksheet, ExcelAddressBase.GetAddress(startRow + i, startColumn), new Color[] { ExcelStyler.ColorSchema.WHITE1, ExcelStyler.ColorSchema.WHITE2 }, 6);
            }

            worksheet.Cells[startRow, startColumn].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);

            if (maxReasonsCount != 0 && maxReasonsCount != minReasonsCount)
            {
                if (applicantIsMax)
                {
                    worksheet.Cells[startRow + minReasonsCount + 4, startColumn + 3, startRow + maxReasonsCount + 3, startColumn + 5].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);
                }
                else
                {
                    worksheet.Cells[startRow + minReasonsCount + 3, startColumn, startRow + maxReasonsCount + 3, startColumn + 2].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);
                }
            }
            

            worksheet.Cells[1, 1].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);

            for (int r = 0; r < maxReasonsCount; r++)
            {
                worksheet.Cells[startRow + r + 4, startColumn + 2].Style.Numberformat.Format = "0.0";
                worksheet.Cells[startRow + r + 4, startColumn + 5].Style.Numberformat.Format = "0.0";
            }

            for (int i = 0; i < 2; i++)
            {
                int parseCount = applicantCount;
                if (i > 0)
                {
                    parseCount = companyCount;
                }
                var cell = worksheet.Cells[startRow + parseCount + 4, startColumn + i * 3 + 1];
                double num = 0;
                if (cell.Value != null && double.TryParse(cell.Value.ToString(), out num) && num > 0)
                {
                    ExcelStyler.SetRowColors(worksheet, cell.Address, new Color[] { ExcelStyler.ColorSchema.YELLOW2, ExcelStyler.ColorSchema.YELLOW1 }, 2);
                }
            }

            int startRowTable2 = startRow + maxReasonsCount + 4;

            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    ExcelStyler.SetRowColors(worksheet, ExcelAddressBase.GetAddress(startRowTable2 + i, startColumn), new Color[] { ExcelStyler.ColorSchema.WHITE1, ExcelStyler.ColorSchema.WHITE1, ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE2 }, 6);
                }
                else
                {
                    ExcelStyler.SetRowColors(worksheet, ExcelAddressBase.GetAddress(startRowTable2 + i, startColumn), new Color[] { ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.WHITE2, ExcelStyler.ColorSchema.GREY1, ExcelStyler.ColorSchema.GREY1 }, 6);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                worksheet.Cells[startRowTable2 + 3, startColumn + i * 2].Style.Fill.SetBackground(ExcelStyler.ColorSchema.YELLOW2);
                worksheet.Cells[startRowTable2 + 3, startColumn + i * 2 + 1].Style.Fill.SetBackground(ExcelStyler.ColorSchema.YELLOW1);
                worksheet.Cells[startRowTable2 + 3, startColumn + i * 2 + 1].Style.Numberformat.Format = "0.0";
            }
        }

        private void AddInterwievTableStyle(ExcelWorksheet worksheet, string cellAdress)
        {
            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            ExcelStyler.ApplyBorderToCell(worksheet.Cells[startRow, startColumn, startRow + 8, startColumn + 3], OfficeOpenXml.Style.ExcelBorderStyle.Thin, ExcelStyler.BorderPosition.ALL);

            for (int i = 0; i < 9; i++)
            {
                if (i % 2 == 0)
                {
                    worksheet.Cells[startRow + i, startColumn, startRow + i, startColumn + 3].Style.Fill.SetBackground(ExcelStyler.ColorSchema.WHITE2);
                }
                else
                {
                    worksheet.Cells[startRow + i, startColumn, startRow + i, startColumn + 3].Style.Fill.SetBackground(ExcelStyler.ColorSchema.WHITE1);
                }
            }

            worksheet.Cells[startRow, startColumn].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);
        }

        private void AddOfferTableStyle(ExcelWorksheet worksheet, string cellAdress)
        {
            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            ExcelStyler.ApplyBorderToCell(worksheet.Cells[startRow, startColumn, startRow + 4, startColumn + 1], OfficeOpenXml.Style.ExcelBorderStyle.Thin, ExcelStyler.BorderPosition.ALL);

            for (int i = 0; i < 5; i++)
            {
                if (i % 2 == 0)
                {
                    worksheet.Cells[startRow + i, startColumn, startRow + i, startColumn + 1].Style.Fill.SetBackground(ExcelStyler.ColorSchema.WHITE2);
                }
                else
                {
                    worksheet.Cells[startRow + i, startColumn, startRow + i, startColumn + 1].Style.Fill.SetBackground(ExcelStyler.ColorSchema.WHITE1);
                }
            }

            worksheet.Cells[startRow, startColumn].Style.Fill.SetBackground(ExcelStyler.ColorSchema.GREY2);
        }

        private void WriteRejectReasonsTable(ExcelWorksheet worksheet, string cellAdress, DateTime date)
        {
            var startCell = worksheet.Cells[cellAdress];
            int startColumn = startCell.Start.Column;
            int startRow = startCell.Start.Row;

            worksheet.Cells[startRow, startColumn, startRow, startColumn + 5].Merge = true;
            worksheet.Cells[startRow + 1, startColumn, startRow + 1, startColumn + 5].Merge = true;

            WriteValuesColumn(worksheet, cellAdress, new string[] { $"[{date.ToString("dd.MM.yyyy")}] {dashboardReport.vacancyName}", "Причины отказов, детализация" });

            WriteValuesRow(worksheet, worksheet.Cells[startRow + 2, startColumn].Address, new string[] { "от кандидата", "от компании" }, 3);
            worksheet.Cells[startRow + 2, startColumn, startRow + 2, startColumn + 2].Merge = true;
            worksheet.Cells[startRow + 2, startColumn + 3, startRow + 2, startColumn + 5].Merge = true;

            for (int i = 0; i < 2; i++)
            {
                WriteValuesRow(worksheet, ExcelCellBase.GetAddress(startRow + 3, startColumn + i * 3), new string[] { "причина", "ед.", "%" });
            }

            var rejectReasonsFromApplicant = dashboardReport.applicantRejectReasonCounts;
            var rejectReasonsFromCompany = dashboardReport.companyRejectReasonCounts;

            int indent1 = 0;
            for (int i = 0; i < rejectReasonsFromApplicant.Length; i++)
            {
                if (rejectReasonsFromApplicant[i].count != 0)
                {
                    WriteValuesRow(worksheet, ExcelCellBase.GetAddress(startRow + 4 + indent1, startColumn), new string[] { rejectReasonsFromApplicant[i].name, rejectReasonsFromApplicant[i].count.ToString() });
                    indent1++;
                }
            }
            var applicantRejectReasonsCountCell = worksheet.Cells[startRow + 4 + indent1, startColumn + 1];
            worksheet.Cells[startRow + 4 + indent1, startColumn].Value = "Всего";
            if (indent1 > 0)
            {
                applicantRejectReasonsCountCell.Formula = $"=SUM({ExcelCellBase.GetAddress(startRow + 4, startColumn + 1)}:{ExcelCellBase.GetAddress(startRow + 3 + indent1, startColumn + 1)})";

                for (int i = 0; i <= indent1; i++)
                {
                    worksheet.Cells[startRow + 4 + i, startColumn + 2].Formula = $"{ExcelCellBase.GetAddress(startRow + 4 + i, startColumn + 1)}/{applicantRejectReasonsCountCell.Address}*100";
                }
            }
            else
            {
                applicantRejectReasonsCountCell.Value = 0;
                worksheet.Cells[startRow + 4 + indent1, startColumn + 2].Value = 0;
            }

            var indent2 = 0;
            for (int i = 0; i < rejectReasonsFromCompany.Length; i++)
            {
                if (rejectReasonsFromCompany[i].count != 0)
                {
                    WriteValuesRow(worksheet, ExcelCellBase.GetAddress(startRow + 4 + indent2, startColumn + 3), new string[] { rejectReasonsFromCompany[i].name, rejectReasonsFromCompany[i].count.ToString() });
                    indent2++;
                }
            }
            var companyRejectReasonsCountCell = worksheet.Cells[startRow + 4 + indent2, startColumn + 4];
            worksheet.Cells[startRow + 4 + indent2, startColumn + 3].Value = "Всего";
            if (indent2 > 0)
            {
                companyRejectReasonsCountCell.Formula = $"=SUM({ExcelCellBase.GetAddress(startRow + 4, startColumn + 4)}:{ExcelCellBase.GetAddress(startRow + 3 + indent2, startColumn + 4)})";

                for (int i = 0; i <= indent2; i++)
                {
                    worksheet.Cells[startRow + 4 + i, startColumn + 5].Formula = $"{ExcelCellBase.GetAddress(startRow + 4 + i, startColumn + 4)}/{companyRejectReasonsCountCell.Address}*100";
                }
            }
            else
            {
                companyRejectReasonsCountCell.Value = 0;
                worksheet.Cells[startRow + 4 + indent2, startColumn + 5].Value = 0;
            }

            int indent = indent1;
            if (indent2 > indent)
            {
                indent = indent2;
            }

            worksheet.Cells[startRow + 5 + indent, startColumn, startRow + 5 + indent, startColumn + 5].Merge = true;
            MergeRows(worksheet, ExcelCellBase.GetAddress(startRow + 6 + indent, startColumn), 2, 3);

            worksheet.Cells[startRow + 5 + indent, startColumn].Value = "Причины отказов, общее";
            WriteValuesRow(worksheet, ExcelCellBase.GetAddress(startRow + 6 + indent, startColumn), new string[] { "от кандидата", "от компании", "по другой причине" }, 2);

            for (int i = 0; i < 3; i++)
            {
                WriteValuesRow(worksheet, ExcelCellBase.GetAddress(startRow + 7 + indent, startColumn + i * 2), new string[] { "ед.", "%" });
            }

            WriteFormulaRow(worksheet, ExcelCellBase.GetAddress(startRow + 8 + indent, startColumn), new string[] { $"={applicantRejectReasonsCountCell.Address}", $"={companyRejectReasonsCountCell.Address}", dashboardReport.otherRejectReasonCount.ToString() }, 2);
            var allFormula = $"({applicantRejectReasonsCountCell.Address}+{companyRejectReasonsCountCell.Address}+{dashboardReport.otherRejectReasonCount})";
            WriteFormulaRow(worksheet, ExcelCellBase.GetAddress(startRow + 8 + indent, startColumn + 1), new string[] { $"={applicantRejectReasonsCountCell.Address}/{allFormula}*100", $"={companyRejectReasonsCountCell.Address}/{allFormula}*100", $"{dashboardReport.otherRejectReasonCount}/{allFormula}*100" }, 2);
        }

        private static bool allValuesIsZero(ExcelWorksheet worksheet, List<string> cellAdresses)
        {
            foreach (var cell in cellAdresses)
            {
                int value;
                if (!int.TryParse(worksheet.Cells[cell].Value.ToString(), out value) || value != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static string[] GetStepNames(ApplicantStep[] steps)
        {
            var result = new string[steps.Length];
            for (int i = 0; i < steps.Length; i++)
            {
                result[i] = steps[i].name;
            }
            return result;
        }

        private static void WriteValuesRow(ExcelWorksheet worksheet, string startCell, string[] values)
        {
            WriteValuesRow(worksheet, startCell, values, 1);
        }

        private static void WriteValuesRow(ExcelWorksheet worksheet, string startCell, string[] values, int spaceCount)
        {
            int column = startCell[0] - 'A' + 1;
            int row = int.Parse(startCell.Substring(1));

            for (int i = 0; i < values.Length; i++)
            {
                int intValue = 0;
                if (int.TryParse(values[i], out intValue))
                {
                    worksheet.Cells[row, column + i * spaceCount].Value = intValue;
                }
                else
                {
                    worksheet.Cells[row, column + i * spaceCount].Value = values[i];
                }
            }
        }

        private static void WriteFormulaRow(ExcelWorksheet worksheet, string startCell, string[] values)
        {
            WriteFormulaRow(worksheet, startCell, values, 1);
        }

        private static void WriteFormulaRow(ExcelWorksheet worksheet, string startCell, string[] values, int spaceCount)
        {
            int column = startCell[0] - 'A' + 1;
            int row = int.Parse(startCell.Substring(1));

            for (int i = 0; i < values.Length; i++)
            {
                worksheet.Cells[row, column + i * spaceCount].Formula = values[i];
            }
        }

        private static void WriteValuesColumn(ExcelWorksheet worksheet, string startCell, string[] values)
        {
            int row = 1;
            foreach (string value in values)
            {
                worksheet.Cells[startCell].Offset(row - 1, 0).Value = value;
                row++;
            }
        }

        private static void MergeColumns(ExcelWorksheet worksheet, string startCell, int cellCount, int columnCount)
        {
            var cellAdress = worksheet.Cells[startCell];
            int startColumn = cellAdress.Start.Column;
            int startRow = cellAdress.Start.Row;
            
            for (int i = 0; i < columnCount; i++)
            {
                worksheet.Cells[startRow, startColumn + i, startRow + (cellCount - 1), startColumn + i].Merge = true;
            }
        }

        private static void MergeRows(ExcelWorksheet worksheet, string startCell, int cellCount, int rowCount)
        {
            var cellAdress = worksheet.Cells[startCell];
            int startColumn = cellAdress.Start.Column;
            int startRow = cellAdress.Start.Row;

            for (int i = 0; i < rowCount; i++)
            {
                worksheet.Cells[startRow, startColumn + i * cellCount, startRow, startColumn + (i + 1) * cellCount - 1].Merge = true;
            }
        }
    }
}
