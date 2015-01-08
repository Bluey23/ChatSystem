using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace ChatServer
{
    class Program
    {

        //The Server's Socket
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Client Sockets
        private static List<Socket> clientSockets = new List<Socket>();

        private static byte[] buffer = new byte[1024];

        static void Main(string[] args)
        {
            SetupServer();
            Console.ReadLine();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up Server....");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
            serverSocket.Listen(1);

            //Begins to receive Client requests
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            //Accepts a new Client
            Socket socket = serverSocket.EndAccept(AR);
            Console.WriteLine("Client Connecting");
            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            byte[] data = Encoding.ASCII.GetBytes(clientSockets.Count.ToString());
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

            //Continues to receive new client requests
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void RecieveCallback(IAsyncResult ar)
        {
            //Receive Messages from clients 
            Socket socket = (Socket)ar.AsyncState;
            int received = socket.EndReceive(ar);
            byte[] dataBuffer = new byte[received];
            Array.Copy(buffer, dataBuffer, received);

            string text = Encoding.ASCII.GetString(dataBuffer);
            Console.WriteLine("Recieved: " + text);

            //Sends recieved messages to other clients
            SendText(text, clientSockets);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            
        }

        private static void SendText(string text, List<Socket> sockets)
        {
            foreach(Socket socket in sockets)
            {
                byte[] data = Encoding.ASCII.GetBytes(text);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            //Sends data through network
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }


    }

   
}
