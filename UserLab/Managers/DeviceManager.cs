using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using SKYNET.Models;

namespace SKYNET.Managers
{
    public partial class DeviceManager
    {
        public IMongoCollection<Device> Devices;

        public List<Device> AllDevices()
        {
            return Devices.Find(FilterDefinition<Device>.Empty, null).ToList();
        }

        public Device Get(uint id)
        {
            return Devices.Find<Device>(u => u.ID == id).FirstOrDefault();
        }
        public Device Get(string name)
        {
            return Devices.Find<Device>(u => u.Model == name).FirstOrDefault();
        }

        public bool Add(DeviceModel dev)
        {
            try
            {
                Device Device = new Device()
                {
                    ID = CreateDeviceId(),
                    Model = dev.Model,
                    IPAddress = dev.IPAddress,
                    MAC = dev.MAC
                };
                Devices.InsertOne(Device);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Add(string Model)
        {

            Device s = Get(Model);
            if (s == null)
            {
                s = new Device()
                {
                    ID = CreateDeviceId(),
                    Model = Model
                };
                Devices.InsertOne(s);
                return true;
            }
            return false;
        }

        public void Update(ulong id, Device uIn)
        {
            Devices.ReplaceOne(u => u.ID == id, uIn);
        }
        public void Update(Device uIn)
        {
            Devices.ReplaceOne(u => u.ID == uIn.ID, uIn);
        }

        public bool Remove(string Model)
        {
            var result = Devices.DeleteOne(u => u.Model == Model);
            return result.DeletedCount != 0;
        }

        public void Remove(ulong id)
        {
            Devices.DeleteOne(u => u.ID == id);
        }
        private uint CreateDeviceId()
        {
            var num = Devices.Find(FilterDefinition<Device>.Empty, null).SortByDescending((Device u) => (object)u.ID).Project((Device u) => u.ID).FirstOrDefault(default(CancellationToken));
            if (num <= 0U)
            {
                return 1U;
            }
            return num + 1;
        }
    }

}
