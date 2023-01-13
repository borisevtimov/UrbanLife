using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data;
using UrbanLife.Data.Data.Models;
using UrbanLife.Data.Enums;

namespace UrbanLife.Core.Services
{
    public class ScheduleService
    {
        private readonly ApplicationDbContext dbContext;

        public ScheduleService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<string>> GetAllStopCodesAsync()
        {
            return await dbContext.Stops.Select(s => s.Code).ToListAsync();
        }

        public async Task<List<string>> GetAllStopNamesAsync()
        {
            return await dbContext.Stops.Select(s => s.Name).ToListAsync();
        }

        public async Task<List<Line>> GetAllLinesAsync()
        {
            return await dbContext.Lines.ToListAsync();
        }

        public async Task<List<Line>> GetLinesByStopNameAsync(string stopName)
        {
            return await dbContext.Lines
                .Where(l => l.Schedules.Select(s => s.Stop.Name).Contains(stopName))
                .ToListAsync();
        }

        public async Task<List<Line>> GetLinesByStopCodeAsync(string stopCode)
        {
            return await dbContext.Lines
                .Where(l => l.Schedules.Select(s => s.StopCode).Contains(stopCode))
                .ToListAsync();
        }

        public async Task<Line> GetLineIdAsync(int lineNumber, LineType lineType)
        {
            Line? line = await dbContext.Lines
                .FirstOrDefaultAsync(l => l.Number == lineNumber && l.Type == lineType);

            if (line == null)
            {
                throw new ArgumentException("Линията не беше намерена!");
            }

            return line;
        }

        public async Task<List<string>> GetStopCodesForLineAsync(string lineId)
        {
            return await dbContext.Schedules
                .Where(s => s.LineId == lineId && s.IsGoing)
                .Select(s => s.StopCode)
                .ToListAsync();
        }

        public async Task<List<string>> GetFirstAndLastStopNameForGoingAsync(string lineId)
        {
            return await dbContext.Schedules.Where(s => s.LineId == lineId && s.IsFirstStop)
                .OrderByDescending(s => s.IsGoing)
                .Select(s => s.Stop.Name)
                .ToListAsync();
        }

        public async Task<List<TimeSpan>> GetArrivalsForLineStopAsync(string lineId, string stopCode, bool isGoing)
        {
            TimeSpan timeSpan = await dbContext.Schedules
                .Where(s => s.LineId == lineId && s.StopCode == stopCode && s.IsGoing == isGoing)
                .Select(s => s.Arrival)
                .FirstOrDefaultAsync();

            return AddArrivalsToLineStopAsync(timeSpan);
        }

        public async Task<bool> IsLineGoingAsync(string lineId, string endStop)
        {
            return await dbContext.Schedules
                .AnyAsync(s => s.LineId == lineId && s.Stop.Name == endStop && s.NextStopCode == null && s.IsGoing);
        }

        private List<TimeSpan> AddArrivalsToLineStopAsync(TimeSpan timeSpan)
        {
            List<TimeSpan> addedHours = DataSeeder.AddHoursToArrival(timeSpan);
            List<TimeSpan> addedMinutes = DataSeeder.AddMinutesToArrival(timeSpan);

            List<TimeSpan> timeSpans = new();
            timeSpans.Add(timeSpan);
            timeSpans.AddRange(addedHours);
            timeSpans.AddRange(addedMinutes);

            return timeSpans;
        }
    }
}