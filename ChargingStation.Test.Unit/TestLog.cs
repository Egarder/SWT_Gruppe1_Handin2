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
    class TestLog
    {
        private LogFile _uut;
        private StreamReader _reader;
        private string _fileName = "TestFile.txt";
        private StringBuilder _sb;
        private string str;
        [SetUp]
        public void Setup()
        {
            _uut = new LogFile(_fileName);
            _reader = new StreamReader(_fileName);
        }

        [TestCase("Denne linje er flot")]
        public void WriteToFile_1TimeDifferentStrings_FileExists(string text)
        {
            _uut.WriteToLog(text);
            Assert.That(File.Exists(_fileName), Is.EqualTo(true));
        }


    }
}
