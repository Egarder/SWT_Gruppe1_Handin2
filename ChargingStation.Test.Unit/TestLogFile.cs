using System;
using System.Collections.Generic;
using System.IO;
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
        private StreamReader _reader;

        [SetUp]
        public void Setup()
        {
            _uut = new LogFile("LogFile.txt");
            _reader = 
        }


        [TestCase("Denne linje er flot")]
        [TestCase("Denne linje er lang")]
        public void WriteToFile_1TimeDifferentStrings_CorrectTextWrittenToFile(string text)
        {
            _uut.WriteToFile(text);
  

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
