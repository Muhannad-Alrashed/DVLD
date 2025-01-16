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
    public partial class ctrlUserCard : UserControl
    {
        private clsUser _User;

        public ctrlUserCard()
        {
            InitializeComponent();
        }


        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);
            
            if(_User != null)
            {
                ctrlPersonCard1.LoadPersonInfo(_User.PersonID);

                lblUserID.Text = _User.UserID.ToString();
                lblUserName.Text = _User.Username;
                lblIsActive.Text = (_User.IsActive) ? "Active" : "Not Active";
            }
        }
    }
}
