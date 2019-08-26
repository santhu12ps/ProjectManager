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
    public class ParentTaskService : IParentTaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParentTaskService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public int CreateParentTask(ParentTaskEntity parentTaskEntity)
        {
            using (var scope = new TransactionScope())
            {
                var task = new ParentTask
                {
                    Parent_ID = parentTaskEntity.Parent_ID,
                    Parent_Task = parentTaskEntity.Parent_Task
                };
                _unitOfWork.ParentTaskRepository.Insert(task);
                _unitOfWork.Save();
                scope.Complete();
                return task.Parent_ID;
            }
        }

        public bool DeleteParentTask(int parentTaskId)
        {
            return true;
        }

        public IEnumerable<ParentTaskEntity> GetAllParentTasks()
        {
            var tasks = _unitOfWork.ParentTaskRepository.GetAll().ToList();
            if (tasks != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<ParentTask, ParentTaskEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var taskModel = mapper.Map<List<ParentTask>, List<ParentTaskEntity>>(tasks);
                return taskModel;
            }
            return null;
        }

        public ParentTaskEntity GetParentTaskById(int parentTaskId)
        {
            return new ParentTaskEntity();
        }

        public bool UpdateParentTask(int parentTaskId, ParentTaskEntity parentTaskEntity)
        {
            return true;
        }
    }

}
