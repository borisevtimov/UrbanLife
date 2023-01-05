using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class ScheduleViewModel
    {
        public string StopNameInput { get; set; }

        public string StopCodeInput { get; set; }

        public List<string> StopNames { get; set; }

        public List<string> StopCodes { get; set; }

        public List<Line> Lines { get; set; }
    }
}