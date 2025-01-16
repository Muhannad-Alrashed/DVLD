using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;

        public int ID {get; set; }
        public string NationalNo {get; set; }
        public string FirstName {get; set; }
        public string SecondName {get; set; }
        public string ThirdName {get; set; }
        public string LastName {get; set; }
        public DateTime DateOfBirth {get; set; }
        public short Gender {get; set; }
        public string Address {get; set; }
        public string Phone {get; set; }
        public string Email {get; set; }
        public int CountryID {get; set; }
        public string ImagePath { get; set; }

        public clsPerson()
        {
        this.ID = -1;
        this.NationalNo = "";
        this.FirstName = "";
        this.SecondName = "";
        this.ThirdName = "";
        this.LastName = "";
        this.DateOfBirth = DateTime.Now;
        this.Gender = 0;
        this.Address = "";
        this.Phone = "";
        this.Email = "";
        this.CountryID = -1;
        this.ImagePath = "";
        this.Mode = enMode.AddNew;
        }

        private clsPerson(int ID, string NationalNo, string FirstName,
            string SecondName, string ThirdName, string LastName,
            DateTime DateOfBirth, short Gender, string Address,
            string Phone, string Email, int CountryID, 
            string ImagePath)
        {
            this.ID = ID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName =LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;
            Mode = enMode.Update;
        }

        public static clsPerson FindByNationalNo(string NationalNo)
        {
            int ID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            string Address = "";
            string Phone = "";
            string Email = "";
            int CountryID = -1;
            string ImagePath = "";

            if (clsPersonData.GetPersonByNationalNo(ref ID,NationalNo, ref FirstName,
                ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gender,
                ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))
                return new clsPerson( ID, NationalNo,  FirstName,
                 SecondName,  ThirdName,  LastName,  DateOfBirth,  Gender,
                 Address,  Phone, Email,  CountryID,  ImagePath);
            else
                return null;
        }

        public static clsPerson FindByPersonID(int PersonID)
        {
            string NationalNo = "";
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            string Address = "";
            string Phone = "";
            string Email = "";
            int CountryID = -1;
            string ImagePath = "";

            if (clsPersonData.GetPersonByPersonID( PersonID, ref NationalNo, ref FirstName,
                ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gender,
                ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))
                return new clsPerson(PersonID, NationalNo, FirstName,
                 SecondName, ThirdName, LastName, DateOfBirth, Gender,
                 Address, Phone, Email, CountryID, ImagePath);
            else
                return null;
        }

        private bool _AddNew()
        {
            this.ID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName,
            this.SecondName, this.ThirdName, this.LastName,
            this.DateOfBirth,this.Gender, this.Address,
            this.Phone, this.Email,this.CountryID,this.ImagePath);
            return (this.ID != -1);
        }

        private bool _Update()
        {
            return clsPersonData.UpdatePerson(this.ID , this.NationalNo, this.FirstName,
            this.SecondName, this.ThirdName, this.LastName,
            this.DateOfBirth, this.Gender, this.Address,
            this.Phone, this.Email, this.CountryID, this.ImagePath);
        }

        public static DataTable ListPeople()
        {
            return clsPersonData.GetAllPoeple();
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

        public static bool Delete(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }
    }
}
