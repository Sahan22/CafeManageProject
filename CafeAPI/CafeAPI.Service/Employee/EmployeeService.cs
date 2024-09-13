using CafeAPI.Core.Cafe;
using CafeAPI.Core.Employee;
using CafeAPI.DataAccess.Cafe;
using CafeAPI.DataAccess.Employee;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {


        #region Private Properties
        private readonly IEmployeeDataAccess iEmployeeDataAccess;
        #endregion

        // Constructor
        public EmployeeService(IConfiguration configurationString, IEmployeeDataAccess IEmployeeDataAccess)
        {
            // Intantiating the object

            this.iEmployeeDataAccess = IEmployeeDataAccess;

        }


        /// <summary>
        /// GetAllEmolyees
        /// </summary>
        /// <returns></returns>
        public List<EmployeeDetails> GetAllEmolyees(int cafeId)
        {
            // Getting all the product list
            List<EmployeeDetails>  List = iEmployeeDataAccess.GetAllEmolyees(cafeId);

            // Return the list
            return List;
        }

        /// <summary>
        /// Insert Employee Details
        /// </summary>
        /// <param name="EmployeeDetails"></param>
        /// <returns></returns>
        public string InsertEmployeeDetails(EmployeeDetails EmployeeDetails)
        {
            // Declare the all state
            string allState = "ERROR";

            bool isAdded = iEmployeeDataAccess.InsertEmployeeDetails(EmployeeDetails);
            if (isAdded)
            {
                allState = "SUCCESS";
            }
            // Return the state
            return allState;

 
        }

        /// <summary>
        ///Update Employee Details
        /// </summary>
        /// <param name="EmployeeDetails"></param>
        /// <returns></returns>
        public string UpdateEmployeeDetails(EmployeeDetails EmployeeDetails)
        {
            // Declare the all state
            string allState = "ERROR";


            bool isUpdated = iEmployeeDataAccess.UpdateEmployeeDetails(EmployeeDetails);
            if (isUpdated)
            {
                allState = "SUCCESS";
            }
            // Return the state
            return allState;
        }


        /// <summary>
        ///Delete Employee Details
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public string DeleteEmployeeDetails(string employeeId)
        {
            // Declare the state
            string allState = "ERROR";

            try
            {
                // Call the data access method to perform the delete operation
                bool isDeleted = iEmployeeDataAccess.DeleteEmployeeDetails(employeeId);

                if (isDeleted)
                {
                    allState = "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log them if necessary
                allState = "ERROR: " + ex.Message;
            }

            // Return the state
            return allState;
        }

    }
}
