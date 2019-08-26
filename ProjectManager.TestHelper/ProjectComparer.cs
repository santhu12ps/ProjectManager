using ProjectManager.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.TestHelper
{
    public class ProjectComparer : IComparer, IComparer<Project>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as Project;
            var rhs = actual as Project;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(Project expected, Project actual)
        {
            int temp;
            return (temp = expected.Project_ID.CompareTo(actual.Project_ID)) != 0 ?
                    temp : expected.ProjectName.CompareTo(actual.ProjectName);
        }
    }
}
