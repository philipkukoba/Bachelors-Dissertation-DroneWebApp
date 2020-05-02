using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DroneWebApp.Models;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class FilesControllerTests
    {
        [TestMethod()]
        public void IndexTest1_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            FilesController controller = new FilesController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Index(3) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void IndexTest2_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            FilesController controller = new FilesController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            //List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();

            var postedFiles = new Mock<List<HttpPostedFileBase>>();
            /*
            using (var stream = new MemoryStream())
            using (var bmp = new Bitmap(1, 1))
            {
                var graphics = Graphics.FromImage(bmp);
                graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
                bmp.Save(stream, ImageFormat.Jpeg);

                postedFiles.Setup(pf => pf.InputStream).Returns(stream);

                var result = controller.Index(3, postedFiles.Object) as ViewResult;
                Assert.AreEqual("Index", result.ViewName);
            }
            */

        }

        [TestMethod()]
        public void GetProgressStatusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetParseResultTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetFileNameTest()
        {
            Assert.Fail();
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