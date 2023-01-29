using Microsoft.AspNetCore.Mvc;
using UrbanLife.Core.Services;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Enums;

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

        public async Task<IActionResult> Line(int lineNumber, LineType lineType, LineViewModel model)
        {
            UrbanLife.Data.Data.Models.Line line = await scheduleService.GetLineIdAsync(lineNumber, lineType);
            List<string> endStopsNames = await scheduleService.GetFirstAndLastStopNameForGoingAsync(line.Id);

            model.FirstStop = endStopsNames[0];
            model.LastStop = endStopsNames[1];

            if (model.ChosenDestination == null)
            {
                model.ChosenDestination = model.LastStop;
            }
            
            bool isLineGoing = await scheduleService.IsLineGoingAsync(line.Id, model.ChosenDestination);
            model.StopCodes = await scheduleService.GetStopCodesAndNamesForLineAsync(line.Id, isLineGoing);

            if (model.ChosenStopCode == null)
            {
                model.ChosenStopCode = model.StopCodes.First();
            }

            model.ChosenStopCode = model.ChosenStopCode.Split(" - ", StringSplitOptions.RemoveEmptyEntries)[0];

            model.Arrivals = await scheduleService
                .GetArrivalsForLineStopAsync(line.Id, model.ChosenStopCode, isLineGoing);

            return View(model);
        }

        public IActionResult ChooseLine(LineViewModel model)
        {
            return RedirectToAction(nameof(Line),
                new { lineNumber = model.LineNumber, lineType = model.LineType.ToString().ToLower(), model });
        }
    }
}
