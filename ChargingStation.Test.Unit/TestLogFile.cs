using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStationClassLib.Models;
using NSubstitute;
using NUnit.Framework;

namespace ChargingStation.Test.Unit
{
    class TestLogFile
    {
        private LogFile _uut;
        private IRFIDReader _reader;
        private IDoor _door;
        private ScanEventArgs _receivedScanEventArgs;

        [SetUp]
        public void Setup()
        {
            _receivedScanEventArgs = null;
            _reader = Substitute.For<IRFIDReader>();
            _door = Substitute.For<IDoor>();
            _uut = new LogFile(_reader,_door);
        }


        [TestCase(true)]
        [TestCase(false)]
        public void WriteToFile_DoorOpened_WrittenToFile(bool opened)
        {
            _door.DoorMoveEvent += Raise.EventWith(new DoorMoveEventArgs {HasOpened = opened});
            _uut.Received(1).
        }

        //Functional tests
        [Test]
        public void CardIDPropertySetGet_ValueIsSet()
        {
            _uut.CardID = 50;

            Assert.That(_uut.CardID, Is.EqualTo(50));
        }

        [Test]
        public void SetCardID_ToNewID_CorrectIDReceived()
        {
            _uut.CardID = 50;

            Assert.That(_receivedScanEventArgs.ID, Is.EqualTo(50));
        }
    }
}
