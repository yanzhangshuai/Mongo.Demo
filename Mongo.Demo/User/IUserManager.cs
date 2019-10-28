using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Demo.User
{
    public interface IUserManager
    {
        IList<User> GetUsers();
        long GetCount();
        Task<IList<User>> GetUsers2();

        Task Inert();
    }
}