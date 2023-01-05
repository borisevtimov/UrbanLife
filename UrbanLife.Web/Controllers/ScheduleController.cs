using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using UrbanLife.Core.Services;
using UrbanLife.Core.ViewModels;

namespace UrbanLife.Web.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ScheduleService scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        public async Task<IActionResult> All()
        {
            ScheduleViewModel scheduleModel = new()
            {
                Lines = await scheduleService.GetAllLinesAsync(),
                StopCodes = await scheduleService.GetAllStopCodesAsync(),
                StopNames = await scheduleService.GetAllStopNamesAsync()
            };

            return View(scheduleModel);
        }

        [HttpPost]
        public async Task<IActionResult> All(ScheduleViewModel model)
        {
            ScheduleViewModel newModel = new()
            {
                StopCodes = await scheduleService.GetAllStopCodesAsync(),
                StopNames = await scheduleService.GetAllStopNamesAsync()
            };

            if (model.StopCodeInput == null && model.StopNameInput == null)
            {
                newModel.Lines = await scheduleService.GetAllLinesAsync();
            }
            else if (model.StopNameInput != null && model.StopCodeInput == null)
            {
                newModel.Lines = await scheduleService.GetLinesByStopNameAsync(model.StopNameInput);
            }
            else if (model.StopCodeInput != null && model.StopNameInput == null)
            {
                newModel.Lines = await scheduleService.GetLinesByStopCodeAsync(model.StopCodeInput);
            }
            else if (model.StopCodeInput != null && model.StopNameInput != null)
            {
                newModel.Lines = await scheduleService.GetLinesByStopCodeAsync(model.StopCodeInput);
            }

            return View(newModel);
        }

        public IActionResult Line(string lineId)
        {
            return View();
        }
    }
}
