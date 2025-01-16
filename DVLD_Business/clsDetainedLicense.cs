using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public double FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }
        
        public clsDetainedLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.Now;
            FineFees = 0;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MinValue;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;
        }

        private clsDetainedLicense(int detainID, int licenseID,
           DateTime detainDate, double fineFees, int createdByUserID,
           bool isReleased, DateTime releaseDate, int releaseByUserID, int releaseApplicationID)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releaseByUserID;
            ReleaseApplicationID = releaseApplicationID;
            Mode = enMode.Update;
        }

        public static clsDetainedLicense FindByDetainID(int detainID)
        {
            int licenseID = -1;
            DateTime detainDate = DateTime.Now;
            double fineFees = 0;
            int createdByUserID = -1;
            bool isReleased = false;
            DateTime releaseDate = DateTime.Now; ;
            int releasedByUserID = -1;
            int releaseApplicationID = -1;

           if(clsDetainedLicenseData.GetByDetainID(detainID, ref licenseID,
                ref detainDate,ref fineFees, ref createdByUserID, ref isReleased,
                ref releaseDate,ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainedLicense(detainID, licenseID,detainDate, fineFees,
                    createdByUserID, isReleased,releaseDate, releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static clsDetainedLicense FindByLicenseID(int licenseID)
        {
            int detainID = -1;
            DateTime detainDate = DateTime.Now;
            double fineFees = 0;
            int createdByUserID = -1;
            bool isReleased = false;
            DateTime releaseDate = DateTime.Now; ;
            int releasedByUserID = -1;
            int releaseApplicationID = -1;

            if (clsDetainedLicenseData.GetByLicenseID(ref detainID, licenseID,
                 ref detainDate, ref fineFees, ref createdByUserID, ref isReleased,
                 ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainedLicense(detainID, licenseID, detainDate, fineFees,
                    createdByUserID, isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNew()
        {
            this.DetainID = clsDetainedLicenseData.AddNew(this.LicenseID ,this.DetainDate
               ,this.FineFees ,this.CreatedByUserID ,this.IsReleased ,this.ReleaseDate
               ,this.ReleasedByUserID ,this.ReleaseApplicationID);
            return (this.DetainID != -1);
        }

        private bool _Update()
        {
            return clsDetainedLicenseData.UpdatePerson(this.DetainID, this.LicenseID, this.DetainDate
               , this.FineFees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate
               , this.ReleasedByUserID, this.ReleaseApplicationID);
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

        public static DataTable ListDetainedLicenses()
        {
            return clsDetainedLicenseData.GetDetainedLicenses();
        }
    }
}
