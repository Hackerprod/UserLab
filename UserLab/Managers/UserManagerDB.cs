using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using SKYNET.Models;

namespace SKYNET.Managers
{
    public partial class UserManager
    {
        public IMongoCollection<User> Users;

        public List<User> AllUsers()
        {
            return Users.Find(FilterDefinition<User>.Empty, null).ToList();
        }

        public User Get(ulong id)
        {
            return Users.Find<User>(u => u.CI == id).FirstOrDefault();
        }

        public User Add(User u)
        {
            Users.InsertOne(u);
            return u;
        }

        public void Update(ulong id, User uIn)
        {
            Users.ReplaceOne(u => u.CI == id, uIn);
        }
        public void Update(User uIn)
        {
            Users.ReplaceOne(u => u.CI == uIn.CI, uIn);
        }
        public void Update(User uIn, ulong userId)
        {
            Users.ReplaceOne(u => u.CI == userId, uIn);
        }
        internal void AddService(ulong userId, Service service)
        {
            User user = Get(userId);
            if (user != null)
            {
                if (!user.Services.Contains(service))
                {
                    user.Services.Add(service);
                }
                Update(user);
            }
        }
        internal void AddDevice(ulong userId, Device device)
        {
            User user = Get(userId);
            if (user != null)
            {
                if (!user.Devices.Contains(device))
                {
                    user.Devices.Add(device);
                }
                Update(user);
            }
        }
        internal void RemoveService(ulong userId, uint serviceID)
        {
            User user = Get(userId);
            if (user != null)
            {
                var s = user.Services.Find(_s => _s.ID == serviceID);
                if (s != null)
                {
                    user.Services.Remove(s);
                    Update(user);
                }
            }
        }
        internal void RemoveDevice(ulong userId, uint deviceID)
        {
            User user = Get(userId);
            if (user != null)
            {
                var d = user.Devices.Find(_d => _d.ID == deviceID);
                if (d != null)
                {
                    user.Devices.Remove(d);
                    Update(user);
                }
            }
        }
        public void Remove(User uIn)
        {
            Users.DeleteOne(u => u.CI == uIn.CI);
        }

        public void Remove(ulong id)
        {
            Users.DeleteOne(u => u.CI == id);
        }
        private ulong CreateUserId()
        {
            var num = Users.Find(FilterDefinition<User>.Empty, null).SortByDescending((User u) => (object)u.CI).Project((User u) => u.CI).FirstOrDefault(default(CancellationToken));
            if (num <= 0U)
            {
                return 1U;
            }
            return num + 1;
        }
    }

}
