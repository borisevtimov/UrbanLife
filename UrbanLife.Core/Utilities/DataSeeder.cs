using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrbanLife.Data.Data;
using UrbanLife.Data.Data.Models;
using UrbanLife.Data.Enums;

namespace UrbanLife.Core.Utilities
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// WARNING: Sets the database in its initial, stable state. Ensures the database is deleted, 
        /// then applies the migrations from the project and seeds the data needed for the application to
        /// work properly (seeds tables - AspNetRoles, Lines, Stops, Schedules)
        /// </summary>
        public async Task InitializeDataBaseAsync()
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();

            await CreateRolesAsync();
            await CreateLinesAsync();
            await CreateStopsAsync();
            await CreateSchedulesAsync();
        }

        private async Task CreateRolesAsync()
        {
            IdentityRole userRole = new() { Name = "User", NormalizedName = "USER" };
            IdentityRole adminRole = new() { Name = "Admin", NormalizedName = "ADMIN" };

            await roleManager.CreateAsync(userRole);
            await roleManager.CreateAsync(adminRole);
        }

        private async Task CreateSchedulesAsync()
        {
            List<Schedule> schedules = GenerateInitialRouteForLines();
            List<Schedule> allSchedules = new(schedules);

            foreach (Schedule schedule in schedules)
            {
                List<Schedule> allSchedulesForStop = new()
                {
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 0, minutes: 20, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                       LineId = schedule.LineId, StopCode = schedule.StopCode,
                       NextStopCode = schedule.NextStopCode,
                       Arrival = schedule.Arrival.Add(new TimeSpan(hours: 0, minutes: 40, seconds: 0)),
                       IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                       LineId = schedule.LineId, StopCode = schedule.StopCode,
                       NextStopCode = schedule.NextStopCode,
                       Arrival = schedule.Arrival.Add(new TimeSpan(hours: 1, minutes: 0, seconds: 0)),
                       IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                       LineId = schedule.LineId, StopCode = schedule.StopCode,
                       NextStopCode = schedule.NextStopCode,
                       Arrival = schedule.Arrival.Add(new TimeSpan(hours: 1, minutes: 20, seconds: 0)),
                       IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 1, minutes: 40, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 2, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 2, minutes: 20, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 2, minutes: 40, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 3, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 3, minutes: 20, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 3, minutes: 40, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 4, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 4, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 4, minutes: 40, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 5, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 5, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 5, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 6, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 6, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 6, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 7, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 7, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 7, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 8, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 8, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 8, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 9, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 9, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 9, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 10, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 10, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 10, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 11, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 11, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 11, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 12, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 12, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 12, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 13, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 13, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 13, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 14, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 14, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 14, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours: 15, minutes: 0, seconds: 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 15, minutes : 20, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    },
                    new Schedule()
                    {
                        LineId = schedule.LineId, StopCode = schedule.StopCode,
                        NextStopCode = schedule.NextStopCode,
                        Arrival = schedule.Arrival.Add(new TimeSpan(hours : 15, minutes : 40, seconds : 0)),
                        IsGoing = schedule.IsGoing, IsFirstStop = schedule.IsFirstStop
                    }
                };

                allSchedules.AddRange(allSchedulesForStop);
            }

            await DeleteAllSchedulesAsync();
            await dbContext.Schedules.AddRangeAsync(allSchedules);
            await dbContext.SaveChangesAsync();
        }

        private List<Schedule> GenerateInitialRouteForLines()
        {
            List<Schedule> resultSchedules = new();

            // Line 5 TRAM
            List<Schedule> lineFiveTram = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "abc", StopCode = "1101", NextStopCode = "1543",
                    Arrival = new TimeSpan(hours: 6, minutes: 42, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "1543", NextStopCode = "5688",
                    Arrival = new TimeSpan(hours: 6, minutes: 51, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "5688", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 53, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "1840", NextStopCode = "4432",
                    Arrival = new TimeSpan(hours: 7, minutes: 0, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "4432", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 8, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "abc", StopCode = "4432", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 44, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "1840", NextStopCode = "5688",
                    Arrival = new TimeSpan(hours: 6, minutes: 52, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "5688", NextStopCode = "1543",
                    Arrival = new TimeSpan(hours: 6, minutes: 59, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "1543", NextStopCode = "1101",
                    Arrival = new TimeSpan(hours: 7, minutes: 1, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "abc", StopCode = "1101", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 10, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 11 BUS
            List<Schedule> lineElevenBus = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "cde", StopCode = "8522", NextStopCode = "2254",
                    Arrival = new TimeSpan(hours: 6, minutes: 34, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "2254", NextStopCode = "3281",
                    Arrival = new TimeSpan(hours: 6, minutes: 36, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "3281", NextStopCode = "6782",
                    Arrival = new TimeSpan(hours: 6, minutes: 38, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "6782", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 41, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "1840", NextStopCode = "2231",
                    Arrival = new TimeSpan(hours: 6, minutes: 42, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "2231", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 52, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "cde", StopCode = "2231", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 31, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "1840", NextStopCode = "6782",
                    Arrival = new TimeSpan(hours: 6, minutes: 41, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "6782", NextStopCode = "3281",
                    Arrival = new TimeSpan(hours: 6, minutes: 42, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "3281", NextStopCode = "2254",
                    Arrival = new TimeSpan(hours: 6, minutes: 45, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "2254", NextStopCode = "8522",
                    Arrival = new TimeSpan(hours: 6, minutes: 47, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "cde", StopCode = "8522", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 49, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 11 TRAM
            List<Schedule> lineElevenTram = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "dadg", StopCode = "3281", NextStopCode = "5529",
                    Arrival = new TimeSpan(hours: 6, minutes: 37, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "5529", NextStopCode = "4432",
                    Arrival = new TimeSpan(hours: 6, minutes: 40, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "4432", NextStopCode = "5133",
                    Arrival = new TimeSpan(hours: 6, minutes: 45, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "5133", NextStopCode = "1543",
                    Arrival = new TimeSpan(hours: 6, minutes: 47, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "1543", NextStopCode = "5688",
                    Arrival = new TimeSpan(hours: 6, minutes: 51, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "5688", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 55, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "dadg", StopCode = "5688", NextStopCode = "1543",
                    Arrival = new TimeSpan(hours: 6, minutes: 41, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "1543", NextStopCode = "5133",
                    Arrival = new TimeSpan(hours: 6, minutes: 45, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "5133", NextStopCode = "4432",
                    Arrival = new TimeSpan(hours: 6, minutes: 47, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "4432", NextStopCode = "5529",
                    Arrival = new TimeSpan(hours: 6, minutes: 52, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "5529", NextStopCode = "3281",
                    Arrival = new TimeSpan(hours: 6, minutes: 55, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dadg", StopCode = "3281", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 58, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 22 TRAM
            List<Schedule> lineTwentyTwoTram = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "dalf", StopCode = "1011", NextStopCode = "1000",
                    Arrival = new TimeSpan(hours: 6, minutes: 37, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "1000", NextStopCode = "2254",
                    Arrival = new TimeSpan(hours: 6, minutes: 38, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "2254", NextStopCode = "3281",
                    Arrival = new TimeSpan(hours: 6, minutes: 48, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "3281", NextStopCode = "4443",
                    Arrival = new TimeSpan(hours: 6, minutes: 49, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "4443", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 53, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "1840", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 55, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "dalf", StopCode = "1840", NextStopCode = "4443",
                    Arrival = new TimeSpan(hours: 6, minutes: 27, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "4443", NextStopCode = "3281",
                    Arrival = new TimeSpan(hours: 6, minutes: 29, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "3281", NextStopCode = "2254",
                    Arrival = new TimeSpan(hours: 6, minutes: 33, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "2254", NextStopCode = "1000",
                    Arrival = new TimeSpan(hours: 6, minutes: 34, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "1000", NextStopCode = "1011",
                    Arrival = new TimeSpan(hours: 6, minutes: 44, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "dalf", StopCode = "1011", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 45, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 83 BUS
            List<Schedule> lineEightyThreeBus = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "edf", StopCode = "2483", NextStopCode = "5564",
                    Arrival = new TimeSpan(hours: 6, minutes: 38, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "5564", NextStopCode = "6782",
                    Arrival = new TimeSpan(hours: 6, minutes: 51, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "6782", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 56, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "1840", NextStopCode = "2231",
                    Arrival = new TimeSpan(hours: 7, minutes: 0, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "2231", NextStopCode = "4555",
                    Arrival = new TimeSpan(hours: 7, minutes: 10, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "4555", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 20, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "edf", StopCode = "4555", NextStopCode = "2231",
                    Arrival = new TimeSpan(hours: 6, minutes: 48, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "2231", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 58, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "1840", NextStopCode = "6782",
                    Arrival = new TimeSpan(hours: 7, minutes: 8, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "6782", NextStopCode = "5564",
                    Arrival = new TimeSpan(hours: 7, minutes: 12, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "5564", NextStopCode = "2483",
                    Arrival = new TimeSpan(hours: 7, minutes: 17, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "edf", StopCode = "2483", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 30, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 8 TRAM
            List<Schedule> lineEightTram = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "5133", NextStopCode = "4432",
                    Arrival = new TimeSpan(hours: 6, minutes: 43, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "4432", NextStopCode = "2254",
                    Arrival = new TimeSpan(hours: 6, minutes: 48, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "2254", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 51, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "1840", NextStopCode = "3002",
                    Arrival = new TimeSpan(hours: 6, minutes: 59, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "3002", NextStopCode = "5564",
                    Arrival = new TimeSpan(hours: 7, minutes: 7, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "5564", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 11, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "5564", NextStopCode = "3002",
                    Arrival = new TimeSpan(hours: 6, minutes: 40, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "3002", NextStopCode = "1840",
                    Arrival = new TimeSpan(hours: 6, minutes: 44, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "1840", NextStopCode = "2254",
                    Arrival = new TimeSpan(hours: 6, minutes: 52, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "2254", NextStopCode = "4432",
                    Arrival = new TimeSpan(hours: 7, minutes: 0, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "4432", NextStopCode = "5133",
                    Arrival = new TimeSpan(hours: 7, minutes: 3, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "gpasc", StopCode = "5133", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 8, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 280 BUS
            List<Schedule> lineTwoHundredAndEightyBus = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "hij", StopCode = "1101", NextStopCode = "1093",
                    Arrival = new TimeSpan(hours: 6, minutes: 32, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1093", NextStopCode = "1072",
                    Arrival = new TimeSpan(hours: 6, minutes: 34, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1072", NextStopCode = "6330",
                    Arrival = new TimeSpan(hours: 6, minutes: 37, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "6330", NextStopCode = "1058",
                    Arrival = new TimeSpan(hours: 6, minutes: 38, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1058", NextStopCode = "1041",
                    Arrival = new TimeSpan(hours: 6, minutes: 43, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1041", NextStopCode = "1023",
                    Arrival = new TimeSpan(hours: 6, minutes: 46, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1023", NextStopCode = "1000",
                    Arrival = new TimeSpan(hours: 6, minutes: 52, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1000", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 53, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "hij", StopCode = "1000", NextStopCode = "1023",
                    Arrival = new TimeSpan(hours: 6, minutes: 21, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1023", NextStopCode = "1041",
                    Arrival = new TimeSpan(hours: 6, minutes: 22, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1041", NextStopCode = "1058",
                    Arrival = new TimeSpan(hours: 6, minutes: 28, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1058", NextStopCode = "6330",
                    Arrival = new TimeSpan(hours: 6, minutes: 31, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "6330", NextStopCode = "1072",
                    Arrival = new TimeSpan(hours: 6, minutes: 36, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1072", NextStopCode = "1093",
                    Arrival = new TimeSpan(hours: 6, minutes: 37, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1093", NextStopCode = "1101",
                    Arrival = new TimeSpan(hours: 6, minutes: 40, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "hij", StopCode = "1101", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 6, minutes: 42, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 5 TROLLEY
            List<Schedule> lineFiveTrolley = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "klm", StopCode = "6122", NextStopCode = "1058",
                    Arrival = new TimeSpan(hours: 6, minutes: 44, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1058", NextStopCode = "1041",
                    Arrival = new TimeSpan(hours: 6, minutes: 50, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1041", NextStopCode = "1023",
                    Arrival = new TimeSpan(hours: 6, minutes: 52, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1023", NextStopCode = "1000",
                    Arrival = new TimeSpan(hours: 6, minutes: 57, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1000", NextStopCode = "8522",
                    Arrival = new TimeSpan(hours: 6, minutes: 58, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "8522", NextStopCode = "6782",
                    Arrival = new TimeSpan(hours: 7, minutes: 3, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "6782", NextStopCode = "5564",
                    Arrival = new TimeSpan(hours: 7, minutes: 9, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "5564", NextStopCode = "2483",
                    Arrival = new TimeSpan(hours: 7, minutes: 14, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "2483", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 24, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "klm", StopCode = "2483", NextStopCode = "5564",
                    Arrival = new TimeSpan(hours: 6, minutes: 42, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "5564", NextStopCode = "6782",
                    Arrival = new TimeSpan(hours: 6, minutes: 52, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "6782", NextStopCode = "8522",
                    Arrival = new TimeSpan(hours: 6, minutes: 57, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "8522", NextStopCode = "1000",
                    Arrival = new TimeSpan(hours: 7, minutes: 3, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1000", NextStopCode = "1023",
                    Arrival = new TimeSpan(hours: 7, minutes: 8, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1023", NextStopCode = "1041",
                    Arrival = new TimeSpan(hours: 7, minutes: 9, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1041", NextStopCode = "1058",
                    Arrival = new TimeSpan(hours: 7, minutes: 14, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "1058", NextStopCode = "6122",
                    Arrival = new TimeSpan(hours: 7, minutes: 16, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "klm", StopCode = "6122", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 22, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            // Line 1 TROLLEY
            List<Schedule> lineOneTrolley = new()
            {
                // For IsGoing = true
                new Schedule()
                {
                    LineId = "phaf", StopCode = "5529", NextStopCode = "2231",
                    Arrival = new TimeSpan(hours: 6, minutes: 57, seconds: 0),
                    IsGoing = true, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "2231", NextStopCode = "8874",
                    Arrival = new TimeSpan(hours: 7, minutes: 3, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "8874", NextStopCode = "5543",
                    Arrival = new TimeSpan(hours: 7, minutes: 6, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "5543", NextStopCode = "1058",
                    Arrival = new TimeSpan(hours: 7, minutes: 21, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "1058", NextStopCode = "3384",
                    Arrival = new TimeSpan(hours: 7, minutes: 33, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "3384", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 58, seconds: 0),
                    IsGoing = true, IsFirstStop = false
                },

                // For IsGoing = false
                new Schedule()
                {
                    LineId = "phaf", StopCode = "3384", NextStopCode = "1058",
                    Arrival = new TimeSpan(hours: 7, minutes: 1, seconds: 0),
                    IsGoing = false, IsFirstStop = true
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "1058", NextStopCode = "5543",
                    Arrival = new TimeSpan(hours: 7, minutes: 26, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "5543", NextStopCode = "8874",
                    Arrival = new TimeSpan(hours: 7, minutes: 34, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "8874", NextStopCode = "2231",
                    Arrival = new TimeSpan(hours: 7, minutes: 49, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "2231", NextStopCode = "5529",
                    Arrival = new TimeSpan(hours: 7, minutes: 52, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                },
                new Schedule()
                {
                    LineId = "phaf", StopCode = "5529", NextStopCode = null,
                    Arrival = new TimeSpan(hours: 7, minutes: 58, seconds: 0),
                    IsGoing = false, IsFirstStop = false
                }
            };

            resultSchedules.AddRange(lineFiveTram);
            resultSchedules.AddRange(lineElevenBus);
            resultSchedules.AddRange(lineElevenTram);
            resultSchedules.AddRange(lineTwentyTwoTram);
            resultSchedules.AddRange(lineEightyThreeBus);
            resultSchedules.AddRange(lineEightTram);
            resultSchedules.AddRange(lineTwoHundredAndEightyBus);
            resultSchedules.AddRange(lineFiveTrolley);
            resultSchedules.AddRange(lineOneTrolley);
            return resultSchedules;
        }

        private async Task DeleteAllSchedulesAsync()
        {
            List<Schedule> schedules = await dbContext.Schedules.ToListAsync();
            dbContext.Schedules.RemoveRange(schedules);

            await dbContext.SaveChangesAsync();
        }

        private async Task CreateLinesAsync()
        {
            List<Line> lines = new()
            {
                new Line { Id = "abc", Number = 5, Type = LineType.TRAM },
                new Line { Id = "cde", Number = 11, Type = LineType.BUS },
                new Line { Id = "dadg", Number = 11, Type = LineType.TRAM },
                new Line { Id = "dalf", Number = 22, Type = LineType.TRAM },
                new Line { Id = "edf", Number = 83, Type = LineType.BUS },
                new Line { Id = "gpasc", Number = 8, Type = LineType.TRAM },
                new Line { Id = "hij", Number = 280, Type = LineType.BUS },
                new Line { Id = "klm", Number = 5, Type = LineType.TROLLEY },
                new Line { Id = "phaf", Number = 1, Type = LineType.TROLLEY }
            };

            await dbContext.Lines.AddRangeAsync(lines);
            await dbContext.SaveChangesAsync();
        }

        private async Task CreateStopsAsync()
        {
            List<Stop> stops = new()
            {
                new Stop { Code = "1000", Name = "Софийски университет \"св. Климент Охридски\"" },
                new Stop { Code = "1011", Name = "ул. \"Ген. Гурко\"" },
                new Stop { Code = "1023", Name = "пл. \"Орлов мост\"" },
                new Stop { Code = "1041", Name = "хотел \"Плиска\"" },
                new Stop { Code = "1058", Name = "площад на Авиацията" },
                new Stop { Code = "1072", Name = "Технически универистет" },
                new Stop { Code = "1093", Name = "ул. \"8-ми декември\"" },
                new Stop { Code = "1101", Name = "Студентски град" },
                new Stop { Code = "1543", Name = "метростанция \"Европейски съюз\"" },
                new Stop { Code = "1840", Name = "метростанция \"Константин Величков\"" },
                new Stop { Code = "2231", Name = "ж.к. \"Красно село\"" },
                new Stop { Code = "2254", Name = "бул. \"Христо Ботев\"" },
                new Stop { Code = "2483", Name = "бул. \"Ломско шосе\"" },
                new Stop { Code = "3002", Name = "метростанция \"Западен парк\"" },
                new Stop { Code = "3281", Name = "ул. \"Опълченска\"" },
                new Stop { Code = "3384", Name = "летище София" },
                new Stop { Code = "4432", Name = "пл. \"Македония\"" },
                new Stop { Code = "4443", Name = "бул. \"Димитър Петков\"" },
                new Stop { Code = "4555", Name = "бул. \"Черни връх\"" },
                new Stop { Code = "4766", Name = "централна ЖП гара" },
                new Stop { Code = "4887", Name = "паметник \"Васил Левски\"" },
                new Stop { Code = "5133", Name = "НДК" },
                new Stop { Code = "5529", Name = "пл. \"Руски паметник\"" },
                new Stop { Code = "5543", Name = "Телевизионна кула" },
                new Stop { Code = "5564", Name = "ул. \"Орион\"" },
                new Stop { Code = "5688", Name = "Южен парк" },
                new Stop { Code = "6122", Name = "метростанция \"Младост 1\"" },
                new Stop { Code = "6330", Name = "метростанция \"Г.М. Димитров\"" },
                new Stop { Code = "6782", Name = "бул. \"Сливница\"" },
                new Stop { Code = "8522", Name = "пл. \"Лъвов мост\"" },
                new Stop { Code = "8874", Name = "бул. \"България\"" }
            };

            await dbContext.Stops.AddRangeAsync(stops);
            await dbContext.SaveChangesAsync();
        }
    }
}