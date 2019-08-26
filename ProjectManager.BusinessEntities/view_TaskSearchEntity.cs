using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessEntities
{
    public class view_TaskSearchEntity
    {
        public string TaskName { get; set; }
        public int Task_ID { get; set; }
        public System.DateTime Start_Date { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public string TaskStartDate { get; set; }
        public string TaskEndDate { get; set; }
        public string TaskPriority { get; set; }
        public string TaskStatus { get; set; }
        public string ParentTask { get; set; }
        public string MappedProject { get; set; }
        public Nullable<int> TaskParentID { get; set; }
        public Nullable<int> TaskProjectID { get; set; }
        public string First_Name { get; set; }
        public Nullable<int> AssignedUserID { get; set; }
        public bool IsTaskCompleted { get; set; }
        public string TaskTooltip { get; set; }
    }
}
