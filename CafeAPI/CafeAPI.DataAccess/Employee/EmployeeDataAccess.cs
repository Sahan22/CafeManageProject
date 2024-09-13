using CafeAPI.Core.Cafe;
using CafeAPI.Core.Employee;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace CafeAPI.DataAccess.Employee
{
    public class EmployeeDataAccess : IEmployeeDataAccess
    {


        private readonly IConfiguration _configuration; // IConfiguration for accessing config values
        private readonly string _connectionString; // Connection string to use with MySQL


        // Constructor initializing the configuration and connection string
        public EmployeeDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SysDBString");  
        }



        /// <summary>
        /// Get All Emolyees
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<EmployeeDetails> GetAllEmolyees(int cafeId)
        {
            List<EmployeeDetails> empList = new List<EmployeeDetails>();

            try
            {
                // Correct usage of MySqlConnection
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("sp_GetAllEmployeeDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Add parameters
                        command.Parameters.AddWithValue("@cafeId", cafeId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var empDetail = new EmployeeDetails
                                {
                                    EMP_Id = reader["EMP_Id"].ToString(),
                                    EMP_Name = reader["EMP_Name"].ToString(),
                                    EMP_Address = reader["EMP_Address"].ToString(),
                                    EMP_PH = reader["EMP_PH"].ToString(),
                                    DaysOfWork = Convert.ToInt32(reader["DaysOfWork"]),
                                    CAF_Id = Convert.ToInt32(reader["CAF_Id"]),
                                    CAF_NAME = reader["CAF_NAME"].ToString(),
                                    EMP_Gender = reader["EMP_Gender"].ToString()
                                };
                                empList.Add(empDetail);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Catch MySqlException for MySQL specific errors
                throw new Exception("Error in GetAllEmolyees: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Catch generic exceptions
                throw new Exception("General error in GetAllEmolyees: " + ex.Message);
            }

            return empList;
        }


        /// <summary>
        /// Insert Employee Details
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool InsertEmployeeDetails(EmployeeDetails employeeDetails)
        {
            try
            {
                // Setting up the SQL connection
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Using MySqlCommand to execute stored procedure
                    using (MySqlCommand sqlCommand = new MySqlCommand("sp_NewEmployee", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Adding stored procedure parameters
                     
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Name", MySqlDbType.VarChar)).Value = employeeDetails.EMP_Name;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Email_address", MySqlDbType.VarChar)).Value = employeeDetails.EMP_Address;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Phone_number", MySqlDbType.VarChar)).Value = employeeDetails.EMP_PH;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Gender", MySqlDbType.VarChar)).Value = employeeDetails.EMP_Gender;  
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_FK_CafeId", MySqlDbType.Int32)).Value = employeeDetails.CAF_Id;

                        // Add output parameter for NewId (though not needed anymore since we will return a bool)
                        MySqlParameter outputIdParam = new MySqlParameter("@p_NewId", MySqlDbType.Int32);
                        outputIdParam.Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add(outputIdParam);

                        // Execute the stored procedure
                        int rowsAffected = sqlCommand.ExecuteNonQuery();

                        // Return true if a row was inserted
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Capture SQL-specific exceptions
                throw new Exception("SQL Error in InsertEmployeeDetails: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // General exception
                throw new Exception("Error in InsertEmployeeDetails: " + ex.Message);
            }
        }


        /// <summary>
        /// Update Employee Details
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateEmployeeDetails(EmployeeDetails employeeDetails)
        {
            try
            {
                // Setting up the SQL connection
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Using MySqlCommand to execute stored procedure
                    using (MySqlCommand sqlCommand = new MySqlCommand("sp_UpdateEmployee", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Adding stored procedure parameters
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Id", MySqlDbType.VarChar)).Value = employeeDetails.EMP_Id;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Name", MySqlDbType.VarChar)).Value = employeeDetails.EMP_Name;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Email_address", MySqlDbType.VarChar)).Value = employeeDetails.EMP_Address;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Phone_number", MySqlDbType.VarChar)).Value = employeeDetails.EMP_PH;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Gender", MySqlDbType.VarChar)).Value = employeeDetails.EMP_Gender;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_DaysOfWork", MySqlDbType.Int32)).Value = employeeDetails.DaysOfWork;
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_FK_CafeId", MySqlDbType.Int32)).Value = employeeDetails.CAF_Id;

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
                throw new Exception("SQL Error in UpdateEmployeeDetails: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // General exception
                throw new Exception("Error in UpdateEmployeeDetails: " + ex.Message);
            }
        }


        /// <summary>
        /// Delete Employee Details
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeleteEmployeeDetails(string employeeId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Using MySqlCommand to execute stored procedure
                    using (MySqlCommand sqlCommand = new MySqlCommand("sp_DeleteEmployee", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Adding stored procedure parameter for employee Id
                        sqlCommand.Parameters.Add(new MySqlParameter("@p_Id", MySqlDbType.VarChar)).Value = employeeId;

                        // Execute the stored procedure and check the number of rows affected
                        int rowsAffected = sqlCommand.ExecuteNonQuery();

                        // Return true if at least one row was affected, otherwise false
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Handle SQL-specific exceptions
                throw new Exception("SQL Error in DeleteEmployeeDetails: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // General exception
                throw new Exception("Error in DeleteEmployeeDetails: " + ex.Message);
            }
        }


    }
}
