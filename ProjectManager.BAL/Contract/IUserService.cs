using ProjectManager.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BAL
{
    public interface IUserService
    {
        UserEntity GetUserById(int userId);
        IEnumerable<UserEntity> GetAllUsers();
        int CreateUsers(UserEntity userEntity);
        bool UpdateUser(int userId, UserEntity userEntity);
        bool DeleteUser(int userId);
    }
}
