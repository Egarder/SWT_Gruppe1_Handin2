using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStationClassLib.Models
{
    public class DoorLockEventArgs : EventArgs
    {
        public bool IsLocked { get; set; }
    }

    public class DoorMoveEventArgs : EventArgs
    {
        public bool HasOpened { get; set; }
    }

    public class Door : IDoor
    {
        private bool Locked { get; set; }
        private bool Closed { get; set; }


        public event EventHandler<DoorLockEventArgs> DoorLockEvent;
        public event EventHandler<DoorMoveEventArgs> DoorMoveEvent;

        public Door()
        {
            Locked = false;
            Closed = true;
        }

        public void OpenDoor()
        {
            if (!Locked && Closed)
            {
                Closed = false;
                OnDoorMoveEvent(new DoorMoveEventArgs { HasOpened = true });
            }
        }

        public void CloseDoor()
        {
            if (!Closed)
            {
                Closed = true;
                OnDoorMoveEvent(new DoorMoveEventArgs { HasOpened = false });
            }
        }

        public void LockDoor()
        {
            if (!Locked && Closed)
            {
                Locked = true;
                OnDoorLockEvent(new DoorLockEventArgs { IsLocked = true });
            }
        }

        public void UnlockDoor()
        {
            if (Locked && Closed)
            {
                Locked = false;
                OnDoorLockEvent(new DoorLockEventArgs { IsLocked = false });
            }
        }

        protected virtual void OnDoorMoveEvent(DoorMoveEventArgs e)
        {
            DoorMoveEvent?.Invoke(this, e);
        }

        protected virtual void OnDoorLockEvent(DoorLockEventArgs e)
        {
            DoorLockEvent?.Invoke(this, e);
        }

    }
}
