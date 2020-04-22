using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DroneWebApp.Models.DataExport
{
    public class FactoryPDF : IFactory
    {
        public void CreateDroneLog(int droneId, DroneDBEntities db, HttpContextBase context)
        {
            throw new NotImplementedException();
        }

        public void CreatePilotLog(int pilotId, DroneDBEntities db, HttpContextBase context)
        {
            throw new NotImplementedException();
        }
    }
}