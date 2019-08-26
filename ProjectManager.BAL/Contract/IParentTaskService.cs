using ProjectManager.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BAL
{
    public interface IParentTaskService
    {
        ParentTaskEntity GetParentTaskById(int parentTaskId);
        IEnumerable<ParentTaskEntity> GetAllParentTasks();
        int CreateParentTask(ParentTaskEntity parentTaskEntity);
        bool UpdateParentTask(int parentTaskId, ParentTaskEntity parentTaskEntity);
        bool DeleteParentTask(int parentTaskId);
    }
}
