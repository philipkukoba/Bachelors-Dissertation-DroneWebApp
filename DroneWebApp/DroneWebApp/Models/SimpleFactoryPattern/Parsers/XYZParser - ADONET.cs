using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
	public class XYZParserADONET : IParser
	{
		public bool Parse(string path, int flightId, DroneDBEntities db)
		{
			// Get the approriate DroneFlight that goes with this data
			DroneFlight droneFlight = db.DroneFlights.Find(flightId);
			PointCloudXYZ pointCloudXYZ;

			// Do not parse a new file, if this flight already has an XYZ file
			if (droneFlight.hasXYZ) return false; 

			// calculate the total amount of lines by going through the whole file once
			int totalLines = Helper.Helper.CountFileLines(path);
			System.Diagnostics.Debug.WriteLine("File size: " + totalLines + " lines\n"); // test

			//get a connection
			ConnectionStringSettings connStringSet = ConfigurationManager.ConnectionStrings["DroneDB_ADONET"]; 

			//factory voor provider aanmaken 
			DbProviderFactory factory = DbProviderFactories.GetFactory(connStringSet.ProviderName);

			//create connection
			DbConnection connection = factory.CreateConnection();
			connection.ConnectionString = connStringSet.ConnectionString;

			
			connection.Open();
			// Parse
			using (TextFieldParser parser = new TextFieldParser(path)) //TODO: using updaten met connection en transaction ??? 
			{
				int lineNo = 0;
				string[] splitLine;

				// Read data
				while (!parser.EndOfData)
				{
					try
					{
						splitLine = parser.ReadLine().Split(' ');

						////new row
						//oneRow = dataTable.NewRow();

						////fill row
						////oneRow["PointCloudXYZId"] = null;      // ID Field, is this generated? 
						//oneRow["X"] = splitLine[0];
						//oneRow["Y"] = splitLine[1];
						//oneRow["Z"] = splitLine[2];
						//oneRow["Red"] = splitLine[3];
						//oneRow["Green"] = splitLine[4];
						//oneRow["Blue"] = splitLine[5];

						//if (splitLine.Length == 7)     //not sure if needs to be checked
						//	oneRow["Intensity"] = splitLine[6];
						//else oneRow["Intensity"] = null; 
						
						//oneRow["FlightId"] = droneFlight.FlightId;

						////add row
						//dataTable.Rows.Add(oneRow);

						DbCommand command = connection.CreateCommand();
						command.Connection = connection;
						command.CommandText = "insert into PointCloudXYZ " +
							"(X, Y, Z, Red, Green, Blue, Intensity, FlightId)" +
							//"VALUES (@PointCloudXYZId, @X, @Y, @Z, @Red, @Green, @Blue, @Intensity, @FlightId)";
							"VALUES (@X, @Y, @Z, @Red, @Green, @Blue, @Intensity, @FlightId)";

						#region Create parameters
						DbParameter param = factory.CreateParameter();
						param.ParameterName = "@X";
						param.DbType = DbType.Double;
						command.Parameters.Add(param);

						DbParameter param2 = factory.CreateParameter();
						param2.ParameterName = "@Y";
						param2.DbType = DbType.Double;
						command.Parameters.Add(param2);

						DbParameter param3 = factory.CreateParameter();
						param3.ParameterName = "@Z";
						param3.DbType = DbType.Double;
						command.Parameters.Add(param3);

						DbParameter param4 = factory.CreateParameter();
						param4.ParameterName = "@Red";
						param4.DbType = DbType.Int64;
						command.Parameters.Add(param4);

						DbParameter param5 = factory.CreateParameter();
						param5.ParameterName = "@Green";
						param5.DbType = DbType.Int64;
						command.Parameters.Add(param5);

						DbParameter param6 = factory.CreateParameter();
						param6.ParameterName = "@Blue";
						param6.DbType = DbType.Int64;
						command.Parameters.Add(param6);

						DbParameter param7 = factory.CreateParameter();
						param7.ParameterName = "@Intensity";
						param7.DbType = DbType.Double;
						command.Parameters.Add(param7);

						DbParameter param8 = factory.CreateParameter();
						param8.ParameterName = "@FlightId";
						param8.DbType = DbType.Int64;
						command.Parameters.Add(param8);

						#endregion

						//set parameters
						//command.Parameters["@PointCloudXYZId"].Value = 1; //?? auto generated door sql server
						command.Parameters["@X"].Value = splitLine[0];
						command.Parameters["@Y"].Value = splitLine[1];
						command.Parameters["@Z"].Value = splitLine[2];
						command.Parameters["@Red"].Value = splitLine[3];
						command.Parameters["@Green"].Value = splitLine[4];
						command.Parameters["@Blue"].Value = splitLine[5];
						if (splitLine.Length == 7)
							command.Parameters["@Intensity"].Value = splitLine[6];
						else 
							command.Parameters["@Intensity"].Value = 0;  //TODO: fix: should be NULL
						command.Parameters["@FlightId"].Value = droneFlight.FlightId;

						command.ExecuteNonQuery();

						//Set hasXYZ to true
						droneFlight.hasXYZ = true;

						//progressbar
						lineNo++;
						if ((lineNo % 10) == 0)
						{
							Helper.Helper.SetProgress((lineNo / (double)totalLines) * 100);
						}

					}
					catch (Exception ex) { 
						//TODO rollback? (kan enkel met transaction)
						System.Diagnostics.Debug.WriteLine(ex);
					}
				}
			}

			//commit datatable??

			connection.Close();
			Helper.Helper.SetProgress(100);
			return true;
		}
	}
}