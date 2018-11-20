using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeshNetworkServer;
using System.Net;
using System.Net.Sockets;

namespace Information_Network
{
    class Program
    {
        static uint[] packID = new uint[100];
        static int packageCount = 0; // надо придумать, как перезаписывать массив с PackageId, не теряя то, что ещё актуально
        int ptrPack = 0; // надо придумать, как перезаписывать массив с PackageId, не теряя то, что ещё актуально
        static IPAddress[] iPAddresses = new IPAddress[100];
        static int[] portsToSend = new int[100];

        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество соседей");
            int c = Convert.ToInt32(Console.ReadLine());
            byte[] data = new byte[21];

            for (int i = 0; i < c; i++)
            {
                Console.WriteLine("Введите адрес соседа");
                IPAddress IPAddress = IPAddress.Parse(Console.ReadLine());
                iPAddresses[i] = IPAddress;
                Console.WriteLine("Введите порт прослушки");
                var portListen = Convert.ToInt32(Console.ReadLine());
                portsToSend[i] = portListen;
                Console.WriteLine("Введите порт отправки");                
                var portSend = Convert.ToInt32(Console.ReadLine());
                Package pack = SetData();
                var packBin = pack.ToBinary(data);
                var taskSend = Task.Factory.StartNew
                    (() => SocketSenderUDP(IPAddress, portSend, packBin));
               /* var taskListen = Task.Factory.StartNew
                    (() => { SocketListenerUDP(IPAddress, portListen); });*/
            }

            while (true)
            {
                Console.WriteLine("0 - выход, остальное  - отправка");
                if (Console.ReadLine() != "0") { SendData(SetData().ToBinary(data)); }
                else break;
            }
        }

        static void SendData(byte[] data)
        {
            int i = 0;
            while (iPAddresses[i] != null) { SocketSenderUDP(iPAddresses[i], portsToSend[i], data); i++; }
            Console.WriteLine("Отправил куче народа!");
        }

        public static void SocketListenerUDP(IPAddress iPAddress, int port) // прослушка и отправка пришедших данных
        {
            UdpClient listener = new UdpClient();

            IPEndPoint endPoint = new IPEndPoint(iPAddress, port);

            Socket socket = new Socket(SocketType.Dgram, ProtocolType.Udp);

            byte[] data = new byte[21];

            socket.Bind(endPoint);

            while (true)
            {
                socket.Receive(data);
                Console.WriteLine("Принял!");
                if (CheckPackID(data) == true) continue;
                else SendData(data);
            }
        }

        public static void SocketSenderUDP(IPAddress iPAddress, int port, byte[] data) // отправка данных
        {
            UdpClient sender = new UdpClient();
            Socket socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPoint = new IPEndPoint(iPAddress, port);

            try
            {
                int i = 0;
                while (packID[i] != 0)
                {
                    i++;
                }

                packID[i] = data[0];
                packageCount++;

                socket.SendTo(data, endPoint);

                Console.WriteLine("Отправил по адресу");
            }
            catch
            {
                Console.WriteLine("Что-то пошло не так!");
            }

            sender.Close();
        }

        static bool CheckPackID(byte[] listen)
        {
            var check = Package.FromBinary(listen);

            int i = 0;

            while (packID[i] != 0)
            {
                if (packID[i] == check.PackageId) return true;
                else i++;
            }

            packID[i] = check.PackageId;
            packageCount++;
            return false;
        }

        /* public void SocketListener(IPAddress iPAddress, int port) // прослушка и отправка пришедших данных
         {
             IPEndPoint endPoint = new IPEndPoint(iPAddress, port);
             socket.Bind(endPoint);
             socket.Listen(10);

             while (true)
             {
                 var connect = socket.Accept();
                 byte[] buffer = new byte[22];
                 var dataFrom = socket.Receive(buffer);
                 connect.Send(buffer);
                 connect.Shutdown(SocketShutdown.Both);
                 connect.Close();
             }
         }*/

        /*public void SocketSender(IPAddress iPAddress, int port) // отправка данных
        {
            IPEndPoint endPoint = new IPEndPoint(iPAddress, port);

            socket.Connect(endPoint);
            socket.Send(package.ToBinary(data));
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }*/

        static Package pack = new Package();

        static Package SetData()
        {
            RandomData randomData = new RandomData();
            Random random = new Random();
            pack.PackageId = Convert.ToUInt32(random.Next(1, 5)); // создать очередь, пока заглушка
            pack.NodeId = 5; // жду конфиг
            pack.Time = DateTime.Now;
            pack.Humidity = randomData.GetHumidity();
            pack.IsFire = randomData.GetFire();
            pack.Lighting = randomData.GetLightning();
            pack.Pressure = randomData.GetPressure();
            pack.Temperature = randomData.GetTemperatue();

            return pack;
        }
    }
}