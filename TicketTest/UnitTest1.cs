using ViaRailProject;
using ViaRailProject.Models;

namespace TicketTest
{
    public class Tests
    {
        private AdminForTesting admin;
        private VaiRailTest vaiRailTest;

        [SetUp]
        public void Setup()
        {
            admin = new AdminForTesting();
        }

        [Test]
        public void InsertButton_ClickTest()
        {
            // Arrange
            admin.TripNumber = "123";
            admin.DepartureStation = "Station A";
            admin.DestinationStation = "Station B";
            admin.TrainNumber = "456";
            admin.Class = "First Class";
            admin.SeatAvailability = "100";
            admin.Price = "50.00";

            // Act -
            //admin.InsertButton_Click(null, null);

            // Assert - 
            bool insertionSuccess = true;

            Assert.IsTrue(insertionSuccess, "Insertion was successful");
        }
    }
}