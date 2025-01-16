using DVLD.Licenses;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Detains
{
    public partial class frmListDetainedLicenses : Form
    {
        private string _FilterBy = string.Empty;
        private string _FilterValue = string.Empty;
        private int _LicenseID;
        private int _DetainID;
        private clsLicense _License;

        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }


        private void _LoadDetainedLicenses()
        {
            DataTable dt = clsDetainedLicense.ListDetainedLicenses();

            if (!string.IsNullOrEmpty(_FilterValue))
            {
                string filterStatement;

                if (_FilterBy == "Full Name" || _FilterBy == "N.No" )
                    filterStatement = $"[{_FilterBy}] like '{_FilterValue}%'";
                else
                    filterStatement = $"[{_FilterBy}] = {_FilterValue}";

                DataView dv = dt.DefaultView;
                dv.RowFilter = filterStatement;
                dgvDetainedLicenses.DataSource = dv;
            }
            else
                dgvDetainedLicenses.DataSource = dt;
            
            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

            if (dgvDetainedLicenses.RowCount > 0)
            {
                dgvDetainedLicenses.Columns[2].Width = 160;
                dgvDetainedLicenses.Columns[5].Width = 160;
                dgvDetainedLicenses.Columns[7].Width = 300;
                dgvDetainedLicenses.Columns[8].Width = 175;
            }
        }


        //------------------------------------


        private void cmsApplications_Opening(object sender, EventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !clsDetainedLicense.FindByDetainID(_DetainID).IsReleased;
        }

        private void dgvDetainedLicenses_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvDetainedLicenses.RowCount == 0)
                _DetainID = _LicenseID = -1;
            else
            {
            _DetainID = (int)dgvDetainedLicenses.CurrentRow.Cells[0].Value;
            _LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            _License = clsLicense.FindByLicenseID(_LicenseID);
            }
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainLicense(-1);
            frm.ShowDialog();
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicenseApplication(-1);
            frm.ShowDialog();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = true;
            cbIsReleased.Visible = false;

            switch (cbFilterBy.Text)
            {
                case "None":
                    {
                        txtFilterValue.Visible = false;
                    }
                    break;
                case "Detain ID":
                    {
                        _FilterBy = "D.ID";
                    }
                    break;
                case "Is Released":
                    {
                        txtFilterValue.Visible = false;
                        cbIsReleased.Visible = true;
                        _FilterBy = "IsReleased";
                    }
                    break;
                case "National No.":
                    {
                        _FilterBy = "N.No";
                    }
                    break;
                case "Full Name":
                    {
                        _FilterBy = "Full Name";
                    }
                    break;
                case "Release Application ID":
                    {
                        _FilterBy = "ReleaseApplicationID";
                    }
                    break;
            }
            _LoadDetainedLicenses();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _FilterValue = txtFilterValue.Text;

            _LoadDetainedLicenses();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_FilterBy != "Full Name" && _FilterBy != "N.No")
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsReleased.Text)
            {
                case "All":
                    {
                        _FilterValue = "";
                    }
                    break;
                case "Released":
                    {
                        _FilterValue = "1";
                    }
                    break;
                case "Detained":
                    {
                        _FilterValue = "0";
                    }
                    break;
            }
            _LoadDetainedLicenses();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicenseApplication(_DetainID);
            frm.ShowDialog();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            txtFilterValue.Visible = false;
            _LoadDetainedLicenses();
        }
    }
}
