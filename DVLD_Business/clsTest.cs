using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTest
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }  
        public int CreatedByUserID { get; set; }

       public clsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1;
        }

        private clsTest(int testID, int testAppointmentID, bool testResult,
            string notes,int createdByUserID)
        {
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;
        }

        public static clsTest FindByTestAppointmentID(int TestAppointmentID)
        {
            int TestID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (clsTestData.GetByTestAppointmentID(ref TestID, TestAppointmentID,
                ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult,
                    Notes, CreatedByUserID);
            }
            else
                return null;
        }

        public bool Add()
        {
            this.TestID = clsTestData.Add(this.TestAppointmentID,
                this.TestResult, this.Notes, this.CreatedByUserID);

            return this.TestID != -1;
        }
    }
}
