using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClientConsole
{
    class Program
    {
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Title = "Client";
            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            while (true)
            {
                Console.Write("Enter Text");
                string text = Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                clientSocket.Send(buffer);

                byte[] receivedBuffer = new byte[1024];
                int dataReceived = clientSocket.Receive(receivedBuffer);
                byte[] data = new byte[dataReceived];
                Array.Copy(receivedBuffer, data, dataReceived);
                Console.WriteLine("Message: " + Encoding.ASCII.GetString(data));
            }
        }

        private static void LoopConnect()
        {
            int attempts = 0;

            while (!clientSocket.Connected)
            {
                attempts++;
                try
                {
                    clientSocket.Connect(IPAddress.Loopback, 8888);
                }
                catch (SocketException)
                {
                    Console.WriteLine("attempts" + attempts);
                }
            }

        }
    }
}
