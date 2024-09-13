using CafeAPI.Core.Cafe;
using CafeAPI.Service.Cafe;
using Microsoft.AspNetCore.Mvc;

namespace CafeAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CafeController : Controller
    {
        #region Private Properties
        private readonly ICafeService iCafeService;
        #endregion

        // Constructor
        public CafeController(ICafeService iCafeService)
        {
            this.iCafeService = iCafeService;

        }

        // Get All Cafe Details
        /// <summary>
        /// Get All Cafe Details
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCafeDetails")]
        public IActionResult GetAllCafeDetails( int locationId)
        {
            try
            {
                // Declare response
                var response = this.iCafeService.GetAllCafeDetails(locationId);
                // Returning the result
                return Json(response);
            }
            catch (Exception ex)
            {
                // Returning the exception
                return Json("System Failed: " + ex.Message);
            }
        }


        //GetAllCafeDropdown
        /// <summary>
        /// GetAllCafeDropdown
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCafeDropdown")]
        public IActionResult GetAllCafeDropdown(int locationId)
        {
            try
            {
                // Declare response
                var response = this.iCafeService.GetAllCafeDropdown(locationId);
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
        ///GetLocations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllLocations")]
        public IActionResult GetLocations()
        {
            try
            {
                // Declare response
                var response = this.iCafeService.GetLocations();
                // Returning the result
                return Json(response);
            }
            catch (Exception ex)
            {
                // Returning the exception
                return Json("System Failed: " + ex.Message);
            }
        }



        //InsertCafeDetails
        /// <summary>
        ///Insert Cafe Details
        /// </summary>
        /// <param name="cafeDetails"></param> 
        /// <returns></returns>
        [HttpPost]
        [Route("InsertCafeDetails")]
        public async Task<IActionResult> InsertCafeDetails([FromForm] CafeDetails cafeDetails, IFormFile logo)
        {
            try
            {
                // Process cafeDetails and logo
                var response = iCafeService.InsertCafeDetails(cafeDetails, logo);

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
        /// Update Cafe Details
        /// </summary>
        /// <param name="cafeDetails"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateCafeDetails")]
        public async Task<IActionResult> UpdateCafeDetails([FromForm] CafeDetails cafeDetails, IFormFile logo)
        {
            try
            {
                // Validate the input parameters


                // Perform the action based on actionType
                var response = iCafeService.UpdateCafeDetails(cafeDetails, logo);

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
        ///Delete Cafe Details
        /// </summary>
        /// <param name="cafeId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteCafeDetails")]
        public IActionResult DeleteCafeDetails(int cafeId)
        {
            try
            {
                // Validate the input parameters


                // Perform the action based on actionType
                var response = iCafeService.DeleteCafeDetails(cafeId);

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
