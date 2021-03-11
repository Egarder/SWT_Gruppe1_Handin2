using ChargingStationClassLib.Models;
using NUnit.Framework;

namespace ChargingStation.Test.Unit
{
    public class Tests
    {

        private IRFIDReader _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new RFIDReader();
        }

        //Functional tests
        [Test]
        public void CardIDPropertySet_ValueIsSet()
        {
            _uut.CardID = 50;

            Assert.That(_uut.CardID)(50);
        }

        //Behavioral tests
    }
}