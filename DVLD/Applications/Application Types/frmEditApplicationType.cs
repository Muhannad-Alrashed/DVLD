using DVLD_Business;
using DVLD.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.ApplicationTypes
{
    public partial class frmEditApplicationType : Form
    {

        private int _ApplicationID;
        private clsApplicationType _Application;

        public frmEditApplicationType(int ApplicationID)
        {
            InitializeComponent();

            _ApplicationID = ApplicationID;
            _Application = clsApplicationType.Find(_ApplicationID);
        }


        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                txtTitle.Focus();
                errorProvider1.SetError(txtTitle, "Invalid Title");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTitle, "");
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (!clsValidation.ValidateDouble(txtFees.Text))
            {
                e.Cancel = true;
                txtFees.Focus();
                errorProvider1.SetError(txtFees, "Invalid Fees");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                _Application.Title = txtTitle.Text;
                _Application.Fees = Convert.ToDouble(txtFees.Text);

                if (_Application.Update())
                {
                    MessageBox.Show("Application Updated Successfully");
                }
                else
                {
                    MessageBox.Show("Update Failed");
                }
            }
            else
            {
                MessageBox.Show("Fill Form Correctly");
            }
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblApplicationTypeID.Text = _Application.ID.ToString();
            txtTitle.Text = _Application.Title;
            txtFees.Text = _Application.Fees.ToString("F1");
        }
    }
}
