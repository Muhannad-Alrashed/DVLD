using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
   public class clsInternationalLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int InternationalLicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public clsDriver DriverInfo;
        public int IssuedUsingLocalLicenseID { set; get; }
        public clsLicense LocalLicenseInfo { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }
        public int CreatedByUserID { set; get; }

        public clsInternationalLicense()
        {
            InternationalLicenseID  = -1;
            ApplicationID  = -1;
            DriverID  = -1;
            DriverInfo = new clsDriver();
            IssuedUsingLocalLicenseID  = -1;
            LocalLicenseInfo = new clsLicense();
            IssueDate  = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(1);
            IsActive = true;
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }

        private clsInternationalLicense(int internationalLicenseID, int applicationID,
            int driverID, int issuedUsingLocallicenseID,DateTime issueDate,
            DateTime expirationDate, bool isActive, int createdByUserID)
        {
            InternationalLicenseID = internationalLicenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            DriverInfo = clsDriver.FindByDriverID(driverID);
            IssuedUsingLocalLicenseID = issuedUsingLocallicenseID;
            LocalLicenseInfo = clsLicense.FindByLicenseID(issuedUsingLocallicenseID);
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUserID = createdByUserID;
            Mode = enMode.Update;
        }

        public static clsInternationalLicense FindByInternationalLicenseID(int internationalLicenseID)
        {
            int applicationID = -1;
            int driverID = -1;
            int issuedUsingLocalLicenesID = -1;
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            bool isActive = true;
            int createdByUserID = -1;

            if (clsInternationalLicenseData.GetByLicenseID(internationalLicenseID, ref applicationID,
               ref driverID,ref issuedUsingLocalLicenesID, ref issueDate,ref expirationDate,
               ref isActive,ref createdByUserID))
            {
                return new clsInternationalLicense(internationalLicenseID, applicationID, driverID,
                    issuedUsingLocalLicenesID, issueDate, expirationDate,  isActive, createdByUserID);
            }
            else
                return null;
        }

        public static clsInternationalLicense FindByLocalLicenseID(int issuedUsingLocalLicenesID)
        {
            int internationalLicenseID = -1;
            int applicationID = -1;
            int driverID = -1;
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            bool isActive = true;
            int createdByUserID = -1;

            if (clsInternationalLicenseData.GetByLocalLicenseID(ref internationalLicenseID, ref applicationID,
               ref driverID, issuedUsingLocalLicenesID, ref issueDate,ref expirationDate,
               ref isActive,ref createdByUserID))
            {
                return new clsInternationalLicense(internationalLicenseID, applicationID, driverID,
                    issuedUsingLocalLicenesID, issueDate, expirationDate,  isActive, createdByUserID);
            }
            else
                return null;
        }
        
        public static clsInternationalLicense FindByApplicationID(int applicationID)
        {
            int internationalLicenseID = -1;
            int driverID = -1;
            clsDriver driverInfo = new clsDriver();
            int issuedUsingLocalLicenesID = -1;
            clsLicense localLicenseInfo = new clsLicense();
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            bool isActive = true;
            int createdByUserID = -1;

            if (clsInternationalLicenseData.GetByApplicationID(ref internationalLicenseID, applicationID,
               ref driverID, ref issuedUsingLocalLicenesID, ref issueDate, ref expirationDate,
               ref isActive, ref createdByUserID))
            {
                return new clsInternationalLicense(internationalLicenseID, applicationID, driverID,
                    issuedUsingLocalLicenesID, issueDate, expirationDate, isActive, createdByUserID);
            }
            else
                return null;
        }

        private bool _AddNew()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewLicense(this.ApplicationID,
                            this.DriverID, this.IssuedUsingLocalLicenseID,this.IssueDate, 
                            this.ExpirationDate, this.IsActive,this.CreatedByUserID);

            return (this.InternationalLicenseID != -1);
        }

        private bool _Update()
        {
            return clsInternationalLicenseData.UpdateLicense(this.InternationalLicenseID,this.ApplicationID,
                            this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate,
                            this.ExpirationDate, this.IsActive, this.CreatedByUserID);
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

        public static bool Delete(int internationalLicenseID)
        {
            return clsInternationalLicenseData.DeleteLicense(internationalLicenseID);
        }

        public static DataTable ListAllInternationalLicense()
        {
            return clsInternationalLicenseData.GetAllLicenses();
        }

        public static DataTable ListPersonInternationalLicenses(int PersonID)
        {
            return clsInternationalLicenseData.GetPersonLicenses(PersonID);
        }
    }
}
