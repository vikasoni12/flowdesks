using Flowdesks.Application.Interfaces.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel;
using System.Reflection;
using Document = iTextSharp.text.Document;

namespace Flowdesks.Infrastructure.Services.Common
{
    public class PDFGeneratorSerice : IPDFGeneratorSerice
    {

        public byte[] GeneratePdf<T>(string heading, List<T> data)
        {
            var memoryStream = new MemoryStream();

            float minimumColumnWidth = 90f;
            float calculatedWidth = Math.Max(data.First()?.GetType().GetProperties().Length * minimumColumnWidth ?? 550f, 550f);
            float tableMargin = 10f;

            // Set page size and margins
            Rectangle rec = new(calculatedWidth + 2 * tableMargin, 550);
            Document pdf = new(rec);
            pdf.SetMargins(0f, 0f, 0f, 0f);

            PdfWriter writer = PdfWriter.GetInstance(pdf, memoryStream);
            pdf.Open();

            // Add the bar at the top
            Rectangle barRect = new(pdf.Left, pdf.Top, pdf.Right, pdf.Top - 50)
            {
                BackgroundColor = new BaseColor(27, 35, 78) // #1b234e
            };

            // Draw the bar
            PdfContentByte canvas = writer.DirectContent;
            canvas.Rectangle(barRect);

            // Add the text on the bar
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, new Phrase("Connexus", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.White)), 36, pdf.Top - 30, 0);

            // Calculate the position and margins for the heading
            float headingX = (pdf.Left + pdf.Right) / 2;
            float headingY = pdf.Top - 80; // Added margin from the bar

            // Add the heading with margins
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase(heading, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.Black)), headingX, headingY, 0);

            // Calculate the maximum number of rows per page
            int rowsPerPage = 30; 

            // Split the data into chunks
            for (int i = 0; i < data.Count; i += rowsPerPage)
            {
                // Create a new table for each chunk
                var chunk = data.Skip(i).Take(rowsPerPage).ToList();
                PdfPTable table = CreateTable(chunk, calculatedWidth);

                // Add a new page if necessary
                if (i > 0)
                {
                    pdf.NewPage();
                }

                // Calculate the position and margins for the table
                float tableX = pdf.Left + tableMargin;
                float tableY = pdf.Top - 100; // Adjusted the position with added margin

                // Write the table to the specified position
                table.WriteSelectedRows(0, -1, tableX, tableY, writer.DirectContent);
            }

            pdf.Close();
            return memoryStream.ToArray();
        }

        private static PdfPTable CreateTable<T>(List<T> data, float tableWidth)
        {
            var properties = typeof(T).GetProperties();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.Black);

            // Create table with headers
            PdfPTable table = new(properties.Length);
            table.TotalWidth = tableWidth; // Set the total width

            foreach (var prop in properties)
            {
                // Get the Description attribute value
                var descriptionAttribute = prop.GetCustomAttribute<DescriptionAttribute>();
                var columnHeader = descriptionAttribute?.Description ?? prop.Name;

                PdfPCell cell = new(new Phrase(columnHeader, titleFont))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 3f
                };

                table.AddCell(cell);
            }

            var contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 7, BaseColor.DarkGray);

            // Add data rows
            foreach (var item in data)
            {
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item);

                    if (value is DateTime time)
                    {
                        value = time.ToString("MMM d, yyyy");
                    }

                    PdfPCell cell = new(new Phrase(value?.ToString(), contentFont))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 3f
                    };

                    table.AddCell(cell);
                }
            }

            return table;
        }

    }
}
