using DVLD_Business;
using DVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmListTestAppointments : Form
    {
        private int _TestTypeID;
        private int _LDLApplicationID;
        private clsTestAppointment _TestAppointment;

        public frmListTestAppointments(int LDLAppliationID)
        {
            InitializeComponent();

            _LDLApplicationID = LDLAppliationID;
        }


        private void _LoadTestInfo()
        {
            clsLocalDrivingLicenseApplication LDLApplication;
            LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(_LDLApplicationID);
            
            switch (LDLApplication.PassedTests)
            {
                case 0:
                    pbTestTypeImage.Image = Resources.Vision_512;
                    lblTitle.Text = "Vision Test Application";
                    _TestTypeID = 1;
                    break;
                case 1:
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    lblTitle.Text = "Written Test Application";
                    _TestTypeID = 2;
                    break;
                case 2:
                    pbTestTypeImage.Image = Resources.Street_Test_32;
                    lblTitle.Text = "Street Test Application";
                    _TestTypeID = 3;
                    break;
            }
        }

        private void _LoadAppointments()
        {
            clsLocalDrivingLicenseApplication LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(_LDLApplicationID);
            DataTable dt = clsTestAppointment.ListAppointments(LDLApplication.LDLApplicationID, _TestTypeID);
            dgvLicenseTestAppointments.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].Width = 150;
                dgvLicenseTestAppointments.Columns[2].Width = 180;
                dgvLicenseTestAppointments.Columns[5].Width = 200;
            }

            lblRecordsCount.Text = dgvLicenseTestAppointments.RowCount.ToString();
        }


        //---------------------------------------


        private void dgvLicenseTestAppointments_SelectionChanged(object sender, EventArgs e)
        {
            int TestAppointmentID;
            if (dgvLicenseTestAppointments.RowCount != 0)
                TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            else
                TestAppointmentID = -1;

            _TestAppointment = clsTestAppointment.FindByTestAppointmentID(TestAppointmentID);
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            takeTestToolStripMenuItem.Enabled = (_TestAppointment != null && !_TestAppointment.IsLocked);
            editToolStripMenuItem.Enabled = (_TestAppointment != null && !_TestAppointment.IsLocked);
            showTestResultToolStripMenuItem.Enabled = (_TestAppointment != null && _TestAppointment.IsLocked);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTakeTest(_TestAppointment.TestAppointmentID);
            frm.ShowDialog();
            _LoadAppointments();

            this.Refresh();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmScheduleTest(_TestAppointment.TestAppointmentID, _LDLApplicationID, _TestTypeID);
            frm.ShowDialog();
            _LoadAppointments();
        }

        private void showTestResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTakeTest(_TestAppointment.TestAppointmentID);
            frm.ShowDialog();
            _LoadAppointments();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            string UserMessage = string.Empty;

            if (dgvLicenseTestAppointments.RowCount > 0)
            {
                clsTest Test = clsTest.FindByTestAppointmentID(
                    (int)dgvLicenseTestAppointments[0, dgvLicenseTestAppointments.RowCount - 1].Value);
                if (Test != null)
                    if(Test.TestResult)
                        UserMessage = "The Test Has Been Passed.\n Cann't Schedule More Tests";
            }


            clsTestAppointment ActiveAppointment = clsTestAppointment.FindtheOngoingAppointment(_LDLApplicationID);
            if(ActiveAppointment != null)
                UserMessage = "There is Already an Active Appointment.\n Cann't Schedule More Tests";


            if (!string.IsNullOrEmpty(UserMessage))
            {
                MessageBox.Show(UserMessage, "Schedule Issue",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            Form frm = new frmScheduleTest(-1, _LDLApplicationID, _TestTypeID);
            frm.ShowDialog();
            _LoadAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestInfo();
            ctrlDrivingLicenseApplicationInfo1.LoadDLDApplicationInfo(_LDLApplicationID);
            _LoadAppointments();

            if (_TestAppointment == null)
                return;

            if (!clsTestAppointment.LockTimePassedTestAppointmentsAndCheckIfHasNew(_LDLApplicationID))
            {
                MessageBox.Show("A Time Passed TestAppointment Has Been Locked\n\nSchedule A New Appointment",
                    "Time Passed Test Appointment");
            }
        }
    }
}
