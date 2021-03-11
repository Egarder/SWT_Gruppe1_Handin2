﻿using System;
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

        //===================================  **HER MANGLER AT BLIVE TESTET DET SIDSTE ELSE STATEMENT I rfidHANDLEREN****=======================================

        //Door handler tests

        [Test]
        public void OpenDoor_RfidNotScanned_WriteToDisplayAndWriteToLog()
        {
            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() {HasClosed = false});

            // Assert
            _display.Received(1).ShowMessage("Please scan card");
            _logfile.Received(1).WriteToLog("Please scan card", DateTime.MinValue);
        }

        [Test]
        public void OpenDoor_RfidScanned_WriteToDisplayAndWriteToLog()
        {
            // Act
            _rfid.ScanEvent += Raise.EventWith(this, new ScanEventArgs() { ID = 1234 });
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = false });

            // Assert
            _display.Received(1).ShowMessage("Door Opened. Please Connect Phone");
            _logfile.Received(1).WriteToLog("Door Opened. Please Connect Phone", DateTime.MinValue);
        }

        [Test]
        public void CloseDoor_InOpenedState_PhoneNotConnected_WriteToDisplayAndWriteToLog()
        {
            // Arrange
            _uut.OldId = 1234;
            _uut.State = StationControl.ChargingStationState.Opened;
            
            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = true });

            // Assert
            _display.Received(1).ShowMessage("Please connect phone");
            _logfile.Received(1).WriteToLog("Please connect phone", DateTime.MinValue);
        }

        [Test]
        public void CloseDoor_InOpenedState_PhoneConnected_WriteToDisplayAndWriteToLog()
        {
            // Arrange
            _uut.OldId = 1234;
            _usbccharge.Connected = true;
            _uut.State = StationControl.ChargingStationState.Opened;

            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = true });

            // Assert
            _display.Received(1).ShowMessage("Charging started");
            _logfile.Received(1).WriteToLog("Charging started", DateTime.MinValue);
        }

        [Test]
        public void CloseDoor_InOpenedState_PhoneConnected_StartChargeCalled()
        {
            // Arrange
            _uut.OldId = 1234;
            _usbccharge.Connected = true;
            _uut.State = StationControl.ChargingStationState.Opened;

            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = true });

            // Assert
            _usbccharge.Received(1).StartCharge();
        }

        [Test]
        public void CloseDoor_InOpenedState_PhoneConnected_StateChangedToLocked()
        {
            // Arrange
            _uut.OldId = 1234;
            _usbccharge.Connected = true;
            _uut.State = StationControl.ChargingStationState.Opened;

            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = true });

            // Assert
            Assert.That(_uut.State, Is.EqualTo(StationControl.ChargingStationState.Locked));
        }

        [Test]
        public void CloseDoor_InLockedState_StateChangedToAvailable()
        {
            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = true });

            // Assert
            Assert.That(_uut.State, Is.EqualTo(StationControl.ChargingStationState.Available));
        }

        [Test]
        public void CloseDoor_InAvailableState_StateChangedToOpened()
        {
            // Arrange
            _uut.OldId = 1234;

            // Act
            _door.DoorMoveEvent += Raise.EventWith(this, new DoorMoveEventArgs() { HasClosed = false });

            // Assert
            Assert.That(_uut.State, Is.EqualTo(StationControl.ChargingStationState.Opened));
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