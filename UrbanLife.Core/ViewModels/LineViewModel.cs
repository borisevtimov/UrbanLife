using System.ComponentModel.DataAnnotations;
using UrbanLife.Data.Enums;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class LineViewModel
    {
        public int LineNumber { get; set; }

        public LineType LineType { get; set; }

        public string ChosenStopCode { get; set; }

        public string FirstStop { get; set; }

        public string LastStop { get; set; }

        public List<string> StopCodes { get; set; }

        public string ChosenDestination { get; set; }

        public List<TimeSpan> Arrivals { get; set; }
    }
}