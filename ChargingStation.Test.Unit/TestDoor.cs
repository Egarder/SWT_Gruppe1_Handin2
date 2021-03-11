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
    class TestDoor
    {
        private IDoor _door;
        private DoorMoveEventArgs _doorMoveEventArgs;
        
        [SetUp]
        public void setup()
        {
            _doorMoveEventArgs = null;
            _door = new Door();

            _door.DoorMoveEvent +=
                (o, args) =>
                {
                    _doorMoveEventArgs = args;
                };
        }

        [Test]
        public void OpenDoor_AssertDoorMoveEvent_Raised()
        {
            // Act
            _door.OpenDoor();
            
            // Assert
            Assert.That(_doorMoveEventArgs, Is.Not.Null);
        }

        [Test]
        public void OpenDoor_AssertHasOpened_True()
        {
            // Act
            _door.OpenDoor();
            
            // Assert
            Assert.That(_doorMoveEventArgs.HasClosed, Is.EqualTo(false));
        }

        [Test]
        public void CloseDoor_AssertDoorMoveEvent_Raised()
        {
            // Act
            _door.OpenDoor();
            
            // Assert
            Assert.That(_doorMoveEventArgs, Is.Not.Null);
        }

        [Test]
        public void CloseDoor_AssertHasOpened_False()
        {
            // Arrange - Door has to be open before it can close
            _door.Closed = false;
            
            // Act
            _door.CloseDoor();
            
            // Assert
            Assert.That(_doorMoveEventArgs.HasClosed, Is.EqualTo(true));
        }

        [Test]
        public void LockDoor_DoorOpen_DoorNotLocked()
        {
            // Arrange - Door has to be open before it can close
            _door.Closed = false;

            // Act
            _door.LockDoor();

            // Assert
            Assert.That(_door.Locked, Is.EqualTo(false));
        }

        [Test]
        public void LockDoor_DoorClosedAndNotLocked_DoorLocked()
        {
            // Arrange - Door has to be open before it can close
            _door.Closed = true;
            _door.Locked = false;

            // Act
            _door.LockDoor();

            // Assert
            Assert.That(_door.Locked, Is.EqualTo(true));
        }

        [Test]
        public void UnLockDoor_DoorLockedAndDoorOpen_DoorNotUnlocked()
        {
            // Arrange - Door has to be open before it can close
            _door.Closed = false;
            _door.Locked = true;

            // Act
            _door.UnlockDoor();

            // Assert
            Assert.That(_door.Locked, Is.EqualTo(true));
        }

        [Test]
        public void UnLockDoor_DoorLockedAndClosed_DoorUnlocked()
        {
            // Arrange - Door has to be open before it can close
            _door.Closed = true;
            _door.Locked = true;

            // Act
            _door.UnlockDoor();

            // Assert
            Assert.That(_door.Locked, Is.EqualTo(false));
        }
    }
}
