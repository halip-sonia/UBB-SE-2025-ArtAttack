using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Windows.Data.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ArtAttack.Domain
{
    internal class Contract
    {

        public Contract() { }
        public void GeneratePDF() {

            QuestPDF.Settings.License = LicenseType.Community; // Free to use

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(16));

                    page.Header()
                        .Text("My PDF Document")
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Text("Hello, this is a generated PDF by Darius, Cailor")
                        .FontSize(14);

                    page.Footer()
                        .AlignCenter()
                        .Text("Page Footer - " + DateTime.Now.ToString("yyyy-MM-dd"));
                });
            });

            document.GeneratePdf("C:\\MyStuff\\COLLEGE\\College_Projects\\UBB-SE-2025-ArtAttack\\ArtAttack\\output.pdf"); // Saves the PDF file
        }
        
    }
}
