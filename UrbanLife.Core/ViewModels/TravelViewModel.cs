#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class TravelViewModel
    {
        public string? ChosenStopName { get; set; }

        public List<string> AllStopNames { get; set; }

        public string? ChosenStopCode { get; set; }

        public List<string> AllStopCodes { get; set; }

        public string? ChosenLastStopName { get; set; }

        public string? ChosenLastStopCode { get; set; }

        public TimeSpan WantedTime { get; set; }

        public Dictionary<string, RouteViewModel> Routes { get; set; }
    }
}