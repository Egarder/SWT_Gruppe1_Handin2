using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private DateTime _dateTime;
        private DateTime _dateTime2;

        [SetUp]
        public void Setup()
        {
            _uut = new LogFile();
            _dateTime = new DateTime(2010, 8, 18);
            _dateTime = new DateTime(2010, 8, 18);
        }

        [TestCase("Denne linje er flot")]
        public void WriteToLog_1Item_ListContains1Item(string text)
        {
            _uut.WriteToLog(text, _dateTime);
            Assert.That(_uut.LogList.Count, Is.EqualTo(1));
        }

        [TestCase("Denne linje er flot", "Denne linje er lang")]
        public void WriteToLog_2Items_ListContains2Items(string text1, string text2)
        {
            _uut.WriteToLog(text1, _dateTime);
            _uut.WriteToLog(text1, _dateTime);
            Assert.That(_uut.LogList.Count, Is.EqualTo(2));
        }
    }

}

