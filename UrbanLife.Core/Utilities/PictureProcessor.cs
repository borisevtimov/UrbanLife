using IronPdf;
using Microsoft.AspNetCore.Http;
using Net.Codecrete.QrCodeGenerator;
using System.Text;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data.Models;
using UrbanLife.Data.Enums;

namespace UrbanLife.Core.Utilities
{
    public class PictureProcessor
    {
        public static async Task<string> DownloadProfilePictureAsync(string webHostEnvironmentUrl, IFormFile image)
        {
            string profilePictureUrl = $"{webHostEnvironmentUrl}/images/profile-pictures/custom-pictures";

            if (image != null && image.FileName != "guest.png")
            {
                string uniqueFileName = Guid.NewGuid()
                    .ToString()
                    .Replace('/', 'a')
                    .Replace('\\', 'b');

                uniqueFileName += "==_" + image.FileName;

                profilePictureUrl = Path.Combine(profilePictureUrl, uniqueFileName);

                using FileStream fileStream = new(profilePictureUrl, FileMode.Create);
                await image.CopyToAsync(fileStream);

                return $"custom-pictures/{uniqueFileName}";
            }

            return "guest.png";
        }

        public static void DeleteProfilePicture(string webHostEnvironmentUrl, string profileImageName)
        {
            string profilePictureUrl = $"{webHostEnvironmentUrl}/images/profile-pictures/{profileImageName}";

            if (File.Exists(profilePictureUrl))
            {
                File.Delete(profilePictureUrl);
            }
        }

        public static void GenerateReceipt(string webHostEnvironmentUrl, BuySubscriptionViewModel model, User user)
        {
            Purchase purchase = new();
            string receiptsUrl = $"{webHostEnvironmentUrl}/images/receipts/{purchase.Id}.pdf";
            StringBuilder resultHtml = new();
            resultHtml.AppendLine("<h1>ЪрбънЛайф АД</h1>");
            resultHtml.AppendLine("<p>България, София</p>");
            resultHtml.AppendLine($"<p>Поръчка #{purchase.Id}</p>");
            resultHtml.AppendLine($"<p>Потребителски номер #{user.Id}</p>");
            resultHtml.AppendLine($"<p>Имена: {user.FirstName.ToUpper()} {user.LastName.ToUpper()}</p>");
            resultHtml.AppendLine($"<p>Имейл адрес: {user.Email}</p>");

            if (model.SubscriptionType == SubscriptionType.CARD)
            {

            }
            else if (model.SubscriptionType == SubscriptionType.TICKET)
            {

            }

            var qr = QrCode.EncodeText("Зареден за всички линии за Пешо", QrCode.Ecc.Medium);
            string svg = qr.ToSvgString(4);
            //File.WriteAllText("hello-world-qr.svg", svg, Encoding.UTF8);

            //var html = @$"
            //<h1>HI..! Welcome to the PDF Tutorial!</h1>
            //<p> This is 1st Page </p>
            //<div style = 'page-break-after: always;' ></div>
            //<h2> This is 2nd Page after page break!</h2>
            //<div style = 'page-break-after: always;' ></div>
            //<p> This is 3rd Page</p>
            //<div style = 'page-break-after: always;' ></div>
            //<img src=""C:\Codes\TU-Graduation-Project\Utility-Test-Apps\PdfDemo\PdfDemo\bin\Debug\net6.0\hello-world-qr.svg"">
            //<link href=""https://fonts.googleapis.com/css?family=Libre Barcode 128""rel = ""stylesheet"" ><p style = ""font-family: 'Libre Barcode 128', serif; font-size:80px;""> Hello Google Fonts</p>";

            // Instantiate Renderer
            var Renderer = new ChromePdfRenderer();

            /* Main Document */
            //As we have a Cover Page, we're going to start the page numbers at 2.
            Renderer.RenderingOptions.FirstPageNumber = 2;

            Renderer.RenderingOptions.HtmlFooter = new HtmlHeaderFooter()
            {
                MaxHeight = 15, //millimeters
                HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
                DrawDividerLine = true
            };

            using PdfDocument Pdf = Renderer.RenderHtmlAsPdf(resultHtml.ToString());

            //PDF Settings
            Pdf.SecuritySettings.AllowUserCopyPasteContent = false;

            Pdf.SaveAs(receiptsUrl);
        }
    }
}