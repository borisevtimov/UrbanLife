#nullable disable warnings

using UrbanLife.Data.Enums;

namespace UrbanLife.Core.ViewModels
{
    public class RouteViewModel
    {
        public string StopCode { get; set; }

        public string StopName { get; set; }

        public string NextStopCode { get; set; }

        public string NextStopName { get; set; }

        public int LineNumber { get; set; }

        public LineType LineType { get; set; }

        public string LineId { get; set; }

        public int LineRepeatings { get; set; } = 1;

        public TimeSpan Arrival { get; set; }
    }
}