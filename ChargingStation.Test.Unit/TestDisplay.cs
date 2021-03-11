using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStationClassLib.Models;
using NUnit.Framework;

namespace ChargingStation.Test.Unit
{
    public class TestDisplay
    {

        private IDisplay _uut;
        
        [SetUp]
        public void Setup()
        {
            _uut = new Display();
        }

        //Functional tests

        //Behavioral tests

        [Test]
        public void testest()
        {
            _uut.ShowMessage("Test");
        }

    }
}
