using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Licenses;
using DVLD_Business;
namespace DVLD
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _LicenseID;
        private int _InternationalLicenseID;

        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }


        public void ShowDriverLicenses(int PersonID)
        {
            dgvLocalLicensesHistory.DataSource = clsLicense.ListPersonLicenses(PersonID);
            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.RowCount.ToString();

            dgvInternationalLicensesHistory.DataSource = clsInternationalLicense.ListPersonInternationalLicenses(PersonID);  
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.RowCount.ToString();

            if ( dgvLocalLicensesHistory.RowCount > 0)
            {
                dgvLocalLicensesHistory.Columns[1].Width = 275;
                dgvLocalLicensesHistory.Columns[2].Width = 150;
                dgvLocalLicensesHistory.Columns[3].Width = 175;
                dgvLocalLicensesHistory.Columns[5].Width = 150;
            }

            if (dgvInternationalLicensesHistory.RowCount > 0)
            {
                dgvInternationalLicensesHistory.Columns[3].Width = 275;
                dgvInternationalLicensesHistory.Columns[5].Width = 175;
                dgvInternationalLicensesHistory.Columns[7].Width = 200;
            }
        }


        //-------------------------------------


        private void dgvLocalLicensesHistory_SelectionChanged(object sender, EventArgs e)
        {
            _LicenseID = (int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value;
        }

        private void dgvInternationalLicensesHistory_SelectionChanged(object sender, EventArgs e)
        {
            _InternationalLicenseID = (int)dgvInternationalLicensesHistory.CurrentRow.Cells[2].Value;
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void InternationalLicenseHistorytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
