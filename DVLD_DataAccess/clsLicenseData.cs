using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {
        public static bool GetByLicenseID(int licenseID,ref int applicationID,
           ref int driverID,ref int licenseClassID,
           ref DateTime issueDate,ref DateTime expirationDate,ref string notes,
           ref double paidFees,ref bool isActive,ref int issueReason,ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Licenses
                            WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassID = (int)reader["LicenseClassID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    paidFees = Convert.ToDouble(reader["PaidFees"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                    issueReason = Convert.ToInt16(reader["IssueReason"]);
                    createdByUserID = (int)reader["CreatedByUserID"];

                    //Notes: allows null in database so we should handle null
                    if (reader["Notes"] != DBNull.Value)
                    {
                        notes = (string)reader["Notes"];
                    }
                    else
                        notes = "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        

        public static bool GetPersonLastLicenseOfSpecificClass(ref int licenseID,ref int applicationID,
           int driverID, int licenseClassID,
           ref DateTime issueDate,ref DateTime expirationDate,ref string notes,
           ref double paidFees,ref bool isActive,ref int issueReason,ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 * FROM Licenses
                                WHERE LicenseClassID = LicenseClassID
                                AND DriverID = DriverID
                                ORDER BY IssueDate DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@DriverID", driverID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseID = (int)reader["LicenseID"];
                    applicationID = (int)reader["ApplicationID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    paidFees = Convert.ToDouble(reader["PaidFees"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                    issueReason = Convert.ToInt16(reader["IssueReason"]);
                    createdByUserID = (int)reader["CreatedByUserID"];

                    //Notes: allows null in database so we should handle null
                    if (reader["Notes"] != DBNull.Value)
                    {
                        notes = (string)reader["Notes"];
                    }
                    else
                        notes = "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        

        public static bool GetByApplicationID(ref int licenseID,int applicationID,
           ref int driverID,ref int licenseClassID,
           ref DateTime issueDate,ref DateTime expirationDate,ref string notes,
           ref double paidFees,ref bool isActive,ref int issueReason,ref int createdByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Licenses
                            WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseID = (int)reader["LicenseID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassID = (int)reader["LicenseClassID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    paidFees = Convert.ToDouble(reader["PaidFees"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                    issueReason = Convert.ToInt16(reader["IssueReason"]);
                    createdByUserID = (int)reader["CreatedByUserID"];

                    //Notes: allows null in database so we should handle null
                    if (reader["Notes"] != DBNull.Value)
                    {
                        notes = (string)reader["Notes"];
                    }
                    else
                        notes = "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static int AddNewLicense(int ApplicationID,
            int DriverID, int LicenseClassID,DateTime IssueDate,
            DateTime ExpirationDate,string Notes, double PaidFees,
            bool IsActive, int IssueReason,int CreatedByUserID)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Licenses
                            (ApplicationID, DriverID,LicenseClassID,IssueDate,
                            ExpirationDate,Notes,PaidFees,IsActive, IssueReason, CreatedByUserID)
                        VALUES (@ApplicationID,@DriverID, @LicenseClassID, @IssueDate,
                            @ExpirationDate,@Notes,@PaidFees,@IsActive, @IssueReason,@CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            if (!string.IsNullOrEmpty(Notes))
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int retrievedID))
                {
                    ID = retrievedID;
                }
            }

            catch (Exception ex)
            {
                string Error = ex.Message;
            }

            finally
            {
                connection.Close();
            }

            return ID ;
        }


        public static bool UpdateLicense(int LicenseID,
                int ApplicationID, int DriverID, int LicenseClassID,
                DateTime IssueDate, DateTime ExpirationDate,
                string Notes, double PaidFees, bool IsActive, int IssueReason,
                int CreatedByUserID)
        {
            int rowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Licenses
                            SET ApplicationID = @ApplicationID,
                                DriverID = @DriverID,
                                LicenseClassID = @LicenseClassID,
                                IssueDate = @IssueDate,
                                ExpirationDate = @ExpirationDate,
                                Notes = @Notes,
                                PaidFees = @PaidFees,
                                IsActive = @IsActive,
                                IssueReason = @IssueReason, 
                                CreatedByUserID = @CreatedByUserID
                            WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            if (!string.IsNullOrEmpty(Notes))
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            try
            {
                connection.Open();

                rowsEffected = command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                string Error = ex.Message;
            }

            finally
            {
                connection.Close();
            }

            return rowsEffected > 0;
        }


        public static bool DeleteLicense(int licenseID)
        {
            int rowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE FROM Licenses 
                            WHERE licenseID = @licenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();

                rowsEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }


            return rowsEffected > 0;
        }


        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT *  FROM Licenses";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }


        public static DataTable GetPersonLicenses(int PersonID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT L.LicenseID, LicenseClasses.ClassName, IssueReason =
		                            CASE
		                            WHEN L.IssueReason = 1 THEN 'First Time'
		                            WHEN L.IssueReason = 2 THEN 'Renew'
		                            WHEN L.IssueReason = 3 THEN 'Damaged Replace'
		                            WHEN L.IssueReason = 1 THEN 'Lost Replace'
		                            END,
	                            L.ExpirationDate, L.IsActive, L.CreatedByUserID
                            FROM Licenses AS L
                            INNER JOIN Drivers 
	                            ON L.DriverID = Drivers.DriverID
                            INNER JOIN LicenseClasses 
	                            ON L.LicenseClassID = LicenseClasses.LicenseClassID
                            WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(@"PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }
   
    
        public static bool IsDetained(int licenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 * FROM DetainedLicenses
                            WHERE LicenseID = @licenseID AND IsReleased = 0
                            ORDER BY DetainID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(@"licenseID", licenseID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isFound = result != null;
            }
            catch(Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
