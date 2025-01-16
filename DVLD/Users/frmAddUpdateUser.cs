using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmAddUpdateUser : Form
    {

        private enum enMode {AddNew , Update };
        private enMode _Mode;

        private int _UserID;
        private int _PersonID;
        private clsUser _User;

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _User = clsUser.Find(_UserID);

            if (_User == null)
            {
                _Mode = enMode.AddNew;
                _PersonID = -1;
            }
            else
            {
                _Mode = enMode.Update;
                _PersonID = _User.PersonID;
            }
        }


        private void _SetUserNewInfo()
        {
            _User.PersonID = _PersonID;
            _User.Username = txtUserName.Text;
            _User.Password = txtPassword.Text;
            _User.IsActive = chkIsActive.Checked;
        }

        private void _ShowHideLoginInfoTabPage()
        {
            if (_PersonID == -1)
            {
                if (tcUserInfo.TabPages.Contains(tpLoginInfo))
                        tcUserInfo.TabPages.Remove(tpLoginInfo);
            }
            else
            {
                if (!tcUserInfo.TabPages.Contains(tpLoginInfo))
                    tcUserInfo.TabPages.Add(tpLoginInfo);
            }
        }

        private void _LoadFormData()
        {
            if (_UserID == -1)
            {
                _Mode = enMode.AddNew;
                _ShowHideLoginInfoTabPage();

                lblUserID.Text = "[???]";
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                lblTitle.Text = "Add User";
                btnPersonInfoNext.Enabled = true;
                chkIsActive.Enabled = true;

                if (_PersonID == -1)
                {
                    btnPersonInfoNext.Text = "Choose Person to Add User";
                    btnPersonInfoNext.Enabled = false;
                    chkIsActive.Enabled = false;
                }
                else
                    btnPersonInfoNext.Text = "Add New User";
            }
            else
            {
                _Mode = enMode.Update;
                _ShowHideLoginInfoTabPage();

                lblUserID.Text = _User.UserID.ToString();
                txtUserName.Text = _User.Username;
                txtPassword.Text = _User.Password;
                txtConfirmPassword.Text = _User.Password;
                chkIsActive.Checked = _User.IsActive;
                lblTitle.Text = "Update User";
                btnPersonInfoNext.Text = "See User Info";
                btnPersonInfoNext.Enabled = true;
                chkIsActive.Enabled = true;
            }
        }

        //Custom Events (OnFindClicked / OnAddClicked)
        private void ReloadDataChanges(clsPerson obj)
        {
            _PersonID = ctrlPersonCardWithFilter1._RetrievedPerson.ID;

            _User = clsUser.FindByPersonID(_PersonID) ?? new clsUser();

            _UserID = (_User != null) ? _User.UserID : -1;

            _LoadFormData();
        }


        //------------------------------------------


        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            string errorMessage = string.Empty;
            clsUser User;

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                errorMessage = "Fill UserName Field!";
            }
            else if ((User = clsUser.FindByUsername(txtUserName.Text)) != null && User.UserID != _UserID)
            {
                errorMessage = "This UserName Is Taken!";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                e.Cancel = true;
                txtUserName.Focus();
                errorProvider1.SetError(txtUserName, errorMessage);
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtUserName, "Invalid Password!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtPassword.Text != txtConfirmPassword.Text)
            {
                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtUserName, "The Two Password Fields Don't Match!");
            }
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            tcUserInfo.SelectedIndex = 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(_PersonID != -1)
            {
                if (ValidateChildren())
                {
                    _SetUserNewInfo();

                    if (_User.Save())
                    {
                        MessageBox.Show("User Saved Successfully");
                        _Mode = enMode.Update;
                        _UserID = _User.UserID;
                        _LoadFormData();
                    }
                    else
                        MessageBox.Show("Failed to Save");
                }
                else
                {
                    MessageBox.Show("Please Fill All The Fields Correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Please Choose a Person.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _LoadFormData();
        }
    }
}
