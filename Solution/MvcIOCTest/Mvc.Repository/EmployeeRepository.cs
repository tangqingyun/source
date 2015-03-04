using Mvc.IRepository;
using Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mvc.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        IList<Models.Employee> list;
        public EmployeeRepository()
        {
            list.Add(new Employee() { UId = Guid.NewGuid(), EmployeeName = "10" });
            list.Add(new Employee() { UId = Guid.NewGuid(), EmployeeName = "11" });
            list.Add(new Employee() { UId = Guid.NewGuid(), EmployeeName = "12" });
            list.Add(new Employee() { UId = Guid.NewGuid(), EmployeeName = "13" });
        }

        public IList<Models.Employee> GetAllEmployee()
        {
            return list;
        }

    }
}
