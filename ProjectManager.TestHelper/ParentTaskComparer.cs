using ProjectManager.DAL;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectManager.TestHelper
{
    public class ParentTaskComparer : IComparer, IComparer<ParentTask>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as ParentTask;
            var rhs = actual as ParentTask;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(ParentTask expected, ParentTask actual)
        {
            int temp;
            return (temp = expected.Parent_ID.CompareTo(actual.Parent_ID)) != 0 ?
                    temp : expected.Parent_Task.CompareTo(actual.Parent_Task);
        }
    }
}
