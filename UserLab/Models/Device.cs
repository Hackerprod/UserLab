using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SKYNET.Models
{
    public class Device
    {
        public uint ID { get; set; }
        public string Model { get; set; }
        public string IPAddress { get; set; }
        public string MAC { get; set; }

    }
}
