using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byrant.EyePower.Control
{
    /// <summary>
    /// Class to communicate with Byrant EyePower Devices
    /// </summary>
    public class EyePowerClient : AsyncTCPClient
    {
        public delegate void OnSerialNumber(string serialNumber);
        public event OnSerialNumber SerialNumber;

        public delegate void OnOutputStatus(EyePowerStatus status);
        public event OnOutputStatus OutputStatus;

        public delegate void OnCommandResponse(Commands c, bool result);
        public event OnCommandResponse CommandResponse;

        protected override void ProcessData()
        {
            byte[] data = ReceivedByteBuffer.ToArray();

            EyePowerPacket p = new EyePowerPacket(data);

            switch(p.Command)
            {
                case Commands.ReadSerialNumber:
                    ProcessSerialNumberBytes(p.MessageBody);
                    break;
                case Commands.TurnAllOff:
                case Commands.TurnIndividualOn:
                case Commands.TurnIndividualOff:
                case Commands.Status:
                    ProcessRelayStatusBytes(p.MessageBody);
                    break;
                default:
                    ProcessOtherCommand(p.Command, p.MessageBody);
                    break;

            }


            ReceivedByteBuffer.Clear();

            base.ProcessData();
        }

        protected void ProcessSerialNumberBytes(IEnumerable<byte> bytes)
        {
            uint value = BitConverter.ToUInt32(bytes.ToArray(), 0);
            if (this.SerialNumber!=null)
            {
                this.SerialNumber(value.ToString());
            }

        }


        protected void ProcessOtherCommand(Commands c, IEnumerable<byte> bs)
        {
            byte b = bs.First();

            if (this.CommandResponse!=null)
            {
                bool res = (b == 0x6);
                this.CommandResponse(c, res);
            }
        }

        protected void ProcessRelayStatusBytes(IEnumerable<byte> bytes)
        {
            if (this.OutputStatus != null)
            {
                byte[] data = bytes.ToArray();

                EyePowerOutput[] ret = new EyePowerOutput[14];



                // Now Get the Relay States as an int
                uint states = ConvertBytesToBigEndian(data[3], data[4]);
                uint expectedstates = ConvertBytesToBigEndian(data[0], data[1]);
                uint inputstates = ConvertBytesToBigEndian(data[5], data[6]);


                for (int i = 0; i < 14; i++)
                {
                    ret[i] = new EyePowerOutput();
                    ret[i].RelayNumber = i+1;

                    ret[i].Active = IsBitSet(states, i);
                    ret[i].ExpectedState = IsBitSet(expectedstates, i);
                    ret[i].InputSupplyAvailable = IsBitSet(inputstates, i);
                }


                EyePowerStatus status = new EyePowerStatus();
                status.Outputs = ret;
                status.MainPowerActive= IsBitSet(inputstates, 14);
                status.BackupPowerActive = IsBitSet(inputstates, 15);
                this.OutputStatus(status);
            }


        }

        private bool IsBitSet(uint c, int bit)
        {
            return (c & (1 << bit)) != 0;
        }

        private uint ConvertBytesToBigEndian(byte a, byte b)
        {
            return (uint)(a << 8) | b;
        }

        public void RequestSerialNumber()
        {
            Send(EyePowerPacketFactory.Create(Commands.ReadSerialNumber));
        }

        public void RequestStatus()
        {
            Send(EyePowerPacketFactory.Create(Commands.Status));
        }



        public void RequestTurnAllOff()
        {
            Send(EyePowerPacketFactory.Create(Commands.TurnAllOff));
        }

        public void RequestChangeOutput(int outputId, bool on)
        {
            byte[] payload = new byte[1];
            payload[0]= (byte)(outputId - 1);

            EyePowerPacket p;
            if (on)
            {
                p = EyePowerPacketFactory.Create(Commands.TurnIndividualOn,payload);
            }
            else
            {
                p = EyePowerPacketFactory.Create(Commands.TurnIndividualOff,payload);
            }
            Send(p);
        }


        private void Send(EyePowerPacket p)
        {
            base.BeginSend(p.Bytes);
        }
    }
}
