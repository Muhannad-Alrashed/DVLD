using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Runtime.CompilerServices;

namespace DVLD_Business
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public clsDriver DriverInfo;
        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClassID { set; get; }
        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public double PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }
        public int CreatedByUserID { set; get; }

        public clsLicense()
        {
            DriverInfo = new clsDriver();
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClassID = -1;
            LicenseClassInfo = new clsLicenseClass();
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(LicenseClassInfo.DefaultValidityLength);
            Notes = "";
            PaidFees = 0;
            IsActive = true;
            IssueReason = enIssueReason.FirstTime;
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }

        private clsLicense(int licenseID, int applicationID,
            int driverID, int licenseClassID,
            DateTime issueDate, DateTime expirationDate, string notes,
            double paidFees, bool isActive, int issueReason, int createdByUserID)
        {
            DriverInfo = clsDriver.FindByDriverID(driverID);
            LicenseID = licenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClassID = licenseClassID;
            LicenseClassInfo = clsLicenseClass.FindLicenseByClassID(licenseClassID);
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = (enIssueReason)issueReason;
            CreatedByUserID = createdByUserID;
            Mode = enMode.Update;
        }

        public static clsLicense FindByLicenseID(int licenseID)
        {
            int applicationID = -1;
            int driverID = -1;
            int licenseClassID = -1;
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            string notes = "";
            double paidFees = 0;
            bool isActive = true;
            int issueReason = 1;
            int createdByUserID = -1;

            if (clsLicenseData.GetByLicenseID(licenseID,ref applicationID,
               ref driverID,ref licenseClassID, ref issueDate,ref expirationDate,
               ref notes,ref paidFees,ref isActive,ref issueReason,ref createdByUserID))
            {
                return new clsLicense(licenseID, applicationID, driverID,
                    licenseClassID, issueDate, expirationDate, notes,
                    paidFees, isActive, issueReason, createdByUserID);
            }
            else
                return null;
        }
     
        public static clsLicense FindPersonLastLicenseOfSpecificClass(int personID, int licenseClassID)
        {
            int licenseID = -1;
            int applicationID = -1;
            int driverID = clsDriver.FindByPersonID(personID).DriverID;
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            string notes = "";
            double paidFees = 0;
            bool isActive = true;
            int issueReason = 1;
            int createdByUserID = -1;

            if (clsLicenseData.GetPersonLastLicenseOfSpecificClass(ref licenseID, ref applicationID,
               driverID, licenseClassID, ref issueDate,ref expirationDate,
               ref notes,ref paidFees,ref isActive,ref issueReason,ref createdByUserID))
            {
                return new clsLicense(licenseID, applicationID, driverID,
                    licenseClassID, issueDate, expirationDate, notes,
                    paidFees, isActive, issueReason, createdByUserID);
            }
            else
                return null;
        }
        
        public static clsLicense FindByApplicationID(int applicationID)
        {
            clsDriver driverInfo = new clsDriver();
            int licenseID = -1;
            int driverID = -1;
            int licenseClassID = -1;
            clsLicenseClass licenseClassInfo = new clsLicenseClass();
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            string notes = "";
            double paidFees = 0;
            bool isActive = true;
            int issueReason = 1;
            int createdByUserID = -1;

            if (clsLicenseData.GetByApplicationID(ref licenseID,applicationID,
               ref driverID,ref licenseClassID, ref issueDate,ref expirationDate,
               ref notes,ref paidFees,ref isActive,ref issueReason,ref createdByUserID))
            {
                return new clsLicense(licenseID, applicationID, driverID,
                    licenseClassID, issueDate, expirationDate, notes,
                    paidFees, isActive, issueReason, createdByUserID);
            }
            else
                return null;
        }

        private bool _AddNew()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID,
                this.DriverID, this.LicenseClassID,
                this.IssueDate, this.ExpirationDate, 
                this.Notes, this.PaidFees, this.IsActive, (int)this.IssueReason,
                this.CreatedByUserID);

            return (this.LicenseID != -1);
        }

        private bool _Update()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID,
                this.ApplicationID, this.DriverID, this.LicenseClassID,
                this.IssueDate, this.ExpirationDate,
                this.Notes, this.PaidFees, this.IsActive, (int)this.IssueReason,
                this.CreatedByUserID);
        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return (_Update());
                default:
                    return false;
            }
        }

        public static bool Delete(int licenseID)
        {
            return clsLicenseData.DeleteLicense(licenseID);
        }

        public static DataTable ListAllLicense()
        {
            return clsLicenseData.GetAllLicenses();
        }

        public static DataTable ListPersonLicenses(int PersonID)
        {
            return clsLicenseData.GetPersonLicenses(PersonID);
        }

        public bool HasActiveInternationalLicense()
        {
            return (clsInternationalLicense.FindByLocalLicenseID(this.LicenseID) != null &&
                    clsInternationalLicense.FindByLocalLicenseID(this.LicenseID).IsActive);
        }

        public bool IsDetained()
        {
            return clsLicenseData.IsDetained(this.LicenseID);
        }
    }
}
