using DVLD.Core;
using DVLD.Licenses;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Business.clsApplication;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Applications.New_Application
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _LicenseID;
        private clsLicense _License;
        private clsApplication _InternationalLicenseApplication;
        private clsInternationalLicense _InternationalLicense;

        public frmNewInternationalLicenseApplication(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
            _License = clsLicense.FindByLicenseID(_LicenseID);
        }


        private void _GetInternationalApplicationData()
        {
            btnIssueLicense.Enabled = false;

            if (_License != null)
            {
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_LicenseID);

                if (_License.IsDetained())
                {
                    MessageBox.Show("This License Is Detained. Can't Issue International License");
                }
                else if (_License.HasActiveInternationalLicense())
                {
                    MessageBox.Show("This License Already Hase An Active International License");
                    _InternationalLicenseApplication = clsApplication.FindByApplicationID(clsInternationalLicense.FindByLocalLicenseID(_LicenseID).ApplicationID);
                    _InternationalLicense = clsInternationalLicense.FindByLocalLicenseID(_LicenseID);
                }
                else
                {
                    btnIssueLicense.Enabled = true;
                    _InternationalLicense = null;
                    _InternationalLicenseApplication = new clsApplication
                    {
                        PersonID = _License.DriverInfo.PersonID,
                        CreatedByUserID = clsGlobal.CurrentUser.UserID,
                        ApplicationTypeID = 6,
                        ApplicationDate = DateTime.Now,
                        LastStatusDate = DateTime.Now,
                        PaidMoney = clsApplicationType.Find(6).Fees
                    };
                }
            }
            else
            {
                _InternationalLicense = null;
                _InternationalLicenseApplication = null;
            }
        }

        private void DisplayApplicationInfo()
        {
            if (_InternationalLicenseApplication != null)
            {
                lblApplicationID.Text = _InternationalLicenseApplication.ID.ToString();
                lblApplicationDate.Text = _InternationalLicenseApplication.ApplicationDate.ToShortDateString();
                lblFees.Text = _InternationalLicenseApplication.PaidMoney.ToString();
                lblLocalLicenseID.Text = _LicenseID.ToString();
            }
            else
            {
                lblApplicationID.Text = "[???]";
                lblApplicationDate.Text = "[???]";
                lblFees.Text = "[???]";
                lblLocalLicenseID.Text = "[???]";
            }

            if (_InternationalLicense != null)
            {
                lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
                lblIssueDate.Text = _InternationalLicense.IssueDate.ToShortDateString();
                lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToShortDateString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserID.ToString();
            }
            else
            {
                lblInternationalLicenseID.Text = "[???]";
                lblIssueDate.Text = "[???]";
                lblExpirationDate.Text = "[???]";
                lblCreatedByUser.Text = "[???]";
            }
        }


        //-------------------------------------


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (!_InternationalLicenseApplication.Save())
            {
                MessageBox.Show("New International License Application Creation Failed");
                return;
            }
            MessageBox.Show("New International License Application Created");

            _InternationalLicense = new clsInternationalLicense
            {
                ApplicationID = _InternationalLicenseApplication.ID,
                DriverID = _License.DriverID,
                DriverInfo = clsDriver.FindByDriverID(_License.DriverID),
                IssuedUsingLocalLicenseID = _LicenseID,
                LocalLicenseInfo = clsLicense.FindByLicenseID(_LicenseID),
                IssueDate = DateTime.Now,
                IsActive = true,
                CreatedByUserID = clsGlobal.CurrentUser.UserID
            };

            if (_InternationalLicense.Save())
            {
                MessageBox.Show("International License Issued Successfully");
                DisplayApplicationInfo();

                _InternationalLicenseApplication.Status = clsApplication.enStatus.Complete;
                _InternationalLicenseApplication.Save();
            }
            else
            {
                MessageBox.Show("International License Issueing Failed");
            }
        }

        private void llShowDriverLicenseHistory_LinkClicked(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, EventArgs e)
        {
            if(_InternationalLicense == null)
            {
                Form frm = new frmShowLicenseInfo(_LicenseID);
                frm.ShowDialog();
            }
            else
            {
                Form frm = new frmShowInternationalLicenseInfo(_InternationalLicense.InternationalLicenseID);
                frm.ShowDialog();
            }

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            _LicenseID = licenseID;
            _License = clsLicense.FindByLicenseID(_LicenseID);

            _GetInternationalApplicationData();
            DisplayApplicationInfo();
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            _GetInternationalApplicationData();
            DisplayApplicationInfo();
        }
    }
}
