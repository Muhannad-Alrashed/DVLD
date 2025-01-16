using DVLD.Core;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlScheduledTest : UserControl
    {
        public int TestTypeID;
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }


        private void _LoadTestImage()
        {
            switch (TestTypeID)
            {
                case 1:
                    pbTestTypeImage.Image = Resources.Vision_512;
                    break;
                case 2:
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    break;
                case 3:
                    pbTestTypeImage.Image = Resources.Street_Test_32;
                    break;
            }
        }

        public void LoadData(clsTestAppointment TestAppointment , clsTest Test)
        {
            _LoadTestImage();
            
            clsLocalDrivingLicenseApplication LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(TestAppointment.LDLApplicationID);
            clsPerson Person = clsPerson.FindByPersonID(LDLApplication.ApplicationInfo.PersonID);

            lblLocalDrivingLicenseAppID.Text = TestAppointment.LDLApplicationID.ToString();
            lblDrivingClass.Text = clsLicenseClass.FindLicenseByClassID(LDLApplication.LicenseClassID).ClassName;
            lblFullName.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " + Person.LastName;
            lblTrial.Text = TestAppointment.GetTestTrials().ToString();
            lblDate.Text = TestAppointment.AppointmentDate.ToShortDateString();
            lblFees.Text = clsTestType.Find(TestTypeID).Fees.ToString();

            if (Test != null)
                lblTestID.Text = Test.TestID.ToString();
            else
                lblTestID.Text = "Not Taken Yet";
        }   
    }
}
