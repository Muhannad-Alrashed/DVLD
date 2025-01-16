using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Text.RegularExpressions;
using System.Collections;
using System.Diagnostics.Contracts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DVLD.Core;
using System.Diagnostics.Eventing.Reader;

namespace DVLD
{
    public partial class frmAddUpdatePerson : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler AddedPersonDataBack;  // Declare an event using the delegate


        private enum enMode { AddNew, Update };
        private enMode _Mode;
        private int _PersonID;
        private clsPerson _Person;
        private string _OldImagePath;

        public frmAddUpdatePerson(int ID)
        {
            InitializeComponent();

            _PersonID = ID;
            _Person = clsPerson.FindByPersonID(_PersonID);
            
            if (_Person == null)
            {
                _Mode = enMode.AddNew;
                _Person = new clsPerson();
            }
            else
            {
                _Mode = enMode.Update;
                _OldImagePath = _Person.ImagePath;
            }
        }


        private void _LoadCountries()
        {
            DataTable dt = clsCountry.GetAllCountries();
            foreach (DataRow country in dt.Rows)
            {
                cbCountry.Items.Add(country["CountryName"]);
            }
        }

        private void _SetDatePicker()
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
        }

        private void _LoadPersonData()
        {
            _LoadCountries();
            _SetDatePicker();

            if (_Mode == enMode.Update)
            {
                _Person = clsPerson.FindByPersonID(_PersonID);
                if (_Person == null)
                {
                    MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Close();
                    return;
                }

                lblTitle.Text = "Update Person Info";
                lblPersonID.Text = _PersonID.ToString();
                txtFirstName.Text = _Person.FirstName;
                txtSecondName.Text = _Person.SecondName;
                txtThirdName.Text = _Person.ThirdName;
                txtLastName.Text = _Person.LastName;
                txtNationalNo.Text = _Person.NationalNo;
                dtpDateOfBirth.Value = _Person.DateOfBirth;

                if (_Person.Gender == 0)
                {
                    rbMale.Checked = true;
                    pbPersonImage.Image = Resources.Male_512;
                }
                else
                {
                    rbFemale.Checked = true;
                    pbPersonImage.Image = Resources.Female_512;
                }

                txtAddress.Text = _Person.Address;
                txtPhone.Text = _Person.Phone;
                txtEmail.Text = _Person.Email;
                cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.FindCountryByID(_Person.CountryID).Name);

                if (_Person.ImagePath != "")
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }
                else
                {
                    pbPersonImage.ImageLocation = "";
                }

                llRemoveImage.Visible = (pbPersonImage.ImageLocation != "");
            }
        }

        private void _HandlePersonImage()
        {
            clsGlobal.DeleteFile(_OldImagePath);

            string newImagePath = "";
            if ( pbPersonImage.ImageLocation != _OldImagePath
                && pbPersonImage.ImageLocation != "")
            {
                newImagePath = clsGlobal.SaveImageAndGetFilePath(pbPersonImage.Image);
            }

            _Person.ImagePath = newImagePath ;
            _OldImagePath = newImagePath;
        }

        private void _SetPersonNewInfo()
        {
            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Gender = rbMale.Checked ? (short)0 : (short)1;
            _Person.Address = txtAddress.Text;
            _Person.Phone = txtPhone.Text;
            _Person.Email = txtEmail.Text;
            _Person.CountryID = clsCountry.FindCountryByName(cbCountry.Text).CountryID;
            _HandlePersonImage();
        }


        //--------------------------------------------------


        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")

                return;
            if (!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                txtEmail.Focus();
                errorProvider1.SetError(txtEmail, "Invalid Email!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
            }
        }
        
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    e.Cancel = true;
                    textBox.Focus();
                    errorProvider1.SetError(textBox, $"{textBox.Tag} Must Be Entered!");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(textBox, "");
                }
            }
        }

        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbCountry.Text))
            {
                e.Cancel = true;
                cbCountry.Focus();
                errorProvider1.SetError(cbCountry, $"Country Must Be Entered!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(cbCountry, "");
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            clsPerson PersonWithEnteredNationalNo = clsPerson.FindByNationalNo(txtNationalNo.Text);

            if (PersonWithEnteredNationalNo != null)
            {
                bool isDuplicate = (_Mode == enMode.AddNew) ||
                                   (_Mode == enMode.Update && PersonWithEnteredNationalNo.ID != _Person.ID);

                if (isDuplicate)
                {
                    e.Cancel = true;
                    txtNationalNo.Focus();
                    errorProvider1.SetError(txtNationalNo, "An existing person has this National Number!");
                }
            }

            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number Must Be Entered!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }
        }


        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.Image = (rbMale.Checked) ? Resources.Male_512 : Resources.Female_512;

            pbPersonImage.ImageLocation = "";

            llRemoveImage.Visible = false;
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.ImageLocation = openFileDialog1.FileName;
                llRemoveImage.Visible = true;
            }
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            pbPersonImage.Image = Resources.Female_512;
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            pbPersonImage.Image = Resources.Male_512;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                _SetPersonNewInfo();

                if (_Person.Save())
                {
                    MessageBox.Show("Person Saved Successfully");

                    _Mode = enMode.Update;
                    _PersonID = _Person.ID;

                    _LoadPersonData();
                    AddedPersonDataBack?.Invoke(this, _PersonID);  // Trigger the event to send data back
                }

                else
                    MessageBox.Show("Failed to Save:");
            }
            else
            {
                MessageBox.Show("Please correct the highlighted errors and try again.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _LoadPersonData();
        }
    }
}
