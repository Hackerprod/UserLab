using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKYNET.Models
{
    public class AuthResponseModel
    {
        public EResult Result { get; set; }
        public string AuthToken { get; set; }
    }
}
