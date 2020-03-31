using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

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
            int startTime = 0;
            int finalTime = 0;
            bool readStartTime = false;

            Dictionary<string, int> dict = null;
            
            // Prepare map useful fields
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                dict = new Dictionary<string, int>();

                CultureInfo customCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                string[] fields = parser.ReadFields();

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
                        droneLogEntry.BatteryLowVoltage = fields[dict["Battery:lowVoltage"]];
                        droneLogEntry.BatteryStatus = fields[dict["Battery:status"]];
                        droneLogEntry.CompassError = fields[dict["compassError"]];
                        droneLogEntry.ConnectedToRC = fields[dict["connectedToRC"]];
                        droneLogEntry.ControllerCTRLMode = fields[dict["Controller:ctrlMode"]];
                        droneLogEntry.FlightTime = Int32.TryParse(fields[dict["flightTime"]], out int iValue) ? iValue : 0;
                        droneLogEntry.FlyCState = fields[dict["flyCState"]];
                        droneLogEntry.GeneralRelHeight = Double.TryParse(fields[dict["General:relativeHeight"]], out double dValue) ? dValue : 0.0;
                        droneLogEntry.GPSUsed = fields[dict["gpsUsed"]];
                        droneLogEntry.NavHealth = Int32.TryParse(fields[dict["navHealth"]], out iValue) ? iValue : 0;
                        droneLogEntry.NonGPSCause = fields[dict["nonGPSCause"]];
                        droneLogEntry.OffsetTime = Double.TryParse(fields[dict["offsetTime"]], out dValue) ? dValue : 0.0;
                        droneLogEntry.SmartBattGoHome = Int32.TryParse(fields[dict["SMART_BATT:goHome%"]], out iValue) ? iValue : 0;
                        droneLogEntry.SmartBattLand = Int32.TryParse(fields[dict["SMART_BATT:land%"]], out iValue) ? iValue : 0;
                        droneLogEntry.Tick_no = long.TryParse(fields[dict["Tick#"]], out long lValue) ? lValue : 0;
                        
                        
                        // Assign data the appropriate FlightId
                        droneLogEntry.FlightId = droneFlight.FlightId;
                        
                        //Set hasDroneLog to true for the Drone Flight
                        droneFlight.hasDroneLog = true;

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
                        droneRTK.Date = Int32.TryParse(fields[dict["RTKdata:Date"]], out iValue) ? iValue : 0;
                        droneRTK.HDOP = Double.TryParse(fields[dict["RTKdata:hdop"]], out dValue) ? dValue : 0.0;
                        droneRTK.HmslP = Double.TryParse(fields[dict["RTKdata:Hmsl_P"]], out dValue) ? dValue : 0.0;
                        droneRTK.HmslS = Double.TryParse(fields[dict["RTKdata:Hmsl_S"]], out dValue) ? dValue : 0.0;
                        droneRTK.LatP = Double.TryParse(fields[dict["RTKdata:Lat_P"]], out dValue) ? dValue : 0.0;
                        droneRTK.LatS = Double.TryParse(fields[dict["RTKdata:Lat_S"]], out dValue) ? dValue : 0.0;
                        droneRTK.LonP = Double.TryParse(fields[dict["RTKdata:Lon_P"]], out dValue) ? dValue : 0.0;
                        droneRTK.LonS = Double.TryParse(fields[dict["RTKdata:Lon_S"]], out dValue) ? dValue : 0.0;
                        droneRTK.Time = Int32.TryParse(fields[dict["RTKdata:Time"]], out iValue) ? iValue : 0;
                        droneRTK.VelD = Double.TryParse(fields[dict["RTKdata:Vel_D"]], out dValue) ? dValue : 0.0;
                        droneRTK.VelE = Double.TryParse(fields[dict["RTKdata:Vel_E"]], out dValue) ? dValue : 0.0;
                        droneRTK.VelN = Double.TryParse(fields[dict["RTKdata:Vel_N"]], out dValue) ? dValue : 0.0;

                        // **DroneIMU**
                        droneIMU.DistanceTravelled = Double.TryParse(fields[dict["IMU_ATTI(0):distanceTravelled"]], out dValue) ? dValue : 0.0;
                        droneIMU.GPS_H = Double.TryParse(fields[dict["IMU_ATTI(0):GPS-H"]], out dValue) ? dValue : 0.0;
                        droneIMU.MagDirectionOfTravel = Double.TryParse(fields[dict["IMU_ATTI(0):directionOfTravel[mag]"]], out dValue) ? dValue : 0.0;
                        droneIMU.Pitch = Double.TryParse(fields[dict["IMU_ATTI(0):pitch"]], out dValue) ? dValue : 0.0;
                        droneIMU.Roll = Double.TryParse(fields[dict["IMU_ATTI(0):roll"]], out dValue) ? dValue : 0.0;
                        droneIMU.Temperature = Double.TryParse(fields[dict["IMU_ATTI(0):temperature"]], out dValue) ? dValue : 0.0;
                        droneIMU.TrueDirectionOfTravel = Double.TryParse(fields[dict["IMU_ATTI(0):directionOfTravel[true]"]], out dValue) ? dValue : 0.0;
                        droneIMU.Yaw = Double.TryParse(fields[dict["IMU_ATTI(0):yaw"]], out dValue) ? dValue : 0.0;

                        // **DroneMotor**
                        droneMotor.CurrentLBack = Double.TryParse(fields[dict["Motor:Current:LBack"]], out dValue) ? dValue : 0.0;
                        droneMotor.CurrentLFront = Double.TryParse(fields[dict["Motor:Current:LFront"]], out dValue) ? dValue : 0.0;
                        droneMotor.CurrentRBack = Double.TryParse(fields[dict["Motor:Current:RBack"]], out dValue) ? dValue : 0.0;
                        droneMotor.CurrentRFront = Double.TryParse(fields[dict["Motor:Current:RFront"]], out dValue) ? dValue : 0.0;
                        
                        // **DroneRC**
                        droneRC.AppLost = fields[dict["RC:appLost"]];
                        droneRC.DataLost = fields[dict["RC:dataLost"]];
                        droneRC.FailSafe = fields[dict["RC:failSafe"]];
                        droneRC.ModeSwitch = fields[dict["RC:ModeSwitch"]];

                        // **DroneGPS**
                        droneGPS.Date = Int32.TryParse(fields[dict["GPS(0):Date"]], out iValue) ? iValue : 0;
                        droneGPS.DateTimeStamp = DateTime.Parse(fields[dict["GPS:dateTimeStamp"]]);
                        droneGPS.HDOP = Double.TryParse(fields[dict["GPS(0):hDOP"]], out dValue) ? dValue : 0.0;
                        droneGPS.HeightMSL = Double.TryParse(fields[dict["GPS(0):heightMSL"]], out dValue) ? dValue : 0.0;
                        droneGPS.Lat = Double.TryParse(fields[dict["GPS(0):Lat"]], out dValue) ? dValue : 0.0;
                        droneGPS.Long = Double.TryParse(fields[dict["GPS(0):Long"]], out dValue) ? dValue : 0.0;
                        droneGPS.NumGLNAS = Int32.TryParse(fields[dict["GPS(0):numGLNAS"]], out iValue) ? iValue : 0;
                        droneGPS.NumGPS = Int32.TryParse(fields[dict["GPS(0):numGPS"]], out iValue) ? iValue : 0;
                        droneGPS.NumSV = Int32.TryParse(fields[dict["GPS(0):numSV"]], out iValue) ? iValue : 0;
                        droneGPS.PDOP = Double.TryParse(fields[dict["GPS(0):pDOP"]], out dValue) ? dValue : 0.0;
                        droneGPS.SAcc = Double.TryParse(fields[dict["GPS(0):sAcc"]], out dValue) ? dValue : 0.0;
                        droneGPS.Time = Int32.TryParse(fields[dict["GPS(0):Time"]], out iValue) ? iValue : 0;
                        droneGPS.VelD = Double.TryParse(fields[dict["GPS(0):velD"]], out dValue) ? dValue : 0.0;
                        droneGPS.VelE = Double.TryParse(fields[dict["GPS(0):velE"]], out dValue) ? dValue : 0.0;
                        droneGPS.VelN = Double.TryParse(fields[dict["GPS(0):velN"]], out dValue) ? dValue : 0.0;

                        // Keep track of start time and final time to calculate total flight time
                        if (!readStartTime) {
                            startTime = (int) droneGPS.Time;
                            readStartTime = true;
                        }
                        if(droneGPS.Time > finalTime)
                        {
                            finalTime = (int) droneGPS.Time;
                        }

                        // **DroneOA**
                        droneOA.AirportLimit = fields[dict["OA:airportLimit"]];
                        droneOA.AvoidObst = fields[dict["OA:avoidObst"]];
                        droneOA.GroundForceLanding = fields[dict["OA:groundForceLanding"]];
                        droneOA.VertAirportLimit = fields[dict["OA:vertAirportLimit"]];

                        
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

                        // Commit changes to the DB
                        db.SaveChanges();
                    }
                    catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine("Caught exception in second try/Catch: " + ex.Message);
                        System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.ToString());
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("Start: " + startTime + " " + "End: " + finalTime);
            try
            {
                droneFlight.StartTime = startTime.ToString();
                droneFlight.StopTime = finalTime.ToString();
                // Commit changes to the DB
                db.SaveChanges();
                // Update the Drone's total flight time
                Helper.Helper.UpdateTotalDroneFlightTime(db);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Caught exception in second try/Catch: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.ToString());
            }
            return true;
        }
    }
}