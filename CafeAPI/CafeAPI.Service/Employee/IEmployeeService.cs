using CafeAPI.Core.Cafe;
using CafeAPI.Core.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.Service.Employee
{
    public interface IEmployeeService
    {
        public List<EmployeeDetails> GetAllEmolyees(int cafeId);



        public string InsertEmployeeDetails(EmployeeDetails employeeDetails);


        public string UpdateEmployeeDetails(EmployeeDetails employeeDetails);


        public string DeleteEmployeeDetails(string employeeId);



    }
}
