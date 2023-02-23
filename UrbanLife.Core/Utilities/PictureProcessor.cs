using Microsoft.AspNetCore.Http;
using Net.Codecrete.QrCodeGenerator;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp.Extended.Svg;
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

        public static void GenerateReceipt(string webHostEnvironmentUrl, BuySubscriptionViewModel model, User user, string purchaseId)
        {
            string receiptsUrl = $"{webHostEnvironmentUrl}/images/receipts/{purchaseId}.pdf";

            QrCode qr = QrCode.EncodeText($"Поръчка #{purchaseId}", QrCode.Ecc.Medium);
            string svg = qr.ToSvgString(4);
            File.WriteAllText($"{webHostEnvironmentUrl}/images/receipts/qr.svg", svg, Encoding.UTF8);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(16, Unit.Point);
                    page.MarginVertical(16, Unit.Point);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(30));

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span("ЪрбънЛайф АД").FontFamily("Consolas");
                            });
                            column.Item().PaddingBottom(20, Unit.Point).AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span("България, София").FontFamily("Consolas");
                            });
                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span($"Потребител #{user.Id}").FontFamily("Consolas").FontSize(20);
                            });
                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span($"Имена: {user.FirstName.ToUpper()} {user.LastName.ToUpper()}").FontFamily("Consolas").FontSize(20);
                            });
                            column.Item().PaddingBottom(20, Unit.Point).AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span($"Имейл: {user.Email}").FontFamily("Consolas").FontSize(20);
                            });
                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span($"Поръчка #{purchaseId}").FontFamily("Consolas").FontSize(20);
                            });
                            if (model.SubscriptionType == SubscriptionType.CARD && model.ChosenCardStartDate.HasValue)
                            {
                                column.Item().AlignCenter().Text(descriptor =>
                                {
                                    descriptor.Span($"Избран абонамент: карта").FontFamily("Consolas").FontSize(20);
                                });
                                column.Item().AlignCenter().Text(descriptor =>
                                {
                                    descriptor.Span($"От: {model.ChosenCardStartDate.Value} ч.").FontFamily("Consolas").FontSize(20);
                                });
                                column.Item().AlignCenter().Text(descriptor =>
                                {
                                    if (model.ChosenDuration == "1-month")
                                    {
                                        descriptor.Span($"До: {model.ChosenCardStartDate.Value.AddMonths(1)} ч.").FontFamily("Consolas").FontSize(20);
                                    }
                                    else if (model.ChosenDuration == "3-month")
                                    {
                                        descriptor.Span($"До: {model.ChosenCardStartDate.Value.AddMonths(3)} ч.").FontFamily("Consolas").FontSize(20);
                                    }
                                    else if (model.ChosenDuration == "1-year")
                                    {
                                        descriptor.Span($"До: {model.ChosenCardStartDate.Value.AddYears(1)} ч.").FontFamily("Consolas").FontSize(20);
                                    }
                                });
                            }
                            else if (model.SubscriptionType == SubscriptionType.TICKET && model.ChosenTicketStartTime.HasValue)
                            {
                                DateTime chosenDateTime = new(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day,
                                        hour: model.ChosenTicketStartTime.Value.Hours, minute: model.ChosenTicketStartTime.Value.Minutes,
                                        second: model.ChosenTicketStartTime.Value.Seconds);

                                column.Item().AlignCenter().Text(descriptor =>
                                {
                                    descriptor.Span($"Избран абонамент: билет").FontFamily("Consolas").FontSize(20);
                                });
                                column.Item().AlignCenter().Text(descriptor =>
                                {
                                    descriptor.Span($"От: {chosenDateTime} ч.").FontFamily("Consolas").FontSize(20);
                                });
                                column.Item().AlignCenter().Text(descriptor =>
                                {
                                    
                                    if (model.ChosenDuration == "30-minute")
                                    {
                                        descriptor.Span($"До: {chosenDateTime.AddMinutes(30)} ч.")
                                                    .FontFamily("Consolas").FontSize(20);
                                    }
                                    else if (model.ChosenDuration == "60-minute" || model.ChosenDuration == "one-way")
                                    {
                                        descriptor.Span($"До: {chosenDateTime.AddHours(1)} ч.")
                                                    .FontFamily("Consolas").FontSize(20);
                                    }
                                    else if (model.ChosenDuration == "1-day")
                                    {
                                        descriptor.Span($"До: {chosenDateTime.AddDays(1)} ч.")
                                                    .FontFamily("Consolas").FontSize(20);
                                    }
                                });
                            }
                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                if (model.ChosenLines.Split(',').Contains("all-lines"))
                                {
                                    descriptor.Span("Линии: Всички").FontFamily("Consolas").FontSize(20);
                                }
                                else
                                {
                                    descriptor.Span($"Линии: {model.ChosenLines}").FontFamily("Consolas").FontSize(20);
                                }
                            });
                            
                            column.Item().PaddingBottom(40, Unit.Point).AlignCenter().Text(descriptor =>
                            {
                                if (model.ChosenDuration == "one-way")
                                {
                                    descriptor.Span("Период: еднократно").FontFamily("Consolas").FontSize(20);
                                }
                                else if (model.ChosenDuration == "30-minute")
                                {
                                    descriptor.Span("Период: 30 минути").FontFamily("Consolas").FontSize(20);
                                }
                                else if (model.ChosenDuration == "60-minute")
                                {
                                    descriptor.Span("Период: 60 минути").FontFamily("Consolas").FontSize(20);
                                }
                                else if (model.ChosenDuration == "1-day")
                                {
                                    descriptor.Span("Период: 1 ден").FontFamily("Consolas").FontSize(20);
                                }
                                else if (model.ChosenDuration == "1-month")
                                {
                                    descriptor.Span("Период: 1 месец").FontFamily("Consolas").FontSize(20);
                                }
                                else if (model.ChosenDuration == "3-month")
                                {
                                    descriptor.Span("Период: 3 месеца").FontFamily("Consolas").FontSize(20);
                                }
                                else if (model.ChosenDuration == "1-year")
                                {
                                    descriptor.Span("Период: 1 година").FontFamily("Consolas").FontSize(20);
                                }
                            });
                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span($"Обща сума: {model.FinalPrice} лв.").FontFamily("Consolas").FontSize(20);
                            });
                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span($"Дата и час: {DateTime.Now} ч.").FontFamily("Consolas").FontSize(20);
                            });

                            column.Item().AlignCenter().Text(descriptor =>
                            {
                                descriptor.Span("СИСТЕМЕН БОН").FontFamily("Consolas").FontSize(20);
                            });
                            column.Item()
                            .Element(element =>
                            {
                                element
                                    .PaddingLeft(4)
                                    .PaddingLeft((PageSizes.A4.Width - 60) / 3)
                                    .Canvas((canvas, size) =>
                                    {
                                        SKSvg pieChartSvg = new();
                                        pieChartSvg.Load($"{webHostEnvironmentUrl}/images/receipts/qr.svg");
                                        canvas.Scale(5f);
                                        canvas.DrawPicture(pieChartSvg.Picture);
                                        File.Delete($"{webHostEnvironmentUrl}/images/receipts/qr.svg");
                                    });
                            }); 
                        });
                });
            })
            .GeneratePdf(receiptsUrl);
        }
    }
}