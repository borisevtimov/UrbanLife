using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
        private readonly DataSeeder dataSeeder;

        public ScheduleService(ApplicationDbContext dbContext, DataSeeder dataSeeder)
        {
            this.dbContext = dbContext;
            this.dataSeeder = dataSeeder;
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

        public async Task<List<string>> GetStopCodesAndNamesForLineAsync(string lineId, bool isGoing)
        {
            var schedules = await dbContext.Schedules
                .Where(s => s.LineId == lineId && s.IsGoing == isGoing)
                .OrderByDescending(s => s.IsFirstStop)
                .Select(s => new
                {
                    StopCode = s.StopCode,
                    IsFirstStop = s.IsFirstStop,
                    NextStopCode = s.NextStopCode,
                    StopName = s.Stop.Name
                })
                .Distinct()
                .ToListAsync();

            List<string> resultStopCodesAndNames = new();
            var currentSchedule = schedules.First(s => s.IsFirstStop);

            for (int i = 0; i < schedules.Count; i++)
            {
                resultStopCodesAndNames.Add($"{currentSchedule.StopCode} - {currentSchedule.StopName}");

                if (currentSchedule.NextStopCode != null)
                {
                    currentSchedule = schedules.First(s => s.StopCode == currentSchedule.NextStopCode);
                }
            }

            return resultStopCodesAndNames;
        }

        public async Task<string> GetStopCodeAsync(string stopName)
        {
            string? stopCode = await dbContext.Stops
                .Where(s => s.Name == stopName)
                .Select(s => s.Code)
                .FirstOrDefaultAsync();

            if (stopCode == null)
            {
                throw new ArgumentException("Такава спирка не съществува!");
            }

            return stopCode;
        }

        public async Task<string> GetStopNameAsync(string stopCode)
        {
            string? stopName = await dbContext.Stops
                .Where(s => s.Code == stopCode)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            if (stopName == null)
            {
                throw new ArgumentException("Такава спирка не съществува!");
            }

            return stopName;
        }

        public async Task<List<string>> GetFirstAndLastStopNameForGoingAsync(string lineId)
        {
            return await dbContext.Schedules.Where(s => s.LineId == lineId && s.IsFirstStop)
                .OrderByDescending(s => s.IsGoing)
                .Select(s => s.Stop.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<TimeSpan>> GetArrivalsForLineStopAsync(string lineId, string stopCode, bool isGoing)
        {
            return await dbContext.Schedules
                .Where(s => s.LineId == lineId && s.StopCode == stopCode && s.IsGoing == isGoing)
                .Select(s => s.Arrival)
                .ToListAsync();
        }

        public async Task<bool> IsLineGoingAsync(string lineId, string endStop)
        {
            return await dbContext.Schedules
                .AnyAsync(s => s.LineId == lineId && s.Stop.Name == endStop && s.NextStopCode == null && s.IsGoing);
        }

        // DELETES AND CREATES THE SCHEDULES!!!
        public async Task GenerateSchedulesAsync()
        {
            await dataSeeder.CreateSchedulesAsync();
        }
    }
}