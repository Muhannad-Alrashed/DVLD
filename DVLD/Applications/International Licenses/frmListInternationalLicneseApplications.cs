using DVLD.Applications.New_Application;
using DVLD.Licenses;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Manage_Applications
{
    public partial class frmListInternationalLicneseApplications : Form
    {
        private string _FilterBy = string.Empty;
        private string _FilterValue = string.Empty;
        private clsInternationalLicense _InternationalLicnese;
        private clsPerson _Person;

        public frmListInternationalLicneseApplications()
        {
            InitializeComponent();
        }


        private void _LoadInternationalLicenses()
        {
            DataTable dt = clsInternationalLicense.ListAllInternationalLicense();

            if(_FilterValue != string.Empty)
            {
                string filterCondition = "["+ _FilterBy + "] = " + _FilterValue;
                
                DataView dv = dt.DefaultView;

                dv.RowFilter = filterCondition;

                dgvInternationalLicenses.DataSource = dv;
            }
            else
            {
                dgvInternationalLicenses.DataSource = dt;
            }

            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();

            if(dgvInternationalLicenses.RowCount > 0)
            {
                dgvInternationalLicenses.Columns[0].Width = 125;
                dgvInternationalLicenses.Columns[1].Width = 75;
                dgvInternationalLicenses.Columns[2].Width = 200;
                dgvInternationalLicenses.Columns[3].Width = 300;
                dgvInternationalLicenses.Columns[4].Width = 150;
                dgvInternationalLicenses.Columns[5].Width = 150;
                dgvInternationalLicenses.Columns[6].Width = 75;
                dgvInternationalLicenses.Columns[7].Width = 175;
            }
        }


        //-----------------------------------


        private void dgvInternationalLicenses_SelectionChanged(object sender, EventArgs e)
        {
            int applicationID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;
           
            _InternationalLicnese = clsInternationalLicense.FindByApplicationID(applicationID);
            if(_InternationalLicnese != null)
                _Person = clsPerson.FindByPersonID(_InternationalLicnese.LocalLicenseInfo.DriverInfo.PersonID);
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewInternationalLicenseApplication(-1);
            frm.ShowDialog();

            _LoadInternationalLicenses();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(_Person.ID);
            frm.ShowDialog();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo(_Person.ID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo(_InternationalLicnese.InternationalLicenseID);
            frm.ShowDialog();
        }

        private void cbActiveState_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbActiveState.Text)
            {
                case "Active":
                    _FilterValue = "1";
                    break;
                case "InActive":
                    _FilterValue = "0";
                    break;
                default:
                    _FilterValue = string.Empty;
                    break;
            }

            _LoadInternationalLicenses();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsReleased.Text)
            {
                case "Yes":
                    _FilterValue = "1";
                    break;
                case "No":
                    _FilterValue = "0";
                    break;
                default:
                    _FilterValue = string.Empty;
                    break;
            }

            _LoadInternationalLicenses();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = string.Empty;
            txtFilterValue.Visible = true;
            cbIsReleased.Visible = false;
            cbActiveState.Visible = false;
            
            switch (cbFilterBy.Text)
            {
                case "None":
                    txtFilterValue.Visible = false;
                    _FilterBy = string.Empty;
                    break;
                case "International LicenseID":
                    _FilterBy = "International LicenseID";
                    break;
                case "ApplicationID":
                    _FilterBy = "ApplicationID";
                    break;
                case "DriverID":
                    _FilterBy = "DriverID";
                    break;
                case "Local LicenseID":
                    _FilterBy = "Local LicenseID";
                    break;
                case "Is Active":
                        txtFilterValue.Visible = false;
                        cbActiveState.Visible = cbFilterBy.Text == (_FilterBy = "Is Active");
                    break;
                case "Is Released":
                    txtFilterValue.Visible = false;
                    cbIsReleased.Visible = cbFilterBy.Text == (_FilterBy = ""); //tackle later
                    break;
            }

            _LoadInternationalLicenses();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _FilterValue = txtFilterValue.Text;

            _LoadInternationalLicenses();
        }

        private void txtFilterValue_KeyPress(object sender ,  KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void frmListInternationalLicesnseApplications_Load(object sender ,  EventArgs e)
        {
            _LoadInternationalLicenses();
        }
    }
}
