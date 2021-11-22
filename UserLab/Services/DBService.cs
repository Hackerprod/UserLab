using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SKYNET.DB;
using SKYNET.Managers;
using SKYNET.Models;

namespace SKYNET.Services
{
    public class DBService
    {
        public UserManager Users;
        public ServiceManager Services;
        public DeviceManager Devices;
        
        [Obsolete]
        public DBService()
        {
            Users = new UserManager();
            Services = new ServiceManager();
            Devices = new DeviceManager();

            SetIgnoreConvention();

            var client = new MongoClient("mongodb://127.0.0.1:28005");
            var database = client.GetDatabase("SKYNET_NetControl");

            Users.Users = database.GetCollection<User>("SKYNET_Users");
            Users.Users.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending((User i) => i.CI));
            Users.Users.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending((User i) => i.Name));
            Users.Users.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending((User i) => i.UserName));
            Users.Users.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending((User i) => i.IPAddress));

            Services.Services = database.GetCollection<Service>("SKYNET_Services");
            Services.Services.Indexes.CreateOne(Builders<Service>.IndexKeys.Ascending((Service i) => i.ID));
            Services.Services.Indexes.CreateOne(Builders<Service>.IndexKeys.Ascending((Service i) => i.Name));

            Devices.Devices = database.GetCollection<Device>("SKYNET_Services");
            Devices.Devices.Indexes.CreateOne(Builders<Device>.IndexKeys.Ascending((Device i) => i.ID));
            Devices.Devices.Indexes.CreateOne(Builders<Device>.IndexKeys.Ascending((Device i) => i.Model));
            Devices.Devices.Indexes.CreateOne(Builders<Device>.IndexKeys.Ascending((Device i) => i.MAC));
            Devices.Devices.Indexes.CreateOne(Builders<Device>.IndexKeys.Ascending((Device i) => i.IPAddress));

            /********************** CREATE BASIC SERVICES **********************/
            Services.Add("TinoRED");
            Services.Add("Nauta");
            Services.Add("Paquete semanal");           

        }
        private void SetIgnoreConvention()
        {
            ConventionPack conventionPack = new ConventionPack
            {
                new MongoIngoreConvention(),
                new IgnoreExtraElementsConvention(ignoreExtraElements: true),
                new UnsignedConventions(),
            };
            conventionPack.AddClassMapConvention("IgnoreExtraElements", delegate (BsonClassMap map)
            {
                map.SetIgnoreExtraElements(ignoreExtraElements: true);
            });
            ConventionRegistry.Register("Ignores", conventionPack, (Type t) => true);

        }



    }

}
