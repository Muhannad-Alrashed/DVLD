using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business
{
    public class clsLocalDrivingLicenseApplication
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        public clsApplication ApplicationInfo { get; set; }
        public int LDLApplicationID { get;set; }
        public int ApplicationID { get;set; }
        public int LicenseClassID { get;set; }
        public int PassedTests { get; set; }

        public clsLocalDrivingLicenseApplication()
        {
            ApplicationInfo = new clsApplication();
            LDLApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
            PassedTests = 0;
        }

        private clsLocalDrivingLicenseApplication( int LDLApplication,int ApplicationID, int licenseClassID , int PassedTests)
        {
            this.ApplicationInfo = clsApplication.FindByApplicationID(ApplicationID);
            this.LDLApplicationID = LDLApplication;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = licenseClassID;
            this.PassedTests = PassedTests;
            Mode = enMode.Update;
        }

        public static clsLocalDrivingLicenseApplication FindByLDLApplicationID(int LDLApplicationID)
        {
            int ApplicationID = -1;
            int LicenseClassID = -1;
            int PassedTests = 0;

            if (clsLocalDrivingLicenseApplicationData.GetByLDLApplicationID(LDLApplicationID,
               ref ApplicationID, ref LicenseClassID , ref PassedTests))
            {
                return new clsLocalDrivingLicenseApplication(LDLApplicationID
                    , ApplicationID, LicenseClassID , PassedTests);
            }
            else
                return null;
        }
        
        public static clsLocalDrivingLicenseApplication FindByPersonID(int PersonID)
        {
            clsApplication ApplicationInfo = new clsApplication();
            int LDLApplicationID = -1;
            int ApplicationID = -1;
            int LicenseClassID = -1;
            int PassedTests = 0;

            if (clsLocalDrivingLicenseApplicationData.GetByPersonID( PersonID,
                ref LDLApplicationID, ref ApplicationID, ref LicenseClassID, ref PassedTests))
            {
                return new clsLocalDrivingLicenseApplication(LDLApplicationID
                    , ApplicationID, LicenseClassID , PassedTests);
            }
            else
                return null;
        }

        private bool _Add()
        {
            if(this.ApplicationInfo.Save())
            {
                this.LDLApplicationID = clsLocalDrivingLicenseApplicationData.Add(
                    this.ApplicationInfo.ID, this.LicenseClassID);

                return this.LDLApplicationID != -1;
            }
            return false;
        }

        private bool _Update()
        {
            if (this.ApplicationInfo.Save())
            {
                return clsLocalDrivingLicenseApplicationData.Update(this.LDLApplicationID,
                            this.LicenseClassID);
            }
            return false;
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
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Update();
                default:
                    return false;
            }
        }

        public bool Delete()
        {
            if(clsLocalDrivingLicenseApplicationData.Delete(this.LDLApplicationID))
            {
                return this.ApplicationInfo.Delete();
            }
            else
                return false;
        }

        public static DataTable ListLDLApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetLDLApplications();
        }
    }
}
