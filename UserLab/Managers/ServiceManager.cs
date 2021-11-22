using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using SKYNET.Models;

namespace SKYNET.Managers
{
    public partial class ServiceManager
    {
        public IMongoCollection<Service> Services;

        public List<Service> AllServices()
        {
            return Services.Find(FilterDefinition<Service>.Empty, null).ToList();
        }

        public Service Get(uint id)
        {
            return Services.Find<Service>(u => u.ID == id).FirstOrDefault();
        }
        public Service Get(string name)
        {
            return Services.Find<Service>(u => u.Name == name).FirstOrDefault();
        }

        public bool Add(ServiceModel name)
        {
            try
            {
                Service service = new Service()
                {
                    ID = CreateServiceId(),
                    Name = name.Name
                };
                Services.InsertOne(service);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Add(string Name)
        {

            Service s = Get(Name);
            if (s == null)
            {
                s = new Service()
                {
                    ID = CreateServiceId(),
                    Name = Name
                };
                Services.InsertOne(s);
                return true;
            }
            return false;
        }

        public void Update(ulong id, Service uIn)
        {
            Services.ReplaceOne(u => u.ID == id, uIn);
        }
        public void Update(Service uIn)
        {
            Services.ReplaceOne(u => u.ID == uIn.ID, uIn);
        }

        public bool Remove(string Name)
        {
            var result = Services.DeleteOne(u => u.Name == Name);
            return result.DeletedCount != 0;
        }

        public void Remove(ulong id)
        {
            Services.DeleteOne(u => u.ID == id);
        }
        private uint CreateServiceId()
        {
            var num = Services.Find(FilterDefinition<Service>.Empty, null).SortByDescending((Service u) => (object)u.ID).Project((Service u) => u.ID).FirstOrDefault(default(CancellationToken));
            if (num <= 0U)
            {
                return 1U;
            }
            return num + 1;
        }
    }

}
