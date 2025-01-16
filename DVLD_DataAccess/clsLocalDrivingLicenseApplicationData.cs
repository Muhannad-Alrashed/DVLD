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
    public class clsLocalDrivingLicenseApplicationData
    {


        public static bool GetByLDLApplicationID(int LDLApplicationID,
                     ref int ApplicationID , ref int LicenseClassID , ref int PassedTests )
        {
        bool IsFound = false;

        SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        string query = @"SELECT * FROM LocalDrivingLicenseApplications
                    WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID";

        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                IsFound = true;

                ApplicationID = (int)reader["ApplicationID"];
                LicenseClassID = (int)reader["LicenseClassID"];

                reader.Close();

                string query2 = @"select sum([Test Result]) as [Passed Tests]
			                    from (select
	  				                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID as [L.D.L.AppID],
					                    case
						                    when TestResult = 1 then 1
						                    else 0
						                    end as [Test Result]
					                    from LocalDrivingLicenseApplications
						                    left join TestAppointments
						                    on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
						                    left join Tests
						                    on TestAppointments.TestAppointmentID = Tests.TestAppointmentID
					                    ) Each_Test_With_Its_Result_Table
					                    Group by [L.D.L.AppID]
										having [L.D.L.AppID] = @LDLApplicationID";


                SqlCommand command2 = new SqlCommand(query2, connection);

                command2.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
                try
                {
                    object result = command2.ExecuteScalar();

                    if (result != null)
                    {
                        PassedTests = (int)result;
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message;
                }
            }
        }

        catch (Exception ex)
        {
            string Error = ex.Message;
            IsFound = false;
        }

        finally
        {
            connection.Close();
        }
            return IsFound;
        }

        
        public static bool GetByPersonID(int PersonID ,ref int LDLApplicationID,
                     ref int ApplicationID , ref int LicenseClassID , ref int PassedTests )
        {
        bool IsFound = false;

        SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        string query = @"SELECT * FROM LocalDrivingLicenseApplications AS LA
                        INNER JOIN Applications AS A ON LA.ApplicationID = A.ApplicationID
                        WHERE ApplicantPersonID = @PersonID";

        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@PersonID", PersonID);

        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                IsFound = true;

                LDLApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                ApplicationID = (int)reader["ApplicationID"];
                LicenseClassID = (int)reader["LicenseClassID"];
                    
                reader.Close();

                string query2 = @"select sum([Test Result]) as [Passed Tests]
			                    from (select
	  				                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID as [L.D.L.AppID], ApplicantPersonID,
					                    case
						                    when TestResult = 1 then 1
						                    else 0
						                    end as [Test Result]
					                    from LocalDrivingLicenseApplications
											inner join Applications
											on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
						                    left join TestAppointments
						                    on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
						                    left join Tests
						                    on TestAppointments.TestAppointmentID = Tests.TestAppointmentID
					                    ) Each_Test_With_Its_Result_Table
					                    Group by [L.D.L.AppID] , ApplicantPersonID
										having ApplicantPersonID = @PersonID";


                SqlCommand command2 = new SqlCommand(query2, connection);

                command2.Parameters.AddWithValue("@PersonID", PersonID);
                try
                {
                    object result = command2.ExecuteScalar();

                    if(result != null)
                    {
                        PassedTests = (int)result;
                    }
                }
                catch(Exception ex)
                {
                    string Error = ex.Message;
                }
            }
        }

        catch (Exception ex)
        {
            string Error = ex.Message;
            IsFound = false;
        }

        finally
        {
            connection.Close();
        }
            return IsFound;
        }


        public static int Add(int ApplicationID , int LicenseClassID)
        {
            int ID = -1;

            SqlConnection con = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications
                            (ApplicationID , LicenseClassID)
                            VALUES(@ApplicationID , @LicenseClassID);
                            SELECT SCOPE_IDENTITY()";

            SqlCommand com = new SqlCommand(query, con);

            com.Parameters.AddWithValue(@"ApplicationID", ApplicationID);
            com.Parameters.AddWithValue(@"LicenseClassID", LicenseClassID);

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


        public static bool Update(int LDLApplicationID, int LicenseClassID)
        {

            int RowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  LocalDrivingLicenseApplications  
                            set LicenseClassID = @LicenseClassID
                            where LocalDrivingLicenseApplicationID = @LDLApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(@"LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue(@"LicenseClassID", LicenseClassID);

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

            string query = @"DELETE FROM LocalDrivingLicenseApplications  
                            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(@"LocalDrivingLicenseApplicationID", ID);

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


        public static DataTable GetLDLApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select
	                        LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID as [L.D.L.AppID],
	                        ClassName as [Driving Class],
	                        NationalNo as [National Number],
	                        Firstname + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName as [Full Name],
	                        ApplicationDate as [Register Date],
	                        [Passed Tests],
	                        case 
		                        when ApplicationStatus = 1 then 'New'
		                        when ApplicationStatus = 2 then 'Canceled'
		                        when ApplicationStatus = 3 then 'Completed'
		                        else 'Unknown'
		                        end as [Status]
                        from Applications
	                        inner join LocalDrivingLicenseApplications on 
	                        Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
	                        inner join LicenseClasses on
	                        LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
	                        inner join People on 
	                        People.PersonID = Applications.ApplicantPersonID
	                        inner join 
		                        (select [L.D.L.AppID] , sum([Test Result]) as [Passed Tests]
			                        from (select
	  				                        LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID as [L.D.L.AppID],
					                        case
						                        when TestResult = 1 then 1
						                        else 0
						                        end as [Test Result]
					                        from LocalDrivingLicenseApplications
						                        left join TestAppointments
						                        on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
						                        left join Tests
						                        on TestAppointments.TestAppointmentID = Tests.TestAppointmentID
					                        ) Each_Test_With_Its_Result_Table
					                        Group by [L.D.L.AppID]
		                        ) All_Passed_Tests_For_Each_Application
	                        on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = All_Passed_Tests_For_Each_Application.[L.D.L.AppID]";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);

                reader.Close();
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
    }
}
