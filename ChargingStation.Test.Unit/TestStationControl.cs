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

            _uut = new StationControl(_door, _logfile, _rfid, _chargecontrol, _usbccharge, _display); //Injects fakes
        }


        //RFID Handler tests
        [TestCase(50)]
        [TestCase(1234)]
        public void RFIDEventhandler_stateAvailable_oldIdIsSet(int id)
        {
            _uut.State = StationControl.ChargingStationState.Available;
            _door.Closed = true;
            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs {ID = id});

            Assert.That(_uut.OldId, Is.EqualTo(id));
        }


        [TestCase(50)]
        [TestCase(1234)]
        public void RFIDEventhandler_stateAvailable_ShowMessageIsCalled(int id)
        {
            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _display.Received(1).ShowMessage(Arg.Any<string>());
        }

        [TestCase(50)]
        public void RFIDEventhandler_stateLocked_CardIDMatch_UnlockDoorCalled(int id)
        {
            _uut.State = StationControl.ChargingStationState.Locked;
            _uut.OldId = id;

            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _door.Received(1).UnlockDoor();
        }

        [TestCase(50)]
        public void RFIDEventhandler_stateLocked_CardIDMatch_StateChanged(int id)
        {
            _uut.State = StationControl.ChargingStationState.Locked;
            _uut.OldId = id;

            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            Assert.That(_uut.State, Is.EqualTo(StationControl.ChargingStationState.Available));

        }

        [TestCase(50)]
        public void RFIDEventhandler_stateLocked_CardIDMatch_DisplayCorrectMessage(int id)
        {
            _uut.State = StationControl.ChargingStationState.Locked;
            _uut.OldId = id;

            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _display.Received(1).ShowMessage("ID Scanned and approved");
        }

        [TestCase(50)]
        public void RFIDEventhandler_stateLocked_CardIDMatch_WriteToLogIsCalled(int id)
        {
            _uut.State = StationControl.ChargingStationState.Locked;
            _uut.OldId = id;

            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _logfile.WriteToLog(Arg.Any<string>(), Arg.Any<DateTime>());
        }


        [TestCase(50)]
        public void RFIDEventhandler_stateLocked_CardIDNoMatch_DisplayCorrectMessage(int id)
        {
            _uut.State = StationControl.ChargingStationState.Locked;

            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _display.Received(1).ShowMessage("ID scanned - Closet already in use");
        }

        [TestCase(50)]
        public void RFIDEventhandler_stateLocked_CardIDNoMatch_WriteToLogIsCalled(int id)
        {
            _uut.State = StationControl.ChargingStationState.Locked;

            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _logfile.WriteToLog(Arg.Any<string>(), Arg.Any<DateTime>());
        }

        [TestCase(50)]
        public void RFIDEventhandler_StateOpened_ShowMessageCalled(int id)
        {
            _uut.State = StationControl.ChargingStationState.Opened;

            _rfid.ScanEvent += Raise.EventWith(new ScanEventArgs { ID = id });

            _display.Received(1).ShowMessage("Please close the door");
        }


        // Door handler tests
        [Test]
        public void DoorClosed_Message_CorrectMessage()
        {
            _door.DoorMoveEvent += Raise.EventWith(new DoorMoveEventArgs {HasClosed = true});
            _display.Received(1).ShowMessage("Please scan card");
        }


        [Test]
        public void OpenDoor_InAvailableState_StateChangedToOpened()
        {
            // Arrange
            _uut.OldId = 1234;

            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = false });

            // Assert
            Assert.That(_uut.State, Is.EqualTo(StationControl.ChargingStationState.Opened));
        }


        //Carge handler tests
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


        // Misc tests

        [Test]
        public void SetTimeStamp_TimeStampHasCorrectValue()
        {
            // Act
            _uut.TimeStamp = DateTime.Today;

            // Assert
            Assert.That(_uut.TimeStamp, Is.EqualTo(DateTime.Today));
        }
    }
}