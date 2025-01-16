using DVLD.Core;
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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }


        //-----------------------------------


        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.FindByUsernameAndPassword(txtUserName.Text, txtPassword.Text);

            if (User == null )
                MessageBox.Show("Invalid Username or Password");
            else 
            {
                if (User.IsActive == false)
                {
                    MessageBox.Show($"User {User.Username} Is Not Active");
                    return;
                }
            

                clsGlobal.CurrentUser = User;

                if (chkRememberMe.Checked)
                    clsGlobal.RememberUserNameAndPassword(User.Username, User.Password);
                else
                    clsGlobal.RememberUserNameAndPassword("", "");

                this.Hide();
                Form frm = new frmMain(this);
                frm.Show();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";
            if (clsGlobal.GetLoginCredentials(ref Username, ref Password))
            {
                txtUserName.Text = Username;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;
        }
    }
}
