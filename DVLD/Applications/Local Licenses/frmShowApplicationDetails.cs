using DVLD_Business;
using DVLD.Core;
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

namespace DVLD.Applications.Manage_Applications
{
    public partial class frmShowApplicationDetails : Form
    {
        private int _LDLApplciationID;

        public frmShowApplicationDetails(int LDLApplicationID)
        {
            InitializeComponent();

            _LDLApplciationID = LDLApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadDLDApplicationInfo(_LDLApplciationID);
        }
    }
}
