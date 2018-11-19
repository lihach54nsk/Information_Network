using MeshNetworkServer;
using System;

namespace MeshNetworkServerGUI
{
    static class PackageConverter
    {
        public static Package ToPackage(PackageModel packageModel)
        {
            var data = new Package
            {
                PackageId = Convert.ToUInt32(packageModel.PackageId),
                NodeId = Convert.ToUInt16(packageModel.NodeId),
                Time = packageModel.Time,
                Humidity = packageModel.Humidity,
                IsFire = packageModel.IsFire,
            };

            if (packageModel.Pressure == null)
            {
                data.Pressure = Convert.ToUInt16(packageModel.Pressure.Value);
            }

            if (packageModel.Lighting == null)
            {
                data.Lighting = Convert.ToUInt16(packageModel.Lighting.Value);
            }

            if (packageModel.Temperature == null)
            {
                data.Temperature = Convert.ToSByte(packageModel.Temperature.Value);
            }

            return data;
        }

        public static PackageModel ToPackageModel(Package package)
        {
            return new PackageModel
            {
                PackageModelId = 0,
                PackageId = package.PackageId,
                NodeId = package.NodeId,
                Time = package.Time,
                Pressure = package.Pressure,
                Lighting = package.Lighting,
                Temperature = package.Temperature,
                Humidity = package.Humidity,
                IsFire = package.IsFire,
            };
        }
    }
}
