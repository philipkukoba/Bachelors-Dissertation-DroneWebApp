using DroneWebApp.Models.SimpleFactoryPattern.Parsers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern
{
    public class Creator
    {
        public Creator(DbContext db)
        {
            Db = (DroneDBEntities) db;
        }

        public DroneDBEntities Db { get; set; }

        // Get the appropriate Parser for this type of document from the ParserFactory
        public void GetParser(string extensionType, string path, int flightId)
        {
            IParser parser = ParserFactory.MakeParser(extensionType);
            if(parser != null)
            {
                parser.Parse(path, flightId, Db);
            }
        }
    }
}