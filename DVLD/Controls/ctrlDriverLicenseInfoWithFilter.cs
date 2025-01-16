using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace DVLD
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        //Custom Events
        public event Action<int> OnLicenseSelected; // Define a custom event handler
        
        protected virtual void LicenseFound(int License) // Create a protected method to be executed
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
                handler(License);
        }

        public bool FilterEnabled { get; set; }

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
            FilterEnabled = true;
        }


        public void LoadLicenseInfo(int LicenseID)
        {
            ctrlDriverLicenseInfo1.LoadLicenseInfo(LicenseID);
        }


        //-----------------------------------


        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtLicenseID.Text))
            {
                e.Cancel = true;
                txtLicenseID.Focus();
                errorProvider1.SetError(txtLicenseID, "Inter LicenseID");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, "");
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                return;
            }

            clsLicense License = clsLicense.FindByLicenseID(int.Parse(txtLicenseID.Text));
            int LicenseID = -1;

            if (License != null)
            {
                ctrlDriverLicenseInfo1.LoadLicenseInfo(License.LicenseID);
                LicenseID = License.LicenseID;
            }
            else
                ctrlDriverLicenseInfo1.LoadLicenseInfo(-1);

            if (OnLicenseSelected != null)   //  Raise the find event 
                LicenseFound(LicenseID);
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void ctrlDriverLicenseInfoWithFilter_Load(object sender, EventArgs e)
        {
            txtLicenseID.Enabled = FilterEnabled;
            btnFind.Enabled = FilterEnabled;
        }
    }
}
