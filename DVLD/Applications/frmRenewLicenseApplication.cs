using DVLD.Core;
using DVLD.Licenses;
using DVLD_Business;
using Microsoft.Win32;
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
using static DVLD_Business.clsLicense;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Applications
{
    public partial class frmRenewLicenseApplication : Form
    {
        private clsLicense _License;
        private clsLicense _NewLicense;
        private clsApplication _Application;

        public frmRenewLicenseApplication()
        {
            InitializeComponent();
        }


        private void _SetupRenewApplication()
        {
            _Application = new clsApplication
            {
                PersonID = _License.DriverInfo.PersonID,
                CreatedByUserID = clsGlobal.CurrentUser.UserID,
                ApplicationTypeID = 2,
                ApplicationDate = DateTime.Now,
                LastStatusDate = DateTime.Now,
                PaidMoney = clsApplicationType.Find(2).Fees,
            };
        }

        private bool _CreateNewLicense()
        {
            _NewLicense = new clsLicense
            {
                DriverInfo = _License.DriverInfo,
                ApplicationID = _Application.ID,
                DriverID = _License.DriverID,
                LicenseClassID = _License.LicenseClassID,
                LicenseClassInfo = _License.LicenseClassInfo,
                IssueDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddYears(10),
                Notes = txtNotes.Text,
                PaidFees = _License.LicenseClassInfo.ClassFees,
                IsActive = true,
                IssueReason = enIssueReason.Renew,
                CreatedByUserID = clsGlobal.CurrentUser.UserID,
            };

            if (_NewLicense.Save())
            {
                _License.IsActive = false;
                _License.Save();

                btnRenewLicense.Enabled = false;
                return true;
            }
            else
                return false;
        }

        private void _DisplayApplicationBasicInfo()
        {
            if (_License != null && _Application != null)
            {
                lblApplicationDate.Text = _Application.ApplicationDate.ToShortDateString();
                lblIssueDate.Text = DateTime.Now.ToShortDateString();
                lblApplicationFees.Text = _Application.PaidMoney.ToString();
                lblLicenseFees.Text = _License.LicenseClassInfo.ClassFees.ToString();
                lblOldLicenseID.Text = _License.LicenseID.ToString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserID.ToString();
                lblTotalFees.Text = (_Application.PaidMoney + _License.LicenseClassInfo.ClassFees).ToString();
            }
            else
            {
                lblApplicationDate.Text = "[???]";      
                lblIssueDate.Text = "[???]";            
                lblApplicationFees.Text = "[???]";      
                lblLicenseFees.Text = "[???]";          
                lblOldLicenseID.Text = "[???]";         
                lblCreatedByUser.Text = "[???]";        
                lblTotalFees.Text = "[???]";            
                
                lblApplicationID.Text = "[???]";        
                lblRenewedLicenseID.Text = "[???]";     
                lblExpirationDate.Text = "[???]";       
            }
        }

        private void _LoadData()
        {
            llShowLicenseInfo.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            btnRenewLicense.Enabled = false;
            _Application = null;

            string userMessage = string.Empty;
            if (_License != null)
            {
                if (!_License.IsActive)
                {
                    userMessage = "Can't Renew an Inactive License";
                }
                else if (_License.ExpirationDate > DateTime.Now)
                {
                    userMessage = $@"Can't Renew License With ID: {_License.LicenseID}, It's Expiration
                                Date is: {_License.ExpirationDate}";
                }
                else if (_License.IsDetained())
                {
                    userMessage = "Can't Renew Detained License. Release It First";
                }
                else
                {
                    _Application = new clsApplication();
                    btnRenewLicense.Enabled = true;
                }
                llShowLicenseInfo.Enabled = true;
                llShowLicenseHistory.Enabled = true;
            }

            if (userMessage != string.Empty)
            {
                MessageBox.Show(userMessage, "Not Allowed",
                    MessageBoxButtons.OK,MessageBoxIcon.Information);
            }

            _DisplayApplicationBasicInfo();
        }


        //-----------------------------------------


        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            _SetupRenewApplication();

            if (MessageBox.Show("Are You sure you want to renew license?",
                    "Confirm", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            if (_Application.Save())
            {
                MessageBox.Show("Renew License Application Created");
                lblApplicationID.Text = _Application.ID.ToString();

                _Application.Status = enStatus.Complete;
                _Application.Save();

                if (_CreateNewLicense())
                {
                    MessageBox.Show("License Has Renewed Successfully");
                    lblRenewedLicenseID.Text = _NewLicense.LicenseID.ToString();
                    lblExpirationDate.Text = _NewLicense.ExpirationDate.ToShortDateString();
                }
                else
                {
                    MessageBox.Show("License Renewing Failed");
                }
            }
            else
                MessageBox.Show("Application Creation Failed");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, EventArgs e)
        {
            Form frm;
            if(_NewLicense != null)
            {
                frm = new frmShowLicenseInfo(_NewLicense.LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("There isn't a New License Yet");
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj )
        {
            _License = clsLicense.FindByLicenseID(obj);

            if(_License != null)
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(obj);
            else
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(-1);

            _LoadData();
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}
