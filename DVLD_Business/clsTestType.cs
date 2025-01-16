using DVLD_Business;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestType
    {
        public int ID;
        public string Title { get; set; }
        public string Description { get; set; }
        public double Fees { get; set; }


        public clsTestType()
        {
            ID = -1;
            Title = string.Empty;
            Description = string.Empty;
            Fees = 0;
        }


        private clsTestType(int ID, string Title ,string description, double Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = description;
            this.Fees = Fees;
        }


        public static clsTestType Find(int ID)
        {
            string Title = string.Empty;
            string Description = string.Empty;
            double Fees = 0;

            if (clsTestTypeData.GetByID(ID , ref Title ,ref Description, ref Fees))
                return new clsTestType(ID , Title , Description, Fees);
            else
                return null;
        }


        public bool Update()
        {
            return clsTestTypeData.Update(this.ID , this.Title ,this.Description, this.Fees);
        }


        public static DataTable ListTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }
    }
}
