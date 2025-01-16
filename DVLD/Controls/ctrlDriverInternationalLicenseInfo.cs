using DVLD.Properties;
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
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }


        //---------------------------------------


        public void LoadInternationalLicenseInfo(int InternationalLicenseID)
        {
            clsInternationalLicense InternationalLicense = clsInternationalLicense.FindByInternationalLicenseID(InternationalLicenseID);

            if (InternationalLicense == null)
            {
                MessageBox.Show("No Info Exists for this License.");
                return;
            }

            clsPerson Person = clsPerson.FindByPersonID(InternationalLicense.LocalLicenseInfo.DriverInfo.PersonID);
            lblFullName.Text = $"{Person.FirstName} {Person.SecondName} {Person.ThirdName} {Person.LastName}";
            
            lblInternationalLicenseID.Text = InternationalLicenseID.ToString();
            lblLocalLicenseID.Text = InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = Person.NationalNo;
            lblGendor.Text = Person.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = InternationalLicense.IssueDate.ToShortDateString();
            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = InternationalLicense.IsActive ? "Active" : "InActive";
            lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();
            lblDriverID.Text = InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = InternationalLicense.ExpirationDate.ToShortDateString();

            if (Person.ImagePath == "")
                pbPersonImage.Image = Person.Gender == 0 ? Resources.Male_512 : Resources.Female_512;
            else
                pbPersonImage.ImageLocation = Person.ImagePath;
        }
    }
}
