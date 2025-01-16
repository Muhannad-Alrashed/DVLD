using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsUser
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;
        
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            Username = string.Empty;
            Password = string.Empty;
            IsActive = true;
            Mode = enMode.AddNew;
        }

        private clsUser(int UserID,int PersonID, string Username, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.FindByPersonID(PersonID);
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;
            this.Mode = enMode.Update;
        }

        public static clsUser Find(int UserID)
        {
            int PersonID = -1;
            string Username = string.Empty;
            string Password = string.Empty;
            bool IsActive = false;

            if (clsUserData.GetUserByID(UserID, ref PersonID, ref Username, ref Password, ref IsActive))
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }

        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string Username = string.Empty;
            string Password = string.Empty;
            bool IsActive = false;

            if (clsUserData.GetUserByPersonID(ref UserID, PersonID, ref Username, ref Password, ref IsActive))
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }

        public static clsUser FindByUsername(string Username)
        {
            int UserID = -1;
            int PersonID = -1;
            string Password = string.Empty;
            bool IsActive = false;

            if (clsUserData.GetUserByUsername(ref UserID, ref PersonID, Username ,
                                                ref Password, ref IsActive))
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }

        public static clsUser FindByUsernameAndPassword(string Username , string Password)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;

            if (clsUserData.GetUserByUsernameAndPassword(ref UserID, ref PersonID, Username, Password, ref IsActive))
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }

        private bool _Add()
        {
            this.UserID = clsUserData.AddUser(this.PersonID, this.Username,
                                                this.Password, this.IsActive);
            return this.UserID != -1;
        }

        private bool _Update()
        {
            return clsUserData.UpdateUser(this.UserID , this.PersonID, this.Username,
                                                this.Password, this.IsActive);
        }

        public bool Delete()
        {
            return clsUserData.DeleteUser(this.UserID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_Add())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    
        public static DataTable ListUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool IsUserExist(string Username , string Password = "")
        {
            if (Password != "")
                return FindByUsernameAndPassword(Username ,Password) != null;
            else
                return FindByUsername(Username) != null;
        }
    }
}
