using DroneWebApp.Models.SimpleFactoryPattern.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern
{
    public class Creator
    {
        public Creator()
        {
            GetDatabaseConnection();
        }
        public void GetParser(string extensionType, string path)
        {
            
            IParser parser = ParserFactory.MakeParser(extensionType);
            parser.Parse(path);
        }

        public void GetDatabaseConnection()
        {

        }
    }
}