using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byrant.EyePower.Control
{
    /// <summary>
    /// Prepresents the status of an EyePower Device
    /// </summary>
    public class EyePowerStatus
    {
        public bool MainPowerActive { get; set; }
        public bool BackupPowerActive { get; set; }

        public EyePowerOutput[] Outputs { get; set; }
    }
}
