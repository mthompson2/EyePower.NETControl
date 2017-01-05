using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Byrant.EyePower.Control
{
    /// <summary>
    /// A Base class for handling tcp comms.  ProcessData must be handled
    /// </summary>
    public  class AsyncTCPClient
    {
        private System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
        private const int BufferSize = 256;
        // Receive buffer.
        private byte[] buffer = new byte[BufferSize];
        // Received data string.
        protected List<byte> ReceivedByteBuffer = new List<byte>();


        public AsyncTCPClient()
        {

        }

        public void Open(string ip, int port)
        {
            client.Connect(ip, port);
            BeginReceive();
        }


        private void BeginReceive()
        {
            try
            {
                // Begin receiving the data from the remote device.
                client.Client.BeginReceive(buffer, 0, BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {

                // Read data from the remote device.
                int bytesRead = client.Client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    ReceivedByteBuffer.AddRange(buffer.Take(bytesRead));

                    // Get the rest of the data.
                    client.Client.BeginReceive(buffer, 0, BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), null);
                }
                if (ReceivedByteBuffer.Count > 1)
                {
                    ProcessData();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected virtual void ProcessData()
        {
            // To be Handled by specific implementations
        }

        protected void BeginSend(byte[] data)
        {
            // Begin sending the data to the remote device.
            client.Client.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        protected void SendCallback(IAsyncResult ar)
        {
            try
            {

                // Complete sending the data to the remote device.
                int bytesSent = client.Client.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
