using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using UrbanLife.Core.Utilities;
using UrbanLife.Core.ViewModels;
using UrbanLife.Data.Data;
using UrbanLife.Data.Data.Models;
using Wintellect.PowerCollections;

namespace UrbanLife.Core.Services
{
    public class TravelService
    {
        private readonly ApplicationDbContext dbContext;

        public TravelService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string[]> FindFastestRouteAsync(string firstStop, string lastStop, TimeSpan startTime)
        {
            Dictionary<string, List<Route>> routesByStop = await FillRoutesAsync(startTime);

            Dictionary<string, double> distance = routesByStop.ToDictionary(e => e.Key, e => double.PositiveInfinity);
            Dictionary<string, string> parent = routesByStop.ToDictionary(e => e.Key, e => string.Empty);

            distance[firstStop] = 0;

            OrderedBag<string> bag = new OrderedBag<string>(Comparer<string>.Create((f, s) => (int)(distance[f] - distance[s])));

            bag.Add(firstStop);

            while (bag.Count > 0)
            {
                string minNode = bag.RemoveFirst();

                if (minNode == null)
                {
                    break;
                }

                if (minNode == lastStop)
                {
                    break;
                }

                foreach (Route edge in routesByStop[minNode])
                {
                    string otherNode = edge.First == minNode ? edge.Second : edge.First;

                    if (otherNode != null && distance[otherNode] == double.PositiveInfinity)
                    {
                        bag.Add(otherNode);
                    }

                    double newDistance = distance[minNode] + edge.Time;

                    if (otherNode != null && newDistance < distance[otherNode])
                    {
                        parent[otherNode] = minNode;
                        distance[otherNode] = newDistance;
                        bag = new OrderedBag<string>(bag, Comparer<string>.Create((f, s) => (int)(distance[f] - distance[s])));
                    }
                }
            }

            if (!distance.ContainsKey(lastStop) || distance[lastStop] == double.PositiveInfinity)
            {
                return null;
            }
            else
            {
                string currentNode = lastStop;
                Stack<string> path = new Stack<string>();

                while (currentNode != string.Empty)
                {
                    path.Push(currentNode);
                    currentNode = parent[currentNode];
                }

                return path.ToArray();
            }
        }

        public async Task<Dictionary<string, RouteViewModel>> GetRouteInfoAsync(string[] stopCodes, TimeSpan wantedTime)
        {
            Dictionary<string, RouteViewModel> repeatingRoutes = new();

            for (int i = 0; i < stopCodes.Length - 1; i++)
            {
                RouteViewModel? route = await dbContext.Schedules
                    .Where(s => s.StopCode == stopCodes[i] && s.NextStopCode == stopCodes[i + 1])
                    .Where(s => s.Arrival > wantedTime)
                    .OrderBy(s => s.Arrival)
                    .Select(s => new RouteViewModel
                    {
                        StopName = s.Stop.Name,
                        StopCode = s.StopCode,
                        NextStopCode = s.NextStopCode,
                        NextStopName = s.NextStop.Name,
                        LineNumber = s.Line.Number,
                        LineType = s.Line.Type,
                        LineId = s.LineId,
                        Arrival = s.Arrival
                    })
                    .FirstOrDefaultAsync();

                if (route != null)
                {

                    if (repeatingRoutes.ContainsKey(route.LineId))
                    {
                        repeatingRoutes[route.LineId].LineRepeatings++;
                        repeatingRoutes[route.LineId].NextStopCode = route.NextStopCode;
                        repeatingRoutes[route.LineId].NextStopName = route.NextStopName;

                        // Пресмята разликата в спирките на една и съща линия
                        //TimeSpan newDuration = route.Arrival.Subtract(repeatingRoutes[route.LineId].Arrival);
                        //
                        //repeatingRoutes[route.LineId].Arrival = repeatingRoutes[route.LineId].Arrival.Add(newDuration);
                    }
                    else
                    {
                        repeatingRoutes.Add(route.LineId, route);
                    }

                    wantedTime = route.Arrival;
                }
            }

            return repeatingRoutes;
        }

        private async Task<Dictionary<string, List<Route>>> FillRoutesAsync(TimeSpan startTime)
        {
            List<Schedule> schedules = await dbContext.Schedules
                .Include(s => s.Line)
                .Include(s => s.Stop)
                .AsNoTracking()
                .ToListAsync();

            Dictionary<string, List<Route>> routesByStop = new();

            foreach (Schedule schedule in schedules)
            {
                string firstStop = schedule.StopCode;
                string? secondStop = schedule.NextStopCode;

                Route edge = new()
                {
                    First = firstStop,
                    Second = secondStop,
                    Time = 1
                };

                if (!routesByStop.ContainsKey(firstStop))
                {
                    routesByStop.Add(firstStop, new List<Route>());
                }

                if (secondStop != null && !routesByStop.ContainsKey(secondStop))
                {
                    routesByStop.Add(secondStop, new List<Route>());
                }

                //if (secondStop != null)
                //{
                    routesByStop[firstStop].Add(edge);
                    //routesByStop[secondStop].Add(edge);
                //}
            }

            return routesByStop;
        }
    }
}