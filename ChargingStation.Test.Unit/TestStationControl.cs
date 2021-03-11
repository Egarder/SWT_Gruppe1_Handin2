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
        private ILogFile _logfile = new LogFile();
        private IDisplay _display = new Display();
        private IDoor _door = new Door();
        private IRFIDReader _rfid = new RFIDReader();
        private IUsbCharger _uscbcharge = new UsbCharger();
        private IChargeControl _chargecontrol = new ChargeControl(_uscbcharge);

        [SetUp]
        public void setup()
        {
            _uut = new StationControl();
        }


        //Event tests


        //Functional tests


        //Behavioral test

    }
}