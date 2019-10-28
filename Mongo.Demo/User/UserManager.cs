using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mongo.Demo.Core;
using MongoDB.Driver;

namespace Mongo.Demo.User
{
    public class UserManager : IUserManager
    {
        private readonly IMongoRepository<Demo.User.User, int> _flcoudHistoryAlarmRepository;

        public UserManager(IMongoRepository<Demo.User.User, int> flcoudHistoryAlarmRepository)
        {
            _flcoudHistoryAlarmRepository = flcoudHistoryAlarmRepository;
        }

        public IList<Demo.User.User> GetUsers()
        {
            var query = _flcoudHistoryAlarmRepository.GetAll();
//                .WhereIf(args.DeviceId > 0, alarm => alarm.DeviceId == args.DeviceId)
//                .Where(alarm => alarm.State == EnumAlarmState.Triggered);
            return query.ToList();
        }

        public long GetCount()
        {
            return _flcoudHistoryAlarmRepository.Count();
        }

        public async Task<IList<Demo.User.User>> GetUsers2()
        {
            var filter = Builders<Demo.User.User>.Filter.Eq(y => y.Name, "张三");
            var query = await _flcoudHistoryAlarmRepository.GetAllListAsync(filter);
            return query;
        }

        public async Task Inert()
        {
            var users = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                users.Add(new User()
                {
                    Id = i,
                    Name = "张三" + i,
                    Address = "邯郸",
                    BornDateTime = DateTime.Now,
                    Phone = new Random().Next(0, 9999999).ToString()
                });
            }

            await _flcoudHistoryAlarmRepository.InsertManyAsync(users);
        }
    }
}