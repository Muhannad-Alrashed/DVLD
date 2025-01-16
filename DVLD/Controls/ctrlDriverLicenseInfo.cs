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
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }


        //--------------------------------


        public void LoadLicenseInfo(int LicenseID)
        {
            clsLicense License = clsLicense.FindByLicenseID(LicenseID);

            if (License == null)
            {
                lblClass.Text = "[???]";           
                lblFullName.Text = "[???]";        
                lblLicenseID.Text = "[???]";       
                lblNationalNo.Text = "[???]";      
                lblGendor.Text = "[???]";          
                lblIssueDate.Text = "[???]";       
                lblIssueReason.Text = "[???]";     
                lblNotes.Text = "[???]";           
                lblIsActive.Text = "[???]";        
                lblDateOfBirth.Text = "[???]";     
                lblDriverID.Text = "[???]";        
                lblExpirationDate.Text = "[???]";  
                lblIsDetained.Text = "[???]";      
                pbPersonImage.Image = Resources.Male_512;     
            }
            else
            {
                lblClass.Text = License.LicenseClassInfo.ClassName;
                clsPerson Person = clsPerson.FindByPersonID(License.DriverInfo.PersonID);
                lblFullName.Text = $"{Person.FirstName} {Person.SecondName} {Person.ThirdName} {Person.LastName}";
                lblLicenseID.Text = License.LicenseID.ToString();
                lblNationalNo.Text = Person.NationalNo;
                lblGendor.Text = Person.Gender == 0 ? "Male" : "Female";
                lblIssueDate.Text = License.IssueDate.ToShortDateString();
                lblIssueReason.Text = License.IssueReason.ToString();
                lblNotes.Text = License.Notes;
                lblIsActive.Text = License.IsActive ? "Active" : "InActive";
                lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();
                lblDriverID.Text = License.DriverID.ToString();
                lblExpirationDate.Text = License.ExpirationDate.ToShortDateString();
                lblIsDetained.Text = (License.IsDetained()) ? "Yes" : "No";

                if (Person.ImagePath == "")
                    pbPersonImage.Image = Person.Gender == 0 ? Resources.Male_512 : Resources.Female_512;
                else
                    pbPersonImage.ImageLocation = Person.ImagePath;
            }
        }
    }
}
