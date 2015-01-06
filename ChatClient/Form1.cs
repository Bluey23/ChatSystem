using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace ChatClient
{
    public partial class Form1 : Form
    {

        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Form1()
        {
            InitializeComponent();
            LoopConnect();
            
            //recieve loop
            while (true)
            {
                byte[] receivedBuffer = new byte[1024];
                int rec = clientSocket.Receive(receivedBuffer);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuffer, data, rec);
                textBox2.Text = "Message" + Encoding.ASCII.GetString(data);
                Console.WriteLine("Hello");
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


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text;

            byte[] buffer = Encoding.ASCII.GetBytes(message);
            clientSocket.Send(buffer);
        }
    }
}
