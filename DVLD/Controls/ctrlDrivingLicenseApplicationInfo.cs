using DVLD_Business;
using DVLD.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private int _LDLApplicationID;
        private clsLocalDrivingLicenseApplication _LDLApplication;

        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();

        }


        public void LoadDLDApplicationInfo(int LDLApplicationID)
        {
            _LDLApplicationID = LDLApplicationID;
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(_LDLApplicationID);
          
            llShowLicenceInfo.Visible = _LDLApplication.ApplicationInfo.Status == clsApplication.enStatus.Complete;

            lblLocalDrivingLicenseApplicationID.Text = _LDLApplication.LDLApplicationID.ToString();
            clsLicenseClass LicenseClass = clsLicenseClass.FindLicenseByClassID(_LDLApplication.LicenseClassID);
            lblAppliedFor.Text = LicenseClass.ClassName;
            lblPassedTests.Text = _LDLApplication.PassedTests.ToString();

            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LDLApplication.ApplicationID);
        }


        //----------------------------------


        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense License = clsLicense.FindByApplicationID(_LDLApplication.ApplicationID);
            Form frm = new frmShowLicenseInfo(License.LicenseID);
            frm.ShowDialog();
        }
    }
}
