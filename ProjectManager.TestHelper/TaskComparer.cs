using System;
using System.Collections;
using System.Collections.Generic;
using Task = ProjectManager.DAL.Task;

namespace ProjectManager.TestHelper
{
    public class TaskComparer : IComparer, IComparer<Task>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as Task;
            var rhs = actual as Task;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(Task expected, Task actual)
        {
            int temp;
            return (temp = expected.Task_ID.CompareTo(actual.Task_ID)) != 0 ?
                    temp : expected.TaskName.CompareTo(actual.TaskName);
        }
    }
}
