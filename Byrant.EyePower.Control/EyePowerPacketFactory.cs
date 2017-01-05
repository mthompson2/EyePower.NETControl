using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byrant.EyePower.Control
{
    static class EyePowerPacketFactory
    {
        /// <summary>
        /// Used to contruct EyePower Packets for transmission
        /// </summary>
        /// <param name="c"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static EyePowerPacket Create(Commands c, byte[] body=null)
        {
            int size = body == null ? 0 : body.Length;

            EyePowerPacket p = new EyePowerPacket(size);
        

            if (body!=null)
            {
                p.MessageBody = body;
            }

            p.Command = c;
            p.SetCheckSum();
            return p;
        }

    }
}
