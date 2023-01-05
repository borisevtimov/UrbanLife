using Microsoft.EntityFrameworkCore;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data;
using UrbanLife.Data.Data.Models;

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
    }
}