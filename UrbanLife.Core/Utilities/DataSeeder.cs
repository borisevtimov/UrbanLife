namespace UrbanLife.Core.Utilities
{
    public class DataSeeder
    {
        public static List<TimeSpan> AddHoursToArrival(TimeSpan arrival)
        {
            return new List<TimeSpan>()
            {
                new TimeSpan(hours: arrival.Hours + 1, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 2, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 3, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 4, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 5, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 6, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 7, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 8, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 9, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 10, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 11, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 12, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 13, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 14, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 15, minutes: arrival.Minutes, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 16, minutes: arrival.Minutes, seconds: arrival.Seconds)
            };
        }

        public static List<TimeSpan> AddMinutesToArrival(TimeSpan arrival)
        {
            return new List<TimeSpan>()
            {
                new TimeSpan(hours: arrival.Hours, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 1, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 1, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 2, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 2, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 3, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 3, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 4, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 4, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 5, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 5, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 6, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 6, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 7, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 7, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 8, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 8, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 9, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 9, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 10, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 10, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 11, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 11, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 12, minutes: arrival.Minutes + 20 , seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 12, minutes: arrival.Minutes + 40 , seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 13, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 13, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 14, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 14, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 15, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 15, minutes: arrival.Minutes + 40, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 16, minutes: arrival.Minutes + 20, seconds: arrival.Seconds),
                new TimeSpan(hours: arrival.Hours + 16, minutes: arrival.Minutes + 40, seconds: arrival.Seconds)
            };
        }
    }
}