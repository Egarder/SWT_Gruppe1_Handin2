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
        private IUsbCharger _usbccharge;
        private IChargeControl _chargecontrol;

        [SetUp]
        public void setup()
        {
            _logfile = new LogFile();
            _display = new Display();
            _door = new Door();
            _rfid = new RFIDReader();
            _usbccharge = new UsbCharger();
            _chargecontrol = new ChargeControl(_usbccharge);

            _uut = new StationControl(_door, _logfile, _rfid, _chargecontrol, _usbccharge);
        }


        //Event tests


        //Functional tests


        //Behavioral test

    }
}