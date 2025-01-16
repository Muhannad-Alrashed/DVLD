using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Licenses;

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        private clsPerson _Person;
        private string _FilterBy = string.Empty;
        private string _FilterValue = string.Empty;
        public frmListDrivers()
        {
            InitializeComponent();
            cbFilterBy.SelectedIndex = 0;
        }


        private void _LoadDriversList()
        {
            DataTable dt = clsDriver.ListAllDrivers();

            if (!string.IsNullOrEmpty(_FilterValue))
            {
                string filterCondition;
                if (_FilterBy == "Driver ID" || _FilterBy == "Person ID")
                    filterCondition = $"[{_FilterBy}] = {_FilterValue}";
                else
                    filterCondition = $"[{_FilterBy}] like '{_FilterValue}%'";

                DataView dv= dt.DefaultView;
                dv.RowFilter = filterCondition;
                dgvDrivers.DataSource = dv;
            }
            else
                dgvDrivers.DataSource = dt;
             
            lblRecordsCount.Text = dgvDrivers.RowCount.ToString();

            if(dgvDrivers.RowCount > 0)
            {
                dgvDrivers.Columns[2].Width = 350;
                dgvDrivers.Columns[3].Width = 175;
                dgvDrivers.Columns[4].Width = 150;
                dgvDrivers.Columns[5].Width = 150;
            }
        }


        //---------------------------------------



        private void dgvDrivers_SelectionChanged(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            _Person = clsPerson.FindByPersonID(PersonID);

            // issueInternationalLicenseToolStripMenuItem.Enabled = !_Person.HasInternationalLicense();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo(_Person.ID);
            frm.ShowDialog();
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Issue international license code here

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(_Person.ID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = true;

            switch (cbFilterBy.Text)
            {
                case "Driver ID":
                        _FilterBy = "Driver ID";
                    break;
                case "Person ID":
                        _FilterBy = "Person ID";
                    break;
                case "National No.":
                        _FilterBy = "National Number";
                    break;
                case "Full Name":
                        _FilterBy = "Driver Full Name";
                    break;
                default:
                    {
                        _FilterBy = string.Empty;
                        txtFilterValue.Visible = false;
                    }
                    break;
            }
            _LoadDriversList();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _FilterValue = txtFilterValue.Text;
            _LoadDriversList();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _LoadDriversList();
        }
    }
}
