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

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class CTRLPointsControllerTests
    {
        [TestMethod()]
        public void GetCTRLPointsTest()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            CTRLPointsController controller = new CTRLPointsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Create a mock DbSet
            List<CTRLPoint> ctrlPoints = GetCTRLPoints();
            var mockSet = CreateMockSet(ctrlPoints);
            // Set up the CTRLPoints property so it returns the mocked DbSet
            mockContext.Setup(o => o.CTRLPoints).Returns(() => mockSet.Object);

            var response = controller.GetCTRLPoints();

            List<CTRLPoint> ctrlReturn = new List<CTRLPoint>();
            Assert.IsTrue(response.TryGetContentValue(out ctrlReturn));
        }

        [TestMethod()]
        public void GetCTRLPointsByFlightIDTest()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            CTRLPointsController controller = new CTRLPointsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Create a mock DbSet
            List<CTRLPoint> ctrlPoints = GetCTRLPoints();
            var mockSet = CreateMockSet(ctrlPoints);
            // Set up the CTRLPoints property so it returns the mocked DbSet
            mockContext.Setup(o => o.CTRLPoints).Returns(() => mockSet.Object);

            var response = controller.GetCTRLPointsByFlightID(3);

            CTRLPoint ctrl;
            Assert.IsTrue(response.TryGetContentValue(out ctrl));
        }

        private List<CTRLPoint> GetCTRLPoints()
        {
            List<CTRLPoint> ctrlPoints = new List<CTRLPoint>();

            for (int i=1; i<=10; i++)
            {
                CTRLPoint ctrl = new CTRLPoint
                {
                    FlightId = 1,
                    CTRLId = i,
                    CTRLName = "ctrl-" + i,
                    X = 1,
                    Y = 2,
                    Z = 3
                };

                ctrlPoints.Add(ctrl);
            }

            return ctrlPoints;
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
                
                flights.Add(flight);
            }
            return flights;
        }

        private Mock<DbSet<CTRLPoint>> CreateMockSet(List<CTRLPoint> ctrlPoints)
        {
            var queryable = ctrlPoints.AsQueryable();
            var mockSet = new Mock<DbSet<CTRLPoint>>();

            mockSet.As<IQueryable<CTRLPoint>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<CTRLPoint>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<CTRLPoint>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<CTRLPoint>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
    }
}