﻿using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
	public class DATParser___ADONET : IParser
	{
		private ConnectionStringSettings connStringSet;
		private DbProviderFactory factory;

		#region  command texts 
		private string InsertionCommand_DroneLogEntry = "insert into DroneLogEntry " +
						"(Tick_no, OffsetTime, FlightTime, NavHealth, GeneralRelHeight, FlyCState, ControllerCTRLMode, BatteryStatus, BatteryPercentage, SmartBattGoHome, SmartBattLand, NonGPSCause, CompassError, ConnectedToRC, BatteryLowVoltage, GPSUsed, FlightId)" +
						"VALUES (@Tick_no, @OffsetTime, @FlightTime, @NavHealth, @GeneralRelHeight, @FlyCState, @ControllerCTRLMode, @BatteryStatus, @BatteryPercentage, @SmartBattGoHome, @SmartBattLand, @NonGPSCause, @CompassError, @ConnectedToRC, @BatteryLowVoltage, @GPSUsed, @FlightId )";
		private string InsertionCommand_DroneIMU_ATTI = "insert into DroneIMU_ATTI" +
						"(IMU_ATTI_Id, GPS_H, Roll, Pitch, Yaw, DistanceTravelled, MagDirectionOfTravel, TrueDirectionOfTravel, Temperature, VelComposite)" +
						"VALUES (@IMU_ATTI_Id, @GPS_H, @Roll, @Pitch, @Yaw, @DistanceTravelled, @MagDirectionOfTravel, @TrueDirectionOfTravel, @Temperature, @VelComposite)";
		private string InsertionCommand_DroneRC = "insert into DroneRC" +
						"(RCId, FailSafe, DataLost, AppLost, ModeSwitch)" +
						"VALUES (@RCId, @FailSafe, @DataLost, @AppLost, @ModeSwitch)";
		private string InsertionCommand_DroneGPS = "insert into DroneGPS" +
						"(GPSId, Long, Lat, Date, Time, DateTimeStamp, HeightMSL, HDOP, PDOP, SAcc, NumGPS, NumGLNAS, NumSV, VelN, VelE, VelD)" +
						"VALUES (@GPSId, @Long, @Lat, @Date, @Time, @DateTimeStamp, @HeightMSL, @HDOP, @PDOP, @SAcc, @NumGPS, @NumGLNAS, @NumSV, @VelN, @VelE, @VelD)";
		private string InsertionCommand_DroneRTKData = "insert into DroneRTKData" +
						"(RTKDataId, Date, Time, LonP, LatP, HmslP, LonS, LatS, HmslS, VelN, VelE, VelD, HDOP)" +
						"VALUES (@RTKDataId, @Date, @Time, @LonP, @LatP, @HmslP, @LonS, @LatS, @HmslS, @VelN, @VelE, @VelD, @HDOP)";
		private string InsertionCommand_DroneOA = "insert into DroneOA" +
						"(OAId, AvoidObst, AirportLimit, GroundForceLanding, VertAirportLimit)" +
						"VALUES (@OAId, @AvoidObst, @AirportLimit, @GroundForceLanding, @VertAirportLimit)";
		private string InsertionCommand_DroneMotor = "insert into DroneMotor" +
						"(MotorId, CurrentRFront, CurrentLFRont, CurrentLBack, CurrentRBack)" +
						"VALUES (@MotorId, @CurrentRFront, @CurrentLFRont, @CurrentLBack, @CurrentRBack)";
		private string InsertionCommand_DroneAttributeValues = "insert into DroneAttributeValues" +
						"(AttributeValueId, ACType, FirmwareDate, DateTime, BatterySN, GeoDeclination, GeoInclination, GeoIntensity)" +
						"VALUES (@AttributeValueId, @ACType, @FirmwareDate, @DateTime, @BatterySN, @GeoDeclination, @GeoInclination, @GeoIntensity)";
		#endregion

		private Dictionary<string, string> fieldsAndParameters_DroneLogEntry;
		private Dictionary<string, string> fieldsAndParameters_DroneIMU_ATTI;
		private Dictionary<string, string> fieldsAndParameters_DroneRC;
		private Dictionary<string, string> fieldsAndParameters_DroneGPS;
		private Dictionary<string, string> fieldsAndParameters_DroneRTKData;
		private Dictionary<string, string> fieldsAndParameters_DroneOA;
		private Dictionary<string, string> fieldsAndParameters_DroneMotor;
		private Dictionary<string, string> fieldsAndParameters_DroneAttributeValues;

		private HashSet<string> doubles;
		private HashSet<string> ints;

		private string DroneLogEntry_ID_QUERY = "SELECT DroneLogEntryId FROM DroneLogEntry WHERE DroneLogEntryId = (SELECT max(DroneLogEntryId) FROM DroneLogEntry)";

		//used for DroneAttributeValues table
		private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
		private string dateFormat = "MMM dd yyyy"; //Dec 24 2018 
		private double geoDeclination;
		private double geoInclination;
		private double geoIntensity;
		private DateTime dateTime;
		public int BatterySN;
		public DateTime FirmwareDate;
		public string ACType;

		public DATParser___ADONET()
		{
			//fill all dictionaries 
			fieldsAndParameters_DroneLogEntry = fill_DroneLogEntry_Dictionary();
			fieldsAndParameters_DroneIMU_ATTI = fill_DroneIMU_ATTI_Dictionary();
			fieldsAndParameters_DroneRC = fill_DroneRC_Dictionary();
			fieldsAndParameters_DroneGPS = fill_DroneGPS_Dictionary();
			fieldsAndParameters_DroneRTKData = fill_DroneRTKData_Dictionary();
			fieldsAndParameters_DroneOA = fill_DroneOA_Dictionary();
			fieldsAndParameters_DroneMotor = fill_DroneMotor_Dictionary();
			fieldsAndParameters_DroneAttributeValues = fill_DroneAttributeValues_Dictionary();

			//fill hashsets
			doubles = fillDoubles();
			ints = fillInts();

			connStringSet = ConfigurationManager.ConnectionStrings["DroneDB_ADONET"];
			factory = DbProviderFactories.GetFactory(connStringSet.ProviderName);
		}

		private Dictionary<string, string> fill_DroneAttributeValues_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();

			possiblefieldsAndParameters.Add("Firmware Date", "@FirmwareDate");
			possiblefieldsAndParameters.Add("ACType", "@ACType");
			possiblefieldsAndParameters.Add("dateTime", "@DateTime");
			possiblefieldsAndParameters.Add("BatterySN", "@BatterySN");
			possiblefieldsAndParameters.Add("geoDeclination", "@GeoDeclination");
			possiblefieldsAndParameters.Add("geoInclination", "@GeoInclination");
			possiblefieldsAndParameters.Add("geoIntensity", "@GeoIntensity");

			return possiblefieldsAndParameters;
		}

		private HashSet<string> fillInts()
		{
			HashSet<string> ints = new HashSet<string>();

			ints.Add("flightTime");
			ints.Add("navHealth");

			ints.Add("SMART_BATT:goHome%");
			ints.Add("SMART_BATT:land%");

			ints.Add("RTKdata:Date");
			ints.Add("RTKdata:Time");

			ints.Add("GPS(0):Date");
			ints.Add("GPS(0):numGLNAS");
			ints.Add("GPS(0):numGPS");
			ints.Add("GPS(0):numSV");
			ints.Add("GPS(0):Time");

			return ints;
		}

		private HashSet<string> fillDoubles()
		{
			HashSet<string> doubles = new HashSet<string>();

			doubles.Add("BattInfo:Remaining%");
			doubles.Add("General:relativeHeight");
			doubles.Add("offsetTime");

			doubles.Add("RTKdata:hdop");
			doubles.Add("RTKdata:Hmsl_P");
			doubles.Add("RTKdata:Hmsl_S");
			doubles.Add("RTKdata:Lat_P");
			doubles.Add("RTKdata:Lat_S");
			doubles.Add("RTKdata:Lon_P");
			doubles.Add("RTKdata:Lon_S");
			doubles.Add("RTKdata:Vel_D");
			doubles.Add("RTKdata:Vel_E");
			doubles.Add("RTKdata:Vel_N");

			doubles.Add("IMU_ATTI(0):distanceTravelled");
			doubles.Add("IMU_ATTI(0):GPS-H");
			doubles.Add("IMU_ATTI(0):directionOfTravel[mag]");
			doubles.Add("IMU_ATTI(0):pitch");
			doubles.Add("IMU_ATTI(0):roll");
			doubles.Add("IMU_ATTI(0):temperature");
			doubles.Add("IMU_ATTI(0):directionOfTravel[true]");
			doubles.Add("IMU_ATTI(0):yaw");
			doubles.Add("IMU_ATTI(0):velComposite");

			doubles.Add("Motor:Current:LBack");
			doubles.Add("Motor:Current:LFront");
			doubles.Add("Motor:Current:RBack");
			doubles.Add("Motor:Current:RFront");

			doubles.Add("GPS(0):hDOP");
			doubles.Add("GPS(0):heightMSL");
			doubles.Add("GPS(0):Lat");
			doubles.Add("GPS(0):Long");
			doubles.Add("GPS(0):pDOP");
			doubles.Add("GPS(0):sAcc");
			doubles.Add("GPS(0):velD");
			doubles.Add("GPS(0):velE");
			doubles.Add("GPS(0):velN");
			return doubles;
		}

		private Dictionary<string, string> fill_DroneLogEntry_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();

			possiblefieldsAndParameters.Add("Battery:lowVoltage", "@BatteryLowVoltage");
			possiblefieldsAndParameters.Add("Battery:status", "@BatteryStatus");
			possiblefieldsAndParameters.Add("BattInfo:Remaining%", "@BatteryPercentage");
			possiblefieldsAndParameters.Add("compassError", "@CompassError");
			possiblefieldsAndParameters.Add("connectedToRC", "@ConnectedToRC");
			possiblefieldsAndParameters.Add("Controller:ctrlMode", "@ControllerCTRLMode");
			possiblefieldsAndParameters.Add("flightTime", "@FlightTime");
			possiblefieldsAndParameters.Add("flyCState", "@FlyCState");
			possiblefieldsAndParameters.Add("General:relativeHeight", "@GeneralRelHeight");
			possiblefieldsAndParameters.Add("gpsUsed", "@GPSUsed");
			possiblefieldsAndParameters.Add("navHealth", "@NavHealth");
			possiblefieldsAndParameters.Add("nonGPSCause", "@NonGPSCause");
			possiblefieldsAndParameters.Add("offsetTime", "@OffsetTime");
			possiblefieldsAndParameters.Add("SMART_BATT:goHome%", "@SmartBattGoHome");
			possiblefieldsAndParameters.Add("SMART_BATT:land%", "@SmartBattLand");
			possiblefieldsAndParameters.Add("Tick#", "@Tick_no");

			return possiblefieldsAndParameters;
		}

		private Dictionary<string, string> fill_DroneIMU_ATTI_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();

			//droneIMU_ATTI
			possiblefieldsAndParameters.Add("IMU_ATTI(0):distanceTravelled", "@DistanceTravelled");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):GPS-H", "@GPS_H");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):directionOfTravel[mag]", "@MagDirectionOfTravel");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):pitch", "@Pitch");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):roll", "@Roll");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):temperature", "@Temperature");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):directionOfTravel[true]", "@TrueDirectionOfTravel");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):yaw", "@Yaw");
			possiblefieldsAndParameters.Add("IMU_ATTI(0):velComposite", "@VelComposite");


			return possiblefieldsAndParameters;
		}

		private Dictionary<string, string> fill_DroneRC_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();

			//dronerc
			possiblefieldsAndParameters.Add("RC:appLost", "@AppLost");
			possiblefieldsAndParameters.Add("RC:dataLost", "@DataLost");
			possiblefieldsAndParameters.Add("RC:failSafe", "@FailSafe");
			possiblefieldsAndParameters.Add("RC:ModeSwitch", "@ModeSwitch");


			return possiblefieldsAndParameters;
		}

		private Dictionary<string, string> fill_DroneGPS_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();

			//dronegps 
			possiblefieldsAndParameters.Add("GPS(0):Date", "@Date");
			possiblefieldsAndParameters.Add("GPS:dateTimeStamp", "@DateTimeStamp");
			possiblefieldsAndParameters.Add("GPS(0):hDOP", "@HDOP");
			possiblefieldsAndParameters.Add("GPS(0):heightMSL", "@HeightMSL");
			possiblefieldsAndParameters.Add("GPS(0):Lat", "@Lat");
			possiblefieldsAndParameters.Add("GPS(0):Long", "@Long");
			possiblefieldsAndParameters.Add("GPS(0):numGLNAS", "@NumGLNAS");
			possiblefieldsAndParameters.Add("GPS(0):numGPS", "@NumGPS");
			possiblefieldsAndParameters.Add("GPS(0):numSV", "@NumSV");
			possiblefieldsAndParameters.Add("GPS(0):pDOP", "@PDOP");
			possiblefieldsAndParameters.Add("GPS(0):sAcc", "@SAcc");
			possiblefieldsAndParameters.Add("GPS(0):Time", "@Time");
			possiblefieldsAndParameters.Add("GPS(0):velD", "@VelD");
			possiblefieldsAndParameters.Add("GPS(0):velE", "@VelE");
			possiblefieldsAndParameters.Add("GPS(0):velN", "@VelN");

			return possiblefieldsAndParameters;
		}

		private Dictionary<string, string> fill_DroneRTKData_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();

			//dronertk
			possiblefieldsAndParameters.Add("RTKdata:Date", "@Date");
			possiblefieldsAndParameters.Add("RTKdata:hdop", "@HDOP");
			possiblefieldsAndParameters.Add("RTKdata:Hmsl_P", "@HmslP");
			possiblefieldsAndParameters.Add("RTKdata:Hmsl_S", "@HmslS");
			possiblefieldsAndParameters.Add("RTKdata:Lat_P", "@LatP");
			possiblefieldsAndParameters.Add("RTKdata:Lat_S", "@LatS");
			possiblefieldsAndParameters.Add("RTKdata:Lon_P", "@LonP");
			possiblefieldsAndParameters.Add("RTKdata:Lon_S", "@LonS");
			possiblefieldsAndParameters.Add("RTKdata:Time", "@Time");
			possiblefieldsAndParameters.Add("RTKdata:Vel_D", "@VelD");
			possiblefieldsAndParameters.Add("RTKdata:Vel_E", "@VelE");
			possiblefieldsAndParameters.Add("RTKdata:Vel_N", "@VelN");

			return possiblefieldsAndParameters;
		}

		private Dictionary<string, string> fill_DroneOA_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();



			possiblefieldsAndParameters.Add("OAId", "@OAId");
			possiblefieldsAndParameters.Add("AvoidObst", "@AvoidObst");
			possiblefieldsAndParameters.Add("AirportLimit", "@AirportLimit");
			possiblefieldsAndParameters.Add("GroundForceLanding", "@GroundForceLanding");
			possiblefieldsAndParameters.Add("VertAirportLimit", "@VertAirportLimit");

			return possiblefieldsAndParameters;
		}

		private Dictionary<string, string> fill_DroneMotor_Dictionary()
		{
			Dictionary<string, string> possiblefieldsAndParameters = new Dictionary<string, string>();

			//dronemotor
			possiblefieldsAndParameters.Add("Motor:Current:LBack", "@CurrentLBack");
			possiblefieldsAndParameters.Add("Motor:Current:LFront", "@CurrentLFRont");
			possiblefieldsAndParameters.Add("Motor:Current:RBack", "@CurrentRBack");
			possiblefieldsAndParameters.Add("Motor:Current:RFront", "@CurrentRFront");


			return possiblefieldsAndParameters;
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
					for (int i = 0; i < fields.Length - 2; i++) //2 useless fields at end are not needed
					{
						if (!headerDict.ContainsKey(fields[i])) //TODO wegdoen?
						{
							headerDict.Add(fields[i], i);
						}
					}
					#endregion
					//delete 2 useless keys from headerDict (ConvertDatV3, 3.7.1)
					//headerDict.Remove("ConvertDatV3");
					//headerDict.Remove("3.7.1");

					#region Create ALL commands 
					DbCommand command_DroneLogEntry = connection.CreateCommand();
					command_DroneLogEntry.Connection = connection;
					command_DroneLogEntry.CommandText = InsertionCommand_DroneLogEntry;
					createParametersForDroneLogEntry(command_DroneLogEntry);

					DbCommand command_DroneIMU_ATTI = connection.CreateCommand();
					command_DroneIMU_ATTI.Connection = connection;
					command_DroneIMU_ATTI.CommandText = InsertionCommand_DroneIMU_ATTI;
					createParametersForDroneIMUATTI(command_DroneIMU_ATTI);

					DbCommand command_DroneRC = connection.CreateCommand();
					command_DroneRC.Connection = connection;
					command_DroneRC.CommandText = InsertionCommand_DroneRC;
					createParametersForDroneRC(command_DroneRC);

					DbCommand command_DroneGPS = connection.CreateCommand();
					command_DroneGPS.Connection = connection;
					command_DroneGPS.CommandText = InsertionCommand_DroneGPS;
					createParametersForDroneGPS(command_DroneGPS);

					DbCommand command_DroneRTKData = connection.CreateCommand();
					command_DroneRTKData.Connection = connection;
					command_DroneRTKData.CommandText = InsertionCommand_DroneRTKData;
					createParametersForDroneRTKData(command_DroneRTKData);

					DbCommand command_DroneOA = connection.CreateCommand();
					command_DroneOA.Connection = connection;
					command_DroneOA.CommandText = InsertionCommand_DroneOA;
					createParametersForDroneOA(command_DroneOA);

					DbCommand command_DroneMotor = connection.CreateCommand();
					command_DroneMotor.Connection = connection;
					command_DroneMotor.CommandText = InsertionCommand_DroneMotor;
					createParametersForDroneMotor(command_DroneMotor);

					DbCommand command_DroneAttributeValues = connection.CreateCommand();
					command_DroneAttributeValues.Connection = connection;
					command_DroneAttributeValues.CommandText = InsertionCommand_DroneAttributeValues;
					createParametersForDroneAttributeValues(command_DroneAttributeValues);

					//command to get id from last filled dronelogentry row (to be used as foreign key in child tables)
					DbCommand IDCommand = connection.CreateCommand();
					IDCommand.Connection = connection;
					IDCommand.CommandText = DroneLogEntry_ID_QUERY;


					#endregion

					int lineNo = 2; //used for progress
									// Read data
					parser.ReadFields(); //one empty line
					while (!parser.EndOfData)
					{
						try
						{
							fields = parser.ReadFields();

							#region set all parameters to DBNull 
							for (int i = 0; i < command_DroneLogEntry.Parameters.Count; i++)
							{
								command_DroneLogEntry.Parameters[i].Value = DBNull.Value;
							}
							for (int i = 0; i < command_DroneGPS.Parameters.Count; i++)
							{
								command_DroneGPS.Parameters[i].Value = DBNull.Value;
							}
							for (int i = 0; i < command_DroneIMU_ATTI.Parameters.Count; i++)
							{
								command_DroneIMU_ATTI.Parameters[i].Value = DBNull.Value;
							}
							for (int i = 0; i < command_DroneMotor.Parameters.Count; i++)
							{
								command_DroneMotor.Parameters[i].Value = DBNull.Value;
							}
							for (int i = 0; i < command_DroneOA.Parameters.Count; i++)
							{
								command_DroneOA.Parameters[i].Value = DBNull.Value;
							}
							for (int i = 0; i < command_DroneRC.Parameters.Count; i++)
							{
								command_DroneRC.Parameters[i].Value = DBNull.Value;
							}
							for (int i = 0; i < command_DroneRTKData.Parameters.Count; i++)
							{
								command_DroneRTKData.Parameters[i].Value = DBNull.Value;
							}
							for (int i = 0; i < command_DroneAttributeValues.Parameters.Count; i++)
							{
								command_DroneAttributeValues.Parameters[i].Value = DBNull.Value;
							}
							#endregion

							//Set parameters for all commands
							//loops through the found headers 
							foreach (var headerPair in headerDict)
							{
								if (fields[headerDict[headerPair.Key]].Length == 0) continue;

								bool isDouble = false;
								bool isInt = false;
								if (doubles.Contains(headerPair.Key))
								{
									isDouble = true;
								}
								else if (ints.Contains(headerPair.Key))
								{
									isInt = true;
								}

								if (fieldsAndParameters_DroneLogEntry.ContainsKey(headerPair.Key))
								{
									if (isDouble)
									{
										command_DroneLogEntry.Parameters[fieldsAndParameters_DroneLogEntry[headerPair.Key]].Value = double.Parse(fields[headerDict[headerPair.Key]], customCulture);
									}
									else if (isInt)
									{
										command_DroneLogEntry.Parameters[fieldsAndParameters_DroneLogEntry[headerPair.Key]].Value = Int32.Parse(fields[headerDict[headerPair.Key]]);
									}
									else
									{
										command_DroneLogEntry.Parameters[fieldsAndParameters_DroneLogEntry[headerPair.Key]].Value = fields[headerDict[headerPair.Key]];
									}

								}
								else if (fieldsAndParameters_DroneGPS.ContainsKey(headerPair.Key))
								{
									if (isDouble)
									{
										command_DroneGPS.Parameters[fieldsAndParameters_DroneGPS[headerPair.Key]].Value = Double.Parse(fields[headerDict[headerPair.Key]], customCulture);
									}
									else if (isInt)
									{
										command_DroneGPS.Parameters[fieldsAndParameters_DroneGPS[headerPair.Key]].Value = Int32.Parse(fields[headerDict[headerPair.Key]]);
									}
									else
									{
										command_DroneGPS.Parameters[fieldsAndParameters_DroneGPS[headerPair.Key]].Value = fields[headerDict[headerPair.Key]];
									}

								}
								else if (fieldsAndParameters_DroneIMU_ATTI.ContainsKey(headerPair.Key))
								{
									if (isDouble)
									{
										command_DroneIMU_ATTI.Parameters[fieldsAndParameters_DroneIMU_ATTI[headerPair.Key]].Value = Double.Parse(fields[headerDict[headerPair.Key]], customCulture);
									}
									else if (isInt)
									{
										command_DroneIMU_ATTI.Parameters[fieldsAndParameters_DroneIMU_ATTI[headerPair.Key]].Value = Int32.Parse(fields[headerDict[headerPair.Key]]);
									}
									else
									{
										command_DroneIMU_ATTI.Parameters[fieldsAndParameters_DroneIMU_ATTI[headerPair.Key]].Value = fields[headerDict[headerPair.Key]];
									}

								}
								else if (fieldsAndParameters_DroneMotor.ContainsKey(headerPair.Key))
								{
									if (isDouble)
									{
										command_DroneMotor.Parameters[fieldsAndParameters_DroneMotor[headerPair.Key]].Value = Double.Parse(fields[headerDict[headerPair.Key]], customCulture);
									}
									else if (isInt)
									{
										command_DroneMotor.Parameters[fieldsAndParameters_DroneMotor[headerPair.Key]].Value = Int32.Parse(fields[headerDict[headerPair.Key]]);
									}
									else
									{
										command_DroneMotor.Parameters[fieldsAndParameters_DroneMotor[headerPair.Key]].Value = fields[headerDict[headerPair.Key]];
									}

								}
								else if (fieldsAndParameters_DroneOA.ContainsKey(headerPair.Key))
								{
									if (isDouble)
									{
										command_DroneOA.Parameters[fieldsAndParameters_DroneOA[headerPair.Key]].Value = Double.Parse(fields[headerDict[headerPair.Key]], customCulture);
									}
									else if (isInt)
									{
										command_DroneOA.Parameters[fieldsAndParameters_DroneOA[headerPair.Key]].Value = Int32.Parse(fields[headerDict[headerPair.Key]]);
									}
									else
									{
										command_DroneOA.Parameters[fieldsAndParameters_DroneOA[headerPair.Key]].Value = fields[headerDict[headerPair.Key]];
									}

								}
								else if (fieldsAndParameters_DroneRC.ContainsKey(headerPair.Key))
								{
									if (isDouble)
									{
										command_DroneRC.Parameters[fieldsAndParameters_DroneRC[headerPair.Key]].Value = Double.Parse(fields[headerDict[headerPair.Key]], customCulture);
									}
									else if (isInt)
									{
										command_DroneRC.Parameters[fieldsAndParameters_DroneRC[headerPair.Key]].Value = Int32.Parse(fields[headerDict[headerPair.Key]]);
									}
									else
									{
										command_DroneRC.Parameters[fieldsAndParameters_DroneRC[headerPair.Key]].Value = fields[headerDict[headerPair.Key]];
									}

								}
								else if (fieldsAndParameters_DroneRTKData.ContainsKey(headerPair.Key))
								{
									if (isDouble)
									{
										command_DroneRTKData.Parameters[fieldsAndParameters_DroneRTKData[headerPair.Key]].Value = Double.Parse(fields[headerDict[headerPair.Key]], customCulture);
									}
									else if (isInt)
									{
										command_DroneRTKData.Parameters[fieldsAndParameters_DroneRTKData[headerPair.Key]].Value = Int32.Parse(fields[headerDict[headerPair.Key]]);
									}
									else
									{
										command_DroneRTKData.Parameters[fieldsAndParameters_DroneRTKData[headerPair.Key]].Value = fields[headerDict[headerPair.Key]];
									}

								}

								//droneattribute values
								else if (headerPair.Key == "Attribute|Value")
								{
									string[] a = fields[headerDict[headerPair.Key]].Split('|');
									if (a[0] == "geoDeclination")  //doubles 
									{
										Debug.WriteLine(a[1].Split(' ')[0]);
										geoDeclination = Double.Parse(a[1].Split(' ')[0],customCulture);
									}
									else if (a[0] == "geoInclination")
									{
										Debug.WriteLine(a[1].Split(' ')[0]);
										geoInclination = Double.Parse(a[1].Split(' ')[0], customCulture);
									}
									else if (a[0] == "geoIntensity")
									{
										Debug.WriteLine(a[1].Split(' ')[0]);
										geoIntensity = Double.Parse(a[1].Split(' ')[0], customCulture);
									}

									else if (a[0] == "BatterySN") //int 
									{
										Debug.WriteLine(a[1].Split(' ')[0]);
										BatterySN = Int32.Parse(a[1].Split(' ')[0]);
									}

									else if (a[0] == "Firmware Date") //date
									{
										Debug.WriteLine("_____________");
										Debug.WriteLine(a[1].Trim());
										Debug.WriteLine(dateFormat);
										Debug.WriteLine("_____________");
										FirmwareDate = DateTime.ParseExact(a[1].Trim(), dateFormat, CultureInfo.InvariantCulture);
									}
									else if (a[0] == "dateTime") //datetime
									{

										//TODO PROPER CONVERSION FOR 2015-10-18 0:0:0    



										string properFormatString = formatDateTimeProperly(a[1]);

										Debug.WriteLine("_____________");
										Debug.WriteLine(a[1]);
										Debug.WriteLine(properFormatString);
										Debug.WriteLine(dateTimeFormat);
										Debug.WriteLine("_____________");


										//TODO WEGDOEN
										//string copy = a[1];
										//string[] copysplit = copy.Split('-');

										//string copyfixed = copysplit[0];
										//copyfixed += '-';
										////month is too short
										//if (copysplit[1].Length == 1)
										//{
										//	copyfixed += '0';

										//}
										//copyfixed += copysplit[1];

										//copyfixed += '-';
										////day is too short 
										//if (copysplit[2].Split(' ')[0].Length == 1)
										//{
										//	copyfixed += '0';
										//}

										//copyfixed += copysplit[2];

										//Debug.WriteLine("_____________");
										//Debug.WriteLine(copyfixed.Substring(0, (copyfixed.Length - 3)).Trim());
										//Debug.WriteLine(dateTimeFormat);
										//Debug.WriteLine("_____________");

										dateTime = DateTime.ParseExact(properFormatString, dateTimeFormat, CultureInfo.InvariantCulture);
									}
									else if (a[0] == "ACType")
									{
										ACType = a[1].Trim();
									}


									//TODO wegdoen
									//Debug.WriteLine("1");
									//if (a[0] == "geoDeclination" ||
									//	a[0] == "geoInclination" ||
									//	a[0] == "geoIntensity") //is double
									//{
									//	Debug.WriteLine("2");
									//	command_DroneAttributeValues.Parameters[fieldsAndParameters_DroneAttributeValues[a[0]]].Value = Double.Parse(a[1]);
									//}
									//else if (a[0] == "BatterySN") //is int 
									//{
									//	Debug.WriteLine("3");
									//	command_DroneAttributeValues.Parameters[fieldsAndParameters_DroneAttributeValues[a[0]]].Value = Int32.Parse(a[1]);
									//}
									//else if (a[0] == "Firmware Date") //date
									//{
									//	Debug.WriteLine("4");
									//	Debug.WriteLine(dateFormat);
									//	Debug.WriteLine(a[1]);
									//	command_DroneAttributeValues.Parameters[fieldsAndParameters_DroneAttributeValues[a[0]]].Value = DateTime.ParseExact(a[1].Trim(), dateFormat, CultureInfo.InvariantCulture);
									//}
									//else if (a[0] == "dateTime") //datetime
									//{
									//	Debug.WriteLine("5");
									//	command_DroneAttributeValues.Parameters[fieldsAndParameters_DroneAttributeValues[a[0]]].Value = DateTime.ParseExact(a[1].Trim(), dateTimeFormat, CultureInfo.InvariantCulture); ;
									//}
									//else
									//{
									//	Debug.WriteLine("6");
									//	command_DroneAttributeValues.Parameters[fieldsAndParameters_DroneAttributeValues[a[0]]].Value = a[1].Trim();
									//}
								}

							}

							command_DroneLogEntry.Parameters["@FlightId"].Value = droneFlight.FlightId;



							command_DroneLogEntry.ExecuteNonQuery();

							DbDataReader reader = IDCommand.ExecuteReader(); //todo wegdoen uit de for loop?
							int DroneLogEntryId;
							if (reader.Read())
							{
								DroneLogEntryId = reader.GetInt32(0);
								reader.Close();
							}
							else return false; //TODO CUSTOM EXCEPTION


							//set this id on all other commands
							command_DroneGPS.Parameters["@GPSId"].Value = DroneLogEntryId;
							command_DroneIMU_ATTI.Parameters["@IMU_ATTI_Id"].Value = DroneLogEntryId;
							command_DroneMotor.Parameters["@MotorId"].Value = DroneLogEntryId;
							command_DroneOA.Parameters["@OAId"].Value = DroneLogEntryId;
							command_DroneRC.Parameters["@RCId"].Value = DroneLogEntryId;
							command_DroneRTKData.Parameters["@RTKDataId"].Value = DroneLogEntryId;

							command_DroneGPS.ExecuteNonQuery();
							command_DroneIMU_ATTI.ExecuteNonQuery();
							command_DroneMotor.ExecuteNonQuery();
							command_DroneOA.ExecuteNonQuery();
							command_DroneRC.ExecuteNonQuery();
							command_DroneRTKData.ExecuteNonQuery();

							Debug.WriteLine("line number: " + lineNo);

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
							return false; //Todo wegdoen
						}
					}

					//droneattributevalues command
					command_DroneAttributeValues.Parameters["@AttributeValueId"].Value = flightId;
					command_DroneAttributeValues.Parameters["@FirmwareDate"].Value = FirmwareDate;
					command_DroneAttributeValues.Parameters["@ACType"].Value = ACType;
					command_DroneAttributeValues.Parameters["@DateTime"].Value = dateTime;
					command_DroneAttributeValues.Parameters["@BatterySN"].Value = BatterySN;
					command_DroneAttributeValues.Parameters["@GeoDeclination"].Value = geoDeclination;
					command_DroneAttributeValues.Parameters["@GeoInclination"].Value = geoInclination;
					command_DroneAttributeValues.Parameters["@GeoIntensity"].Value = geoIntensity;
					command_DroneAttributeValues.ExecuteNonQuery();

					connection.Close(); //TODO not sure if needed
				}
			}
			//Set hasDroneLog to true
			droneFlight.hasDroneLog = true;
			db.SaveChanges();

			Helper.Helper.SetProgress(100);
			return true;
		}


		private string formatDateTimeProperly(string a)
		{
			string result = "";

			string date = a.Split(' ')[0];
			string timestamp = a.Split(' ')[1];

			string[] year_month_day = date.Split('-');
			string[] hour_minute_seconds = timestamp.Split(':');

			result += year_month_day[0];
			result += '-';
			//month is too short 
			if (year_month_day[1].Length == 1)
			{
				result += '0';
			}
			result += year_month_day[1];

			result += '-';
			//day is too short
			if (year_month_day[2].Length == 1)
			{
				result += '0';
			}
			result += year_month_day[2];

			result += ' ';

			//hour is too short
			if (hour_minute_seconds[0].Length == 1)
			{
				result += '0';
			}
			result += hour_minute_seconds[0];

			result += ':';
			//minutes is too short
			if (hour_minute_seconds[1].Length == 1)
			{
				result += '0';
			}
			result += hour_minute_seconds[1];

			result += ':';
			//seconds is too short 
			if (hour_minute_seconds[2].Length == 1)
			{
				result += '0';
			}
			result += hour_minute_seconds[2];

			return result; 
		}

		private void createParametersForDroneAttributeValues(DbCommand command)
		{
			DbParameter param = factory.CreateParameter();
			param.ParameterName = "@AttributeValueId";
			param.DbType = DbType.Int32;
			command.Parameters.Add(param);

			DbParameter param1 = factory.CreateParameter();
			param1.ParameterName = "@ACType";
			param1.DbType = DbType.String;
			command.Parameters.Add(param1);

			DbParameter param2 = factory.CreateParameter();
			param2.ParameterName = "@FirmwareDate";
			param2.DbType = DbType.Date;
			command.Parameters.Add(param2);

			DbParameter param3 = factory.CreateParameter();
			param3.ParameterName = "@DateTime";
			param3.DbType = DbType.DateTime;
			command.Parameters.Add(param3);

			DbParameter param4 = factory.CreateParameter();
			param4.ParameterName = "@BatterySN";
			param4.DbType = DbType.Int32;
			command.Parameters.Add(param4);

			DbParameter param5 = factory.CreateParameter();
			param5.ParameterName = "@GeoDeclination";
			param5.DbType = DbType.Double;
			command.Parameters.Add(param5);

			DbParameter param6 = factory.CreateParameter();
			param6.ParameterName = "@GeoInclination";
			param6.DbType = DbType.Double;
			command.Parameters.Add(param6);

			DbParameter param7 = factory.CreateParameter();
			param7.ParameterName = "@GeoIntensity";
			param7.DbType = DbType.Double;
			command.Parameters.Add(param7);
		}

		private void createParametersForDroneMotor(DbCommand command)
		{
			DbParameter param = factory.CreateParameter();
			param.ParameterName = "@MotorId";
			param.DbType = DbType.Int32;
			command.Parameters.Add(param);

			DbParameter param1 = factory.CreateParameter();
			param1.ParameterName = "@CurrentRFront";
			param1.DbType = DbType.Double;
			command.Parameters.Add(param1);

			DbParameter param2 = factory.CreateParameter();
			param2.ParameterName = "@CurrentLFront";
			param2.DbType = DbType.Double;
			command.Parameters.Add(param2);

			DbParameter param3 = factory.CreateParameter();
			param3.ParameterName = "@CurrentLBack";
			param3.DbType = DbType.Double;
			command.Parameters.Add(param3);

			DbParameter param4 = factory.CreateParameter();
			param4.ParameterName = "@CurrentRBack";
			param4.DbType = DbType.Double;
			command.Parameters.Add(param4);
		}

		private void createParametersForDroneOA(DbCommand command)
		{
			DbParameter param = factory.CreateParameter();
			param.ParameterName = "@OAId";
			param.DbType = DbType.Int32;
			command.Parameters.Add(param);

			DbParameter param1 = factory.CreateParameter();
			param1.ParameterName = "@AvoidObst";
			param1.DbType = DbType.String;
			command.Parameters.Add(param1);

			DbParameter param2 = factory.CreateParameter();
			param2.ParameterName = "@AirportLimit";
			param2.DbType = DbType.String;
			command.Parameters.Add(param2);

			DbParameter param3 = factory.CreateParameter();
			param3.ParameterName = "@GroundForceLanding";
			param3.DbType = DbType.String;
			command.Parameters.Add(param3);

			DbParameter param4 = factory.CreateParameter();
			param4.ParameterName = "@VertAirportLimit";
			param4.DbType = DbType.String;
			command.Parameters.Add(param4);
		}

		private void createParametersForDroneRTKData(DbCommand command)
		{
			DbParameter param = factory.CreateParameter();
			param.ParameterName = "@RTKDataId";
			param.DbType = DbType.Int32;
			command.Parameters.Add(param);

			DbParameter param1 = factory.CreateParameter();
			param1.ParameterName = "@Date";
			param1.DbType = DbType.Int32;
			command.Parameters.Add(param1);

			DbParameter param2 = factory.CreateParameter();
			param2.ParameterName = "@Time";
			param2.DbType = DbType.Int32;
			command.Parameters.Add(param2);

			DbParameter param3 = factory.CreateParameter();
			param3.ParameterName = "@LonP";
			param3.DbType = DbType.Double;
			command.Parameters.Add(param3);

			DbParameter param4 = factory.CreateParameter();
			param4.ParameterName = "@LatP";
			param4.DbType = DbType.Double;
			command.Parameters.Add(param4);

			DbParameter param5 = factory.CreateParameter();
			param5.ParameterName = "@HmslP";
			param5.DbType = DbType.Double;
			command.Parameters.Add(param5);

			DbParameter param6 = factory.CreateParameter();
			param6.ParameterName = "@LonS";
			param6.DbType = DbType.Double;
			command.Parameters.Add(param6);

			DbParameter param7 = factory.CreateParameter();
			param7.ParameterName = "@LatS";
			param7.DbType = DbType.Double;
			command.Parameters.Add(param7);

			DbParameter param8 = factory.CreateParameter();
			param8.ParameterName = "@HmslS";
			param8.DbType = DbType.Double;
			command.Parameters.Add(param8);

			DbParameter param9 = factory.CreateParameter();
			param9.ParameterName = "@VelN";
			param9.DbType = DbType.Double;
			command.Parameters.Add(param9);

			DbParameter param10 = factory.CreateParameter();
			param10.ParameterName = "@VelE";
			param10.DbType = DbType.Double;
			command.Parameters.Add(param10);

			DbParameter param11 = factory.CreateParameter();
			param11.ParameterName = "@VelD";
			param11.DbType = DbType.Double;
			command.Parameters.Add(param11);

			DbParameter param12 = factory.CreateParameter();
			param12.ParameterName = "@HDOP";
			param12.DbType = DbType.Double;
			command.Parameters.Add(param12);
		}

		private void createParametersForDroneGPS(DbCommand command)
		{
			DbParameter param = factory.CreateParameter();
			param.ParameterName = "@GPSId";
			param.DbType = DbType.Int32;
			command.Parameters.Add(param);

			DbParameter param1 = factory.CreateParameter();
			param1.ParameterName = "@Long";
			param1.DbType = DbType.Double;
			command.Parameters.Add(param1);

			DbParameter param2 = factory.CreateParameter();
			param2.ParameterName = "@Lat";
			param2.DbType = DbType.Double;
			command.Parameters.Add(param2);

			DbParameter param3 = factory.CreateParameter();
			param3.ParameterName = "@Date";
			param3.DbType = DbType.Int32;
			command.Parameters.Add(param3);

			DbParameter param4 = factory.CreateParameter();
			param4.ParameterName = "@Time";
			param4.DbType = DbType.Int32;
			command.Parameters.Add(param4);

			DbParameter param5 = factory.CreateParameter();
			param5.ParameterName = "@DateTimeStamp";
			param5.DbType = DbType.DateTime;
			command.Parameters.Add(param5);

			DbParameter param6 = factory.CreateParameter();
			param6.ParameterName = "@HeightMSL";
			param6.DbType = DbType.Double;
			command.Parameters.Add(param6);

			DbParameter param7 = factory.CreateParameter();
			param7.ParameterName = "@HDOP";
			param7.DbType = DbType.Double;
			command.Parameters.Add(param7);

			DbParameter param8 = factory.CreateParameter();
			param8.ParameterName = "@PDOP";
			param8.DbType = DbType.Double;
			command.Parameters.Add(param8);

			DbParameter param9 = factory.CreateParameter();
			param9.ParameterName = "@SAcc";
			param9.DbType = DbType.Double;
			command.Parameters.Add(param9);

			DbParameter param10 = factory.CreateParameter();
			param10.ParameterName = "@NumGPS";
			param10.DbType = DbType.Int32;
			command.Parameters.Add(param10);

			DbParameter param11 = factory.CreateParameter();
			param11.ParameterName = "@NumGLNAS";
			param11.DbType = DbType.Int32;
			command.Parameters.Add(param11);

			DbParameter param12 = factory.CreateParameter();
			param12.ParameterName = "@NumSV";
			param12.DbType = DbType.Int32;
			command.Parameters.Add(param12);

			DbParameter param13 = factory.CreateParameter();
			param13.ParameterName = "@VelN";
			param13.DbType = DbType.Double;
			command.Parameters.Add(param13);

			DbParameter param14 = factory.CreateParameter();
			param14.ParameterName = "@VelE";
			param14.DbType = DbType.Double;
			command.Parameters.Add(param14);

			DbParameter param15 = factory.CreateParameter();
			param15.ParameterName = "@VelD";
			param15.DbType = DbType.Double;
			command.Parameters.Add(param15);
		}

		private void createParametersForDroneRC(DbCommand command)
		{
			DbParameter param = factory.CreateParameter();
			param.ParameterName = "@RCId";
			param.DbType = DbType.Int32;
			command.Parameters.Add(param);

			DbParameter param1 = factory.CreateParameter();
			param1.ParameterName = "@FailSafe";
			param1.DbType = DbType.String;
			command.Parameters.Add(param1);

			DbParameter param2 = factory.CreateParameter();
			param2.ParameterName = "@DataLost";
			param2.DbType = DbType.String;
			command.Parameters.Add(param2);

			DbParameter param3 = factory.CreateParameter();
			param3.ParameterName = "@AppLost";
			param3.DbType = DbType.String;
			command.Parameters.Add(param3);

			DbParameter param4 = factory.CreateParameter();
			param4.ParameterName = "@ModeSwitch";
			param4.DbType = DbType.String;
			command.Parameters.Add(param4);
		}

		private void createParametersForDroneIMUATTI(DbCommand command)
		{
			DbParameter param = factory.CreateParameter();
			param.ParameterName = "@IMU_ATTI_Id";
			param.DbType = DbType.Int32;
			command.Parameters.Add(param);

			DbParameter param1 = factory.CreateParameter();
			param1.ParameterName = "@GPS_H";
			param1.DbType = DbType.Double;
			command.Parameters.Add(param1);

			DbParameter param2 = factory.CreateParameter();
			param2.ParameterName = "@Roll";
			param2.DbType = DbType.Double;
			command.Parameters.Add(param2);

			DbParameter param3 = factory.CreateParameter();
			param3.ParameterName = "@Pitch";
			param3.DbType = DbType.Double;
			command.Parameters.Add(param3);

			DbParameter param4 = factory.CreateParameter();
			param4.ParameterName = "@Yaw";
			param4.DbType = DbType.Double;
			command.Parameters.Add(param4);

			DbParameter param5 = factory.CreateParameter();
			param5.ParameterName = "@DistanceTravelled";
			param5.DbType = DbType.Double;
			command.Parameters.Add(param5);

			DbParameter param6 = factory.CreateParameter();
			param6.ParameterName = "@MagDirectionOfTravel";
			param6.DbType = DbType.Double;
			command.Parameters.Add(param6);

			DbParameter param7 = factory.CreateParameter();
			param7.ParameterName = "@TrueDirectionOfTravel";
			param7.DbType = DbType.Double;
			command.Parameters.Add(param7);

			DbParameter param8 = factory.CreateParameter();
			param8.ParameterName = "@Temperature";
			param8.DbType = DbType.Double;
			command.Parameters.Add(param8);

			DbParameter param9 = factory.CreateParameter();
			param9.ParameterName = "@VelComposite";
			param9.DbType = DbType.Double;
			command.Parameters.Add(param9);
		}

		private void createParametersForDroneLogEntry(DbCommand command)
		{
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
		}


		//Reverse geocode location from coordinates
		private string reverseGeocode(double lon, double lat)
		{
			string URL = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?f=json&featureTypes=&token=" + generateToken() + "&location=" + lon + "," + lat;

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

		//Generate token to allow saving reverse geocoded location to database
		private string generateToken()
		{
			string clientId = "lnhfzAV3Fx5oCtIy";
			string clientSecret = "97a8213a907742429b84246f379c5178";
			string URL = "https://www.arcgis.com/sharing/oauth2/token?client_id=" + clientId + "&grant_type=client_credentials&client_secret=" + clientSecret + "&f=pjson";

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

			//Convert jsonString to JObject and get access_token from JObject
			JObject json = JObject.Parse(jsonString);
			string location = (string)json["access_token"];

			return location;
		}

	}
}
