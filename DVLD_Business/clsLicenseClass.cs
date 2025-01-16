using DVLD_DataAccess;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLicenseClass
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public double ClassFees { set; get; }

        public clsLicenseClass()

        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;

            Mode = enMode.AddNew;

        }

        
        private clsLicenseClass(int licenseClassID, string className,
            string classDescription, byte minimumAllowedAge,
            byte defaultValidityLength, double classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = className;
            ClassDescription = classDescription;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
            Mode = enMode.Update;
        }


        public static clsLicenseClass FindLicenseByClassName(string ClassName)
        {
            int LicenseClassID = -1;
            string ClassDescription = "";
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            double ClassFees = 0;

            if (clsLicenseClassData.GetByClassName(ref LicenseClassID , 
                ClassName , ref ClassDescription, ref MinimumAllowedAge ,
                ref DefaultValidityLength , ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                 MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }


        public static clsLicenseClass FindLicenseByClassID(int  LicenseClassID)
        {
            string ClassName = "";
            string ClassDescription = "";
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            double ClassFees = 0;

            if (clsLicenseClassData.GetByClassID(LicenseClassID , ref ClassName
                , ref ClassDescription, ref MinimumAllowedAge ,
                ref DefaultValidityLength , ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                 MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }


        public static DataTable ListLicenseClasses()
        {
            return clsLicenseClassData.GetLicenseClasses();
        }
    }
}
