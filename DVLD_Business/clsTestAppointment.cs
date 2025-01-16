using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LDLApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public double PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }

        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LDLApplicationID = -1;
            AppointmentDate = DateTime.MinValue;
            PaidFees = 0;
            CreatedByUserID = -1;
            IsLocked = false;
            RetakeTestApplicationID = -1;
        }

        private clsTestAppointment(int testAppointmentID, int testTypeID,
            int lDLApplicationID, DateTime appointmentDate, double paidFees,
            int createdByUserID, bool isLooked, int retakeTestApplicationID)
        {
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LDLApplicationID = lDLApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLooked;
            RetakeTestApplicationID = retakeTestApplicationID;
            Mode = enMode.Update;
        }

        public static clsTestAppointment FindByTestAppointmentID(int TestAppointmentID)
        {
            int TestTypeID = -1;
            int LDLApplicationID = -1;
            DateTime AppointmentDate = new DateTime();
            double PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLooked = false;
            int RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GetByTestAppointmentID(TestAppointmentID,
               ref TestTypeID, ref LDLApplicationID, ref AppointmentDate,
               ref PaidFees, ref CreatedByUserID, ref IsLooked,
               ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID,
               TestTypeID, LDLApplicationID, AppointmentDate,
                PaidFees, CreatedByUserID, IsLooked,
               RetakeTestApplicationID);
            }
            else
                return null;
        }

        public static clsTestAppointment FindtheOngoingAppointment(int ApplicationID)
        {
            int TestAppointmentID = -1;
            int TestTypeID = -1;
            DateTime AppointmentDate = new DateTime();
            double PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLooked = false;
            int RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GettheOngoingAppointment(ref TestAppointmentID,
               ref TestTypeID, ApplicationID, ref AppointmentDate,
               ref PaidFees, ref CreatedByUserID,ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID,
               TestTypeID, ApplicationID, AppointmentDate,
                PaidFees, CreatedByUserID, IsLooked,
               RetakeTestApplicationID);
            }
            else
                return null;
        }

        private bool _Add()
        {
            this.TestAppointmentID = clsTestAppointmentData.Add(this.TestTypeID,
                this.LDLApplicationID, this.AppointmentDate,this.PaidFees,
                this.CreatedByUserID, this.IsLocked,this.RetakeTestApplicationID);
            return this.TestAppointmentID != -1;
        }

        private bool _Update()
        {
            return clsTestAppointmentData.Update(this.TestAppointmentID,
               this.TestTypeID, this.LDLApplicationID, this.AppointmentDate,
                this.PaidFees, this.CreatedByUserID, this.IsLocked,
               this.RetakeTestApplicationID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_Add())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _Update();
                default:
                 return false;
            }
        }

        public static DataTable ListAppointments(int LDLApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.GetTestAppointments(LDLApplicationID , TestTypeID);
        }

        public static bool LockTimePassedTestAppointmentsAndCheckIfHasNew(int LDLApplicationID)
        {
            return clsTestAppointmentData.LockTimePassedTestAppointmentsAndCheckIfHasNew(LDLApplicationID);
        }

        public int GetTestTrials()
        {
            return clsTestAppointmentData.GetTestTrials(this.TestAppointmentID,this.LDLApplicationID, this.TestTypeID);
        }
    }
}
