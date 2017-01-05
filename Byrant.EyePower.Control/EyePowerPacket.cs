using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byrant.EyePower.Control
{
    /// <summary>
    /// Represents a packed of transmitted data to or from an eyepower device
    /// </summary>
    class EyePowerPacket
    {
        private byte[] data;

        public EyePowerPacket(int bodyLength)
        {
             data = new byte[7 + bodyLength];
            data[0] = 0x10;
            data[1] = 0x2;
            data[2] = 0xFA; // Relay Processor is 0xFA.  Measurement Processor is 0xFB
            data[data.Length - 2] = 0x10;
            data[data.Length - 1] = 0x3; 
        }

     
        public EyePowerPacket(byte[] bytes)
        {
            data = bytes;
        }

        public Commands Command
        {
            get
            {
                return (Commands)data[3];
            }
            set
            {
                data[3] = (byte)value;
            }
        }

        public IEnumerable<byte> MessageBody
        {
            get
            {
                return data.Skip(4).Take(BodyLength);
            }
            set
            {
                byte[] newVal = value.ToArray();
                newVal.CopyTo(data, 4);
            }
        }

        public int BodyLength
        {
            get
            {
                return data.Length - 7;
            }
        }

        public byte[] Bytes
        {
            get
            {
                return data;
            }
        }

        public void SetCheckSum()
        {
            uint c = 0;
            for (int i=2;i<(data.Length-5)+2;i++)
            {
                c += (uint)data[i];
            }
            data[data.Length - 3] = (byte)c;
        }




    }
}
