using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byrant.EyePower.Control
{
    /// <summary>
    /// Defines the list of EyePower COmmands supported
    /// </summary>
    public enum Commands : byte
    {

        ReadSerialNumber = 0x21,
        Status = 0x31,
        TurnAllOff = 0x33,
        TurnIndividualOn = 0x34,
        TurnIndividualOff = 0x35

    }
}
