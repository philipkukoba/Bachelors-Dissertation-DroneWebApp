﻿using DroneWebApp.Models.SimpleFactoryPattern.Parsers;
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
            Db = (DroneDBEntities)db;
        }

        public DroneDBEntities Db { get; set; }

        public void GetParser(string extensionType, string path, int flightId)
        {
            IParser parser = ParserFactory.MakeParser(extensionType);
            parser.Parse(path, flightId, Db);
        }
    }
}