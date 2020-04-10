using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.Helper
{
    public class Helper
    {
        public static double progress = 0;
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

        // Runs through a file once to count its total amount of lines
        public static int CountFileLines(string path)
        {
            int totalLines = 0;
            using (StreamReader r = new StreamReader(path))
            {
                while (r.ReadLine() != null)
                {
                    totalLines++;
                }
            }
            return totalLines;
        }

        public static void SetProgress(double newProgress)
        {
            progress = newProgress;
        }
    }
}