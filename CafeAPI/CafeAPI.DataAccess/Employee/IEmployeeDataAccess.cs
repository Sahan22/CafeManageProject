using CafeAPI.Core.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.DataAccess.Employee
{
    public interface IEmployeeDataAccess
    {
        public List<EmployeeDetails> GetAllEmolyees(int cafeId);

        public bool InsertEmployeeDetails(EmployeeDetails employeeDetails);


        public bool UpdateEmployeeDetails(EmployeeDetails employeeDetails);
        public bool DeleteEmployeeDetails(string employeeId);

    }
}
