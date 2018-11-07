using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Information_Network
{
    class RandomData
    {
        Random random = new Random();

        public ushort GetPressure() => Convert.ToUInt16(random.Next(999));

        public ushort GetLightning() => Convert.ToUInt16(random.Next(9999));

        public sbyte GetTemperatue()
        {
            switch (random.Next(1))
            {
                case 0:return Convert.ToSByte(-random.Next(80));
                case 1:return Convert.ToSByte(random.Next(80));
            }
            return 0;
        }

        public byte GetHumidity() => Convert.ToByte(random.Next(100));

        public bool GetFire()
        {
            switch (random.Next(1))
            {
                case 0: return false;
                case 1: return true;
            }
            return false;
        }
    }
}