using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace HuntflowAPI.Office
{
    public class ExcelStyler
    {
        private ExcelStyler() { }

        public static void ApplyBorderToCell(ExcelRange cell, ExcelBorderStyle borderStyle, BorderPosition borderPosition)
        {
            switch (borderPosition)
            {
                case BorderPosition.TOP:
                    cell.Style.Border.Top.Style = borderStyle;
                    break;
                case BorderPosition.BOTTOM:
                    cell.Style.Border.Bottom.Style = borderStyle;
                    break;
                case BorderPosition.LEFT:
                    cell.Style.Border.Left.Style = borderStyle;
                    break;
                case BorderPosition.RIGHT:
                    cell.Style.Border.Right.Style = borderStyle;
                    break;
                case BorderPosition.ALL:
                    cell.Style.Border.Top.Style = borderStyle;
                    cell.Style.Border.Bottom.Style = borderStyle;
                    cell.Style.Border.Left.Style = borderStyle;
                    cell.Style.Border.Right.Style = borderStyle;
                    break;
                default:
                    break;
            }
        }

        public static Color[] GetEvenlyDistributedColors(Color startColor, Color endColor, int resultCount)
        {
            Color[] colors = new Color[resultCount];

            for (int i = 0; i < resultCount; i++)
            {
                float ratio = (float)i / (resultCount - 1);

                int a = (int)(startColor.A + ratio * (endColor.A - startColor.A));
                int r = (int)(startColor.R + ratio * (endColor.R - startColor.R));
                int g = (int)(startColor.G + ratio * (endColor.G - startColor.G));
                int b = (int)(startColor.B + ratio * (endColor.B - startColor.B));

                colors[i] = Color.FromArgb(a, r, g, b);
            }

            return colors;
        }

        public static void SetColumnsWidth(ExcelWorksheet worksheet, int startColumnIndex, int[] widthArray)
        {
            for (int i = 0; i < widthArray.Length; i++)
            {
                worksheet.Columns[startColumnIndex + i].Width = widthArray[i];
            }
        }

        public static void SetRowColor(ExcelWorksheet worksheet, string cellAdress, Color color, int countCell, int spaceCount)
        {
            var cell = worksheet.Cells[cellAdress];
            int startRow = cell.Start.Row;
            int startColumn = cell.Start.Column;

            for (int i = 0; i < countCell; i++)
            {
                worksheet.Cells[startRow, startColumn + i * spaceCount].Style.Fill.SetBackground(color);
            }
        }

        public static void SetRowColors(ExcelWorksheet worksheet, string cellAdress, Color[] colors, int countCell)
        {
            var cell = worksheet.Cells[cellAdress];
            int startRow = cell.Start.Row;
            int startColumn = cell.Start.Column;

            int colorIndex = 0;
            for (int i = 0; i < countCell; i++)
            {
                if (colorIndex == colors.Length)
                {
                    colorIndex = 0;
                }
                worksheet.Cells[startRow, startColumn + i].Style.Fill.SetBackground(colors[colorIndex]);
                colorIndex++;
            }
        }

        public static Color[] GetLightRainbowColors(int count)
        {
            List<Color> colors = new List<Color>();

            int interval = 180 / count;
            //all - 360

            for (int i = 0; i < count; i++)
            {
                Color color = ColorFromHSV(interval * i, 0.2, 1);
                colors.Add(color);
            }

            return colors.ToArray();
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public enum BorderPosition
        {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT,
            ALL
        }

        public class ColorSchema
        {
            public static Color WHITE1 = Color.FromArgb(255, 255, 255, 255);
            public static Color WHITE2 = Color.FromArgb(255, 240, 240, 240);

            public static Color YELLOW1 = Color.FromArgb(255, 255, 229, 153);
            public static Color YELLOW2 = Color.FromArgb(255, 255, 217, 102);

            public static Color GREEN1 = Color.FromArgb(255, 204, 255, 204);
            public static Color GREEN2 = Color.FromArgb(255, 153, 255, 153);

            public static Color BLUE1 = Color.FromArgb(255, 204, 229, 255);
            public static Color BLUE2 = Color.FromArgb(255, 153, 204, 255);

            public static Color RED1 = Color.FromArgb(255, 255, 204, 204);
            public static Color RED2 = Color.FromArgb(255, 255, 153, 153);

            public static Color GREY1 = Color.FromArgb(255, 224, 224, 224);
            public static Color GREY2 = Color.FromArgb(255, 192, 192, 192);
            public static Color GREY3 = Color.FromArgb(255, 160, 160, 160);
            public static Color GREY4 = Color.FromArgb(128, 128, 128, 128);

            private ColorSchema() { }
        }
    }
}
