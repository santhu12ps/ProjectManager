using ProjectManager.DAL.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.UnitOfWork
{
    /// <summary>  
    /// Unit of Work class responsible for DB transactions  
    /// </summary>  
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region Private member variables...  

        private ProjectManagerDBEntities _context = null;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Project> _projectRepository;
        private GenericRepository<Task> _taskRepository;
        private GenericRepository<ParentTask> _parentTaskRepository;
        private GenericRepository<view_ProjectSearch> _viewProjectSearchRepository;
        private GenericRepository<view_TaskSearch> _viewTaskSearchRepository;

        #endregion

        public UnitOfWork()
        {
            _context = new ProjectManagerDBEntities();
        }

        #region Public Repository Creation properties...  

        /// <summary>  
        /// Get/Set Property for product repository.  
        /// </summary>  
        public GenericRepository<Project> ProjectRepository
        {
            get
            {
                if (this._projectRepository == null)
                    this._projectRepository = new GenericRepository<Project>(_context);
                return _projectRepository;
            }
        }

        public GenericRepository<view_ProjectSearch> ProjectSearchRepository
        {
            get
            {
                if (this._viewProjectSearchRepository == null)
                    this._viewProjectSearchRepository = new GenericRepository<view_ProjectSearch>(_context);
                return _viewProjectSearchRepository;
            }
        }

        /// <summary>  
        /// Get/Set Property for user repository.  
        /// </summary>  
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }

        /// <summary>  
        /// Get/Set Property for token repository.  
        /// </summary>  
        public GenericRepository<Task> TaskRepository
        {
            get
            {
                if (this._taskRepository == null)
                    this._taskRepository = new GenericRepository<Task>(_context);
                return _taskRepository;
            }
        }

        public GenericRepository<ParentTask> ParentTaskRepository
        {
            get
            {
                if (this._parentTaskRepository == null)
                    this._parentTaskRepository = new GenericRepository<ParentTask>(_context);
                return _parentTaskRepository;
            }
        }

        public GenericRepository<view_TaskSearch> TaskSearchRepository
        {
            get
            {
                if (this._viewTaskSearchRepository == null)
                    this._viewTaskSearchRepository = new GenericRepository<view_TaskSearch>(_context);
                return _viewTaskSearchRepository;
            }
        }
        #endregion

        #region Public member methods...  
        /// <summary>  
        /// Save method.  
        /// </summary>  
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...  

        #region private dispose variable declaration...  
        private bool disposed = false;
        #endregion

        /// <summary>  
        /// Protected Virtual Dispose method  
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>  
        /// Dispose method  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }

}
