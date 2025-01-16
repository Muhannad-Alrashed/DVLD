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

namespace DVLD.Licenses
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _PersonID;

        public frmShowPersonLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }


        //-----------------------------


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.Find(_PersonID);
            ctrlDriverLicenses1.ShowDriverLicenses(_PersonID);
        }

        private void ctrlPersonCardWithFilter1_OnFindClicked(DVLD_Business.clsPerson obj)
        {
            if(obj != null)
                ctrlDriverLicenses1.ShowDriverLicenses(obj.ID);
            else
                ctrlDriverLicenses1.ShowDriverLicenses(-1);
        }
    }
}
