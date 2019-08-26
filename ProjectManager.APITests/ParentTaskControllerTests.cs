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

namespace ProjectManager.APITests
{
    public class ParentTaskControllerTests
    {
        #region Variables  
        private IParentTaskService _taskService;
        private IUnitOfWork _unitOfWork;
        private List<ParentTask> _tasks;
        private GenericRepository<ParentTask> _taskRepository;
        private ProjectManagerDBEntities _dbEntities;
        private HttpClient _client;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://172.18.7.224:10/api/parenttask/";

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

        private GenericRepository<ParentTask> SetUpTaskRepository()
        {

            // Initialise repository  
            var mockRepo = new Mock<GenericRepository<ParentTask>>(MockBehavior.Default, _dbEntities);

            // Setup mocking behavior  
            mockRepo.Setup(p => p.GetAll()).Returns(_tasks);

            mockRepo.Setup(p => p.GetByID(It.IsAny<int>()))
                .Returns(new Func<int, ParentTask>(
                    id => _tasks.Find(p => p.Parent_ID.Equals(id))));


            mockRepo.Setup(p => p.Insert((It.IsAny<ParentTask>())))
                .Callback(new Action<ParentTask>(newTask =>
                {
                    _tasks.Add(newTask);
                }));


            mockRepo.Setup(p => p.Update(It.IsAny<ParentTask>()))
                .Callback(new Action<ParentTask>(tsk =>
                {
                    var oldTask = _tasks.Find(a => a.Parent_ID == tsk.Parent_ID);
                    oldTask = tsk;
                }));

            mockRepo.Setup(p => p.Delete(It.IsAny<ParentTask>()))
                .Callback(new Action<ParentTask>(tsk =>
                {
                    var tasksToRemove =
                        _tasks.Find(a => a.Parent_ID == tsk.Parent_ID);

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
            unitOfWork.SetupGet(s => s.ParentTaskRepository).Returns(_taskRepository);
            _unitOfWork = unitOfWork.Object;
            _taskService = new ParentTaskService();
            _client = new HttpClient
            {
                BaseAddress = new Uri(ServiceBaseURL)
            };
        }

        private static List<ParentTask> SetUpTasks()
        {
            var tasks = DataInitializer.GetAllParentTasks();
            return tasks;
        }
        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _tasks = null;
        }

        [Test]
        public void GetAllParentTasksTest()
        {
            var taskController = new ParentTaskController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL)
                }
            };
            taskController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = taskController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<ParentTask>>(_response.Content.ReadAsStringAsync().Result);
            var prntTaskList =
                responseResultSearch.Select(
                    parentTaskEntity =>
                    new ParentTask
                    {
                        Parent_ID = parentTaskEntity.Parent_ID,
                        Parent_Task = parentTaskEntity.Parent_Task
                    }).ToList();
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(prntTaskList.Any(), true);
        }

        [Test]
        public void CreateParentTaskTest()
        {
            var taskController = new ParentTaskController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "Create")
                }
            };
            taskController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var newTask = new ParentTaskEntity()
            {
                Parent_ID = 5,
                Parent_Task = "Parent Task5"
            };
            taskController.Post(newTask);

            _response = taskController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<ParentTask>>(_response.Content.ReadAsStringAsync().Result);
            var prntTaskList =
                responseResultSearch.Select(
                    parentTaskEntity =>
                    new ParentTask
                    {
                        Parent_ID = parentTaskEntity.Parent_ID,
                        Parent_Task = parentTaskEntity.Parent_Task
                    }).ToList();
            var addedParentTask = new ParentTask()
            {
                Parent_ID = newTask.Parent_ID,
                Parent_Task = newTask.Parent_Task
            };
            AssertObjects.PropertyValuesAreEquals(addedParentTask, prntTaskList.Last());
        }

        #region Integration Test

        /// <summary>
        /// Get all parent task test
        /// </summary>
        [Test]
        public void GetAllParentTasksIntegrationTest()
        {
            #region To be written inside Setup method specifically for integration tests
            var client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            #endregion
            _response = client.GetAsync(ServiceBaseURL).Result;
            var responseResult =
                JsonConvert.DeserializeObject<List<ParentTask>>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.Any(), true);
        }

        #endregion
    }
}
