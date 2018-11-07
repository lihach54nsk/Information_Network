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
        byte[] data = new byte[22];

        Package package = new Package();    

        void Main(string[] args)
        {
            // работа через Task
        }

        Socket socket;

        public void SocketListener(IPAddress iPAddress, int port) // прослушка и отправка пришедших данных
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
        }

        public void SocketSender(IPAddress iPAddress,int port) // отправка данных
        {
            IPEndPoint endPoint = new IPEndPoint(iPAddress, port);

            socket.Connect(endPoint);
            socket.Send(package.ToBinary(data));
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        void SetData()
        {
            RandomData randomData = new RandomData();
            DateTime dateTime = new DateTime();
            //package.PackageId = // создать очередь
            package.NodeId = 1; // жду конфиг
            //package.Time = dateTime // сейчас
            package.Humidity = randomData.GetHumidity();
            package.IsFire = randomData.GetFire();
            package.Lighting = randomData.GetLightning();
            package.Pressure = randomData.GetPressure();
            package.Temperature = randomData.GetTemperatue();

            data = package.ToBinary(data);
        }
    }
}