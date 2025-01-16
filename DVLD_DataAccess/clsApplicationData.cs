using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {

        public static bool GetByApplicationID(int ID, ref int PersonID,
            ref int CreatedByUserID, ref int ApplicationTypeID, ref DateTime ApplicationDate,
            ref DateTime LastStatusDate,ref int Status, ref double PaidMoney)
        {
            bool isFound = false;

            SqlConnection con = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Applications
                            WHERE ApplicationID = @ApplicationID";

            SqlCommand com = new SqlCommand(query, con);

            com.Parameters.AddWithValue("@ApplicationID", ID);

            try
            {
                con.Open();

                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["ApplicantPersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    Status = Convert.ToInt16(reader["ApplicationStatus"]);
                    PaidMoney = Convert.ToDouble(reader["PaidFees"]);
                    reader.Close();
                }
            }   
            catch(Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                con.Close();
            }
                            
            return isFound;
        }


        public static bool GetByPersonID(ref int ID, int PersonID,
            ref int CreatedByUserID, ref int ApplicationTypeID, ref DateTime ApplicationDate,
            ref DateTime LastStatusDate,ref int Status, ref double PaidMoney)
        {
            bool isFound = false;

            SqlConnection con = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Applications
                            WHERE ApplicantPersonID = @ApplicantPersonID";

            SqlCommand com = new SqlCommand(query, con);

            com.Parameters.AddWithValue("@ApplicantPersonID", PersonID);

            try
            {
                con.Open();

                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ID = (int)reader["ApplicationID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    Status = (int)reader["ApplicationStatus"];
                    PaidMoney = Convert.ToDouble(reader["PaidFees"]);
                   
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return isFound;
        }


        public static int AddNewApplication(int PersonID , int UserID ,
                            int ApplicationTypeID ,int Status, double PaidMoney)
        {
            int ID = -1;


            SqlConnection con = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Applications (ApplicantPersonID , CreatedByUserID,
                                ApplicationTypeID ,ApplicationDate, LastStatusDate,ApplicationStatus,PaidFees)
                            VALUES(@ApplicantPersonID , @CreatedByUserID, @ApplicationTypeID ,
                                    @ApplicationDate, @LastStatusDate,@ApplicationStatus,@PaidFees);
                            SELECT SCOPE_IDENTITY()";

            SqlCommand com = new SqlCommand(query, con);

            com.Parameters.AddWithValue(@"ApplicantPersonID", PersonID);
            com.Parameters.AddWithValue(@"CreatedByUserID", UserID);
            com.Parameters.AddWithValue(@"ApplicationTypeID", ApplicationTypeID);
            com.Parameters.AddWithValue(@"ApplicationDate", DateTime.Now);
            com.Parameters.AddWithValue(@"LastStatusDate", DateTime.Now);
            com.Parameters.AddWithValue(@"ApplicationStatus", Status);
            com.Parameters.AddWithValue(@"PaidFees", PaidMoney);

            try
            {
                con.Open();

                object result = com.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int retrievedID))
                    ID = retrievedID;

            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return ID;
        }


        public static bool UpdateApplication(int ID, int PersonID, int UserID,
                    int ApplicationTypeID, DateTime ApplicationDate,
                    DateTime LastStatusDate,int Status, double PaidMoney)
        {
            int RowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  ApplicationsView  
                            set ApplicantPersonID = @PersonID,
                                CreatedByUserID = @UserID,
                                ApplicationTypeID = @ApplicationTypeID,
                                ApplicationDate = @ApplicationDate,
                                LastStatusDate = @LastStatusDate, 
                                ApplicationStatus = @ApplicationStatus,
                                PaidFees = @PaidFees
                            where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(@"ApplicationID", ID);
            command.Parameters.AddWithValue(@"PersonID", PersonID);
            command.Parameters.AddWithValue(@"UserID", UserID);
            command.Parameters.AddWithValue(@"ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue(@"ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue(@"LastStatusDate", DateTime.Now);
            command.Parameters.AddWithValue(@"ApplicationStatus", Status);
            command.Parameters.AddWithValue(@"PaidFees", PaidMoney);

            try
            {
                connection.Open();

                RowsEffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return RowsEffected > 0;
        }


        public static bool Delete(int ID)
        {
            int RowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE FROM Applications  
                            where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(@"ApplicationID", ID);

            try
            {
                connection.Open();

                RowsEffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return RowsEffected > 0;
        }


        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Applications";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);

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
    }
}
