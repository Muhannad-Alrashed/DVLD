using DVLD.Core;
using DVLD.Licenses;
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
using static DVLD_Business.clsApplication;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Applications.Detains
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        private clsDetainedLicense _DetainedLicense;
        private clsLicense _License;
        private clsApplication _ReleaseLicenseApplication;

        public frmReleaseDetainedLicenseApplication(int detainedID)
        {
            InitializeComponent();

            _DetainedLicense = clsDetainedLicense.FindByDetainID(detainedID);
            if(_DetainedLicense != null)
            {
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                _License = clsLicense.FindByLicenseID(_DetainedLicense.LicenseID);
            }
        }


        private void _DisplayReleaseApplicationBasicInfo()
        {
            btnRelease.Enabled = false;
            string UserMessage = string.Empty;

            if(_License != null)
            {
                if (!_License.IsActive)
                {
                    UserMessage = "This License Is Not Active";
                }
                else if (_DetainedLicense == null)
                {
                    UserMessage = "License Is Not Detained";
                }
                else if (_DetainedLicense.IsReleased)
                {
                    UserMessage = "License Has Been Released";
                }
                else
                {
                    btnRelease.Enabled = true;

                    lblDetainID.Text = _DetainedLicense.DetainID.ToString();
                    lblDetainDate.Text = DateTime.Now.ToShortDateString();
                    lblApplicationFees.Text = clsApplicationType.Find(5).Fees.ToString();
                    lblLicenseID.Text = _License.LicenseID.ToString();
                    lblCreatedByUser.Text = clsGlobal.CurrentUser.UserID.ToString();
                    lblFineFees.Text = _DetainedLicense.FineFees.ToString();
                    lblTotalFees.Text = (clsApplicationType.Find(5).Fees + _DetainedLicense.FineFees).ToString();
                }
            }
            else
            {
                lblApplicationID.Text = "[???]";
                lblDetainID.Text = "[???]";
                lblDetainDate.Text = "[???]";        
                lblApplicationFees.Text = "[???]";   
                lblLicenseID.Text = "[???]";         
                lblCreatedByUser.Text = "[???]";     
                lblFineFees.Text = "[???]";          
                lblTotalFees.Text = "[???]";
            }

            if(UserMessage != string.Empty)
            {
                MessageBox.Show(UserMessage, "Servece Issues",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool _CreateReleaseApplication()
        {
            _ReleaseLicenseApplication = new clsApplication
            {
                PersonID = _License.DriverInfo.PersonID,
                CreatedByUserID = clsGlobal.CurrentUser.UserID,
                ApplicationTypeID = 5,
                ApplicationDate = DateTime.Now,
                LastStatusDate = DateTime.Now,
                PaidMoney = clsApplicationType.Find(5).Fees,
            };

            if (_ReleaseLicenseApplication.Save())
            {
                btnRelease.Enabled = false;
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

            if (_License != null)
            {
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_License.LicenseID);

                llShowLicenseHistory.Enabled = true;
                llShowLicenseInfo.Enabled = true;
            }

            _DisplayReleaseApplicationBasicInfo();
        }


        //-----------------------------------------


        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure you want to release detained license?",
                    "Confirm", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            if (_CreateReleaseApplication())
            {
                MessageBox.Show("License Released Successfully");

                lblApplicationID.Text = _ReleaseLicenseApplication.ApplicationTypeID.ToString();
                _ReleaseLicenseApplication.Status = enStatus.Complete;
                _ReleaseLicenseApplication.Save();
            }
            else
            {
                MessageBox.Show("License Release Failed");
            }
        }

        private void btnClose_Click (object sender, EventArgs e)
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

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            _License = clsLicense.FindByLicenseID(licenseID);
            _DetainedLicense = clsDetainedLicense.FindByLicenseID(licenseID);

            _LoadData();
        }

        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}
