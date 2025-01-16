using DVLD_Business;
using DVLD.Core;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Licenses
{
    public partial class frmIssueDriverLicenseFirstTime : Form
    {
        private int _LDLApplicationID;
        private clsLocalDrivingLicenseApplication _LDLApplication;

        public frmIssueDriverLicenseFirstTime(int  LDLApplicationID)
        {
            InitializeComponent();
            _LDLApplicationID = LDLApplicationID;
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            clsLicense License = new clsLicense
            {
                ApplicationID = _LDLApplication.ApplicationID,
                LicenseClassID = _LDLApplication.LicenseClassID,
                LicenseClassInfo = clsLicenseClass.FindLicenseByClassID(_LDLApplication.LicenseClassID),
                IssueDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.FindLicenseByClassID(_LDLApplication.LicenseClassID).DefaultValidityLength),
                Notes = txtNotes.Text,
                PaidFees = _LDLApplication.ApplicationInfo.PaidMoney,
                IsActive = true,
                IssueReason = clsLicense.enIssueReason.FirstTime,
                CreatedByUserID = clsGlobal.CurrentUser.UserID
            };

            clsDriver NewDriver = clsDriver.FindByPersonID(License.DriverInfo.PersonID) ?? new clsDriver
            {
                PersonID = _LDLApplication.ApplicationInfo.PersonID,
                CreatedDate = DateTime.Now,
                CreatedByUserID = clsGlobal.CurrentUser.UserID
            };

            if (!NewDriver.Add())
            {
                MessageBox.Show("New Driver Addding Failed");
                return;
            }
            else
            {
                MessageBox.Show("New Driver Added Successfully");
            }

            License.DriverInfo = clsDriver.FindByDriverID(NewDriver.DriverID);
            License.DriverID = NewDriver.DriverID;

            if (License.Save())
            {
                MessageBox.Show("New License Issued Successfully");
                _LDLApplication.ApplicationInfo.Status = clsApplication.enStatus.Complete;
                _LDLApplication.Save();
            }
            else
                MessageBox.Show("New License Issueing Fialed");

            this.Refresh();
        }


        private void frmIssueDriverLicenseFirstTime_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadDLDApplicationInfo(_LDLApplicationID);
        }
    }
}
