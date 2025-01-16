using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using DVLD.Applications.New_Application;
using DVLD.Licenses;
using DVLD.Tests;

namespace DVLD.Applications.Manage_Applications
{
    public partial class frmListLocalApplications : Form
    {
        private string _FilterBy = string.Empty;
        private string _FilterToken = string.Empty;
        private clsLocalDrivingLicenseApplication _LDLApplication = new clsLocalDrivingLicenseApplication();

        public frmListLocalApplications()
        {
            InitializeComponent();
        }


        private void _LoadApplicationsList()
        {
            DataTable dt = clsLocalDrivingLicenseApplication.ListLDLApplications();
            
            if(_FilterToken != "")
            {
                string condition;

                if (_FilterBy == "L.D.L.AppID")
                    condition = _FilterBy + "=" + _FilterToken;
                else
                    condition = $"[{_FilterBy}] like '{_FilterToken}%'";

                DataView dv = dt.DefaultView;
                dv.RowFilter = condition;
                
                dgvLocalDrivingLicenseApplications.DataSource = dv;
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
            }
            else
            {
                dgvLocalDrivingLicenseApplications.DataSource = dt;
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
            }

            dgvLocalDrivingLicenseApplications.Columns[1].Width = 300;
            dgvLocalDrivingLicenseApplications.Columns[3].Width = 400;
            dgvLocalDrivingLicenseApplications.Columns[4].Width = 200;
        }

        private void ScheduleTest(object sender, EventArgs e)
        {
            Form frm = new frmListTestAppointments(_LDLApplication.LDLApplicationID);
            frm.ShowDialog();

            _LoadApplicationsList();
        }


        //-----------------------------------------


        private void dgvLocalDrivingLicenseApplications_SelectionChanged(object sender, EventArgs e)
        {
            int ID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(ID);
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int PassedTests = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;
            clsApplication.enStatus Status = _LDLApplication.ApplicationInfo.Status;

            showDetailsToolStripMenuItem.Enabled = true;
            editToolStripMenuItem.Enabled = PassedTests < 2;
            DeleteApplicationToolStripMenuItem.Enabled = Status != clsApplication.enStatus.Complete;
            CancelApplicaitonToolStripMenuItem.Enabled = Status == clsApplication.enStatus.New;
            ScheduleTestsMenue.Enabled = Status == clsApplication.enStatus.New;
            scheduleVisionTestToolStripMenuItem.Enabled = PassedTests == 0;
            scheduleWrittenTestToolStripMenuItem.Enabled = PassedTests == 1;
            scheduleStreetTestToolStripMenuItem.Enabled = PassedTests == 2;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (PassedTests == 3 && Status == clsApplication.enStatus.New);
            showLicenseToolStripMenuItem.Enabled = Status == clsApplication.enStatus.Complete;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = Status == clsApplication.enStatus.Complete;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowApplicationDetails(_LDLApplication.LDLApplicationID);
            frm.ShowDialog();
         
            _LoadApplicationsList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateLocalLicenseApplication(_LDLApplication.LDLApplicationID);
            frm.ShowDialog();

            _LoadApplicationsList();
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to delete this Application",
                "Confirm Deletion",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.OK)
            {
                if (_LDLApplication.Delete())
                    MessageBox.Show("Application Deleted Successfully");
                else
                    MessageBox.Show("Failed to Delete Application");

            }
            _LoadApplicationsList();
        }

        private void CancelApplicaitonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_LDLApplication.ApplicationInfo.Status == clsApplication.enStatus.Canceled)
            {
                MessageBox.Show("This Application is Already Canceled");
                return;
            }
            if (MessageBox.Show("Are you sure you want to cancel this Application",
                    "Confirm Cancelation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                _LDLApplication.ApplicationInfo.Status = clsApplication.enStatus.Canceled;

                if (_LDLApplication.ApplicationInfo.Save())
                    MessageBox.Show("Application Canceled Successfully");
                else
                    MessageBox.Show("Failed to Cancel Application");
            }
            _LoadApplicationsList();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmIssueDriverLicenseFirstTime(_LDLApplication.LDLApplicationID);
            frm.ShowDialog();

            _LoadApplicationsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicense License = clsLicense.FindByApplicationID(_LDLApplication.ApplicationID);
            Form frm = new frmShowLicenseInfo(License.LicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsPerson Person = clsPerson.FindByPersonID(_LDLApplication.ApplicationInfo.PersonID);
            Form frm = new frmShowPersonLicenseHistory(Person.ID);
            frm.ShowDialog();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbStatus.Visible = false;
            txtFilterValue.Visible = true;
            txtFilterValue.Clear();

            switch (cbFilterBy.Text)
            {
                case ("None"):
                    txtFilterValue.Visible = false;
                    _FilterBy = "";
                    break;
                case ("L.D.L.AppID"):
                    _FilterBy = "L.D.L.AppID";
                    break;
                case ("National No."):
                    _FilterBy = "National Number";
                    break;
                case ("Full Name"):
                    _FilterBy = "Full Name";
                    break;
                case ("Status"):
                    txtFilterValue.Visible = false;
                    cbStatus.Visible = true;
                    cbStatus.SelectedIndex = 0;
                    _FilterBy = "Status";
                    break;
            }

            _LoadApplicationsList();
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbStatus.Text)
            {
                case ("All"):
                    _FilterToken = "";
                    break;
                case ("New"):
                    _FilterToken = "New";
                    break;
                case ("Canceled"):
                    _FilterToken = "Canceled";
                    break;
                case ("Completed"):
                    _FilterToken = "Completed";
                    break;
            }

            _LoadApplicationsList();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _FilterToken = txtFilterValue.Text;
            _LoadApplicationsList();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L.AppID")
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdateLocalLicenseApplication(-1);
            frm.ShowDialog();
            _LoadApplicationsList();
        }

        private void frmManageLocalApplications_Load(object sender, EventArgs e)
        {
            _LoadApplicationsList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
