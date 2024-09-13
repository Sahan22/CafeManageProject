using CafeAPI.Core.Cafe;
using CafeAPI.Core.Employee;
using CafeAPI.Service.Cafe;
using CafeAPI.Service.Employee;
using Microsoft.AspNetCore.Mvc;

namespace CafeAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        #region Private Properties
        private readonly IEmployeeService iEmployeeService;
        #endregion


        // Constructor
        public EmployeeController(IEmployeeService iEmployeeService)
        {
            this.iEmployeeService = iEmployeeService;

        }


        /// <summary>
        ///Get All Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllEmolyees")]
        public IActionResult GetAllEmolyees(int cafeId)
        {
            try
            {
                // Declare response
                var response = this.iEmployeeService.GetAllEmolyees(cafeId);
                // Returning the result
                return Json(response);
            }
            catch (Exception ex)
            {
                // Returning the exception
                return Json("System Failed: " + ex.Message);
            }
        }


        /// <summary>
        /// InsertEmployeeDetails
        /// </summary>
        /// <param name="EmployeeDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertEmployeeDetails")]
        public IActionResult InsertEmployeeDetails([FromBody] EmployeeDetails EmployeeDetails)
        {
            try
            {
                // Validate the input parameters


                // Perform the action based on actionType
                var response = iEmployeeService.InsertEmployeeDetails(EmployeeDetails);

                // Return the result as JSON
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle the exception and return error response
                return StatusCode(500, "System Failed: " + ex.Message);
            }
        }


        /// <summary>
        ///Update Employee Details
        /// </summary>
        /// <param name="EmployeeDetails"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateEmployeeDetails")]
        public IActionResult UpdateEmployeeDetails([FromBody] EmployeeDetails EmployeeDetails)
        {
            try
            {
 


                // Perform the action based on actionType
                var response = iEmployeeService.UpdateEmployeeDetails(EmployeeDetails);

                // Return the result as JSON
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle the exception and return error response
                return StatusCode(500, "System Failed: " + ex.Message);
            }
        }


        /// <summary>
        /// Delete Employee Details
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteEmployeeDetails")]
        public IActionResult DeleteEmployeeDetails(string employeeId)
        {
            try
            {
 
                // Perform the action based on actionType
                var response = iEmployeeService.DeleteEmployeeDetails(employeeId);

                // Return the result as JSON
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle the exception and return error response
                return StatusCode(500, "System Failed: " + ex.Message);
            }
        }



    }
}
