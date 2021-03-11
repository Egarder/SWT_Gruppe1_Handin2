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
    class TestStationController
    {
        private StationControl _uut;
        private ILogFile _logfile;
        private IDisplay _display;
        private IDoor _door;
        private IRFIDReader _rfid;
        private IUsbCharger _uscbcharge;
        private IChargeControl _chargecontrol;

        [SetUp]
        public void setup()
        {
            _uut = new StationControl( );
        }


        //Event tests


        //Functional tests


        //Behavioral test

    }
}