using Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mvc.IRepository
{
    public interface IEmployeeRepository
    {
        IList<Employee> GetAllEmployee();
    }
}
