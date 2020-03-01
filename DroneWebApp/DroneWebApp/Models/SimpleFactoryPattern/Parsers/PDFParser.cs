using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class PDFParser : IParser
    {
        public void Parse(string path, int flightId, DroneDBEntities db)
        {
            throw new NotImplementedException();
        }
    }
}