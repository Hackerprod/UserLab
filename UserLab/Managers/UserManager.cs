using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SKYNET.Models;

namespace SKYNET.Managers
{
    public partial class UserManager
    {
        public UserManager()
        {
        }
        public User Authenticate(string username, string password)
        {
            var user = AllUsers().SingleOrDefault(x => x.UserName.ToLower() == username.ToLower() && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            user.Token = GenerateToken(user);

            // remove password before returning
            user.Password = null;

            return user;
        }

        private string GenerateToken(User user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Startup.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.CI.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        internal User Create(RegisterModel register, string remoteIP)
        {
            User user = new User()
            {
                CI = register.CI,
                Name = register.Name,
                UserName = register.UserName,
                Password = register.Password,
                Phone = register.Phone,
                Role = "Registered",
                Devices = new List<Device>(),
                Services = new List<Service>()
                  
            };
            if (IPAddress.TryParse(remoteIP, out _))
            {
                user.IPAddress = remoteIP;
            }

            Add(user);
            return user;
        }

        //public User Update(uint userId, UserUpdate user)
        //{
        //    User _User = Get(userId);
        //    if (_User == null)
        //    {
        //        return null;
        //    }
        //    _User.Email = user.Email;
        //    _User.FirstName = user.FirstName;
        //    _User.LastName = user.LastName;
        //    _User.NickName = user.NickName;
        //    _User.Language = user.Language;
        //    _User.Password = user.Password;
        //    Update(_User);
        //    return _User;
        //}
    }
    }
