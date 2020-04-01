using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.Helper
{
    public class Helper
    {
        // Calculate and update the total flight time drone
        // Must run through all of them every time to verify integrity (in case of reassignment of drone to droneflight)
        public static void UpdateTotalDroneFlightTime(DroneDBEntities db)
        {
            List<Drone> drones = db.Drones.ToList();
            foreach(Drone d in drones)
            {
                TimeSpan totalTime = new TimeSpan(0, 0, 0);
                foreach (DroneFlight df in d.DroneFlights)
                {
                    if (df != null && df.hasDroneLog)
                    {
                        totalTime = totalTime.Add(((TimeSpan)df.DestinationInfo.UTCTime).Subtract((TimeSpan)df.DepartureInfo.UTCTime));
                    }
                }
                d.TotalFlightTime = totalTime;
                db.SaveChanges();
            }
        }
    }
}