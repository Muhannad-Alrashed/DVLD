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

namespace DVLD.Users
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _User = clsUser.Find(_UserID);
        }

        
        //-----------------------------------------------


        private void txtCurrentPassword_Validating(object sender ,  CancelEventArgs e)
        {
            if(txtCurrentPassword.Text != _User.Password)
            {
                e.Cancel = true;
                txtCurrentPassword.Focus();
                errorProvider1.SetError(txtCurrentPassword, "The Current Password Isn't Correct");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void txtNewPassword_Validating(object sender ,  CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel = true;
                txtNewPassword.Focus();
                errorProvider1.SetError(txtNewPassword, "Enter a Valid Password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender ,  CancelEventArgs e)
        {
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "The Two Passwords Do not Match");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void btnClose_Click(object sender ,  EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender ,  EventArgs e)
        {
            if (ValidateChildren())
            {
                _User.Password = txtNewPassword.Text;

                if (_User.Save())
                {
                    MessageBox.Show("Password Changed Successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to Change Password.");
                }
            }
            else
            {
                MessageBox.Show("Please Fill All The Fields Correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmChangePassword_Load(object sender ,  EventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(_UserID);
        }
    }
}
