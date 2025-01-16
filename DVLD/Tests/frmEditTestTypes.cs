using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmEditTestTypes : Form
    {

        private int _TestID;
        private clsTestType _Test;

        public frmEditTestTypes(int testID)
        {
            InitializeComponent();

            _TestID = testID;
            _Test = clsTestType.Find(_TestID);
        }


        //-------------------------------------
        

        private void txtTitle_Validating(object sender , CancelEventArgs e)
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

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                e.Cancel = true;
                txtDescription.Focus();
                errorProvider1.SetError(txtDescription, "Invalid Description");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtDescription, "");
            }
        }

        private void txtFees_Validating(object sender , CancelEventArgs e)
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

        private void btnClose_Click(object sender , EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                _Test.Title = txtTitle.Text;
                _Test.Description = txtDescription.Text;
                _Test.Fees = Convert.ToDouble(txtFees.Text);

                if (_Test.Update())
                {
                    MessageBox.Show("Test Updated Successfully");
                }
                else
                {
                    MessageBox.Show("Update Failed");
                }
            }
            else
            {
                MessageBox.Show("Fill All Fields Correctly");
            }
        }

        private void frmEditTestType_Load(object sender , EventArgs e)
        {
            lblTestTypeID.Text = _Test.ID.ToString();
            txtTitle.Text = _Test.Title;
            txtDescription.Text = _Test.Description;
            txtFees.Text = _Test.Fees.ToString();
        }
    }
}
