using Microsoft.AspNetCore.Http;
using UrbanLife.Data.Data.Models;
using static System.Net.Mime.MediaTypeNames;

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
    }
}