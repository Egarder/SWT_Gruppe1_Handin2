using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStationClassLib.Models;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace ChargingStation.Test.Unit
{
    [TestFixture]
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
        public void Setup()
        {
            _logfile = Substitute.For<ILogFile>();
            _door = Substitute.For<IDoor>();
            _rfid = Substitute.For<IRFIDReader>();
            _usbccharge = Substitute.For<IUsbCharger>();
            _display = Substitute.For<IDisplay>();
            _chargecontrol = new ChargeControl(_usbccharge);

            _uut = new StationControl(_door, _logfile, _rfid, _chargecontrol, _usbccharge, _display); //Injects including fakes
        }


        //RFID Handler tests

        [TestCase(1234)]
        public void RFIDEventhandler_stateAvailable_isSubscribed(int id)
        {
            
        }

        [TestCase(50)]
        [TestCase(1234)]
        public void RFIDEventhandler_stateAvailable_oldIdIsSet(int id)
        {
            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs {ID = id});

            Assert.That(_uut.OldId, Is.EqualTo(id));
        }

        [TestCase(50)]
        [TestCase(1234)]
        public void RFIDEventhandler_stateAvailable_unlockDoorIsCalled( int id)
        {
            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _door.Received(1).UnlockDoor();
        }

        [TestCase(50)]
        [TestCase(1234)]
        public void RFIDEventhandler_stateAvailable_ShowMessageIsCalled(int id)
        {
            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _display.Received(1).ShowMessage(Arg.Any<string>());
        }

        //Door handler tests
        [Test]
        public void DoorClosed_ChargingStateAvailableUSBChargerConnected_DoorLocked()
        {
            // Arrange
            _door.Closed = false;
            _uut.State = StationControl.ChargingStationState.Available;
            _usbccharge.Connected = true;

            // Act
            _door.CloseDoor();

            // Assert
            _door.Received(1).CloseDoor();
        }

        [Test]
        public void DoorClosed_ChargingStateAvailableUSBChargerConnected_WrittenToLog()
        {
            // Arrange
            _door.Closed = false;
            _uut.State = StationControl.ChargingStationState.Available;
            _usbccharge.Connected = true;

            // Act
            _door.CloseDoor();

            // Assert
            _logfile.Received(1).WriteToLog(Arg.Any<string>(), Arg.Any<DateTime>());
        }

        [Test]
        public void DoorClosed_ChargingStateAvailableUSBChargerConnected_StateChangedToLocked()
        {
            // Arrange
            _door.Closed = false;
            _uut.State = StationControl.ChargingStationState.Available;
            _usbccharge.Connected = true;

            // Act
            _door.CloseDoor();

            // Assert
            Assert.That(_uut, Is.EqualTo(StationControl.ChargingStationState.Locked));
        }


        [Test]
        public void DoorClosed_ChargingStateLocked_StateChangedToAvailable()
        {
            // Arrange
            _door.Closed = false;
            _uut.State = StationControl.ChargingStationState.Locked;
            _usbccharge.Connected = false;

            // Act
            _door.CloseDoor();

            // Assert
            Assert.That(_uut.State, Is.EqualTo(StationControl.ChargingStationState.Available));
        }

        //Behavioral test
        [Test]
        public void ChargeChanged_CurrentUnderFiveShowMessage_ShowMessage()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 2 });
            _display.Received(1).ShowMessage("Phone fully charged");
        }

        [Test]
        public void ChargeChanged_CurrentUnderFiveShowMessage_WriteLog()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 2 });
            _logfile.Received(1).WriteToLog("Phone fully charged", Arg.Any<DateTime>());
        }

        [Test]
        public void ChargeChanged_CurrentOverFiveHundredShowMessage_ShowMessage()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 502 });
            _display.Received(1).ShowMessage("ERROR! Faulty Charger!");
        }
        [Test]
        public void ChargeChanged_CurrentOverFiveHundredShowMessage_WriteLog()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 502 });
            _logfile.Received(1).WriteToLog("ERROR! Faulty Charger!", Arg.Any<DateTime>());
        }

        [Test]
        public void ChargeChanged_JustCharing_ShowMessage()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 300 });
            _display.Received(1).ShowMessage("Charging");
        }
        [Test]
        public void ChargeChanged_JustCharing_WriteLog()
        {
            _usbccharge.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 300 });
            _logfile.Received(1).WriteToLog("Charging", Arg.Any<DateTime>());
        }
    }
}