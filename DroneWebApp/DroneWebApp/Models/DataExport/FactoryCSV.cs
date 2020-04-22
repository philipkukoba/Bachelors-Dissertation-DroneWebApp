﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DroneWebApp.Models.DataExport
{
    public class FactoryCSV : IFactory
    {
        public void CreateDroneLog(int droneId, DroneDBEntities db, HttpContextBase context)
        {
            StringBuilder csvBuilder = new StringBuilder();

            List<string> columns = new List<string>
            {
                "Registration",
                "DroneType",
                "DroneName",
                "Date",
                "PilotName",
                "DepartureLongitude",
                "DepartureLatitude",
                "DepartureUTCTime",
                "DestinationLongitude",
                "DestinationLatitude",
                "DestinationUTCTime",
                "TypeOfActivity",
                "FlightTime",
                "Other",
                "Simulator",
                "Instructor",
                "Remarks"
            };

            csvBuilder.Append(string.Join(",", columns.ToArray())).Append("\n");

            List<DroneFlight> flights = db.DroneFlights.Where(df => df.DroneId == droneId).ToList();
            
            if (flights.Count != 0)
            {
                foreach (DroneFlight flight in flights)
                {
                    System.Diagnostics.Debug.WriteLine("foreach");
                    
                    if (flight.DepartureInfo == null)
                    {
                        flight.DepartureInfo = new DepartureInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }
                    if (flight.DestinationInfo == null)
                    {
                        flight.DestinationInfo = new DestinationInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }

                    List<string> fields = new List<string>
                    {
                        flight.Drone.Registration ?? "",
                        flight.Drone.DroneType ?? "",
                        flight.Drone.DroneName ?? "",
                        flight.Date.ToString() ?? "",
                        flight.Pilot.PilotName ?? "",
                        flight.DepartureInfo.Longitude.ToString() ?? "",
                        flight.DepartureInfo.Longitude.ToString() ?? "",
                        flight.DepartureInfo.UTCTime.ToString() ?? "",
                        flight.DestinationInfo.Longitude.ToString() ?? "",
                        flight.DestinationInfo.Latitude.ToString() ?? "",
                        flight.DestinationInfo.UTCTime.ToString() ?? "",
                        flight.TypeOfActivity ?? "",
                        GetFlightTime(flight).ToString() ?? "",
                        flight.Other ?? "",
                        flight.Simulator ?? "",
                        flight.Instructor ?? "",
                        flight.Remarks ?? ""
                    };

                    csvBuilder.Append(string.Join(",", fields.ToArray())).Append("\n");
                }
            }

            System.Diagnostics.Debug.WriteLine(db.Drones.Find(droneId).DroneName);

            string filename = db.Drones.Find(droneId).DroneName;
            System.Diagnostics.Debug.WriteLine(filename);
            filename += droneId;
            System.Diagnostics.Debug.WriteLine(filename);
            filename = ReplaceInvalidChars(filename);
            System.Diagnostics.Debug.WriteLine(filename);

            context.Response.Clear();
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".csv");
            context.Response.Write(csvBuilder.ToString());
            context.Response.End();
        }
        
        public void CreatePilotLog(int pilotId, DroneDBEntities db, HttpContextBase context)
        {
            StringBuilder csvBuilder = new StringBuilder();

            List<string> columns = new List<string>
            {
                "PilotName",
                "Street",
                "ZIP",
                "City",
                "Country",
                "Phone",
                "LicenseNo",
                "Email",
                "EmergencyPhone",
                "Date",
                "DepartureLongitude",
                "DepartureLatitude",
                "DepartureUTCTime",
                "DestinationLongitude",
                "DestinationLatitude",
                "DestinationUTCTime",
                "Registration",
                "DroneType",
                "FlightTime",
                "Other",
                "Simulator",
                "Instructor",
                "Remarks"
            };

            csvBuilder.Append(string.Join(",", columns.ToArray())).Append("\n");

            List<DroneFlight> flights = db.DroneFlights.Where(df => df.PilotId == pilotId).ToList();

            if (flights.Count != 0)
            {
                foreach (DroneFlight flight in flights)
                {
                    if (flight.DepartureInfo == null)
                    {
                        flight.DepartureInfo = new DepartureInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }
                    if (flight.DestinationInfo == null)
                    {
                        flight.DestinationInfo = new DestinationInfo
                        {
                            Longitude = null,
                            Latitude = null,
                            UTCTime = null
                        };
                    }

                    List<string> fields = new List<string>
                    {
                        flight.Pilot.PilotName ?? "",
                        flight.Pilot.Street ?? "",
                        flight.Pilot.ZIP.ToString() ?? "",
                        flight.Pilot.City ?? "",
                        flight.Pilot.Country ?? "",
                        flight.Pilot.Phone ?? "",
                        flight.Pilot.LicenseNo.ToString() ?? "",
                        flight.Pilot.Email ?? "",
                        flight.Pilot.EmergencyPhone ?? "",
                        flight.Date.ToString() ?? "",
                        flight.DepartureInfo.Longitude.ToString() ?? "",
                        flight.DepartureInfo.Longitude.ToString() ?? "",
                        flight.DepartureInfo.UTCTime.ToString() ?? "",
                        flight.DestinationInfo.Longitude.ToString() ?? "",
                        flight.DestinationInfo.Latitude.ToString() ?? "",
                        flight.DestinationInfo.UTCTime.ToString() ?? "",
                        flight.Drone.Registration ?? "",
                        flight.Drone.DroneType ?? "",
                        GetFlightTime(flight).ToString() ?? "",
                        flight.Other ?? "",
                        flight.Simulator ?? "",
                        flight.Instructor ?? "",
                        flight.Remarks ?? ""
                    };

                    csvBuilder.Append(string.Join(",", fields.ToArray())).Append("\n");
                }
            }

            string filename = db.Pilots.Find(pilotId).PilotName;
            filename += pilotId;
            filename = ReplaceInvalidChars(filename);

            context.Response.Clear();
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".csv");
            context.Response.Write(csvBuilder.ToString());
            context.Response.End();
        }

        private TimeSpan? GetFlightTime(DroneFlight flight)
        {
            TimeSpan totalTime = new TimeSpan(0, 0, 0);
            if (flight != null && flight.hasDroneLog)
            {
                totalTime = totalTime.Add(((TimeSpan)flight.DestinationInfo.UTCTime).Subtract((TimeSpan)flight.DepartureInfo.UTCTime));
            }
            return totalTime;
        }

        private string ReplaceInvalidChars(string filename)
        {
            string result = string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
            result = string.Join("", result.Split(' '));

            return result;
        }
    }
}