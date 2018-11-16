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
        uint[] packID = new uint[1000];
        int packageCount = 0;
        IPAddress[] iPAddresses = new IPAddress[100];
        int[] portsToSend = new int[100];

        Socket socket;

        void Main(string[] args)
        {
            Console.WriteLine("Введите количество соседей");
            int c = Convert.ToInt32(Console.ReadLine());
            byte[] data = new byte[22];

            for (int i = 0; i < c; i++)
            {
                Console.WriteLine("Введите адрес соседа");
                IPAddress IPAddress = IPAddress.Parse(Console.ReadLine());
                iPAddresses[i] = IPAddress;
                Console.WriteLine("Введите порт работы");
                var port = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите порт соседа");
                portsToSend[i] = port;
                var portEnd = Convert.ToInt32(Console.ReadLine());
                var taskListen = Task.Factory.StartNew
                    (() => { SocketListenerUDP(IPAddress, portEnd); });
                var taskSend = Task.Factory.StartNew
                    (() => SocketSenderUDP(IPAddress, port, SetData().ToBinary(data)));
            }
            // работа через Task
        }

        void SendData(byte[] data)
        {
            int i = 0;
            while (iPAddresses[i] != null) SocketSenderUDP(iPAddresses[i], portsToSend[i], data);
        }

        public void SocketListenerUDP(IPAddress iPAddress, int port) // прослушка и отправка пришедших данных
        {
            UdpClient listener = new UdpClient();

            IPEndPoint endPoint = new IPEndPoint(iPAddress, port);

            try
            {
                while (true)
                {
                    byte[] listen = listener.Receive(ref endPoint);

                    if (CheckPackID(listen) == true) continue;
                    else SendData(listen);
                }
            }
            catch
            {
                Console.WriteLine("Миша, всё хуйня, давай Пановой!");
            }

            listener.Close();
        }

        public void SocketSenderUDP(IPAddress iPAddress, int port, byte[] data) // отправка данных
        {
            UdpClient sender = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(iPAddress, port);
            Package package = new Package();

            try
            {
                sender.Send(package.ToBinary(data), data.Length, endPoint);
                int i = 0;

                while (packID[i] != 0)
                {
                    i++;
                }

                packID[i] = package.PackageId;
                packageCount++;
            }
            catch
            {
                Console.WriteLine("Миша, всё хуйня, давай Пановой!");
            }

            sender.Close();
        }

        bool CheckPackID(byte[] listen)
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

        Package SetData()
        {
            Package package = new Package();
            RandomData randomData = new RandomData();
            Random random = new Random();
            package.PackageId = Convert.ToUInt32(random.Next(0, 100)); // создать очередь, пока заглушка
            package.NodeId = 1; // жду конфиг
            package.Time = DateTime.Now;
            package.Humidity = randomData.GetHumidity();
            package.IsFire = randomData.GetFire();
            package.Lighting = randomData.GetLightning();
            package.Pressure = randomData.GetPressure();
            package.Temperature = randomData.GetTemperatue();

            return package;
        }
    }
}