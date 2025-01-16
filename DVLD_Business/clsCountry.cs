using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsCountry
        {
            public enum enMode { AddNew, Update };

            public int CountryID;
            public string Name;
            public enMode Mode;

            public clsCountry()
            {
                this.CountryID = -1;
                this.Name = "";
            }

            private clsCountry(int CountryID, string Name)
            {
                this.CountryID = CountryID;
                this.Name = Name;

            }

            public static clsCountry FindCountryByID(int CountryID)
            {

                string Name = "";

                if (!clsCountryData.GetCountryByID(CountryID, ref Name))
                    return null;
                else
                    return new clsCountry(CountryID, Name);
            }

            public static clsCountry FindCountryByName(string CountryName)
            {
                int CountryID = -1;
            if (clsCountryData.GetCountryByName(ref CountryID, CountryName))
                    return new clsCountry(CountryID, CountryName);
                else
                    return null;
            }

            private bool _AddNewCountry()
            {
                this.CountryID = clsCountryData.AddNewCountry(this.Name);

                return this.CountryID != -1;
            }

            private bool _UpdateCountry()
            {
                return clsCountryData.UpdateCountry(this.CountryID, this.Name);
            }

            public static bool DeleteCountry(int CountryID)
            {
                return clsCountryData.DeleteCountry(CountryID);
            }

            public static DataTable GetAllCountries()
            {
                return clsCountryData.GetAllCountries();
            }

            public static bool IsCountryExist(int ID)
            {
                return clsCountryData.IsCountryExist(ID);
            }

            public static bool IsCountryExist(string CountryName)
            {
                return clsCountryData.IsCountryExist(CountryName);
            }

            public bool Save()
            {
                switch (this.Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewCountry())
                        {
                            this.Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    case enMode.Update:
                        return _UpdateCountry();
                    default:
                        return false;
                }
            }
        }
    }