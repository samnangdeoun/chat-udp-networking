using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatUDP
{
    class Program
    {
        static int RemotePort;
        static int LocalPort;
        static IPAddress RemoteIPAddress;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(60, 20);
                Console.Title = ("ChatUPP");
                Console.Write("Enter Remote IP Address: ");
                RemoteIPAddress = IPAddress.Parse(Console.ReadLine());
                Console.Write("Enter Remote Port: ");
                RemotePort = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter Local Port: ");
                LocalPort = Convert.ToInt32(Console.ReadLine());

                Thread thread = new Thread(new ThreadStart(ThreadFuncReceive));
                thread.IsBackground= true;
                thread.Start();

                Console.ForegroundColor = ConsoleColor.Blue;
                while(true)
                {
                    SendData(Console.ReadLine());
                }
            }
            catch (FormatException formatEx)
            {
                Console.WriteLine("Conversion Impossible: " + formatEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

        }

        static void ThreadFuncReceive()
        {
            try
            {
                while (true)
                {
                    //Connecting to the local host
                    UdpClient udpClient = new UdpClient(LocalPort);
                    IPEndPoint localEndPoint = null;
                    byte[] response = udpClient.Receive(ref localEndPoint);
                    string strResult = Encoding.Unicode.GetString(response);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(strResult);
                    Console.ForegroundColor= ConsoleColor.Blue;
                    udpClient.Close();
                }
            }
            catch(SocketException sockEx) 
            {
                Console.WriteLine("Socket Exception: " + sockEx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        static void SendData(string datagram)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint remoteEndPoint = new IPEndPoint(RemoteIPAddress, RemotePort);
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(datagram);
                udpClient.Send(bytes, bytes.Length, remoteEndPoint);

            }
            catch(SocketException sockEx)
            {
                Console.WriteLine("Socket Exception: " + sockEx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}
