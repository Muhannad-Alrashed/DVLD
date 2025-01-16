using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsDetainedLicenseData
    {
        public static bool GetByDetainID(int detainID,ref int licenseID,
         ref  DateTime detainDate,ref double fineFees,ref int createdByUserID,
         ref  bool isReleased,ref DateTime releaseDate,ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM DetainedLicenses
                                WHERE detainID = @detainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@detainID", detainID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    licenseID = (int)reader["licenseID"];
                    detainDate = (DateTime)reader["detainDate"];
                    fineFees = Convert.ToDouble(reader["fineFees"]);
                    createdByUserID = (int)reader["createdByUserID"];
                    isReleased = Convert.ToBoolean(reader["isReleased"]);

                    if (reader["releaseDate"] != DBNull.Value)
                        releaseDate = (DateTime)reader["releaseDate"];
                    else
                        releaseDate = DateTime.MinValue;

                    if (reader["releasedByUserID"] != DBNull.Value)
                        releasedByUserID = (int)reader["releasedByUserID"];
                    else
                        releasedByUserID = -1;

                    if (reader["releaseApplicationID"] != DBNull.Value)
                        releaseApplicationID = (int)reader["releaseApplicationID"];
                    else
                        releaseApplicationID = -1;
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


        public static bool GetByLicenseID(ref int detainID,int licenseID,
         ref  DateTime detainDate,ref double fineFees,ref int createdByUserID,
         ref  bool isReleased,ref DateTime releaseDate,ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 * FROM DetainedLicenses
                                WHERE licenseID = @licenseID
                                ORDER BY DetainID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    detainID = (int)reader["detainID"];
                    detainDate = (DateTime)reader["detainDate"];
                    fineFees = Convert.ToDouble(reader["fineFees"]);
                    createdByUserID = (int)reader["createdByUserID"];
                    isReleased = Convert.ToBoolean(reader["isReleased"]);

                    if (reader["releaseDate"] != DBNull.Value)
                        releaseDate = (DateTime)reader["releaseDate"];
                    else
                        releaseDate = DateTime.MinValue;

                    if (reader["releasedByUserID"] != DBNull.Value)
                        releasedByUserID = (int)reader["releasedByUserID"];
                    else
                        releasedByUserID = -1;

                    if (reader["releaseApplicationID"] != DBNull.Value)
                        releaseApplicationID = (int)reader["releaseApplicationID"];
                    else
                        releaseApplicationID = -1;
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


        public static int AddNew(int licenseID , DateTime detainDate,
               double fineFees , int createdByUserID , bool isReleased , DateTime releaseDate,
               int releasedByUserID , int releaseApplicationID )
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO DetainedLicenses
                                (licenseID, detainDate, fineFees, createdByUserID, isReleased, releaseDate,
                                releasedByUserID, releaseApplicationID)
                            VALUES
                                (@licenseID, @detainDate, @fineFees, @createdByUserID, @isReleased, @releaseDate,
                                @releasedByUserID, @releaseApplicationID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);
            command.Parameters.AddWithValue("@detainDate" , detainDate);
            command.Parameters.AddWithValue("@fineFees" , fineFees);
            command.Parameters.AddWithValue("@createdByUserID" , createdByUserID);
            command.Parameters.AddWithValue("@isReleased" ,isReleased);

            if (releaseDate != DateTime.MinValue)
                command.Parameters.AddWithValue("@releaseDate", releaseDate);
            else
                command.Parameters.AddWithValue("@releaseDate", DBNull.Value);

            if (releasedByUserID != -1)
                command.Parameters.AddWithValue("@releasedByUserID", releasedByUserID);
            else
                command.Parameters.AddWithValue("@releasedByUserID", DBNull.Value);

            if (releaseApplicationID != -1)
                command.Parameters.AddWithValue("@releaseApplicationID", releaseApplicationID);
            else
                command.Parameters.AddWithValue("@releaseApplicationID", DBNull.Value);

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

            return ID;
        }


        public static bool UpdatePerson(int detainID , int licenseID , DateTime detainDate,
               double fineFees , int createdByUserID , bool isReleased , DateTime releaseDate,
               int releasedByUserID , int releaseApplicationID )
        {
            int rowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE  DetainedLicenses
                            SET licenseID  = @licenseID,
                                detainDate = @detainDate,
                                fineFees  = @fineFees,
                                createdByUserID  = @createdByUserID,
                                isReleased  = @isReleased,
                                releaseDate  = @releaseDate,
                                releasedByUserID = @releasedByUserID,
                                releasedApplicationID = @releasedApplicationID
                            WEHERE
                                detainID = @detainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@detainID", detainID);
            command.Parameters.AddWithValue("@licenseID", licenseID);
            command.Parameters.AddWithValue("@detainDate", detainDate);
            command.Parameters.AddWithValue("@fineFees", fineFees);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@isReleased", isReleased);

            if (releaseDate != DateTime.MinValue)
                command.Parameters.AddWithValue("@releaseDate", releaseDate);
            else
                command.Parameters.AddWithValue("@releaseDate", DBNull.Value);

            if (releasedByUserID != -1)
                command.Parameters.AddWithValue("@releasedByUserID", releasedByUserID);
            else
                command.Parameters.AddWithValue("@releasedByUserID", DBNull.Value);

            if (releaseApplicationID != -1)
                command.Parameters.AddWithValue("@releaseApplicationID", releaseApplicationID);
            else
                command.Parameters.AddWithValue("@releaseApplicationID", DBNull.Value);

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


        public static DataTable GetDetainedLicenses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT DetainID [D.ID],Licenses.LicenseID [L.ID],DetainDate [D.Date],
                                IsReleased, FineFees,ReleaseDate, NationalNo [N.No],
                                FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName [Full Name],
                                ReleaseApplicationID
                            FROM DetainedLicenses
                                INNER JOIN Licenses
                                ON Licenses.LicenseID = DetainedLicenses.LicenseID
                                INNER JOIN Drivers
                                ON Licenses.DriverID = Drivers.DriverID
                                INNER JOIN People
                                ON Drivers.PersonID = People.PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
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

            return dt;
        }
    }
}
