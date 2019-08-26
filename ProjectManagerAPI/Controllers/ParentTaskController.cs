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
    public class ParentTaskController : ApiController
    {
        private readonly IParentTaskService _parentTaskService;
        private readonly ILoggerService _loggerService;

        public ParentTaskController()
        {
            _parentTaskService = new ParentTaskService();
            _loggerService = new LoggerService();
        }
        // GET: api/ParentTask
        public HttpResponseMessage Get()
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : ParentTask - Method Name : GetAllParentTasks - Description : Method Begin", LoggerConstants.Info.APIInfo);

                var tasks = _parentTaskService.GetAllParentTasks();
                if (tasks != null)
                {
                    var taskEntities = tasks as List<ParentTaskEntity> ?? tasks.ToList();
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

        // GET: api/ParentTask/5
        public string Get(int id)
        {
            return "values";
        }

        // POST: api/ParentTask
        public int Post([FromBody]ParentTaskEntity parentTaskEntity)
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : ParentTask - Method Name : CreateParentTask - Description : Method Begin", LoggerConstants.Info.APIInfo);

                return _parentTaskService.CreateParentTask(parentTaskEntity);
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return 0;
        }

        // PUT: api/ParentTask/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ParentTask/5
        public void Delete(int id)
        {
        }
    }

}
