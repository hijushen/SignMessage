using System;
using System.Collections.Generic;
using System.Text;

namespace OAUtil
{
    public class OAPerson
    {
        public static string[] getEmploy()
        {
            NewOA.Employee emp = NewOA.Employee.GetEmployee("", "coocge");

            return new string[] { emp.JobName, emp.JobLevel };

        }
    }
}
