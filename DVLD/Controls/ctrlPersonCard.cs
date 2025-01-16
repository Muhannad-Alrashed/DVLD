using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Properties;
using DVLD_Business;

namespace DVLD
{
    public partial class ctrlPersonCard : UserControl
    {
        // Declare a delegate 
        public delegate void DataBackEventHandler(object sender, clsPerson Person);
        // Declare an event using the delegate
        public event DataBackEventHandler FoundPersonDataBack;

        public bool EnableEdit { get; set; }
        private int _PersonID;

        public ctrlPersonCard()
        {
            InitializeComponent();

            llEditPersonInfo.Enabled = EnableEdit;
        }


        public void LoadPersonInfo(int PersonID)
        {
            DisplayPersonInfo(clsPerson.FindByPersonID(PersonID));
        }

        public void LoadPersonInfo(string NationalNo)
        {
            DisplayPersonInfo(clsPerson.FindByNationalNo(NationalNo));
        }

        public void DisplayPersonInfo(clsPerson Person)
        {
            if (Person == null)
            {
                llEditPersonInfo.Enabled = false;

                _PersonID = -1;
                lblPersonID.Text = "[????]";
                lblFullName.Text = "[????]";
                lblNationalNo.Text = "[????]";
                lblDateOfBirth.Text = "[????]";
                lblGendor.Text = "[????]";
                lblAddress.Text = "[????]";
                lblPhone.Text = "[????]";
                lblEmail.Text = "[????]";
                lblCountry.Text = "[????]";
                pbPersonImage.Image = Resources.Male_512;

                FoundPersonDataBack?.Invoke(this, clsPerson.FindByPersonID(-1));  // Trigger the event to send data back
            }
            else
            {
                llEditPersonInfo.Enabled = true;

                _PersonID = Person.ID;
                lblPersonID.Text = Person.ID.ToString();
                lblFullName.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " + Person.LastName;
                lblNationalNo.Text = Person.NationalNo;
                lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();
                lblGendor.Text = (Person.Gender == 0) ? "Male" : "Female";
                lblAddress.Text = Person.Address;
                lblPhone.Text = Person.Phone;
                lblEmail.Text = Person.Email;
                lblCountry.Text = clsCountry.FindCountryByID(Person.CountryID).Name;

                if (Person.ImagePath != "")
                    pbPersonImage.ImageLocation = Person.ImagePath;
                else
                    pbPersonImage.Image = (Person.Gender == 0) ? Resources.Male_512 : Resources.Female_512;

                FoundPersonDataBack?.Invoke(this, Person);  // Trigger the event to send data back           
            }
        }


        //--------------------------------------------


        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();

            LoadPersonInfo(_PersonID);
        }
    }
}
