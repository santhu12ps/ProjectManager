using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessEntities
{
    public class ProjectEntity
    {
        public int Project_ID { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
        public string Priority { get; set; }
        public string ProjectName { get; set; }
        public int Manager_ID { get; set; }
    }
}
