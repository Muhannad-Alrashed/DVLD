using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplication
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        public enum enStatus { New=1, Canceled, Complete }
        public enStatus Status = enStatus.New;

        public int ID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public int ApplicationTypeID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public double PaidMoney { get; set; }

        public clsApplication()
        {
            ID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            ApplicationTypeID = -1;
            ApplicationDate = DateTime.Now;
            LastStatusDate = DateTime.Now;
            PaidMoney = 0;
            Status = enStatus.New;
        }

        private clsApplication(int iD, int personID, int createdByUserID,
            int applicationTypeID, DateTime applicationDate,
            DateTime lastStatusDate,int status, double paidMoney)
        {
            ID = iD;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            ApplicationTypeID = applicationTypeID;
            ApplicationDate = applicationDate;
            LastStatusDate = lastStatusDate;
            PaidMoney = paidMoney;
            Status = (enStatus)status;
            Mode = enMode.Update;
        }

        public static clsApplication FindByApplicationID(int ID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            int ApplicationTypeID = -1;
            DateTime ApplicationDate = DateTime.Now;
            DateTime LastStatusDate = DateTime.Now;
            int Status = 1;
            double PaidMoney = 0;

            if (clsApplicationData.GetByApplicationID(ID, ref PersonID, ref CreatedByUserID,
                ref ApplicationTypeID, ref ApplicationDate, ref LastStatusDate,
                ref Status ,ref PaidMoney))
                return new clsApplication(ID, PersonID, CreatedByUserID,
                    ApplicationTypeID, ApplicationDate, LastStatusDate,
                    Status , PaidMoney);
            else
                return null;
        }

        public static clsApplication FindByPersonID(int PersonID)
        {
            int ID = -1;
            int CreatedByUserID = -1;
            int ApplicationTypeID = -1;
            DateTime ApplicationDate = DateTime.Now;
            DateTime LastStatusDate = DateTime.Now;
            int Status = 1;
            double PaidMoney = 0;

            if (clsApplicationData.GetByPersonID(ref ID, PersonID, ref CreatedByUserID,
                ref ApplicationTypeID, ref ApplicationDate, ref LastStatusDate,ref Status, ref PaidMoney))
                return new clsApplication(ID, PersonID, CreatedByUserID,
                    ApplicationTypeID, ApplicationDate, LastStatusDate,Status, PaidMoney);
            else
                return null;
        }

        private bool _Add()
        {
            this.ID = clsApplicationData.AddNewApplication(this.PersonID,
                this.CreatedByUserID, this.ApplicationTypeID, (int)this.Status,
                this.PaidMoney);

            return this.ID != -1;
        }

        private bool _Update()
        {
            return clsApplicationData.UpdateApplication(this.ID, this.PersonID,
                this.CreatedByUserID, this.ApplicationTypeID, this.ApplicationDate,
                this.LastStatusDate, (int)this.Status, this.PaidMoney);
        }

        public bool Delete()
        {
            return clsApplicationData.Delete(this.ID);
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

        public static DataTable ListApplications()
        {
            return clsApplicationData.GetAllApplications();
        }
    }
}
