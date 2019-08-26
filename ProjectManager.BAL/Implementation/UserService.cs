using AutoMapper;
using ProjectManager.BusinessEntities;
using ProjectManager.DAL;
using ProjectManager.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ProjectManager.BAL
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>  
        /// Public constructor.  
        /// </summary>  
        public UserService()
        {
            _unitOfWork = new UnitOfWork();
        }
        /// <summary>  
        /// Creates a user  
        /// </summary>  
        /// <param name="userEntity"></param>  
        /// <returns></returns>
        public int CreateUsers(UserEntity userEntity)
        {
            using (var scope = new TransactionScope())
            {
                var user = new User
                {
                    First_Name = userEntity.First_Name,
                    Last_Name = userEntity.Last_Name,
                    Employee_ID = userEntity.Employee_ID,
                    Project_ID = userEntity.Project_ID,
                    Task_ID = userEntity.Task_ID
                };
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();
                scope.Complete();
                return user.User_ID;
            }
        }
        /// <summary>  
        /// Deletes a particular user  
        /// </summary>  
        /// <param name="projectId"></param>  
        /// <returns></returns>
        public bool DeleteUser(int userId)
        {
            var success = false;
            if (userId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                        _unitOfWork.UserRepository.Delete(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        /// <summary>  
        /// Fetches all the users.  
        /// </summary>  
        /// <returns></returns>
        public IEnumerable<UserEntity> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll().ToList();
            if (users != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<User, UserEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var userModel = mapper.Map<List<User>, List<UserEntity>>(users);
                return userModel;
            }
            return null;
        }
        /// <summary>  
        /// Fetches user details by id  
        /// </summary>  
        /// <param name="userId"></param>  
        /// <returns></returns>  
        public UserEntity GetUserById(int userId)
        {
            var user = _unitOfWork.UserRepository.GetByID(userId);
            if (user != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<User, UserEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var userModel = mapper.Map<User, UserEntity>(user);
                return userModel;
            }
            return null;
        }
        /// <summary>  
        /// Updates a user  
        /// </summary>  
        /// <param name="userId"></param>  
        /// <param name="userEntity"></param>  
        /// <returns></returns>
        public bool UpdateUser(int userId, UserEntity userEntity)
        {
            var success = false;
            if (userEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                        user.First_Name = userEntity.First_Name;
                        user.Last_Name = userEntity.Last_Name;
                        user.Employee_ID = userEntity.Employee_ID;
                        user.Project_ID = userEntity.Project_ID;
                        user.Task_ID = userEntity.Task_ID;

                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }

}
