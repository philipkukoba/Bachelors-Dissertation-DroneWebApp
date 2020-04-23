using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneWebApp.Models;
using System.Web.Mvc;
using Moq;
using System.Data.Entity;
using Xunit;
using Intuit.Ipp.Data;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class DroneFlightsControllerTests
    {

        [TestMethod()]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);


            // Create a mock DbSet
            var mockSet = new Mock<DbSet<DroneFlight>>();
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);

            var queryable = getFlights().AsQueryable();
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        [TestMethod()]
        public void IndexTest()
        {
            
           

            // Create cibtext
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();

            // Create a mock DbSet
            var mockSet = new Mock<DbSet<DroneFlight>>();

            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);

            //  Set up the DbSet as an IQueryable so it can be enumerated
            var queryable = getFlights().AsQueryable();
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            //ViewResult result = controller.Index() as ViewResult;

            // Assert
            var result = controller.Index();

            //Assert.IsNotNull(result);
            //Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QualityReportTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CTRLPointsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GCPPointsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TFWTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MapTest()
        {
            Assert.Fail();
        }

        List<DroneFlight> getFlights()
        {
            // Initialise a list of DroneFlight objects to back the DbSet with.
            // Arrange
            var droneFlights = new List<DroneFlight>();
            for (int i = 0; i < 10; i++)
            {
                DroneFlight df = new DroneFlight();
                
                Project project = new Project();
                project.ProjectCode = "PRJ-001";
                Pilot pilot = new Pilot();
                pilot.PilotName = "Bryan" + i;
                Drone drone = new Drone();
                drone.DroneType = "XYZ" + i;
                drone.Registration = "ABC";
                DateTime date = new DateTime(2020, 01, 01);

                df.Project = project;
                df.Pilot = pilot;
                df.Drone = drone;
                df.Date = date;




                droneFlights.Add(df);
            }
            return droneFlights;
        }
    }
}

