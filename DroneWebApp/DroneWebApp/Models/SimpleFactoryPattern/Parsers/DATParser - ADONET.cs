using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
	public class DATParser___ADONET: IParser
	{
		private ConnectionStringSettings connStringSet;
		private DbProviderFactory factory;

        #region  command texts 
        private string InsertionCommand_DroneLogEntry = "insert into DroneLogEntry " +
						"(Tick_no, OffsetTime, FlightTime, NavHealth, GeneralRelHeight, FlyCState, ControllerCTRLMode, BatteryStatus, BatteryPercentage, SmartBattGoHome, SmartBattLand, NonGPSCause, CompassError, ConnectedToRC, BatteryLowVoltage, GPSUsed, FlightId)" +
						"VALUES (@Tick_no, @OffsetTime, @FlightTime, @NavHealth, @GeneralRelHeight, @FlyCState, @ControllerCTRLMode, @BatteryStatus, @BatteryPercentage, @SmartBattGoHome, @SmartBattLand, @NonGPSCause, @CompassError, @ConnectedToRC, @BatteryLowVoltage, @GPSUsed, @FlightId )";
		private string InsertionCommand_DroneIMU_ATTI = "insert into DroneIMU_ATTI" + 
						"(GPS_H, Roll, Pitch, Yaw, DistanceTravelled, MagDirectionOfTravel, TrueDirectionOfTravel, Temperature, VelComposite)" +
						"VALUES (@GPS_H, @Roll, @Pitch, @Yaw, @DistanceTravelled, @MagDirectionOfTravel, @TrueDirectionOfTravel, @Temperature, @VelComposite)";
		private string InsertionCommand_DroneRC = "insert into DroneRC" +
						"(FailSafe, DataLost, AppLost, ModeSwitch)" +
						"VALUES (@FailSafe, @DataLost, @AppLost, @ModeSwitch)";
		private string InsertionCommand_DroneGPS = "insert into DroneGPS" +
						"(Long, Lat, Date, Time, DateTimeStamp, HeightMSL, HDOP, PDOP, SAcc, NumGPS, NumGLNAS, NumSV, VeIN, VeIE, VeID)" +
						"VALUES (@Long, @Lat, @Date, @Time, @DateTimeStamp, @HeightMSL, @HDOP, @PDOP, @SAcc, @NumGPS, @NumGLNAS, @NumSV, @VeIN, @VeIE, @VeID)";
		private string InsertionCommand_DroneRTKData = "insert into DroneRTKData" +
						"(Date, Time, LonP, LatP, HmslP, LonS, LatS, HmslS, VeIN, VeIE, VeID, HDOP)" +
						"VALUES (@Date, @Time, @LonP, @LatP, @HmslP, @LonS, @LatS, @HmslS, @VeIN, @VeIE, @VeID, @HDOP)";
		private string InsertionCommand_DroneOA = "insert into DroneOA" +
						"(AvoidObst, AirportLimit, GroundForceLanding, VertAirportLimit)" +
						"VALUES (@AvoidObst, @AirportLimit, @GroundForceLanding, @VertAirportLimit)";
		private string InsertionCommand_DroneMotor = "insert into DroneMotor" +
						"(CurrentRFront, CurrentLFRont, CurrentLBack, CurrentRBack)" +
						"VALUES (@CurrentRFront, @CurrentLFRont, @CurrentLBack, @CurrentRBack)";
        #endregion 

        private Dictionary<string, string> fieldsAndParameters;

        public DATParser___ADONET()
		{
            fieldsAndParameters = fillDictionary(); 
			connStringSet = ConfigurationManager.ConnectionStrings["DroneDB_ADONET"];
			factory = DbProviderFactories.GetFactory(connStringSet.ProviderName);
		}

        private Dictionary<string, string> fillDictionary()
        {
            Dictionary<string, string> fieldsAndParameters = new Dictionary<string, string>();
            fieldsAndParameters.Add("Battery:lowVoltage", "@BatteryLowVoltage");
            fieldsAndParameters.Add("Battery:status", "@BatteryStatus");
            fieldsAndParameters.Add("BattInfo:Remaining%", "@BatteryPercentage");
            fieldsAndParameters.Add("compassError", "@CompassError");
            fieldsAndParameters.Add("connectedToRC", "@ConnectedToRC");
            fieldsAndParameters.Add("Controller:ctrlMode", "@ControllerCTRLMode");
            fieldsAndParameters.Add("flightTime", "@FlightTime");
            fieldsAndParameters.Add("flyCState", "@FlyCState");
            fieldsAndParameters.Add("General:relativeHeight", "@GeneralRelHeight");
            fieldsAndParameters.Add("gpsUsed", "@GPSUsed");
            fieldsAndParameters.Add("navHealth", "@NavHealth");
            fieldsAndParameters.Add("nonGPSCause", "@NonGPSCause");
            fieldsAndParameters.Add("offsetTime", "@OffsetTime");
            fieldsAndParameters.Add("SMART_BATT:goHome%", "@SmartBattGoHome");
            fieldsAndParameters.Add("SMART_BATT:land%", "@SmartBattLand");
            fieldsAndParameters.Add("Tick#", "@Tick_no");
            return fieldsAndParameters;
        }

        public bool Parse(string path, int flightId, DroneDBEntities db)
		{
			// Get the approriate DroneFlight that goes with this data
			DroneFlight droneFlight = db.DroneFlights.Find(flightId);

			// Do not parse a new file, if this flight already has an XYZ file
			if (droneFlight.hasDroneLog) return false;

			// calculate the total amount of lines by going through the whole file once
			int totalLines = Helper.Helper.CountFileLines(path);
			System.Diagnostics.Debug.WriteLine("File size: " + totalLines + " lines\n"); // test

			// Parse
			using (TextFieldParser parser = new TextFieldParser(path))
			{
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                #region Set culture to ensure decimal point
                CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
                #endregion

                using (DbConnection connection = factory.CreateConnection())
				{
					connection.ConnectionString = connStringSet.ConnectionString;
					connection.Open();
		
                    Dictionary<string, int> headerDict = new Dictionary<string, int>();
                    string[] fields = parser.ReadFields();
                    #region Read headers of csv file
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (!headerDict.ContainsKey(fields[i]))
                        {
                            headerDict.Add(fields[i], i);
                        }

                        #region todo wegdoen 
                        //if (fields[i].Equals("Tick#"))
                        //{
                        //    dict.Add("Tick#", i);
                        //}
                        //else if (fields[i].Equals("offsetTime"))
                        //{
                        //    dict.Add("offsetTime", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):GPS-H"))
                        //{
                        //    dict.Add("IMU_ATTI(0):GPS-H", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):roll"))
                        //{
                        //    dict.Add("IMU_ATTI(0):roll", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):pitch"))
                        //{
                        //    dict.Add("IMU_ATTI(0):pitch", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):yaw"))
                        //{
                        //    dict.Add("IMU_ATTI(0):yaw", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):distanceTravelled"))
                        //{
                        //    dict.Add("IMU_ATTI(0):distanceTravelled", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):directionOfTravel[mag]"))
                        //{
                        //    dict.Add("IMU_ATTI(0):directionOfTravel[mag]", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):directionOfTravel[true]"))
                        //{
                        //    dict.Add("IMU_ATTI(0):directionOfTravel[true]", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):temperature"))
                        //{
                        //    dict.Add("IMU_ATTI(0):temperature", i);
                        //}
                        //else if (fields[i].Equals("flightTime"))
                        //{
                        //    dict.Add("flightTime", i);
                        //}
                        //else if (fields[i].Equals("navHealth"))
                        //{
                        //    dict.Add("navHealth", i);
                        //}
                        //else if (fields[i].Equals("General:relativeHeight"))
                        //{
                        //    dict.Add("General:relativeHeight", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):Long"))
                        //{
                        //    dict.Add("GPS(0):Long", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):Lat"))
                        //{
                        //    dict.Add("GPS(0):Lat", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):Date"))
                        //{
                        //    dict.Add("GPS(0):Date", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):Time"))
                        //{
                        //    dict.Add("GPS(0):Time", i);
                        //}
                        //else if (fields[i].Equals("GPS:dateTimeStamp"))
                        //{
                        //    dict.Add("GPS:dateTimeStamp", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):heightMSL"))
                        //{
                        //    dict.Add("GPS(0):heightMSL", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):hDOP"))
                        //{
                        //    dict.Add("GPS(0):hDOP", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):pDOP"))
                        //{
                        //    dict.Add("GPS(0):pDOP", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):sAcc"))
                        //{
                        //    dict.Add("GPS(0):sAcc", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):numGPS"))
                        //{
                        //    dict.Add("GPS(0):numGPS", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):numGLNAS"))
                        //{
                        //    dict.Add("GPS(0):numGLNAS", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):numSV"))
                        //{
                        //    dict.Add("GPS(0):numSV", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):velN"))
                        //{
                        //    dict.Add("GPS(0):velN", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):velE"))
                        //{
                        //    dict.Add("GPS(0):velE", i);
                        //}
                        //else if (fields[i].Equals("GPS(0):velD"))
                        //{
                        //    dict.Add("GPS(0):velD", i);
                        //}
                        //else if (fields[i].Equals("Controller:ctrlMode"))
                        //{
                        //    dict.Add("Controller:ctrlMode", i);
                        //}
                        //else if (fields[i].Equals("RC:failSafe"))
                        //{
                        //    dict.Add("RC:failSafe", i);
                        //}
                        //else if (fields[i].Equals("RC:dataLost"))
                        //{
                        //    dict.Add("RC:dataLost", i);
                        //}
                        //else if (fields[i].Equals("RC:appLost"))
                        //{
                        //    dict.Add("RC:appLost", i);
                        //}
                        //else if (fields[i].Equals("Battery:status"))
                        //{
                        //    dict.Add("Battery:status", i);
                        //}
                        //else if (fields[i].Equals("BattInfo:Remaining%"))
                        //{
                        //    dict.Add("BattInfo:Remaining%", i);
                        //}
                        //else if (fields[i].Equals("SMART_BATT:goHome%"))
                        //{
                        //    dict.Add("SMART_BATT:goHome%", i);
                        //}
                        //else if (fields[i].Equals("SMART_BATT:land%"))
                        //{
                        //    dict.Add("SMART_BATT:land%", i);
                        //}
                        //else if (fields[i].Equals("OA:avoidObst"))
                        //{
                        //    dict.Add("OA:avoidObst", i);
                        //}
                        //else if (fields[i].Equals("OA:airportLimit"))
                        //{
                        //    dict.Add("OA:airportLimit", i);
                        //}
                        //else if (fields[i].Equals("OA:groundForceLanding"))
                        //{
                        //    dict.Add("OA:groundForceLanding", i);
                        //}
                        //else if (fields[i].Equals("OA:vertAirportLimit"))
                        //{
                        //    dict.Add("OA:vertAirportLimit", i);
                        //}
                        //else if (fields[i].Equals("Motor:Current:RFront"))
                        //{
                        //    dict.Add("Motor:Current:RFront", i);
                        //}
                        //else if (fields[i].Equals("Motor:Current:LFront"))
                        //{
                        //    dict.Add("Motor:Current:LFront", i);
                        //}
                        //else if (fields[i].Equals("Motor:Current:LBack"))
                        //{
                        //    dict.Add("Motor:Current:LBack", i);
                        //}
                        //else if (fields[i].Equals("Motor:Current:RBack"))
                        //{
                        //    dict.Add("Motor:Current:RBack", i);
                        //}
                        //else if (fields[i].Equals("flyCState") && !dict.ContainsKey("flyCState"))
                        //{
                        //    dict.Add("flyCState", i);
                        //}
                        //else if (fields[i].Equals("nonGPSCause"))
                        //{
                        //    dict.Add("nonGPSCause", i);
                        //}
                        //else if (fields[i].Equals("compassError"))
                        //{
                        //    dict.Add("compassError", i);
                        //}
                        //else if (fields[i].Equals("connectedToRC"))
                        //{
                        //    dict.Add("connectedToRC", i);
                        //}
                        //else if (fields[i].Equals("Battery:lowVoltage"))
                        //{
                        //    dict.Add("Battery:lowVoltage", i);
                        //}
                        //else if (fields[i].Equals("RC:ModeSwitch"))
                        //{
                        //    dict.Add("RC:ModeSwitch", i);
                        //}
                        //else if (fields[i].Equals("gpsUsed"))
                        //{
                        //    dict.Add("gpsUsed", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Date"))
                        //{
                        //    dict.Add("RTKdata:Date", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Time"))
                        //{
                        //    dict.Add("RTKdata:Time", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Lon_P"))
                        //{
                        //    dict.Add("RTKdata:Lon_P", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Lat_P"))
                        //{
                        //    dict.Add("RTKdata:Lat_P", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Hmsl_P"))
                        //{
                        //    dict.Add("RTKdata:Hmsl_P", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Lon_S"))
                        //{
                        //    dict.Add("RTKdata:Lon_S", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Lat_S"))
                        //{
                        //    dict.Add("RTKdata:Lat_S", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Hmsl_S"))
                        //{
                        //    dict.Add("RTKdata:Hmsl_S", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Vel_N"))
                        //{
                        //    dict.Add("RTKdata:Vel_N", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Vel_E"))
                        //{
                        //    dict.Add("RTKdata:Vel_E", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:Vel_D"))
                        //{
                        //    dict.Add("RTKdata:Vel_D", i);
                        //}
                        //else if (fields[i].Equals("RTKdata:hdop"))
                        //{
                        //    dict.Add("RTKdata:hdop", i);
                        //}
                        //else if (fields[i].Equals("Attribute|Value"))
                        //{
                        //    dict.Add("Attribute|Value", i);
                        //}
                        //else if (fields[i].Equals("IMU_ATTI(0):velComposite"))
                        //{
                        //    dict.Add("IMU_ATTI(0):velComposite", i);
                        //}
                        #endregion 
                    }
                    #endregion

                    #region Create MAIN command
                    DbCommand command = connection.CreateCommand();
					command.Connection = connection;
					command.CommandText = "insert into DroneLogEntry " +
						"(Tick_no, OffsetTime, FlightTime, NavHealth, GeneralRelHeight, FlyCState, ControllerCTRLMode, BatteryStatus, BatteryPercentage, SmartBattGoHome, SmartBattLand, NonGPSCause, CompassError, ConnectedToRC, BatteryLowVoltage, GPSUsed, FlightId)" +
						"VALUES (@Tick_no, @OffsetTime, @FlightTime, @NavHealth, @GeneralRelHeight, @FlyCState, @ControllerCTRLMode, @BatteryStatus, @BatteryPercentage, @SmartBattGoHome, @SmartBattLand, @NonGPSCause, @CompassError, @ConnectedToRC, @BatteryLowVoltage, @GPSUsed, @FlightId )";
					#endregion

					#region Create parameters for MAIN Command
					//dronelogentryid autogenerated

					DbParameter param1 = factory.CreateParameter();
					param1.ParameterName = "@Tick_no";
					param1.DbType = DbType.Int64;
					command.Parameters.Add(param1);

					DbParameter param2 = factory.CreateParameter();
					param2.ParameterName = "@OffsetTime";
					param2.DbType = DbType.Double;
					command.Parameters.Add(param2);

					DbParameter param3 = factory.CreateParameter();
					param3.ParameterName = "@FlightTime";
					param3.DbType = DbType.Int32;
					command.Parameters.Add(param3);

					DbParameter param4 = factory.CreateParameter();
					param4.ParameterName = "@NavHealth";
					param4.DbType = DbType.Int32;
					command.Parameters.Add(param4);

					DbParameter param5 = factory.CreateParameter();
					param5.ParameterName = "@GeneralRelHeight";
					param5.DbType = DbType.Double;
					command.Parameters.Add(param5);

					DbParameter param6 = factory.CreateParameter();
					param6.ParameterName = "@FlyCState";
					param6.DbType = DbType.String;
					command.Parameters.Add(param6);

					DbParameter param7 = factory.CreateParameter();
					param7.ParameterName = "@ControllerCTRLMode";
					param7.DbType = DbType.String;
					command.Parameters.Add(param7);

					DbParameter param8 = factory.CreateParameter();
					param8.ParameterName = "@BatteryStatus";
					param8.DbType = DbType.String;
					command.Parameters.Add(param8);

					DbParameter param9 = factory.CreateParameter();
					param9.ParameterName = "@BatteryPercentage";
					param9.DbType = DbType.Double;
					command.Parameters.Add(param9);

					DbParameter param10 = factory.CreateParameter();
					param10.ParameterName = "@SmartBattGoHome";
					param10.DbType = DbType.Int32;
					command.Parameters.Add(param10);

					DbParameter param11 = factory.CreateParameter();
					param11.ParameterName = "@SmartBattLand";
					param11.DbType = DbType.Int32;
					command.Parameters.Add(param11);

					DbParameter param12 = factory.CreateParameter();
					param12.ParameterName = "@NonGPSCause";
					param12.DbType = DbType.String;
					command.Parameters.Add(param12);

					DbParameter param13 = factory.CreateParameter();
					param13.ParameterName = "@CompassError";
					param13.DbType = DbType.String;
					command.Parameters.Add(param13);

					DbParameter param14 = factory.CreateParameter();
					param14.ParameterName = "@ConnectedToRC";
					param14.DbType = DbType.String;
					command.Parameters.Add(param14);

					DbParameter param15 = factory.CreateParameter();
					param15.ParameterName = "@BatteryLowVoltage";
					param15.DbType = DbType.String;
					command.Parameters.Add(param15);

					DbParameter param16 = factory.CreateParameter();
					param16.ParameterName = "@GPSUsed";
					param16.DbType = DbType.String;
					command.Parameters.Add(param16);

					DbParameter param17 = factory.CreateParameter();
					param17.ParameterName = "@FlightId";
					param17.DbType = DbType.Int64;
					command.Parameters.Add(param17);

                    #endregion

                    int lineNo = 2; //used for progress
                    // Read data
                    parser.ReadFields(); //one empty line
                    while (!parser.EndOfData)
					{
						try
						{
                            fields = parser.ReadFields();

                            //set parameters for DroneLogEntry insert
                            foreach (var pair in fieldsAndParameters) {
                                if (headerDict.ContainsKey(pair.Key))
                                {
                                    command.Parameters[pair.Value].Value = fields[headerDict[pair.Key]];
                                }
                                else
                                {
                                    command.Parameters[pair.Value].Value = DBNull.Value;
                                }
                            }
                            command.Parameters["@FlightId"].Value = droneFlight.FlightId;
                            command.ExecuteNonQuery(); //execute

							#region progressbar
							lineNo++;
							if ((lineNo % 10) == 0)
							{
								Helper.Helper.SetProgress((lineNo / (double)totalLines) * 100);
							}
							#endregion
						}
						catch (Exception ex)
						{
							//TODO rollback? (kan enkel met transaction)
							System.Diagnostics.Debug.WriteLine(ex);
                            return false; 
						}
					}
					connection.Close(); //TODO not sure if needed
				}
			}
			//Set hasDroneLog to true
			droneFlight.hasDroneLog = true;
			db.SaveChanges();

			Helper.Helper.SetProgress(100);
			return true;
		}
	}
}
