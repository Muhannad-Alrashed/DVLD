using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplicationType
    {
        public int ID;
        public string Title { get; set; }
        public double Fees { get; set; }


        public clsApplicationType()
        {
            ID = -1;
            Title = string.Empty;
            Fees = 0;
        }


        private clsApplicationType(int ID, string Title ,double Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;
        }


        public static clsApplicationType Find(int ID)
        {
            string Title = string.Empty;
            double Fees = 0;

            if (clsApplicationTypeData.GetByID(ID , ref Title , ref Fees))
                return new clsApplicationType(ID , Title , Fees);
            else
                return null;
        }


        public static clsApplicationType Find(string Title)
        {
            int ID = -1;
            double Fees = 0;

            if (clsApplicationTypeData.GetByTitle(ref ID,Title, ref Fees))
                return new clsApplicationType(ID, Title, Fees);
            else
                return null;
        }


        public bool Update()
        {
            return clsApplicationTypeData.Update(this.ID , this.Title , this.Fees);
        }


        public static DataTable ListApplicationTypes()
        {
            return clsApplicationTypeData.GetAllAplicationTypes();
        }

    }
}
