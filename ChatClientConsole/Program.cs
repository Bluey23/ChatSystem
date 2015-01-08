using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClientConsole
{
    class Program
    {
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string clientName = "";

        static void Main(string[] args)
        {
            Console.Title = "Client";
            //Connects the client to the server
            Connect();

            //Background thread for receiving messages
            Thread ReceivingThread = new Thread(new ThreadStart(Receive));
            ReceivingThread.Start();

            //Sends messages to the server
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            while (true)
            {
                //Sending Text
                Console.WriteLine("Enter Text");
                string text = Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes(clientName + ": " + text);
                clientSocket.Send(buffer);                
            }
        }

        private static void Receive()
        {
            while (true)
            {
                //Receiveing Text
                byte[] receivedBuffer = new byte[1024];
                int dataReceived = clientSocket.Receive(receivedBuffer);
                byte[] data = new byte[dataReceived];
                Array.Copy(receivedBuffer, data, dataReceived);
                Console.WriteLine(Encoding.ASCII.GetString(data));
            }
        }

        private static void Connect()
        {
            while (!clientSocket.Connected)
            {
                try
                {
                    clientSocket.Connect(IPAddress.Loopback, 8888);

                    //Set Client Number
                    //Could Change so that the user can set their name
                    //Just Wanted a commit
                    byte[] receivedBuffer = new byte[1024];
                    int dataReceived = clientSocket.Receive(receivedBuffer);
                    byte[] data = new byte[dataReceived];
                    Array.Copy(receivedBuffer, data, dataReceived);
                    Console.WriteLine("Client: " + Encoding.ASCII.GetString(data));
                    clientName = Encoding.ASCII.GetString(data);
                }
                catch (SocketException)
                {
                }
            }

        }
    }
}
