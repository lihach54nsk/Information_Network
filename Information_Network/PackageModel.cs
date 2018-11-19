using System;

namespace MeshNetworkServerGUI
{
    class PackageModel
    {
        public long PackageModelId { get; set; }
        public long PackageId { get; set; }
        public int NodeId { get; set; }
        public DateTime Time { get; set; }
        public int? Pressure { get; set; }
        public int? Lighting { get; set; }
        public short? Temperature { get; set; }
        public byte? Humidity { get; set; }
        public bool? IsFire { get; set; }
    }
}
