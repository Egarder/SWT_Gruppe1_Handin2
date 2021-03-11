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
        private string _fileName;
        private StringBuilder _sb;
        private string _line;
        [SetUp]
        public void Setup()
        {
            _uut = new LogFile(_fileName);
            _reader = new StreamReader(_fileName);

        }

        [TestCase("Denne linje er flot")]
        [TestCase("Denne linje er lang")]
        public void WriteToFile_1TimeDifferentStrings_FileExists(string text)
        {
            _uut.WriteToFile(text);
            Assert.That(File.Exists(_fileName), Is.EqualTo(true));
        }

        [TestCase("Denne linje er flot")]
        [TestCase("Denne linje er lang")]
        public void WriteToFile_1TimeDifferentStrings_CorrectTextWrittenToFile(string text)
        {
            _uut.WriteToFile(text);
            Assert.That(_reader.ReadLine(),Is.EqualTo(text));
        }

        [TestCase("Denne linje er flot", "Denne linje er lang")]
        [TestCase("Denne linje er lang", "Denne linje er lang")]
        public void WriteToFile_MultipleTimesDifferentStrings_CorrectTextWrittenToFile(string text1,string text2)
        {
            //act on file
            _uut.WriteToFile(text1);
            _uut.WriteToFile(text2);

            //read on file
            string str = _reader.ReadToEnd();

            Assert.Multiple(() =>
            {
                Assert.That(str, Contains.Substring(text1));
                Assert.That(str, Contains.Substring(text2));
            });
        }

    }
}
