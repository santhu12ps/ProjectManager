using ProjectManager.DAL;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectManager.TestHelper
{
    public class UserComparer : IComparer, IComparer<User>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as User;
            var rhs = actual as User;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(User expected, User actual)
        {
            int temp;
            return (temp = expected.User_ID.CompareTo(actual.User_ID)) != 0 ?
                    temp : expected.First_Name.CompareTo(actual.First_Name);
        }
    }
}
