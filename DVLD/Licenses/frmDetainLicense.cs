using DVLD.Core;
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
using System.Xml.Serialization;
using static DVLD_Business.clsApplication;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Licenses
{
    public partial class frmDetainLicense : Form
    {
        private clsLicense _License;
        private clsDetainedLicense _DetainedLicense;

        public frmDetainLicense(int licenseID)
        {
            InitializeComponent();

            _License = clsLicense.FindByLicenseID(licenseID);
        }


        private bool _CreateDetainedLicense()
        {
            _DetainedLicense = new clsDetainedLicense
            {
                LicenseID = _License.LicenseID,
                DetainDate = DateTime.Now,
                FineFees = Convert.ToDouble(txtFineFees.Text),
                CreatedByUserID = clsGlobal.CurrentUser.UserID,
            };

            if (_DetainedLicense.Save())
            {
                btnDetain.Enabled = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void _LoadData()
        {
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            btnDetain.Enabled = false;

            string UserMessage = string.Empty;
            if(_License != null)
            {
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_License.LicenseID);
                clsDetainedLicense detainedLicense = clsDetainedLicense.FindByLicenseID(_License.LicenseID);

                if (!_License.IsActive)
                {
                    UserMessage = "Can't Detain an Inactive License";
                }
                else if (detainedLicense != null && !detainedLicense.IsReleased)
                {
                    UserMessage = "Can't Detain a Detained License";
                }
                else
                {
                    btnDetain.Enabled = true;
                    lblDetainDate.Text = DateTime.Now.ToShortDateString();
                    lblLicenseID.Text = _License.LicenseID.ToString();
                    lblCreatedByUser.Text = clsGlobal.CurrentUser.UserID.ToString();
                }
                llShowLicenseHistory.Enabled = true;
                llShowLicenseInfo.Enabled = true;
            }
            else
            {
                lblDetainID.Text = "[???]";      
                lblDetainDate.Text = "[???]";    
                lblLicenseID.Text = "[???]";     
                lblCreatedByUser.Text = "[???]";
            }

            if(UserMessage != string.Empty)
                MessageBox.Show(UserMessage,"Servece Issues",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //----------------------------------------


        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Inter Fine Ammount");
                return;
            }

            if (MessageBox.Show("Are You sure you want to detain this license?",
                "Confirm", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            if (_CreateDetainedLicense())
            {
                MessageBox.Show("License Was Detained Successfully");
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_License.LicenseID);
                lblDetainID.Text = _DetainedLicense.DetainID.ToString();

                txtFineFees.Enabled = false;
            }
            else
            {
                MessageBox.Show("License Detaining Failed");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_License.LicenseID);
                frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (_License == null)
                return;

            if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                e.Cancel = true;
                txtFineFees.Focus();
                errorProvider1.SetError(txtFineFees, "Inter Fine Ammount");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, "");
            }
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            _License = clsLicense.FindByLicenseID(LicenseID);

            _LoadData();
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}
