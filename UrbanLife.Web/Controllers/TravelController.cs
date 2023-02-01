using Microsoft.AspNetCore.Mvc;
using System.Text;
using UrbanLife.Core.Services;
using UrbanLife.Core.ViewModels;

namespace UrbanLife.Web.Controllers
{
    public class TravelController : Controller
    {
        private readonly TravelService travelService;

        public TravelController(TravelService travelService)
        {
            this.travelService = travelService;
        }

        public async Task<IActionResult> Choose()
        {
            string firstStopCode = "6330";
            string lastStopCode = "2483";
            TimeSpan startTime = new(hours: 21, minutes: 20, seconds: 0);

            string[] stopCodes = await travelService.FindFastestRouteAsync(firstStopCode, lastStopCode, startTime);
            TravelViewModel travelViewModel = new();
            travelViewModel = await travelService.GetRouteInfoAsync(stopCodes, startTime);

            return View(travelViewModel);
        }

        public IActionResult ShowOptions()
        {
            return RedirectToAction(nameof(Choose));
        }
    }
}
