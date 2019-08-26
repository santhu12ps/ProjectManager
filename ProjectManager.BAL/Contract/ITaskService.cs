using ProjectManager.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BAL
{
    public interface ITaskService
    {
        TaskEntity GetTaskById(int taskId);
        IEnumerable<TaskEntity> GetAllTasks();
        int CreateTask(TaskEntity taskEntity);
        bool UpdateTask(int taskId, TaskEntity taskEntity);
        bool DeleteTask(int taskId);
        IEnumerable<view_TaskSearchEntity> GetTaskSearch();
    }
}
