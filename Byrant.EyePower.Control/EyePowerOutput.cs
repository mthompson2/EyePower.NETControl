using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byrant.EyePower.Control
{
    /// <summary>
    /// Provides information about a specific output on an eyepower device
    /// </summary>
    public class EyePowerOutput
    {
        public int RelayNumber { get; set; }
        public bool InputSupplyAvailable { get; set; }
        public bool Active { get; set; }

        public bool ExpectedState { get; set; }

    }
}
