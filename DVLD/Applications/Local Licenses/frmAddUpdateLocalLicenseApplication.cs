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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications.New_Application
{
    public partial class frmAddUpdateLocalLicenseApplication : Form
    {
        
        private clsPerson _Person ;
        clsLocalDrivingLicenseApplication _LDLApplication;

        public frmAddUpdateLocalLicenseApplication(int LDLApplicationID)
        {
            InitializeComponent();

            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);
            if(_LDLApplication != null )
                _Person = clsPerson.FindByPersonID(_LDLApplication.ApplicationInfo.PersonID);
            else
                _LDLApplication = new clsLocalDrivingLicenseApplication();
        }


        private void _LoadLicenseClass()
        {
            DataTable dt = clsLicenseClass.ListLicenseClasses();
            foreach (DataRow row in dt.Rows)
                cbLicenseClass.Items.Add(row["ClassName"]);
        }

        private bool _ValidateInputs()
        {
            if (_Person == null)
            {
                MessageBox.Show("Choose a Person");
                return false;
            }

            if (string.IsNullOrEmpty(cbLicenseClass.Text))
            {
                MessageBox.Show("Select a License Class");
                return false;
            }

            return true;
        }

        private bool _CheckForDuplicateApplication()
        {
            clsLocalDrivingLicenseApplication oldApplication = clsLocalDrivingLicenseApplication.FindByPersonID(_Person.ID);
            
            if (oldApplication != null &&
                oldApplication.LicenseClassID == _LDLApplication.LicenseClassID &&
                oldApplication.ApplicationInfo.Status == clsApplication.enStatus.New &&
                oldApplication.ApplicationInfo.PersonID == _LDLApplication.ApplicationInfo.PersonID &&
                oldApplication.LDLApplicationID != _LDLApplication.LDLApplicationID)
            {
                MessageBox.Show("This person has applied for the same license class.\nYou can apply for another license class",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            return false;
        }

        private bool _CheckForExistingActiveLicense()
        {
            clsLicense oldLicense = clsLicense.FindPersonLastLicenseOfSpecificClass(_Person.ID, _LDLApplication.LicenseClassID);
            
            if (oldLicense != null && oldLicense.IsActive)
            {
                MessageBox.Show("This Person has an active license of this class.\nYou can apply for another license class",
                        "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            return false;
        }

        private void _FillOrUpdateApplication()
        {
            clsApplication applicationInfo = _LDLApplication.ApplicationInfo;
            applicationInfo.PersonID = _Person.ID;
            applicationInfo.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            applicationInfo.ApplicationTypeID = 1;
            applicationInfo.LastStatusDate = DateTime.Now;
            applicationInfo.PaidMoney = clsApplicationType.Find(1).Fees;

            _LDLApplication.ApplicationInfo = applicationInfo;
            _LDLApplication.LicenseClassID = clsLicenseClass.FindLicenseByClassName(cbLicenseClass.Text).LicenseClassID;
        }

        private void _LoadData()
        {
            // Load Global Data
            lblFees.Text = clsApplicationType.Find(1).Fees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.Username;

            // Load Person Data
            if (_Person != null)
                ctrlPersonCardWithFilter1.Find(_Person.ID);

            // Load Application Data (Update Mode)
            if (_LDLApplication.LDLApplicationID != -1)
            {
                lblApplicationDate.Text = _LDLApplication.ApplicationInfo.ApplicationDate.ToString();
                lblLocalDrivingLicebseApplicationID.Text = _LDLApplication.LDLApplicationID.ToString();
                clsLicenseClass licenseClass = clsLicenseClass.FindLicenseByClassID(_LDLApplication.LicenseClassID);
                cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(licenseClass.ClassName);
            }
             // Setup New Application (AddNew Mode)
            else
            {
                tcApplicationInfo.TabPages.Remove(tpApplicationInfo);
                cbLicenseClass.Items.Clear();
                _LoadLicenseClass();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblLocalDrivingLicebseApplicationID.Text = "[???]";
            }
        }


        //------------------------------------


        private void ctrlPersonCardWithFilter1_OnAddFindClicked(clsPerson obj)
        {
            if (tcApplicationInfo.TabPages.Contains(tpApplicationInfo))
                tcApplicationInfo.TabPages.Remove(tpApplicationInfo);

            _LDLApplication = new clsLocalDrivingLicenseApplication();
            _Person = ctrlPersonCardWithFilter1._RetrievedPerson;
            _LoadData();
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if(_Person != null)
            {
                if (!tcApplicationInfo.TabPages.Contains(tpApplicationInfo))
                    tcApplicationInfo.TabPages.Add(tpApplicationInfo);

                tcApplicationInfo.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("Please Select a Person");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateInputs())
                return;

            if (_CheckForDuplicateApplication())
                return;

            if (_CheckForExistingActiveLicense())
                return;

            _FillOrUpdateApplication();

            if (_LDLApplication.Save())
            {
                MessageBox.Show("Application Saved Successfully");
                _LDLApplication.ApplicationInfo.Save();
            }
            else
            {
                MessageBox.Show("Failed to save Application");
            }

            _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}
