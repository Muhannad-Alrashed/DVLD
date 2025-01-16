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

namespace DVLD
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsPerson _Person;

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }


        public void LoadApplicationInfo(int ApplicationID)
        {
            clsApplication _Application = clsApplication.FindByApplicationID(ApplicationID);
            
            if (_Application != null)
            {
                _Person = clsPerson.FindByPersonID(_Application.PersonID);

                lblApplicationID.Text = _Application.ID.ToString();
                lblStatus.Text = _Application.Status.ToString();
                lblFees.Text = _Application.PaidMoney.ToString();
                lblType.Text = clsApplicationType.Find(_Application.ApplicationTypeID).Title.ToString();
                lblApplicant.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
                lblDate.Text = _Application.ApplicationDate.ToLongDateString();
                lblStatusDate.Text = _Application.LastStatusDate.ToLongDateString();
                lblCreatedByUser.Text = clsUser.Find(_Application.CreatedByUserID).Username;
            }
        }


        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonInfo(_Person.ID);
            frm.ShowDialog();
        }
    }
}
