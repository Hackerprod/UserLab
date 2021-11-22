using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SKYNET.Models
{
    public class User
    {
        public ulong CI { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string IPAddress { get; set; }
        public string Role { get; set; }
        public List<Device> Devices { get; set; }
        public List<Service> Services { get; set; }
        public string Token { get; set; }
    }
}
