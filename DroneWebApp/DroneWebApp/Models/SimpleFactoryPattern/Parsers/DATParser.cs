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
        public void Parse(string path, int flightId, DroneDBEntities db)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);

            DroneLog droneLog;
            DroneRTKData droneRTK;
            DroneIMU_ATTI droneIMU;
            DroneMotor droneMotor;
            DroneRC droneRC;
            DroneGP droneGPS;
            DroneOA droneOA;

            Dictionary<string, int> dict = new Dictionary<string, int>();

            using (TextFieldParser parser = new TextFieldParser(@"C:\Users\niels\OneDrive\Documents\UGent\Industrieel ingenieur\3de bachelor\Bachelorproef\Drone\Fields of interest_drone logging.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    dict.Add(fields[0], 1);
                }
            }

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                CultureInfo customCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                string[] fields = parser.ReadFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (dict.ContainsKey(fields[i]))
                    {
                        dict[fields[i]] = i;
                    }
                }
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    droneLog = new DroneLog();
                    droneRTK = new DroneRTKData();
                    droneIMU = new DroneIMU_ATTI();
                    droneMotor = new DroneMotor();
                    droneRC = new DroneRC();
                    droneGPS = new DroneGP();
                    droneOA = new DroneOA();

                    fields = parser.ReadFields();

                    droneLog.FlightId = droneFlight.FlightId;
                    droneLog.BatteryLowVoltage = fields[dict["Battery:lowVoltage"]];
                    droneLog.BatteryStatus = fields[dict["Battery:status"]];
                    droneLog.CompassError = fields[dict["compassError"]];
                    droneLog.ConnectedToRC = fields[dict["connectedToRC"]];
                    droneLog.ControllerCTRLMode = fields[dict["Controller:ctrlMode"]];
                    droneLog.FlightTime = Int32.TryParse(fields[dict["flightTime"]], out int iValue) ? iValue : 0;
                    droneLog.FlyCState = fields[dict["flyCState"]];
                    droneLog.GeneralRelHeight = Double.TryParse(fields[dict["General:relativeHeight"]], out double dValue) ? dValue : 0.0;
                    droneLog.GPSUsed = fields[dict["gpsUsed"]];
                    droneLog.NavHealth = Int32.TryParse(fields[dict["navHealth"]], out iValue) ? iValue : 0;
                    droneLog.NonGPSCause = fields[dict["nonGPSCause"]];
                    droneLog.OffsetTime = Double.TryParse(fields[dict["offsetTime"]], out dValue) ? dValue : 0.0;
                    droneLog.SmartBattGoHome = Int32.TryParse(fields[dict["SMART_BATT:goHome%"]], out iValue) ? iValue : 0;
                    droneLog.SmartBattLand = Int32.TryParse(fields[dict["SMART_BATT:land%"]], out iValue) ? iValue : 0;
                    droneLog.Tick_no = long.TryParse(fields[dict["Tick#"]], out long lValue) ? lValue : 0;

                    droneRTK.DroneLogId = droneLog.DroneLogId;
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

                    droneIMU.DroneLogId = droneLog.DroneLogId;
                    droneIMU.DistanceTravelled = Double.TryParse(fields[dict["IMU_ATTI(0):distanceTravelled"]], out dValue) ? dValue : 0.0;
                    droneIMU.GPS_H = Double.TryParse(fields[dict["IMU_ATTI(0):GPS-H"]], out dValue) ? dValue : 0.0;
                    droneIMU.MagDirectionOfTravel = Double.TryParse(fields[dict["IMU_ATTI(0):directionOfTravel[mag]"]], out dValue) ? dValue : 0.0;
                    droneIMU.Pitch = Double.TryParse(fields[dict["IMU_ATTI(0):pitch"]], out dValue) ? dValue : 0.0;
                    droneIMU.Roll = Double.TryParse(fields[dict["IMU_ATTI(0):roll"]], out dValue) ? dValue : 0.0;
                    droneIMU.Temperature = Double.TryParse(fields[dict["IMU_ATTI(0):temperature"]], out dValue) ? dValue : 0.0;
                    droneIMU.TrueDirectionOfTravel = Double.TryParse(fields[dict["IMU_ATTI(0):directionOfTravel[true]"]], out dValue) ? dValue : 0.0;
                    droneIMU.Yaw = Double.TryParse(fields[dict["IMU_ATTI(0):yaw"]], out dValue) ? dValue : 0.0;

                    droneMotor.DroneLogId = droneLog.DroneLogId;
                    droneMotor.CurrentLBack = Double.TryParse(fields[dict["Motor:Current:LBack"]], out dValue) ? dValue : 0.0;
                    droneMotor.CurrentLFront = Double.TryParse(fields[dict["Motor:Current:LFront"]], out dValue) ? dValue : 0.0;
                    droneMotor.CurrentRBack = Double.TryParse(fields[dict["Motor:Current:RBack"]], out dValue) ? dValue : 0.0;
                    droneMotor.CurrentRFront = Double.TryParse(fields[dict["Motor:Current:RFront"]], out dValue) ? dValue : 0.0;

                    droneRC.DroneLogId = droneLog.DroneLogId;
                    droneRC.AppLost = fields[dict["RC:appLost"]];
                    droneRC.DataLost = fields[dict["RC:dataLost"]];
                    droneRC.FailSafe = fields[dict["RC:failSafe"]];
                    droneRC.ModeSwitch = fields[dict["RC:ModeSwitch"]];

                    droneGPS.DroneLogId = droneLog.DroneLogId;
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

                    droneOA.DroneLogId = droneLog.DroneLogId;
                    droneOA.AirportLimit = fields[dict["OA:airportLimit"]];
                    droneOA.AvoidObst = fields[dict["OA:avoidObst"]];
                    droneOA.GroundForceLanding = fields[dict["OA:groundForceLanding"]];
                    droneOA.VertAirportLimit = fields[dict["OA:vertAirportLimit"]];

                    droneLog.DroneRTKDatas.Add(droneRTK);
                    droneLog.DroneIMU_ATTI.Add(droneIMU);
                    droneLog.DroneMotors.Add(droneMotor);
                    droneLog.DroneRCs.Add(droneRC);
                    droneLog.DroneGPS.Add(droneGPS);
                    droneLog.DroneOAs.Add(droneOA);

                    db.DroneLogs.Add(droneLog);
                }

                db.SaveChanges();
            }
        }
    }
}