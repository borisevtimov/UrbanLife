using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrbanLife.Core.Services;
using UrbanLife.Core.ViewModels;

namespace UrbanLife.Web.Controllers
{
    public class TravelController : Controller
    {
        private readonly TravelService travelService;
        private readonly ScheduleService scheduleService;

        public TravelController(TravelService travelService, ScheduleService scheduleService)
        {
            this.travelService = travelService;
            this.scheduleService = scheduleService;
        }

        public async Task<IActionResult> Choose(TravelViewModel model)
        {
            model.AllStopNames = await scheduleService.GetAllStopNamesAsync();
            model.AllStopCodes = await scheduleService.GetAllStopCodesAsync();

            if (TempData["WantedTime"] == null)
            {
                model.WantedTime = new TimeSpan(hours: 6, minutes: 30, seconds: 0);
            }

            if (TempData["Routes"] != null)
            {
                model.Routes = JsonConvert.DeserializeObject<Dictionary<string, RouteViewModel>>(TempData["Routes"].ToString());
            }

            return View(model);
        }

        public async Task<IActionResult> GetRoutes(TravelViewModel model)
        {
            try
            {
                string[]? stopCodes = null;
                if (model.ChosenStopCode != null && model.ChosenStopName != null
                    && model.ChosenLastStopCode != null && model.ChosenLastStopName != null)
                {
                    model.ChosenStopName = await scheduleService.GetStopNameAsync(model.ChosenStopCode);
                    model.ChosenLastStopName = await scheduleService.GetStopNameAsync(model.ChosenLastStopCode);
                }
                else if (model.ChosenStopName != null && model.ChosenLastStopName != null)
                {
                    model.ChosenStopCode = await scheduleService.GetStopCodeAsync(model.ChosenStopName);
                    model.ChosenLastStopCode = await scheduleService.GetStopCodeAsync(model.ChosenLastStopName);
                }
                else if (model.ChosenStopCode != null && model.ChosenLastStopName != null)
                {
                    model.ChosenLastStopCode = await scheduleService.GetStopCodeAsync(model.ChosenLastStopName);
                    model.ChosenStopName = await scheduleService.GetStopNameAsync(model.ChosenStopCode);
                }
                else if (model.ChosenStopName != null && model.ChosenLastStopCode != null)
                {
                    model.ChosenStopCode = await scheduleService.GetStopCodeAsync(model.ChosenStopName);
                    model.ChosenLastStopName = await scheduleService.GetStopNameAsync(model.ChosenLastStopCode);
                }
                else if (model.ChosenStopCode != null && model.ChosenLastStopCode != null)
                {
                    model.ChosenStopName = await scheduleService.GetStopNameAsync(model.ChosenStopCode);
                    model.ChosenLastStopName = await scheduleService.GetStopNameAsync(model.ChosenLastStopCode);
                }
                else if (model.ChosenStopCode == null || model.ChosenLastStopCode == null)
                {
                    return RedirectToAction(nameof(Choose));
                }

                stopCodes = await travelService
                    .FindFastestRouteAsync(model.ChosenStopCode, model.ChosenLastStopCode, model.WantedTime);

                if (stopCodes == null)
                {
                    return RedirectToAction(nameof(Choose));
                }

                Dictionary<string, RouteViewModel> routeViewModel = await travelService.GetRouteInfoAsync(stopCodes, model.WantedTime);
                model.Routes = routeViewModel;

                TempData["Routes"] = JsonConvert.SerializeObject(model.Routes);
                TempData["WantedTime"] = JsonConvert.SerializeObject(model.WantedTime);
            }
            catch (ArgumentException)
            {
                return RedirectToAction(nameof(Choose));
            }

            return RedirectToAction(nameof(Choose), model);
        }
    }
}
