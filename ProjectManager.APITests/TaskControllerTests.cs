using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectManager.BAL;
using ProjectManager.BusinessEntities;
using ProjectManager.DAL;
using ProjectManager.DAL.GenericRepository;
using ProjectManager.DAL.UnitOfWork;
using ProjectManagerAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Hosting;
using ProjectManager.TestHelper;
using Task = ProjectManager.DAL.Task;

namespace ProjectManager.APITests
{
    public class TaskControllerTests
    {
        #region Variables  
        private ITaskService _taskService;
        private IUnitOfWork _unitOfWork;
        private List<Task> _tasks;
        private GenericRepository<Task> _taskRepository;
        private ProjectManagerDBEntities _dbEntities;
        private HttpClient _client;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://172.18.7.224:10/api/task/";

        #endregion

        #region Setup  
        ///<summary>  
        /// Re-initializes test.  
        ///</summary>  
        [SetUp]
        public void ReInitializeTest()
        {
            _client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };

        }

        #endregion

        private GenericRepository<Task> SetUpTaskRepository()
        {

            // Initialise repository  
            var mockRepo = new Mock<GenericRepository<Task>>(MockBehavior.Default, _dbEntities);

            // Setup mocking behavior  
            mockRepo.Setup(p => p.GetAll()).Returns(_tasks);

            mockRepo.Setup(p => p.GetByID(It.IsAny<int>()))
                .Returns(new Func<int, Task>(
                    id => _tasks.Find(p => p.Task_ID.Equals(id))));


            mockRepo.Setup(p => p.Insert((It.IsAny<Task>())))
                .Callback(new Action<Task>(newTask =>
                {
                    _tasks.Add(newTask);
                }));


            mockRepo.Setup(p => p.Update(It.IsAny<Task>()))
                .Callback(new Action<Task>(tsk =>
                {
                    var oldTask = _tasks.Find(a => a.Task_ID == tsk.Task_ID);
                    oldTask = tsk;
                }));

            mockRepo.Setup(p => p.Delete(It.IsAny<Task>()))
                .Callback(new Action<Task>(tsk =>
                {
                    var tasksToRemove =
                        _tasks.Find(a => a.Task_ID == tsk.Task_ID);

                    if (tasksToRemove != null)
                        _tasks.Remove(tasksToRemove);
                }));

            // Return mock implementation object  
            return mockRepo.Object;
        }

        [TearDown]
        public void DisposeTest()
        {
            if (_response != null) _response.Dispose();
            if (_client != null) _client.Dispose();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _tasks = SetUpTasks();
            _dbEntities = new Mock<ProjectManagerDBEntities>().Object;
            _taskRepository = SetUpTaskRepository();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.TaskRepository).Returns(_taskRepository);
            _unitOfWork = unitOfWork.Object;
            _taskService = new TaskService();
            _client = new HttpClient
            {
                BaseAddress = new Uri(ServiceBaseURL)
            };
        }

        private static List<Task> SetUpTasks()
        {
            var tasks = DataInitializer.GetAllTasks();
            return tasks;
        }
        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _tasks = null;
        }

        [Test]
        public void GetAllTasksTest()
        {
            var taskController = new TaskController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL)
                }
            };
            taskController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = taskController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<view_TaskSearch>>(_response.Content.ReadAsStringAsync().Result);
            var taskList =
                responseResultSearch.Select(
                    taskEntity =>
                    new Task
                    {
                        Task_ID = taskEntity.Task_ID,
                        Parent_ID = taskEntity.TaskParentID,
                        Project_ID = taskEntity.TaskProjectID,
                        TaskName = taskEntity.TaskName,
                        Start_Date = taskEntity.Start_Date,
                        End_Date = taskEntity.End_Date,
                        Priority = taskEntity.TaskPriority,
                        Status = taskEntity.TaskStatus
                    }).ToList();
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(taskList.Any(), true);
        }

        [Test]
        public void GetTaskByIdTest()
        {
            var taskController = new TaskController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "get/2")
                }
            };
            taskController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = taskController.Get(2);
            var responseResult = JsonConvert.DeserializeObject<Task>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void CreateTaskTest()
        {
            var taskController = new TaskController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "Create")
                }
            };
            taskController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var newTask = new TaskEntity()
            {
                Task_ID = 4,
                Parent_ID = 2,
                Project_ID = 2,
                TaskName = "Child Task3",
                Start_Date = Convert.ToDateTime("2018-12-28"),
                End_Date = Convert.ToDateTime("2018-12-31"),
                Priority = "15",
                Status = null
            };
            taskController.Post(newTask);

            _response = taskController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<view_TaskSearch>>(_response.Content.ReadAsStringAsync().Result);
            var taskList =
                responseResultSearch.Select(
                    taskEntity =>
                    new Task
                    {
                        Task_ID = taskEntity.Task_ID,
                        Parent_ID = taskEntity.TaskParentID,
                        Project_ID = taskEntity.TaskProjectID,
                        TaskName = taskEntity.TaskName,
                        Start_Date = taskEntity.Start_Date,
                        End_Date = taskEntity.End_Date,
                        Priority = taskEntity.TaskPriority,
                        Status = taskEntity.TaskStatus
                    }).ToList();
            var addedtask = new Task()
            {
                Task_ID = newTask.Task_ID,
                Parent_ID = newTask.Parent_ID,
                Project_ID = newTask.Project_ID,
                TaskName = newTask.TaskName,
                Start_Date = newTask.Start_Date,
                End_Date = newTask.End_Date,
                Priority = newTask.Priority,
                Status = newTask.Status
            };
            AssertObjects.PropertyValuesAreEquals(addedtask, taskList.Last());
        }
        [Test]
        public void UpdateTaskTest()
        {
            var taskController = new TaskController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "put/2")
                }
            };
            taskController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var firstTask = _tasks.First();
            firstTask.TaskName = "Child Task3 Updated";
            var updatedTask = new TaskEntity()
            {
                Task_ID = firstTask.Task_ID,
                Parent_ID = firstTask.Parent_ID,
                Project_ID = firstTask.Project_ID,
                TaskName = firstTask.TaskName,
                Start_Date = firstTask.Start_Date,
                End_Date = firstTask.End_Date,
                Priority = firstTask.Priority,
                Status = firstTask.Status
            };
            taskController.Put(firstTask.Task_ID, updatedTask);
            Assert.That(firstTask.Task_ID, Is.EqualTo(2)); // hasn't changed
        }

        [Test]
        public void DeleteTaskTest()
        {
            var taskController = new TaskController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "delete")
                }
            };
            taskController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var lastTask = _tasks.Last();
            int maxID = _tasks.Max(a => a.Task_ID); // Before removal

            // Remove last user
            taskController.Delete(lastTask.Task_ID);

            _response = taskController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<view_TaskSearch>>(_response.Content.ReadAsStringAsync().Result);
            var taskList =
                responseResultSearch.Select(
                    taskEntity =>
                    new Task
                    {
                        Task_ID = taskEntity.Task_ID,
                        Parent_ID = taskEntity.TaskParentID,
                        Project_ID = taskEntity.TaskProjectID,
                        TaskName = taskEntity.TaskName,
                        Start_Date = taskEntity.Start_Date,
                        End_Date = taskEntity.End_Date,
                        Priority = taskEntity.TaskPriority,
                        Status = taskEntity.TaskStatus
                    }).ToList();

            Assert.That(maxID, Is.GreaterThan(taskList.Max(a => a.Task_ID))); // Max id reduced by 1
        }

        #region Integration Test

        /// <summary>
        /// Get all users test
        /// </summary>
        [Test]
        public void GetAllTasksIntegrationTest()
        {
            #region To be written inside Setup method specifically for integration tests
            var client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            #endregion
            _response = client.GetAsync(ServiceBaseURL).Result;
            var responseResult =
                JsonConvert.DeserializeObject<List<view_TaskSearch>>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.Any(), true);
        }

        #endregion
    }

}
