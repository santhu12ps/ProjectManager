using ProjectManager.BAL;
using ProjectManager.BusinessEntities;
using ProjectManager.Logger;
using ProjectManagerAPI.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace ProjectManagerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ILoggerService _loggerService;

        #region Public Constructor  

        /// <summary>  
        /// Public constructor to initialize user service instance  
        /// </summary>  
        public UserController()
        {
            _userService = new UserService();
            _loggerService = new LoggerService();
        }

        #endregion

        // GET: api/User
        public HttpResponseMessage Get()
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : User - Method Name : GetAllUsers - Description : Method Begin", LoggerConstants.Info.APIInfo);
                var users = _userService.GetAllUsers();
                if (users != null)
                {
                    var userEntities = users as List<UserEntity> ?? users.ToList();
                    if (userEntities.Any())
                        return Request.CreateResponse(HttpStatusCode.OK, userEntities);
                }
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Users not found");
        }

        // GET: api/User/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : User - Method Name : GetUserById - Description : Method Begin", LoggerConstants.Info.APIInfo);
                var user = _userService.GetUserById(id);
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, user);

                throw new ApiDataException(1001, "No user found for this id.", HttpStatusCode.NotFound);

            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No user found for this id");
        }

        // POST: api/User
        [ResponseType(typeof(UserEntity))]
        public IHttpActionResult PostUser(UserEntity userEntity)
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : User - Method Name : CreateUser - Description : Method Begin", LoggerConstants.Info.APIInfo);
                _userService.CreateUsers(userEntity);
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return CreatedAtRoute("DefaultApi", new { id = userEntity.User_ID }, userEntity);
        }

        // PUT: api/User/5
        public bool Put(int id, UserEntity userEntity)
        {
            try
            {
                if (id > 0)
                {
                    _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : User - Method Name : UpdateUser - Description : Method Begin", LoggerConstants.Info.APIInfo);
                    return _userService.UpdateUser(id, userEntity);
                }
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return false;
        }

        // DELETE: api/User/5
        public bool Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : User - Method Name : DeleteUser - Description : Method Begin", LoggerConstants.Info.APIInfo);
                    return _userService.DeleteUser(id);
                }
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return false;
        }
    }

}
