using System;
using ChargingStationClassLib.Models;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace ChargingStation.Test.Unit
{
    public class TestRFIDReader
    {
        private RFIDReader _uut;
        private ScanEventArgs _receivedScanEventArgs;

        [SetUp]
        public void Setup()
        {
            _receivedScanEventArgs = null; 

            _uut = new RFIDReader();

            //Event listener:
            _uut.ScanEvent += (o, args) => { _receivedScanEventArgs = args; };
        }

        //Functional tests
        [Test]
        public void CardIDPropertySetGet_ValueIsSet()
        {
            _uut.CardID = 50;

            Assert.That(_uut.CardID, Is.EqualTo(50));
        }

        [Test]
        public void SetCardID_ToNewID_CorrectIDReceivedByEvent()
        {
            _uut.CardID = 50;

            Assert.That(_receivedScanEventArgs.ID, Is.EqualTo(50));
        }


        //Behavioral tests

        [Test]
        public void SetCardID_ToNewID_EventTriggered()
        {
            _uut.CardID = 50;

            Assert.That(_receivedScanEventArgs, Is.Not.Null);
        }

        [Test]
        public void CardIDPropertySetGetNegativeInt_idErrorCalled()
        {
            _uut.CardID = -50;

            Assert.IsTrue(_uut.Error);
        }
    }
}