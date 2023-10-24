using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Inputs
{
    public struct KeyEvent
    {
        public VirtualKey VirtualKey { get; set; }
        public KeyState KeyState { get; private set; }
        public int TimeStamp { get; private set; }

        public KeyEvent(VirtualKey virtualKey, KeyState keyState, int timeStamp)
        {
            VirtualKey = virtualKey;
            KeyState = keyState;
            TimeStamp = timeStamp;
        }
    }
}
