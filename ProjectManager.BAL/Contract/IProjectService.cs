using ProjectManager.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BAL
{
    public interface IProjectService
    {
        ProjectEntity GetProjectById(int projectId);
        IEnumerable<ProjectEntity> GetAllProjects();
        int CreateProject(ProjectEntity projectEntity);
        bool UpdateProject(int projectId, ProjectEntity projectEntity);
        bool DeleteProject(int projectId);
        IEnumerable<view_ProjectSearchEntity> GetProjectsSearch();
    }
}
