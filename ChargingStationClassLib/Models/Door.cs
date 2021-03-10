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

    class Door : IDoor
    {
        public event EventHandler<DoorLockEventArgs> DoorLockEvent;
        public event EventHandler<DoorMoveEventArgs> DoorMoveEvent;

        public Door()
        {
            
        }
        public void OpenDoor()
        {
            OnDoorMoveEvent(new DoorMoveEventArgs { HasOpened = true });
        }

        public void CloseDoor()
        {
            OnDoorMoveEvent(new DoorMoveEventArgs { HasOpened = false });
        }

        public void LockDoor()
        {
            OnDoorLockEvent(new DoorLockEventArgs { IsLocked = true });
        }

        public void UnlockDoor()
        {
            OnDoorLockEvent(new DoorLockEventArgs { IsLocked = false });
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
