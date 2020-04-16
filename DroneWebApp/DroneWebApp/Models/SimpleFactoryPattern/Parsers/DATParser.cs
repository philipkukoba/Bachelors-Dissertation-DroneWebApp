using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.WebPages;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class DATParser : IParser
    {
        public bool Parse(string path, int flightId, DroneDBEntities db)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);
            DroneLogEntry droneLogEntry;
            DroneRTKData droneRTK;
            DroneIMU_ATTI droneIMU;
            DroneMotor droneMotor;
            DroneRC droneRC;
            DroneGP droneGPS;
            DroneOA droneOA;
            DepartureInfo departureInfo;
            DestinationInfo destinationInfo;
            int startTime = 0;
            int finalTime = 0;
            double startLat = 0;
            double startLong = 0;
            double endLat = 0;
            double endLong = 0;
            bool firstRead = false; // bool to read in the starting time and starting latitude & longitude

            Dictionary<string, int> dict = null;

            // Do not parse a new file, if this flight already has a DAT file
            if (droneFlight.hasDroneLog)
            {
                return false;
            }

            // calculate the total amount of lines by going through the whole file once
            int totalLines = Helper.Helper.CountFileLines(path);
            System.Diagnostics.Debug.WriteLine("File size: " + totalLines + " lines\n"); // test

            // Prepare map useful fields
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int lineNo = 0;
                dict = new Dictionary<string, int>();

                CultureInfo customCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                string[] fields = parser.ReadFields();

                #region Read headers of csv file
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].Equals("Tick#"))
                    {
                        dict.Add("Tick#", i);
                    }
                    else if (fields[i].Equals("offsetTime"))
                    {
                        dict.Add("offsetTime", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):GPS-H"))
                    {
                        dict.Add("IMU_ATTI(0):GPS-H", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):roll"))
                    {
                        dict.Add("IMU_ATTI(0):roll", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):pitch"))
                    {
                        dict.Add("IMU_ATTI(0):pitch", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):yaw"))
                    {
                        dict.Add("IMU_ATTI(0):yaw", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):distanceTravelled"))
                    {
                        dict.Add("IMU_ATTI(0):distanceTravelled", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):directionOfTravel[mag]"))
                    {
                        dict.Add("IMU_ATTI(0):directionOfTravel[mag]", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):directionOfTravel[true]"))
                    {
                        dict.Add("IMU_ATTI(0):directionOfTravel[true]", i);
                    }
                    else if (fields[i].Equals("IMU_ATTI(0):temperature"))
                    {
                        dict.Add("IMU_ATTI(0):temperature", i);
                    }
                    else if (fields[i].Equals("flightTime"))
                    {
                        dict.Add("flightTime", i);
                    }
                    else if (fields[i].Equals("navHealth"))
                    {
                        dict.Add("navHealth", i);
                    }
                    else if (fields[i].Equals("General:relativeHeight"))
                    {
                        dict.Add("General:relativeHeight", i);
                    }
                    else if (fields[i].Equals("GPS(0):Long"))
                    {
                        dict.Add("GPS(0):Long", i);
                    }
                    else if (fields[i].Equals("GPS(0):Lat"))
                    {
                        dict.Add("GPS(0):Lat", i);
                    }
                    else if (fields[i].Equals("GPS(0):Date"))
                    {
                        dict.Add("GPS(0):Date", i);
                    }
                    else if (fields[i].Equals("GPS(0):Time"))
                    {
                        dict.Add("GPS(0):Time", i);
                    }
                    else if (fields[i].Equals("GPS:dateTimeStamp"))
                    {
                        dict.Add("GPS:dateTimeStamp", i);
                    }
                    else if (fields[i].Equals("GPS(0):heightMSL"))
                    {
                        dict.Add("GPS(0):heightMSL", i);
                    }
                    else if (fields[i].Equals("GPS(0):hDOP"))
                    {
                        dict.Add("GPS(0):hDOP", i);
                    }
                    else if (fields[i].Equals("GPS(0):pDOP"))
                    {
                        dict.Add("GPS(0):pDOP", i);
                    }
                    else if (fields[i].Equals("GPS(0):sAcc"))
                    {
                        dict.Add("GPS(0):sAcc", i);
                    }
                    else if (fields[i].Equals("GPS(0):numGPS"))
                    {
                        dict.Add("GPS(0):numGPS", i);
                    }
                    else if (fields[i].Equals("GPS(0):numGLNAS"))
                    {
                        dict.Add("GPS(0):numGLNAS", i);
                    }
                    else if (fields[i].Equals("GPS(0):numSV"))
                    {
                        dict.Add("GPS(0):numSV", i);
                    }
                    else if (fields[i].Equals("GPS(0):velN"))
                    {
                        dict.Add("GPS(0):velN", i);
                    }
                    else if (fields[i].Equals("GPS(0):velE"))
                    {
                        dict.Add("GPS(0):velE", i);
                    }
                    else if (fields[i].Equals("GPS(0):velD"))
                    {
                        dict.Add("GPS(0):velD", i);
                    }
                    else if (fields[i].Equals("Controller:ctrlMode"))
                    {
                        dict.Add("Controller:ctrlMode", i);
                    }
                    else if (fields[i].Equals("RC:failSafe"))
                    {
                        dict.Add("RC:failSafe", i);
                    }
                    else if (fields[i].Equals("RC:dataLost"))
                    {
                        dict.Add("RC:dataLost", i);
                    }
                    else if (fields[i].Equals("RC:appLost"))
                    {
                        dict.Add("RC:appLost", i);
                    }
                    else if (fields[i].Equals("Battery:status"))
                    {
                        dict.Add("Battery:status", i);
                    }
                    else if (fields[i].Equals("BattInfo:Remaining%"))
                    {
                        dict.Add("BattInfo:Remaining%", i);
                    }
                    else if (fields[i].Equals("SMART_BATT:goHome%"))
                    {
                        dict.Add("SMART_BATT:goHome%", i);
                    }
                    else if (fields[i].Equals("SMART_BATT:land%"))
                    {
                        dict.Add("SMART_BATT:land%", i);
                    }
                    else if (fields[i].Equals("OA:avoidObst"))
                    {
                        dict.Add("OA:avoidObst", i);
                    }
                    else if (fields[i].Equals("OA:airportLimit"))
                    {
                        dict.Add("OA:airportLimit", i);
                    }
                    else if (fields[i].Equals("OA:groundForceLanding"))
                    {
                        dict.Add("OA:groundForceLanding", i);
                    }
                    else if (fields[i].Equals("OA:vertAirportLimit"))
                    {
                        dict.Add("OA:vertAirportLimit", i);
                    }
                    else if (fields[i].Equals("Motor:Current:RFront"))
                    {
                        dict.Add("Motor:Current:RFront", i);
                    }
                    else if (fields[i].Equals("Motor:Current:LFront"))
                    {
                        dict.Add("Motor:Current:LFront", i);
                    }
                    else if (fields[i].Equals("Motor:Current:LBack"))
                    {
                        dict.Add("Motor:Current:LBack", i);
                    }
                    else if (fields[i].Equals("Motor:Current:RBack"))
                    {
                        dict.Add("Motor:Current:RBack", i);
                    }
                    else if (fields[i].Equals("flyCState") && !dict.ContainsKey("flyCState"))
                    {
                        dict.Add("flyCState", i);
                    }
                    else if (fields[i].Equals("nonGPSCause"))
                    {
                        dict.Add("nonGPSCause", i);
                    }
                    else if (fields[i].Equals("compassError"))
                    {
                        dict.Add("compassError", i);
                    }
                    else if (fields[i].Equals("connectedToRC"))
                    {
                        dict.Add("connectedToRC", i);
                    }
                    else if (fields[i].Equals("Battery:lowVoltage"))
                    {
                        dict.Add("Battery:lowVoltage", i);
                    }
                    else if (fields[i].Equals("RC:ModeSwitch"))
                    {
                        dict.Add("RC:ModeSwitch", i);
                    }
                    else if (fields[i].Equals("gpsUsed"))
                    {
                        dict.Add("gpsUsed", i);
                    }
                    else if (fields[i].Equals("RTKdata:Date"))
                    {
                        dict.Add("RTKdata:Date", i);
                    }
                    else if (fields[i].Equals("RTKdata:Time"))
                    {
                        dict.Add("RTKdata:Time", i);
                    }
                    else if (fields[i].Equals("RTKdata:Lon_P"))
                    {
                        dict.Add("RTKdata:Lon_P", i);
                    }
                    else if (fields[i].Equals("RTKdata:Lat_P"))
                    {
                        dict.Add("RTKdata:Lat_P", i);
                    }
                    else if (fields[i].Equals("RTKdata:Hmsl_P"))
                    {
                        dict.Add("RTKdata:Hmsl_P", i);
                    }
                    else if (fields[i].Equals("RTKdata:Lon_S"))
                    {
                        dict.Add("RTKdata:Lon_S", i);
                    }
                    else if (fields[i].Equals("RTKdata:Lat_S"))
                    {
                        dict.Add("RTKdata:Lat_S", i);
                    }
                    else if (fields[i].Equals("RTKdata:Hmsl_S"))
                    {
                        dict.Add("RTKdata:Hmsl_S", i);
                    }
                    else if (fields[i].Equals("RTKdata:Vel_N"))
                    {
                        dict.Add("RTKdata:Vel_N", i);
                    }
                    else if (fields[i].Equals("RTKdata:Vel_E"))
                    {
                        dict.Add("RTKdata:Vel_E", i);
                    }
                    else if (fields[i].Equals("RTKdata:Vel_D"))
                    {
                        dict.Add("RTKdata:Vel_D", i);
                    }
                    else if (fields[i].Equals("RTKdata:hdop"))
                    {
                        dict.Add("RTKdata:hdop", i);
                    }
                    else if (fields[i].Equals("Attribute|Value"))
                    {
                        dict.Add("Attribute|Value", i);
                    }
                }
                #endregion

                #region Parse the file
                // Parse the file
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    try
                    {
                        // Create ORM object
                        droneLogEntry = new DroneLogEntry();

                        // Read data
                        fields = parser.ReadFields();

                        // **DroneLogEntry**
                        droneLogEntry.BatteryLowVoltage = dict.ContainsKey("Battery:lowVoltage") ? fields[dict["Battery:lowVoltage"]] : "";
                        droneLogEntry.BatteryStatus = dict.ContainsKey("Battery:status") ? fields[dict["Battery:status"]] : "";
                        droneLogEntry.BatteryPercentage = dict.ContainsKey("BattInfo:Remaining%") ? (Double.TryParse(fields[dict["BattInfo:Remaining%"]], out double dValue) ? dValue : 0.0) : 0.0;
                        droneLogEntry.CompassError = dict.ContainsKey("compassError") ? fields[dict["compassError"]] : "";
                        droneLogEntry.ConnectedToRC = dict.ContainsKey("connectedToRC") ? fields[dict["connectedToRC"]] : "";
                        droneLogEntry.ControllerCTRLMode = dict.ContainsKey("Controller:ctrlMode") ? fields[dict["Controller:ctrlMode"]] : "";
                        droneLogEntry.FlightTime = dict.ContainsKey("flightTime") ? (Int32.TryParse(fields[dict["flightTime"]], out int iValue) ? iValue : 0) : 0;
                        droneLogEntry.FlyCState = dict.ContainsKey("flyCState") ? fields[dict["flyCState"]] : "";
                        droneLogEntry.GeneralRelHeight = dict.ContainsKey("General:relativeHeight") ? (Double.TryParse(fields[dict["General:relativeHeight"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneLogEntry.GPSUsed = dict.ContainsKey("gpsUsed") ? fields[dict["gpsUsed"]] : "";
                        droneLogEntry.NavHealth = dict.ContainsKey("navHealth") ? (Int32.TryParse(fields[dict["navHealth"]], out iValue) ? iValue : 0) : 0;
                        droneLogEntry.NonGPSCause = dict.ContainsKey("nonGPSCause") ? fields[dict["nonGPSCause"]] : "";
                        droneLogEntry.OffsetTime = dict.ContainsKey("offsetTime") ? (Double.TryParse(fields[dict["offsetTime"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneLogEntry.SmartBattGoHome = dict.ContainsKey("SMART_BATT:goHome%") ? (Int32.TryParse(fields[dict["SMART_BATT:goHome%"]], out iValue) ? iValue : 0) : 0;
                        droneLogEntry.SmartBattLand = dict.ContainsKey("SMART_BATT:land%") ? (Int32.TryParse(fields[dict["SMART_BATT:land%"]], out iValue) ? iValue : 0) : 0;
                        droneLogEntry.Tick_no = dict.ContainsKey("Tick#") ? (long.TryParse(fields[dict["Tick#"]], out long lValue) ? lValue : 0) : 0;


                        // Assign data the appropriate FlightId
                        droneLogEntry.FlightId = droneFlight.FlightId;
                        
                        // Add the DroneLogEntry to the list to be added to the database
                        db.DroneLogEntries.Add(droneLogEntry);

                        // Commit changes to the DB
                        db.SaveChanges();


                        // Create ORM objects that have 1-to-1 relationship with a DroneLogEntry
                        droneRTK = new DroneRTKData();
                        droneIMU = new DroneIMU_ATTI();
                        droneMotor = new DroneMotor();
                        droneRC = new DroneRC();
                        droneGPS = new DroneGP();
                        droneOA = new DroneOA();


                        // **DroneRTK**
                        droneRTK.Date = dict.ContainsKey("RTKdata:Date") ? (Int32.TryParse(fields[dict["RTKdata:Date"]], out iValue) ? iValue : 0) : 0;
                        droneRTK.HDOP = dict.ContainsKey("RTKdata:hdop") ? (Double.TryParse(fields[dict["RTKdata:hdop"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.HmslP = dict.ContainsKey("RTKdata:Hmsl_P") ? (Double.TryParse(fields[dict["RTKdata:Hmsl_P"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.HmslS = dict.ContainsKey("RTKdata:Hmsl_S") ? (Double.TryParse(fields[dict["RTKdata:Hmsl_S"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.LatP = dict.ContainsKey("RTKdata:Lat_P") ? (Double.TryParse(fields[dict["RTKdata:Lat_P"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.LatS = dict.ContainsKey("RTKdata:Lat_S") ? (Double.TryParse(fields[dict["RTKdata:Lat_S"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.LonP = dict.ContainsKey("RTKdata:Lon_P") ? (Double.TryParse(fields[dict["RTKdata:Lon_P"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.LonS = dict.ContainsKey("RTKdata:Lon_S") ? (Double.TryParse(fields[dict["RTKdata:Lon_S"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.Time = dict.ContainsKey("RTKdata:Time") ? (Int32.TryParse(fields[dict["RTKdata:Time"]], out iValue) ? iValue : 0) : 0;
                        droneRTK.VelD = dict.ContainsKey("RTKdata:Vel_D") ? (Double.TryParse(fields[dict["RTKdata:Vel_D"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.VelE = dict.ContainsKey("RTKdata:Vel_E") ? (Double.TryParse(fields[dict["RTKdata:Vel_E"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneRTK.VelN = dict.ContainsKey("RTKdata:Vel_N") ? (Double.TryParse(fields[dict["RTKdata:Vel_N"]], out dValue) ? dValue : 0.0) : 0.0;

                        // **DroneIMU**
                        droneIMU.DistanceTravelled = dict.ContainsKey("IMU_ATTI(0):distanceTravelled") ? (Double.TryParse(fields[dict["IMU_ATTI(0):distanceTravelled"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneIMU.GPS_H = dict.ContainsKey("IMU_ATTI(0):GPS-H") ? (Double.TryParse(fields[dict["IMU_ATTI(0):GPS-H"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneIMU.MagDirectionOfTravel = dict.ContainsKey("IMU_ATTI(0):directionOfTravel[mag]") ? (Double.TryParse(fields[dict["IMU_ATTI(0):directionOfTravel[mag]"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneIMU.Pitch = dict.ContainsKey("IMU_ATTI(0):pitch") ? (Double.TryParse(fields[dict["IMU_ATTI(0):pitch"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneIMU.Roll = dict.ContainsKey("IMU_ATTI(0):roll") ? (Double.TryParse(fields[dict["IMU_ATTI(0):roll"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneIMU.Temperature = dict.ContainsKey("IMU_ATTI(0):temperature") ? (Double.TryParse(fields[dict["IMU_ATTI(0):temperature"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneIMU.TrueDirectionOfTravel = dict.ContainsKey("IMU_ATTI(0):directionOfTravel[true]") ? (Double.TryParse(fields[dict["IMU_ATTI(0):directionOfTravel[true]"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneIMU.Yaw = dict.ContainsKey("IMU_ATTI(0):yaw") ? (Double.TryParse(fields[dict["IMU_ATTI(0):yaw"]], out dValue) ? dValue : 0.0) : 0.0;

                        // **DroneMotor**
                        droneMotor.CurrentLBack = dict.ContainsKey("Motor:Current:LBack") ? (Double.TryParse(fields[dict["Motor:Current:LBack"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneMotor.CurrentLFront = dict.ContainsKey("Motor:Current:LFront") ? (Double.TryParse(fields[dict["Motor:Current:LFront"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneMotor.CurrentRBack = dict.ContainsKey("Motor:Current:RBack") ? (Double.TryParse(fields[dict["Motor:Current:RBack"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneMotor.CurrentRFront = dict.ContainsKey("Motor:Current:RFront") ? (Double.TryParse(fields[dict["Motor:Current:RFront"]], out dValue) ? dValue : 0.0) : 0.0;

                        // **DroneRC**
                        droneRC.AppLost = dict.ContainsKey("RC:appLost") ? fields[dict["RC:appLost"]] : "";
                        droneRC.DataLost = dict.ContainsKey("RC:dataLost") ? fields[dict["RC:dataLost"]] : "";
                        droneRC.FailSafe = dict.ContainsKey("RC:failSafe") ? fields[dict["RC:failSafe"]] : "";
                        droneRC.ModeSwitch = dict.ContainsKey("RC:ModeSwitch") ? fields[dict["RC:ModeSwitch"]] : "";

                        // **DroneGPS**
                        droneGPS.Date = dict.ContainsKey("GPS(0):Date") ? (Int32.TryParse(fields[dict["GPS(0):Date"]], out iValue) ? iValue : 0) : 0;
                        droneGPS.DateTimeStamp = dict.ContainsKey("GPS:dateTimeStamp") ? DateTime.Parse(fields[dict["GPS:dateTimeStamp"]]) : DateTime.MinValue;
                        droneGPS.HDOP = dict.ContainsKey("GPS(0):hDOP") ? (Double.TryParse(fields[dict["GPS(0):hDOP"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.HeightMSL = dict.ContainsKey("GPS(0):heightMSL") ? (Double.TryParse(fields[dict["GPS(0):heightMSL"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.Lat = dict.ContainsKey("GPS(0):Lat") ? (Double.TryParse(fields[dict["GPS(0):Lat"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.Long = dict.ContainsKey("GPS(0):Long") ? (Double.TryParse(fields[dict["GPS(0):Long"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.NumGLNAS = dict.ContainsKey("GPS(0):numGLNAS") ? (Int32.TryParse(fields[dict["GPS(0):numGLNAS"]], out iValue) ? iValue : 0) : 0;
                        droneGPS.NumGPS = dict.ContainsKey("GPS(0):numGPS") ? (Int32.TryParse(fields[dict["GPS(0):numGPS"]], out iValue) ? iValue : 0) : 0;
                        droneGPS.NumSV = dict.ContainsKey("GPS(0):numSV") ? (Int32.TryParse(fields[dict["GPS(0):numSV"]], out iValue) ? iValue : 0) : 0;
                        droneGPS.PDOP = dict.ContainsKey("GPS(0):pDOP") ? (Double.TryParse(fields[dict["GPS(0):pDOP"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.SAcc = dict.ContainsKey("GPS(0):sAcc") ? (Double.TryParse(fields[dict["GPS(0):sAcc"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.Time = dict.ContainsKey("GPS(0):Time") ? (Int32.TryParse(fields[dict["GPS(0):Time"]], out iValue) ? iValue : 0) : 0;
                        droneGPS.VelD = dict.ContainsKey("GPS(0):velD") ? (Double.TryParse(fields[dict["GPS(0):velD"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.VelE = dict.ContainsKey("GPS(0):velE") ? (Double.TryParse(fields[dict["GPS(0):velE"]], out dValue) ? dValue : 0.0) : 0.0;
                        droneGPS.VelN = dict.ContainsKey("GPS(0):velN") ? (Double.TryParse(fields[dict["GPS(0):velN"]], out dValue) ? dValue : 0.0) : 0.0;

                        // Keep track of start time and final time to calculate total flight time
                        // Keep track of start longitude and latitude
                        if (!firstRead) {
                            startTime = (int) droneGPS.Time;
                            startLong = (double) droneGPS.Long;
                            startLat = (double) droneGPS.Lat;
                            firstRead = true;
                            if (droneFlight.Location.IsEmpty())
                            {
                                try
                                {
                                    droneFlight.Location = reverseGeocode(startLong, startLat);
                                }
                                catch(NullReferenceException e)
                                {
                                    droneFlight.Location = "Location not found";
                                }
                            }
                        }
                        // These two fields will contain the final ending longitude and latitude once the while-loop ends
                        endLong = (double) droneGPS.Long;
                        endLat = (double) droneGPS.Lat;

                        // Keep track of final time of the flight
                        if (droneGPS.Time > finalTime)
                        {
                            finalTime = (int) droneGPS.Time;
                        }

                        // **DroneOA**
                        droneOA.AirportLimit = dict.ContainsKey("OA:airportLimit") ? fields[dict["OA:airportLimit"]] : "";
                        droneOA.AvoidObst = dict.ContainsKey("OA:avoidObst") ? fields[dict["OA:avoidObst"]] : "";
                        droneOA.GroundForceLanding = dict.ContainsKey("OA:groundForceLanding") ? fields[dict["OA:groundForceLanding"]] : "";
                        droneOA.VertAirportLimit = dict.ContainsKey("OA:vertAirportLimit") ? fields[dict["OA:vertAirportLimit"]] : "";
                        
                        // Map all ids 1-to-1
                        droneRTK.RTKDataId = droneLogEntry.DroneLogEntryId;
                        droneIMU.IMU_ATTI_Id = droneLogEntry.DroneLogEntryId;
                        droneMotor.MotorId = droneLogEntry.DroneLogEntryId;
                        droneRC.RCId = droneLogEntry.DroneLogEntryId;
                        droneGPS.GPSId = droneLogEntry.DroneLogEntryId;
                        droneOA.OAId = droneLogEntry.DroneLogEntryId;

                        // Add all ORM-objects to lists to be added to the database
                        db.DroneRTKDatas.Add(droneRTK);
                        db.DroneIMU_ATTI.Add(droneIMU);
                        db.DroneMotors.Add(droneMotor);
                        db.DroneRCs.Add(droneRC);
                        db.DroneGPS.Add(droneGPS);
                        db.DroneOAs.Add(droneOA);

                        //Set hasDroneLog to true for the Drone Flight
                        droneFlight.hasDroneLog = true;

                        // Commit changes to the DB
                        db.SaveChanges();

                        lineNo++;
                        if ((lineNo % 10) == 0)
                        {
                            Helper.Helper.SetProgress((lineNo / (double)totalLines) * 100);
                        }
                    }
                    catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine("Caught first exception in try/Catch: " + ex.Message);
                    }
                }
                #endregion
            }
            try
            {
                #region Departure and Destination Information
                departureInfo = new DepartureInfo();
                destinationInfo = new DestinationInfo();

                // Map all ids 1-to-1
                departureInfo.DepartureInfoId = droneFlight.FlightId;
                destinationInfo.DestinationInfoId = droneFlight.FlightId;

                // Assign Time fields for DepartureInfo and DestinationInfo of flight
                departureInfo.UTCTime = TimeSpan.ParseExact(startTime.ToString(), "hhmmss", CultureInfo.InvariantCulture);
                destinationInfo.UTCTime = TimeSpan.ParseExact(finalTime.ToString(), "hhmmss", CultureInfo.InvariantCulture);

                // Assign starting and ending longitude and latitude for DepartureInfo and DestinationInfo of flight
                departureInfo.Longitude = startLong;
                departureInfo.Latitude = startLat;
                destinationInfo.Longitude = endLong;
                destinationInfo.Latitude = endLat;

                // Add all ORM-objects to lists to be added to the database
                db.DepartureInfoes.Add(departureInfo);
                db.DestinationInfoes.Add(destinationInfo);

                // Set hasDepInfo and hasDestInfo to true for the Drone Flight
                droneFlight.hasDepInfo = true;
                droneFlight.hasDestInfo = true;
                #endregion

                // Commit changes to the DB
                db.SaveChanges();

                Helper.Helper.SetProgress(100);

                // Update the Drone's total flight time
                Helper.Helper.UpdateTotalDroneFlightTime(db);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Caught exception in second try/Catch: " + ex.Message);
                //System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.ToString());
            }
            return true;
        }

        //Reverse geocode location from coordinates
        private string reverseGeocode(double lon, double lat)
        {
            string URL = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?f=json&featureTypes=&location=" + lon + "," + lat;

            //GET rest call
            WebRequest requestObjGet = WebRequest.Create(URL);
            requestObjGet.Method = "GET";
            HttpWebResponse responseObjGet = null;
            responseObjGet = (HttpWebResponse)requestObjGet.GetResponse();

            //Generate string containing json from rest call
            string jsonString = null;
            using (Stream stream = responseObjGet.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                jsonString = sr.ReadToEnd();
                sr.Close();
            }

            //Convert jsonString to JObject and get city from JObject
            JObject json = JObject.Parse(jsonString);
            string location = (string)json["address"]["City"];

            return location;
        }
    }
}
 