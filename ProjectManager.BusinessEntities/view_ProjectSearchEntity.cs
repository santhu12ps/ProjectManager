using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessEntities
{
    public class view_ProjectSearchEntity
    {
        public int Project_ID { get; set; }
        public string ProjectName { get; set; }
        public string Project_Start_Date { get; set; }
        public string Project_End_Date { get; set; }
        public string Project_Priority { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> User_ProjectID { get; set; }
        public Nullable<int> User_EmployeeID { get; set; }
        public string User_FirstName { get; set; }
        public string User_LastName { get; set; }
        public string User_FullName { get; set; }
        public Nullable<int> No_OfTask { get; set; }
        public Nullable<int> NumberOfTaskCompleted { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
    }
}
