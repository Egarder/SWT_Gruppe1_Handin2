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
            _door = Substitute.For<IDoor>();
            _rfid = Substitute.For<IRFIDReader>();
            _usbccharge = Substitute.For<IUsbCharger>();
            _chargecontrol = new ChargeControl(_usbccharge);

            _uut = new StationControl(_door, _logfile, _rfid, _chargecontrol, _usbccharge);
        }


        //RFID Handler tests
        [Test]
        public void RFIDEventhandler_stateAvailable_oldIdIsSet()
        {
            _rfid.CardID = 50;

            Assert.That(_uut.OldId, Is.EqualTo(50));
        }

        [Test]
        public void RFIDEventhandler_stateAvailable_unlockDoorIsCalled()
        {
            _rfid.CardID = 50;
        }

        [Test]
        public void RFIDEventhandler_stateAvailable_ShowMessageIsCalled()
        {

        }

        //Door handler tests
        [Test]
        public void DoorClosed_ChargingStateAvailableUSBChargerConnected()
        {
            

            _door.CloseDoor();
        }


        //if (!e.HasOpened && _state == ChargingStationState.Available && _usbCharger.Connected)
        //{
        //    _door.LockDoor();
        //    _chargeControl.StartCharge();
        //    message = "Door locked";
        //    _state = ChargingStationState.Locked;
        //}

        //Behavioral test
        [Test]
        public void ChargeChanged_CurrentUnderFiveShowMessage_ShowMessage()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 2 });
            _display.Received(1).ShowMessage("");
        }
        [Test]
        public void ChargeChanged_CurrentUnderFiveShowMessage_WriteLog()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 2 });
            _logfile.Received(1).WriteToLog("");
        }

        [Test]
        public void ChargeChanged_CurrentOverFiveHundredShowMessage_ShowMessage()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 502 });
            _display.Received(1).ShowMessage("");
        }
        [Test]
        public void ChargeChanged_CurrentOverFiveHundredShowMessage_WriteLog()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 502 });
            _logfile.Received(1).WriteToLog("");
        }

    }
}