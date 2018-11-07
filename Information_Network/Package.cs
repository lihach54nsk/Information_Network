using System;
using System.Collections.Generic;
using System.Text;

namespace MeshNetworkServer
{
    struct Package
    {
        public uint PackageId { get; set; }
        public ushort NodeId { get; set; }
        public DateTime Time { get; set; }
        public ushort? Pressure { get; set; }
        public ushort? Lighting { get; set; }
        public sbyte? Temperature { get; set; }
        public byte? Humidity { get; set; }
        public bool? IsFire { get; set; }

        private const int bufferSize = 22;

        public byte[] ToBinary(byte[] pool)
        {
            if (pool.Length < bufferSize)
            {
                throw new ArgumentException();
            }

            var flags = SensorFlags.None;

            WriteDataToArray(pool, BitConverter.GetBytes(PackageId), 0, 4);
            WriteDataToArray(pool, BitConverter.GetBytes(NodeId), 4, 2);
            WriteDataToArray(pool, BitConverter.GetBytes(Time.Ticks), 6, 8);

            if (Pressure != null)
            {
                WriteDataToArray(pool, BitConverter.GetBytes(Pressure.Value), 14, 2);
                flags |= SensorFlags.Pressure;
            }

            if (Lighting != null)
            {
                WriteDataToArray(pool, BitConverter.GetBytes(Lighting.Value), 16, 2);
                flags |= SensorFlags.Lighting;
            }

            if (Temperature != null)
            {
                pool[18] = (byte)Temperature.Value;
                flags |= SensorFlags.Temperature;
            }

            if (Humidity != null)
            {
                pool[19] = Humidity.Value;
                flags |= SensorFlags.Humidity;
            }

            if (IsFire != null)
            {
                pool[20] = (byte)(IsFire.Value ? 1 : 0);
                flags |= SensorFlags.IsFire;
            }

            pool[21] = (byte)flags;

            return pool;
        }

        public static Package FromBinary(byte[] buffer)
        {
            if (buffer.Length < bufferSize)
            {
                throw new ArgumentException("Buffer must be greater than 22");
            }

            var data = new Package();
            var flags = (SensorFlags)buffer[21];

            data.PackageId = BitConverter.ToUInt32(buffer, 0);
            data.NodeId = BitConverter.ToUInt16(buffer, 4);
            data.Time = new DateTime(BitConverter.ToInt64(buffer, 6));

            if ((byte)(flags & SensorFlags.Pressure) != 0)
            {
                data.Pressure = BitConverter.ToUInt16(buffer, 14);
            }

            if ((byte)(flags & SensorFlags.Lighting) != 0)
            {
                data.Lighting = BitConverter.ToUInt16(buffer, 16);
            }

            if ((byte)(flags & SensorFlags.Temperature) != 0)
            {
                data.Temperature = (sbyte)buffer[18];
            }

            if ((byte)(flags & SensorFlags.Humidity) != 0)
            {
                data.Humidity = buffer[19];
            }

            if ((byte)(flags & SensorFlags.IsFire) != 0)
            {
                data.IsFire = buffer[20] == 1;
            }

            return data;
        }

        private static void WriteDataToArray<T>(T[] destination, T[] source, int index, int length)
        {
            for (int i = 0; i < length; i++)
            {
                destination[index + i] = source[i];
            }
        }
    }

    [Flags]
    enum SensorFlags : byte
    {
        None = 0b00000000,
        Pressure = 0b00000001,
        Lighting = 0b00000010,
        Temperature = 0b00000100,
        Humidity = 0b00001000,
        IsFire = 0b00010000
    }
}