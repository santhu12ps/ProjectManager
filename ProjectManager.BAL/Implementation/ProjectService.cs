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
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>  
        /// Public constructor.  
        /// </summary>  
        public ProjectService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>  
        /// Fetches project details by id  
        /// </summary>  
        /// <param name="productId"></param>  
        /// <returns></returns>  
        public ProjectManager.BusinessEntities.ProjectEntity GetProjectById(int projectId)
        {
            var project = _unitOfWork.ProjectRepository.GetByID(projectId);
            if (project != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Project, ProjectEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var productModel = mapper.Map<Project, ProjectEntity>(project);
                return productModel;
            }
            return null;
        }

        /// <summary>  
        /// Fetches all the projects.  
        /// </summary>  
        /// <returns></returns>  
        public IEnumerable<ProjectEntity> GetAllProjects()
        {
            var projects = _unitOfWork.ProjectRepository.GetAll().ToList();
            if (projects.Any())
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Project, ProjectEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var projectsModel = mapper.Map<List<Project>, List<ProjectEntity>>(projects);
                return projectsModel;
            }
            return null;
        }

        /// <summary>  
        /// Creates a Project  
        /// </summary>  
        /// <param name="projectEntity"></param>  
        /// <returns></returns>  
        public int CreateProject(ProjectEntity projectEntity)
        {
            using (var scope = new TransactionScope())
            {
                var project = new Project
                {
                    ProjectName = projectEntity.ProjectName,
                    Start_Date = projectEntity.Start_Date,
                    End_Date = projectEntity.End_Date,
                    Priority = projectEntity.Priority
                };
                _unitOfWork.ProjectRepository.Insert(project);
                _unitOfWork.Save();
                scope.Complete();
                return project.Project_ID;
            }
        }

        /// <summary>  
        /// Updates a project  
        /// </summary>  
        /// <param name="projectId"></param>  
        /// <param name="projectEntity"></param>  
        /// <returns></returns>  
        public bool UpdateProject(int projectId, ProjectEntity projectEntity)
        {
            var success = false;
            if (projectEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var project = _unitOfWork.ProjectRepository.GetByID(projectId);
                    if (project != null)
                    {
                        project.ProjectName = projectEntity.ProjectName;
                        project.Start_Date = projectEntity.Start_Date;
                        project.End_Date = projectEntity.End_Date;
                        project.Priority = projectEntity.Priority;

                        _unitOfWork.ProjectRepository.Update(project);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        /// <summary>  
        /// Deletes a particular project  
        /// </summary>  
        /// <param name="projectId"></param>  
        /// <returns></returns>  
        public bool DeleteProject(int projectId)
        {
            var success = false;
            if (projectId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var project = _unitOfWork.ProjectRepository.GetByID(projectId);
                    if (project != null)
                    {
                        _unitOfWork.ProjectRepository.Delete(project);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        /// <summary>
        /// Get Projecct Search
        /// </summary>
        /// <returns></returns>
        public IEnumerable<view_ProjectSearchEntity> GetProjectsSearch()
        {
            var projects = _unitOfWork.ProjectSearchRepository.GetAll().ToList();
            if (projects.Any())
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<view_ProjectSearch, view_ProjectSearchEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var projectsModel = mapper.Map<List<view_ProjectSearch>, List<view_ProjectSearchEntity>>(projects);
                return projectsModel;
            }
            return null;
        }
    }

}
