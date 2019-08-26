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
    public class UserControllerTest
    {
        #region Variables  
        private IUserService _userService;
        private IUnitOfWork _unitOfWork;
        private List<User> _users;
        private GenericRepository<User> _userRepository;
        private ProjectManagerDBEntities _entities;
        private HttpClient _client;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://172.18.7.224:10/api/user/";

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

        private GenericRepository<User> SetUpUserRepository()
        {

            // Initialise repository  
            var mockRepo = new Mock<GenericRepository<User>>(MockBehavior.Default, _entities);

            // Setup mocking behavior  
            mockRepo.Setup(p => p.GetAll()).Returns(_users);

            mockRepo.Setup(p => p.GetByID(It.IsAny<int>()))
                .Returns(new Func<int, User>(
                    id => _users.Find(p => p.User_ID.Equals(id))));


            mockRepo.Setup(p => p.Insert((It.IsAny<User>())))
                .Callback(new Action<User>(newUser =>
                {
                    _users.Add(newUser);
                }));


            mockRepo.Setup(p => p.Update(It.IsAny<User>()))
                .Callback(new Action<User>(usr =>
                {
                    var oldUser = _users.Find(a => a.User_ID == usr.User_ID);
                    oldUser = usr;
                }));

            mockRepo.Setup(p => p.Delete(It.IsAny<User>()))
                .Callback(new Action<User>(usr =>
                {
                    var usersToRemove =
                        _users.Find(a => a.User_ID == usr.User_ID);

                    if (usersToRemove != null)
                        _users.Remove(usersToRemove);
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
            _users = SetUpUsers();
            _entities = new Mock<ProjectManagerDBEntities>().Object;
            _userRepository = SetUpUserRepository();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.UserRepository).Returns(_userRepository);
            _unitOfWork = unitOfWork.Object;
            _userService = new UserService();
            _client = new HttpClient
            {
                BaseAddress = new Uri(ServiceBaseURL)
            };
        }

        private static List<User> SetUpUsers()
        {
            var users = DataInitializer.GetAllUsers();
            return users;
        }
        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _users = null;
        }

        [Test]
        public void GetAllUsersTest()
        {
            var userController = new UserController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL)
                }
            };
            userController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = userController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<User>>(_response.Content.ReadAsStringAsync().Result);
            var userList =
               responseResultSearch.Select(
                   userEntity =>
                   new User
                   {
                       User_ID = userEntity.User_ID,
                       First_Name = userEntity.First_Name,
                       Last_Name = userEntity.Last_Name,
                       Employee_ID = userEntity.Employee_ID,
                       Project_ID = userEntity.Project_ID,
                       Task_ID = userEntity.Task_ID
                   }).ToList();
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(userList.Any(), true);
        }

        [Test]
        public void GetUserByIdTest()
        {
            var userController = new UserController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "get/10")
                }
            };
            userController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = userController.Get(1);
            var responseResult = JsonConvert.DeserializeObject<User>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            AssertObjects.PropertyValuesAreEquals(responseResult, _users.Find(a => a.First_Name.Contains("Test")));
        }

        [Test]
        public void CreateUserTest()
        {
            var userController = new UserController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "Create")
                }
            };
            userController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var newUser = new UserEntity()
            {
                User_ID = 11,
                First_Name = "Test",
                Last_Name = "User3",
                Employee_ID = 600,
                Project_ID = null,
                Task_ID = null
            };
            userController.PostUser(newUser);

            _response = userController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<User>>(_response.Content.ReadAsStringAsync().Result);
            var userList =
                responseResultSearch.Select(
                    userEntity =>
                    new User
                    {
                        User_ID = userEntity.User_ID,
                        First_Name = userEntity.First_Name,
                        Last_Name = userEntity.Last_Name,
                        Employee_ID = userEntity.Employee_ID,
                        Project_ID = userEntity.Project_ID,
                        Task_ID = userEntity.Task_ID
                    }).ToList();
            var addeduser = new User()
            {
                User_ID = newUser.User_ID,
                First_Name = newUser.First_Name,
                Last_Name = newUser.Last_Name,
                Employee_ID = newUser.Employee_ID,
                Project_ID = newUser.Project_ID,
                Task_ID = newUser.Task_ID
            };
            AssertObjects.PropertyValuesAreEquals(addeduser, userList.Last());
        }
        [Test]
        public void UpdateUserTest()
        {
            var userController = new UserController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "put/10")
                }
            };
            userController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var firstUser = _users.First();
            firstUser.First_Name = "Test3";
            var updateduser = new UserEntity()
            {
                User_ID = firstUser.User_ID,
                First_Name = firstUser.First_Name,
                Last_Name = firstUser.Last_Name,
                Employee_ID = firstUser.Employee_ID,
                Project_ID = firstUser.Project_ID,
                Task_ID = firstUser.Task_ID
            };
            userController.Put(firstUser.User_ID, updateduser);
            Assert.That(firstUser.User_ID, Is.EqualTo(10)); // hasn't changed
        }

        [Test]
        public void DeleteUserTest()
        {
            var userController = new UserController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "delete")
                }
            };
            userController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var lastUser = _users.Last();
            int maxID = _users.Max(a => a.User_ID); // Before removal

            // Remove last user
            userController.Delete(lastUser.User_ID);

            _response = userController.Get();
            var responseResultSearch = JsonConvert.DeserializeObject<List<User>>(_response.Content.ReadAsStringAsync().Result);
            var userList =
                responseResultSearch.Select(
                    userEntity =>
                    new User
                    {
                        User_ID = userEntity.User_ID,
                        First_Name = userEntity.First_Name,
                        Last_Name = userEntity.Last_Name,
                        Employee_ID = userEntity.Employee_ID,
                        Project_ID = userEntity.Project_ID,
                        Task_ID = userEntity.Task_ID
                    }).ToList();
            if (userList != null)
                Assert.That(maxID, Is.GreaterThan(userList.Max(a => a.User_ID))); // Max id reduced by 1
        }


        #region Integration Test

        /// <summary>
        /// Get all users test
        /// </summary>
        [Test]
        public void GetAllUsersIntegrationTest()
        {
            #region To be written inside Setup method specifically for integration tests
            var client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            #endregion
            _response = client.GetAsync(ServiceBaseURL).Result;
            var responseResult =
                JsonConvert.DeserializeObject<List<User>>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.Any(), true);
        }

        #endregion
    }

}
