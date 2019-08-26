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

namespace ProjectManagerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class ProjectController : ApiController
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly ILoggerService _loggerService;


        #region Public Constructor  

        /// <summary>  
        /// Public constructor to initialize project service instance  
        /// </summary>  
        public ProjectController()
        {
            _projectService = new ProjectService();
            _userService = new UserService();
            _loggerService = new LoggerService();
        }

        #endregion

        // GET: api/Project
        public HttpResponseMessage Get()
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Project - Method Name : GetAllProjects - Description : Method Begin", LoggerConstants.Info.APIInfo);
                var projects = _projectService.GetProjectsSearch();
                if (projects != null)
                {
                    var projectEntities = projects as List<view_ProjectSearchEntity> ?? projects.ToList();
                    if (projectEntities.Any())
                        return Request.CreateResponse(HttpStatusCode.OK, projectEntities);
                }
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Projects not found");
        }

        // GET: api/Project/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                if (id > 0)
                {
                    _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Project - Method Name : GetProjectById - Description : Method Begin", LoggerConstants.Info.APIInfo);
                    var project = _projectService.GetProjectById(id);
                    if (project != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, project);
                    }
                    throw new ApiDataException(1001, "No project found for this id.", HttpStatusCode.NotFound);
                }
                else
                    throw new ApiException() { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = "Bad Request..." };
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No project found for this id");
        }

        // POST api/project  
        public int Post(ProjectEntity projectEntity)
        {
            try
            {
                _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Project - Method Name : CreateProject - Description : Method Begin", LoggerConstants.Info.APIInfo);

                int projectId = _projectService.CreateProject(projectEntity);

                var user = _userService.GetUserById(projectEntity.Manager_ID);
                user.Project_ID = projectId;

                if (projectEntity.Manager_ID != 0)
                    _userService.UpdateUser(projectEntity.Manager_ID, user);
                return projectId;
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return 0;

        }

        // PUT api/project/5  
        public bool Put(int id, ProjectEntity projectEntity)
        {
            try
            {
                if (id > 0)
                {
                    _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Project - Method Name : UpdateProject - Description : Method Begin", LoggerConstants.Info.APIInfo);
                    bool returnStatus = _projectService.UpdateProject(id, projectEntity);
                    if (projectEntity.Manager_ID != 0)
                    {
                        var user = _userService.GetUserById(projectEntity.Manager_ID);
                        user.Project_ID = projectEntity.Project_ID;

                        if (projectEntity.Manager_ID != 0)
                            _userService.UpdateUser(projectEntity.Manager_ID, user);
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

        // DELETE api/project/5  
        public bool Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    _loggerService.LogInfo("InfoCode: API Info - Message :" + "Controller Name : Project - Method Name : DeleteProject - Description : Method Begin", LoggerConstants.Info.APIInfo);
                    var success = _projectService.DeleteProject(id);

                    if (success)
                    {
                        return true;
                    }
                    throw new ApiDataException(1002, "Project is already deleted or not exist in system.", HttpStatusCode.NoContent);
                }
                else
                    throw new ApiException() { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = "Bad Request..." };
            }
            catch (Exception exception)
            {
                _loggerService.LogException(exception, LoggerConstants.Info.APIInfo);
            }
            return false;
        }
    }

}
