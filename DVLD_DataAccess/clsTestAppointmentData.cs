using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        public static bool GetByTestAppointmentID(int TestAppointmentID,
               ref int TestTypeID, ref int LDLApplicationID, ref DateTime AppointmentDate,
               ref double PaidFees, ref int CreatedByUserID, ref bool IsLooked,
               ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestAppointments
                            WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTypeID = (int)reader["TestTypeID"];
                    LDLApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToDouble(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLooked = Convert.ToBoolean(reader["IsLocked"]);
                    if(reader["RetakeTestApplicationID"] != DBNull.Value)
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    else
                        RetakeTestApplicationID = -1;                
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
       

        public static bool GettheOngoingAppointment(ref int TestAppointmentID,
               ref int TestTypeID, int LDLApplicationID, ref DateTime AppointmentDate,
               ref double PaidFees, ref int CreatedByUserID,ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TestAppointments.* ,L.ApplicationID  FROM TestAppointments
                            inner join LocalDrivingLicenseApplications as L
                            on TestAppointments.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
                            where L.LocalDrivingLicenseApplicationID = @LDLApplicationID and IsLocked = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestTypeID = (int)reader["TestTypeID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToDouble(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    if(reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    RetakeTestApplicationID = -1;                
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


        public static int Add(int TestTypeID, int LDLApplicationID,
            DateTime AppointmentDate,double PaidFees, int CreatedByUserID,
            bool IsLocked,int RetakeTestApplicationID) 
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO TestAppointments
                            (LocalDrivingLicenseApplicationID,TestTypeID,AppointmentDate,
                            PaidFees,CreatedByUserID,Islocked,RetakeTestApplicationID)
                            VALUES(@LocalDrivingLicenseApplicationID,@TestTypeID,@AppointmentDate,
                            @PaidFees,@CreatedByUserID,@Islocked,@RetakeTestApplicationID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@Islocked", IsLocked);
            if(RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != null && int.TryParse(result.ToString(),out int retrievedID))
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


        public static bool Update(int TestAppointmentID,int TestTypeID,
            int LDLApplicationID, DateTime AppointmentDate,double PaidFees,
            int CreatedByUserID, bool IsLocked,int RetakeTestApplicationID)
        {
            int RowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE  TestAppointments  
                            set TestTypeID = @TestTypeID,
                                LocalDrivingLicenseApplicationID = @LDLApplicationID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID,
                                IsLocked = @IsLooked,
                                RetakeTestApplicationID = @RetakeTestApplicationID
                                WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsLooked", IsLocked);
            if (RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                connection.Open();

                RowsEffected = command.ExecuteNonQuery();

            }
            catch(Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return RowsEffected > 0;
        }


        public static DataTable GetTestAppointments(int LDLApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TestAppointmentID,LocalDrivingLicenseApplicationID as [LDL.AppID],
                            AppointmentDate,PaidFees, IsLocked , RetakeTestApplicationID 
                            FROM TestAppointments
                            where LocalDrivingLicenseApplicationID = @LDLApplicationID
                            and TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);
            }
            catch(Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            
            return dt;
        }
    
    
        public static bool LockTimePassedTestAppointmentsAndCheckIfHasNew(int LDLApplicationID)
        {
            bool HasNewAppointment = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE TimePassedTestAppointments
                            SET IsLocked = 1
                            WHERE IsLocked = 0 and LocalDrivingLicenseApplicationID = @LDLApplicationID;
                            SELECT NewActiveAppointment
                            FROM TimePassedTestAppointments
                            WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                HasNewAppointment = result != null;
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return HasNewAppointment;
        }
    
    
        public static int GetTestTrials(int TestAppointmentID, int LDLApplicationID, int TestTypeID)
        {
            int Trials = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT COUNT(*)  FROM TestAppointments
                            WHERE TestAppointmentID <= @TestAppointmentID
                            AND LocalDrivingLicenseApplicationID = @LDLApplicationID
                            AND TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                Trials = (int)command.ExecuteScalar();

            }
            catch(Exception ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return Trials;
        }
    }
}
