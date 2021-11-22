using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKYNET.Models
{
    public class RegisterModel
    {
        public ulong CI { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
