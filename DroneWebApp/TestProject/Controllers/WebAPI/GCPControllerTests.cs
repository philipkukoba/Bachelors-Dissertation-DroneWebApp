using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DroneWebApp.Models;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class GCPControllerTests
    {
        [TestMethod()]
        public void GetGroundControlPointsByFlightIDTest_LinkGeneration()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            GCPController controller = new GCPController();
            controller.Request = new HttpRequestMessage { RequestUri = new Uri("http://localhost:44378/api/GCP") };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "GCP" } });

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var response = controller.GetGroundControlPointsByFlightID(3);

            Assert.AreEqual("http://localhost:44378/api/GCP/3", response.Headers.Location.AbsoluteUri);
        }

        [TestMethod()]
        public void GetGroundControlPointsByFlightIDTest_ActionExecutes()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            GCPController controller = new GCPController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var httpContextMock = new Mock<HttpContextBase>();

            

            //controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            //var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            //mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            //mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var response = controller.GetGroundControlPointsByFlightID(3);

            System.Diagnostics.Debug.WriteLine(response.Content);
            Assert.AreEqual(flights.FirstOrDefault(df => df.FlightId == 3).GroundControlPoints, response.Content);
        }

        private List<DroneFlight> GetFlights()
        {
            // Initialise a list of DroneFlight objects to back the DbSet with.
            // Arrange
            List<DroneFlight> flights = new List<DroneFlight>();

            Pilot pilot = new Pilot
            {
                PilotId = 1,
                PilotName = "Pilot1"
            };

            Drone drone = new Drone
            {
                DroneId = 1,
                DroneType = "type1",
                Registration = "registration"
            };

            Project project = new Project
            {
                ProjectId = 1,
                ProjectCode = "Project1"
            };

            for (int i = 1; i <= 10; i++)
            {
                DroneFlight flight = new DroneFlight
                {
                    FlightId = i,
                    ProjectId = 1,
                    Project = project,
                    PilotId = 1,
                    Pilot = pilot,
                    DroneId = 1,
                    Drone = drone,
                    Date = DateTime.Now,
                };

                List<GroundControlPoint> gcps = new List<GroundControlPoint>();

                for (int j = 1; j <= 5; j++)
                {
                    GroundControlPoint gcp = new GroundControlPoint { FlightId = i, DroneFlight = flight, GCPId = j, GCPName = "gcp-" + j, X = 1, Y = 2, Z = 3 };

                    gcps.Add(gcp);
                }

                flight.GroundControlPoints = gcps;
                flights.Add(flight);
            }
            return flights;
        }

        private Mock<DbSet<DroneFlight>> CreateMockSet(List<DroneFlight> flights)
        {
            var queryable = flights.AsQueryable();
            var mockSet = new Mock<DbSet<DroneFlight>>();

            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
    }
}