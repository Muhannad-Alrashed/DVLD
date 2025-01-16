using DVLD.Applications.New_Application;
using DVLD.Core;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class ctrlScheduleTest : UserControl
    {
        private clsLocalDrivingLicenseApplication _LDLApplication;
        private clsTestAppointment _TestAppointment;
        private clsApplication _NewApplication;
        private clsPerson _Person;

        public int TestTypeID;
        public ctrlScheduleTest()
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

        private void _CreateReTakeTestApplicaion()
        {
            _NewApplication = new clsApplication();
            _NewApplication.ApplicationDate = DateTime.Now;
            _NewApplication.LastStatusDate = DateTime.Now;
            _NewApplication.Status = clsApplication.enStatus.New;
            _NewApplication.PaidMoney = clsApplicationType.Find(7).Fees;
            _NewApplication.ApplicationTypeID = 7;
            _NewApplication.PersonID = _Person.ID;
            _NewApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
        }

        private void _CreateNewTestAppointment()
        {
            if (_TestAppointment != null)
                return;

            _TestAppointment = new clsTestAppointment();
            _TestAppointment.TestTypeID = TestTypeID;
            _TestAppointment.LDLApplicationID = _LDLApplication.LDLApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _TestAppointment.IsLocked = false;
        }
       
        private void _DisplayInfo()
        {
            lblLocalDrivingLicenseAppID.Text = _LDLApplication.LDLApplicationID.ToString();
            lblDrivingClass.Text = clsLicenseClass.FindLicenseByClassID(_LDLApplication.LicenseClassID).ClassName;
            lblFullName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblFees.Text = clsTestType.Find(TestTypeID).Fees.ToString();
            if (_TestAppointment != null)
                lblTrial.Text = _TestAppointment.GetTestTrials().ToString();
            else
                lblTrial.Text = "1";

            lblRetakeTestAppID.Text = "[???]";
            lblRetakeAppFees.Text = "[$$$]";
            lblTotalFees.Text = "[$$$]";
        }

        private void _HandleScheduling()
        {
            _DisplayInfo();
            dtpTestDate.MinDate = DateTime.Now.AddDays(1);

            // Add Appointment
            if (_TestAppointment == null) 
            {
                lblUserMessage.Text = "Schedule Appointment";
                _CreateNewTestAppointment();

                // Retake Test
                DataTable dt = clsTestAppointment.ListAppointments(_LDLApplication.LDLApplicationID, TestTypeID);
                if(dt.Rows.Count > 0)
                {
                    clsTest PreviousTest = clsTest.FindByTestAppointmentID((int)dt.Rows[dt.Rows.Count - 1][0]);
                    if (PreviousTest == null || !PreviousTest.TestResult)   
                    {
                        _CreateReTakeTestApplicaion();
                        gbRetakeTestInfo.Enabled = true;
                        
                        lblRetakeAppFees.Text = clsApplicationType.Find(7).Fees.ToString();
                        _TestAppointment.PaidFees = clsApplicationType.Find(7).Fees + clsTestType.Find(TestTypeID).Fees;
                        lblTotalFees.Text = _TestAppointment.PaidFees.ToString();
                    }
                }
            }
            // Edit Appointment
            else
            {
		        // Retake Test
                if (_TestAppointment.RetakeTestApplicationID != -1)
                {
                    gbRetakeTestInfo.Enabled = true;
                    lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();

                    lblRetakeAppFees.Text = clsApplicationType.Find(7).Fees.ToString();
                    _TestAppointment.PaidFees = clsApplicationType.Find(7).Fees + clsTestType.Find(TestTypeID).Fees;
                    lblTotalFees.Text = _TestAppointment.PaidFees.ToString();
                }
		
                lblUserMessage.Text = "Update Appointment";
                dtpTestDate.Enabled = true;
                btnSave.Enabled = true;
                dtpTestDate.Value = _TestAppointment.AppointmentDate;
            }
        }

        public void LoadAppointmentInfo(int TestAppointmentID ,int LDLApplicationID)
        {
            _LoadTestImage();
            
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);
            _Person = clsPerson.FindByPersonID(_LDLApplication.ApplicationInfo.PersonID);
            _TestAppointment = clsTestAppointment.FindByTestAppointmentID(TestAppointmentID);

            _HandleScheduling();
        }


        //---------------------------------


        private void dtpTestDate_ValueChanged(object sender, EventArgs e)
        {
            if (_TestAppointment != null)
                _TestAppointment.AppointmentDate = dtpTestDate.Value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(_NewApplication != null)
            {
                if (_NewApplication.Save())
                    MessageBox.Show("ReTake Test Application Created");
                else
                    MessageBox.Show("Application Creation Failed");

                _TestAppointment.RetakeTestApplicationID = _NewApplication.ID;
            }

            if(_TestAppointment.Save())
                MessageBox.Show("Test Appointment Saved Successfully");
            else
                MessageBox.Show("Test Appointment Save Failed");
        }
    }
}
