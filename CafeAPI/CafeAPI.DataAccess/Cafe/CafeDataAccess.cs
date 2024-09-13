using CafeAPI.Core.Cafe;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient; // Correct MySQL provider
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CafeAPI.DataAccess.Cafe
{
    public class CafeDataAccess : ICafeDataAccess
    {



        private readonly IConfiguration _configuration; // IConfiguration for accessing config values
        private readonly string _connectionString; // Connection string to use with MySQL

        // Constructor initializing the configuration and connection string
        public CafeDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SysDBString");  
        }

        ///GetAllCafeDetails
        /// <summary>
        /// GetAllCafeDetails
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<CafeDetails> GetAllCafeDetails(int locationId)
        {
            List<CafeDetails> cafeDetailsList = new List<CafeDetails>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("sp_GetCafesByLocation", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@location_id", locationId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var logoBytes = reader["Logo"] as byte[];
                                var cafeDetail = new CafeDetails
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    CafeName = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    // Convert byte array to base64 string
                                    Logo = logoBytes != null ? Convert.ToBase64String(logoBytes) : null,
                                    FK_LocationId = Convert.ToInt32(reader["FK_LocationId"]),
                                    LocationName = reader["LocationName"].ToString(),
                                    EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
                                };
                                cafeDetailsList.Add(cafeDetail);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Catch MySqlException for MySQL specific errors
                throw new Exception("Error in CafeDataAccess_GetAllCafeDetails: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Catch generic exceptions
                throw new Exception("General error in CafeDataAccess_GetAllCafeDetails: " + ex.Message);
            }

            return cafeDetailsList;
        }



        //Get Locations
        /// <summary>
        ///Get Locations
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<LocationDetails> GetLocations()
        {
            List<LocationDetails> LocationDetailsList = new List<LocationDetails>();

            try
            {
                // Correct usage of MySqlConnection
                using (MySqlConnection connection = new MySqlConnection(_connectionString))  
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("sp_GetLocations", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
 

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var LocationDetails = new LocationDetails
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    LocationName = reader["location_name"].ToString() 
                                };
                                LocationDetailsList.Add(LocationDetails);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Catch MySqlException for MySQL specific errors
                throw new Exception("Error in CafeDataAccess_GetLocations: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Catch generic exceptions
                throw new Exception("General error in CafeDataAccess_GetLocations: " + ex.Message);
            }

            return LocationDetailsList;
        }







        //Insert Cafe Details
        /// <summary>
        /// InsertCafeDetails
        /// </summary>
        /// <param name="CafeDetails"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int InsertCafeDetails(CafeDetails cafeDetails, IFormFile logo)
        {
            int newId = 0;

            try
            {
                // Convert the IFormFile to a byte array
                byte[] logoBytes = null;
                if (logo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        logo.CopyTo(memoryStream);
                        logoBytes = memoryStream.ToArray();
                    }
                }

                // Setting up the SQL connection
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Using MySqlCommand to execute stored procedure
                    using (MySqlCommand sqlCommand = new MySqlCommand("sp_SetNewCafe", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Adding stored procedure parameters
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Name", MySqlDbType.VarChar)).Value = cafeDetails.CafeName;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Description", MySqlDbType.VarChar)).Value = cafeDetails.Description;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Logo", MySqlDbType.Blob)).Value = (object)logoBytes ?? DBNull.Value;  
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Location", MySqlDbType.VarChar)).Value = cafeDetails.LocationName;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_FK_LocationId", MySqlDbType.Int64)).Value = cafeDetails.FK_LocationId;

                        // Add output parameter for NewId
                        MySqlParameter outputIdParam = new MySqlParameter("@p_NewId", MySqlDbType.Int32);
                        outputIdParam.Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add(outputIdParam);

                        // Execute the stored procedure
                        sqlCommand.ExecuteNonQuery();

                        // Retrieve the new cafe ID
                        newId = Convert.ToInt32(outputIdParam.Value);
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Capture SQL-specific exceptions
                throw new Exception("SQL Error in InsertCafeDetails: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // General exception
                throw new Exception("Error in InsertCafeDetails: " + ex.Message);
            }

            // Return the new ID
            return newId;
        }




        //UpdateCafeDetails
        /// <summary>
        ///Update Cafe Details
        /// </summary>
        /// <param name="CafeDetails"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateCafeDetails(CafeDetails cafeDetails, IFormFile logo = null)
        {
            try
            {
                // Convert the IFormFile to a byte array if provided
                byte[] logoBytes = null;
                if (logo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        logo.CopyTo(memoryStream);
                        logoBytes = memoryStream.ToArray();
                    }
                }

                // Setting up the SQL connection
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Using MySqlCommand to execute stored procedure
                    using (MySqlCommand sqlCommand = new MySqlCommand("sp_UpdateCafe", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Adding stored procedure parameters
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Id", MySqlDbType.Int32)).Value = cafeDetails.Id;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Name", MySqlDbType.VarChar)).Value = cafeDetails.CafeName;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Description", MySqlDbType.Text)).Value = cafeDetails.Description;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Location", MySqlDbType.VarChar)).Value = cafeDetails.LocationName;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_FK_LocationId", MySqlDbType.Int32)).Value = cafeDetails.FK_LocationId;
                        // Handle Logo as a BLOB
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Logo", MySqlDbType.Blob)).Value = (object)logoBytes ?? DBNull.Value;
                       

                        // Execute the stored procedure and check the number of rows affected
                        int rowsAffected = sqlCommand.ExecuteNonQuery();

                        // Return true if at least one row was affected, otherwise false
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Capture SQL-specific exceptions
                throw new Exception("SQL Error in UpdateCafeDetails: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // General exception
                throw new Exception("Error in UpdateCafeDetails: " + ex.Message);
            }
        }



        /// <summary>
        ///Delete Cafe Details
        /// </summary>
        /// <param name="CafeDetails"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeleteCafeDetails(int cafeId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    using (MySqlCommand sqlCommand = new MySqlCommand("sp_DeleteCafe", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Id", MySqlDbType.Int32)).Value = cafeId;

                        int rowsAffected = sqlCommand.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Handle SQL-specific exceptions
                throw new Exception("SQL Error in DeleteCafeDetails: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // General exception
                throw new Exception("Error in DeleteCafeDetails: " + ex.Message);
            }
        }




        /// <summary>
        ///Get All Cafe Dropdown
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<CafeDetails> GetAllCafeDropdown(int locationId)
        {
            List<CafeDetails> cafeDetailsList = new List<CafeDetails>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("sp_GetCafesByLocation", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@location_id", locationId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var logoBytes = reader["Logo"] as byte[];
                                var cafeDetail = new CafeDetails
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    CafeName = reader["Name"].ToString(),
                         
                                };
                                cafeDetailsList.Add(cafeDetail);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Catch MySqlException for MySQL specific errors
                throw new Exception("Error in GetAllCafeDropdown: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Catch generic exceptions
                throw new Exception("General error in GetAllCafeDropdown: " + ex.Message);
            }

            return cafeDetailsList;
        }



    }
}
