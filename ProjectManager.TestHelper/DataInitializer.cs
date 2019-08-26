using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = ProjectManager.DAL.Task;

namespace ProjectManager.TestHelper
{
    public class DataInitializer
    {
        ///<summary>  
        /// Dummy products  
        ///</summary>  
        ///<returns></returns>  
        public static List<Project> GetAllProjects()
        {
            var projects = new List<Project>
                {
                new Project()
                {
                    Project_ID = 3,
                    ProjectName="Test Project1",
                    Priority = "10",
                    Start_Date = Convert.ToDateTime("2018-01-15"),
                    End_Date = Convert.ToDateTime("2018-01-20")
                },
                new Project()
                {
                    Project_ID = 5,
                    ProjectName="Test Project2",
                    Priority = "10",
                    Start_Date = Convert.ToDateTime("2018-01-27"),
                    End_Date = Convert.ToDateTime("2018-01-30")
                }
            };
            return projects;
        }

        public static List<User> GetAllUsers()
        {
            var users = new List<User>
                {

                new User()
                {
                    User_ID = 10,
                    First_Name="Test",
                    Last_Name = "User3",
                    Employee_ID = 600,
                    Project_ID = null,
                    Task_ID = null
                },
                 new User()
                {
                    User_ID = 11,
                    First_Name="Test",
                    Last_Name = "User3",
                    Employee_ID = 600,
                    Project_ID = null,
                    Task_ID = null
                }
            };
            return users;
        }

        public static List<Task> GetAllTasks()
        {
            var tasks = new List<Task>
                {
                new Task()
                {
                    Task_ID =4,
                    Parent_ID = 2,
                    Project_ID = 2,
                    TaskName = "Child Task3",
                    Start_Date = Convert.ToDateTime("2018-12-28"),
                    End_Date = Convert.ToDateTime("2018-12-31"),
                    Priority = "15",
                    Status = null
                },
                new Task()
                {
                    Task_ID =5,
                    Parent_ID = 2,
                    Project_ID = 2,
                    TaskName = "Child Task4",
                    Start_Date = Convert.ToDateTime("2018-12-28"),
                    End_Date = Convert.ToDateTime("2018-12-31"),
                    Priority = "25",
                    Status = null
                }
            };
            return tasks;
        }

        public static List<ParentTask> GetAllParentTasks()
        {
            var tasks = new List<ParentTask>
                {
                new ParentTask()
                {
                    Parent_ID = 2,
                    Parent_Task = "Parent Task2"
                },
                new ParentTask()
                {
                    Parent_ID = 3,
                    Parent_Task = "Parent Task3"
                },
                new ParentTask()
                {
                    Parent_ID = 4,
                    Parent_Task = "Parent Task4"
                },
                new ParentTask()
                {
                    Parent_ID = 5,
                    Parent_Task = "Parent Task5"
                }
            };
            return tasks;
        }

    }
}
