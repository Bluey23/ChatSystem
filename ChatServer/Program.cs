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
        static void Main(string[] args)
        {
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];

            TcpListener serverSocket = new TcpListener(ipAddress, 8888);

            serverSocket.Start();
            Console.WriteLine("Server Started");

            //client counter
            int counter = 0;

            while (true)
            {
                counter += 1;


                TcpClient clientSocket = serverSocket.AcceptTcpClient();

                // Read the data stream from the client. 
                byte[] bytes = new byte[256];
                NetworkStream stream = clientSocket.GetStream();
                stream.Read(bytes, 0, bytes.Length);
                
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                
                handleClinet client = new handleClinet();
                client.processMsg(clientSocket, stream, bytes);

                
                //client.StartClient(clientSocket, Convert.ToString(counter));
                
            }
        }

    }

    public class handleClinet
    {
        TcpClient clientSocket;
        string clientNo;
        string mstrMessage;
        string mstrResponse;
        byte[] bytesSent;

        public void StartClient(TcpClient inClientSocket, string clientNo)
        {
            this.clientSocket = inClientSocket;
            this.clientNo = clientNo;
            //Thread ctThread = new Thread(processMsg(clientSocket,);
            //ctThread.Start();
        }

        public void processMsg(TcpClient client, NetworkStream stream, byte[] bytesReceived)
        {
            // Handle the message received and  
            // send a response back to the client.
            mstrMessage = Encoding.ASCII.GetString(bytesReceived, 0, bytesReceived.Length);
            clientSocket = client;
            mstrMessage = mstrMessage.Substring(0, 5);
            if (mstrMessage.Equals("Hello"))
            {
                mstrResponse = "Goodbye";
            }
            else
            {
                mstrResponse = "What?";
            }
            bytesSent = Encoding.ASCII.GetBytes(mstrResponse);
            stream.Write(bytesSent, 0, bytesSent.Length);
        }
    }
}
