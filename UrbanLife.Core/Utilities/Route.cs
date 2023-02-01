using UrbanLife.Data.Enums;
#nullable disable warnings

namespace UrbanLife.Core.Utilities
{
    public class Route
    {
        public string First { get; set; }

        public string Second { get; set; }

        public int Time { get; set; }

        public LineType LineType { get; set; }

        public int LineNumber { get; set; }

        public string StopCode { get; set; }

        public string NextStopCode { get; set; }
    }
}