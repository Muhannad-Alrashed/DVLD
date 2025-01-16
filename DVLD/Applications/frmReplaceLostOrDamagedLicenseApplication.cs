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
using static DVLD_Business.clsLicense;

namespace DVLD.Applications
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private clsLicense _License;
        private clsLicense _NewLicense;
        private clsApplication _Application;

        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }


        private void _SetupReplaceApplication()
        {
            _Application = new clsApplication
            {
                PersonID = _License.DriverInfo.PersonID,
                CreatedByUserID = clsGlobal.CurrentUser.UserID,
                ApplicationDate = DateTime.Now,
                LastStatusDate = DateTime.Now,
            };

            if (rbLostLicense.Checked)
            {
                _Application.ApplicationTypeID = 3;
                _Application.PaidMoney = clsApplicationType.Find(3).Fees;
            }
            else
            {
                _Application.ApplicationTypeID = 4;
                _Application.PaidMoney = clsApplicationType.Find(4).Fees;
            }
        }

        private bool _CreateNewLicense()
        {
            _NewLicense = _License;
            _NewLicense.ApplicationID = _Application.ID;
            _NewLicense.IssueDate = DateTime.Now;
            _NewLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (rbLostLicense.Checked)
                _NewLicense.IssueReason = enIssueReason.LostReplacement;
            else
                _NewLicense.IssueReason = enIssueReason.DamagedReplacement;

            if (_NewLicense.Save())
            {
                _License.IsActive = false;
                _License.Save();

                btnIssueReplacement.Enabled = false;
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
                lblApplicationFees.Text = _Application.PaidMoney.ToString();
                lblOldLicenseID.Text = _License.LicenseID.ToString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserID.ToString();
            }
            else
            {
                lblApplicationDate.Text = "[???]";
                lblApplicationFees.Text = "[???]";
                lblOldLicenseID.Text = "[???]";
                lblCreatedByUser.Text = "[???]";
                lblRreplacedLicenseID.Text = "[???]";
                lblApplicationID.Text = "[???]";
            }
        }

        private void _LoadData()
        {
            llShowLicenseInfo.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            btnIssueReplacement.Enabled = false;
            _Application = null;

            if (_License != null)
            {
                if (!_License.IsActive)
                {
                    MessageBox.Show("Can't Replace an Inactive License", "Not Allowed",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(_License.IsDetained())
                {
                    MessageBox.Show("Can't Replace a Detained License", "Not Allowed",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _Application = new clsApplication();
                    btnIssueReplacement.Enabled = true;
                }
                llShowLicenseInfo.Enabled = true;
                llShowLicenseHistory.Enabled = true;
            }

            _DisplayApplicationBasicInfo();
        }


        //--------------------------------------


        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if(!rbLostLicense.Checked && !rbDamagedLicense.Checked)
            {
                MessageBox.Show("Must Choose The Reason For Replacement");
                return;
            }

            if(MessageBox.Show("Are You sure you want to issue a replacement?",
                "Confirm", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            _SetupReplaceApplication();

            if (_Application.Save())
            {
                MessageBox.Show("Replace License Application Created");
                lblApplicationID.Text = _Application.ID.ToString();

                _Application.Status = enStatus.Complete;
                _Application.Save();

                if (_CreateNewLicense())
                {
                    MessageBox.Show("New Replacement License Issued Successfully");
                    lblRreplacedLicenseID.Text = _NewLicense.LicenseID.ToString();
                }
                else
                {
                    MessageBox.Show("License Replacing Failed");
                }
            }
            else
                MessageBox.Show("Application Creation Failed");
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm;
            if (_NewLicense != null)
            {
                frm = new frmShowLicenseInfo(_NewLicense.LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("There isn't a New License Yet");
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
                Form frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
                frm.ShowDialog();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            _License = clsLicense.FindByLicenseID(LicenseID);

            if (_License != null)
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(LicenseID);
            else    
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(-1);

            _LoadData();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}
