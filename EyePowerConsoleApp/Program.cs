using Byrant.EyePower.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyePowerConsoleApp
{
    class Program
    {
        /// <summary>
        /// Pass in the IP to connect to
        /// The following key presses work:
        /// q - quit
        /// o - Will lead to any subsequent numeric key presses turning the coresponding output on
        /// f - Will lead to any subsequent numeric key presses turning the coresponding output off
        /// 1-9 - Turns on or off the coresponding output based on previous o or f key press
        /// s - Reads the Status back
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            EyePowerClient c = new Byrant.EyePower.Control.EyePowerClient();
            c.SerialNumber += C_SerialNumber;
            c.OutputStatus += C_OutputStatus;
            c.CommandResponse += C_CommandResponse;
            c.Open(args[0], 1243);

            char ch = Console.ReadKey().KeyChar;
            bool bOn = false;
            while (ch != 'q')
            {
                if (ch == 's')
                {
                    c.RequestStatus();
                }
                else if (ch == 'o')
                {
                    bOn = true;
                }

                else if (ch == 'f')
                {
                    bOn = false;
                }
                else if (Char.IsDigit(ch))
                {
                    int id = ch - '0';
                    c.RequestChangeOutput(id, bOn);

                }
                ch = Console.ReadKey().KeyChar;
            }
        }

        private static void C_CommandResponse(Commands c, bool result)
        {
            Console.WriteLine("");
            Console.WriteLine(string.Format("Command {0} returned {1}", c.ToString(), result));
        }

        private static void C_OutputStatus(EyePowerStatus status)
        {
            Console.WriteLine("");
            Console.WriteLine(string.Format("Main Power {0}, Backup Power {1}", status.MainPowerActive, status.BackupPowerActive));

            foreach (EyePowerOutput o in status.Outputs)
            {

                Console.WriteLine(string.Format("{0}, Input {1}, Expected {2}, Actual {3}", o.RelayNumber, o.InputSupplyAvailable, o.ExpectedState, o.Active));
            }
        }

        private static void C_SerialNumber(string serialNumber)
        {
            Console.WriteLine(serialNumber);
        }
    }
}
