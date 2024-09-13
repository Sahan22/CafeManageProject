using CafeAPI.Core.Cafe;
using CafeAPI.DataAccess.Cafe;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.Service.Cafe
{
    public class CafeService : ICafeService
    {

        #region Private Properties
        private readonly ICafeDataAccess iCafeDataAccess;
        #endregion

        // Constructor
        public CafeService(IConfiguration configurationString, ICafeDataAccess ICafeDataAccess)
        {
            // Intantiating the object

            this.iCafeDataAccess = ICafeDataAccess;

        }

         
        /// <summary>
        /// Get All Cafe Details
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<CafeDetails> GetAllCafeDetails(int locationId)
        {
            // Getting all the product list
            List<CafeDetails> List = iCafeDataAccess.GetAllCafeDetails(locationId);

            // Return the list
            return List;
        }


        /// <summary>
        /// Get All Cafe Dropdown
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<CafeDetails> GetAllCafeDropdown(int locationId)
        {
            // Getting all the product list
            List<CafeDetails> List = iCafeDataAccess.GetAllCafeDropdown(locationId);

            // Return the list
            return List;
        }



        /// <summary>
        /// Get Locations
        /// </summary>
        /// <returns></returns>
        public List<LocationDetails> GetLocations()
        {
            // Getting all the product list
            List<LocationDetails> LocationDetailsList = iCafeDataAccess.GetLocations();

            // Return the list
            return LocationDetailsList;
        }

         
        /// <summary>
        /// Inser Cafe Details
        /// </summary>
        /// <param name="CafeDetails"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public string InsertCafeDetails(CafeDetails CafeDetails, IFormFile logo)
        {
            // Declare the all state
            string allState = "ERROR";

            int newID = 0;

            newID = iCafeDataAccess.InsertCafeDetails(CafeDetails, logo);
            if (newID > 0)
            {
                // Setting the new Id
                allState = newID.ToString();
                allState = "SUCCESS";
            }

            // Return the state
            return allState;
        }


        /// <summary>
        ///Update Cafe Details
        /// </summary>
        /// <param name="CafeDetails"></param>
        /// <returns></returns>
        public string UpdateCafeDetails(CafeDetails CafeDetails, IFormFile logo)
        {
            // Declare the all state
            string allState = "ERROR";

            
            bool isUpdated = iCafeDataAccess.UpdateCafeDetails(CafeDetails,logo);
            if (isUpdated)
            {
                allState = "SUCCESS"; 
            }
            // Return the state
            return allState;
        }



        /// <summary>
        /// DeleteCafeDetails
        /// </summary>
        /// <param name="cafeId"></param>
        /// <returns></returns>
        public string DeleteCafeDetails(int cafeId)
        {
            // Declare the state
            string allState = "ERROR";

            try
            {
                // Call the data access method to perform the delete operation
                bool isDeleted = iCafeDataAccess.DeleteCafeDetails(cafeId);

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
