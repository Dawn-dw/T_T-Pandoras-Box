using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Inputs
{
    public class InputKeyEventArgs
    {
        public bool BlockInput { get; set; }
        public KeyEvent KeyEvent { get; private set; }

        public InputKeyEventArgs(KeyEvent keyEvent)
        {
            KeyEvent = keyEvent;
            BlockInput = false;
        }
    }
}
