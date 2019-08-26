using ProjectManager.BAL;
using ProjectManager.BusinessEntities;
using ProjectManager.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ProjectManagerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class TaskController : ApiController
    {
        private readonly ITaskService _taskService;
        private readonly ILoggerService _loggerService;
        private readonly IUserService _userService;


        #region Public Constructor  

        /// <summary>  
        /// Public constructor to initialize user service instance  
        /// </summary>  
        public TaskController()
        {
            _taskService = new TaskService();
            _userService = new UserService();
            _loggerService = new LoggerService();
        }

        #endregion
        // GET: api/Task
        public HttpResponseMessage Get()
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Task - Method Name : GetAllTasks - Description : Method Begin", LoggerConstants.Info.APIInfo);
                var tasks = _taskService.GetTaskSearch();
                if (tasks != null)
                {
                    var taskEntities = tasks as List<view_TaskSearchEntity> ?? tasks.ToList();
                    if (taskEntities.Any())
                        return Request.CreateResponse(HttpStatusCode.OK, taskEntities);
                }
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tasks not found");
        }

        // GET: api/Task/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Task - Method Name : GetTaskById - Description : Method Begin", LoggerConstants.Info.APIInfo);
                var task = _taskService.GetTaskById(id);
                if (task != null)
                    return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No user found for this id");
        }

        // POST: api/Task
        public int Post([FromBody]TaskEntity taskEntity)
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Task - Method Name : CreateTask - Description : Method Begin", LoggerConstants.Info.APIInfo);
                int iTaskID = _taskService.CreateTask(taskEntity);
                if (taskEntity.User_ID != null)
                {
                    int iUserID = Convert.ToInt32(taskEntity.User_ID);
                    var user = _userService.GetUserById(iUserID);
                    user.Task_ID = iTaskID;

                    if (taskEntity.User_ID != 0)
                        _userService.UpdateUser(iUserID, user);
                }
                return iTaskID;
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return 0;
        }

        // PUT: api/Task/5
        public bool Put(int id, [FromBody]TaskEntity taskEntity)
        {
            try
            {
                if (id > 0)
                {
                    _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Task - Method Name : UpdateTask - Description : Method Begin", LoggerConstants.Info.APIInfo);
                    bool returnStatus = _taskService.UpdateTask(id, taskEntity);

                    if (taskEntity.User_ID != null)
                    {
                        var user = _userService.GetUserById(Convert.ToInt32(taskEntity.User_ID));
                        user.Task_ID = taskEntity.Task_ID;

                        if (taskEntity.User_ID > 0)
                            _userService.UpdateUser(Convert.ToInt32(taskEntity.User_ID), user);
                    }
                    return returnStatus;
                }
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return false;
        }

        // DELETE: api/Task/5
        public bool Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Task - Method Name : DeleteTask(Update Task Status) - Description : Method Begin", LoggerConstants.Info.APIInfo);
                    var task = _taskService.GetTaskById(id);
                    if (task != null)
                    {
                        task.Status = "Completed";
                    }
                    return _taskService.UpdateTask(id, task);
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
