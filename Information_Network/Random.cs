using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Information_Network
{
    class Random
    {
        Random random = new Random();

        public ushort GetPressure() => Convert.ToUInt16(random.Next(999));

        public ushort GetLightning() => Convert.ToInt16(random.Next(9999));

        public sbyte GetTemperatue()
        {
            //ask about it
        }

        public byte GetHumidity() => Convert.ToByte(random.Next(100));

        public bool GetFire()
        {
            switch (random.Next(1))
            {
                case 0: return false;
                case 1: return true;
            }
        }
    }
}