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

namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {
        private clsTestAppointment _TestAppointment;
        private int _TestTypeID;
        private clsTest _Test;
        private bool _IsTestAbailable;

        public frmTakeTest(int TestAppointmentID)
        {
            InitializeComponent();
            _TestAppointment = clsTestAppointment.FindByTestAppointmentID(TestAppointmentID);
            _TestTypeID = _TestAppointment.TestTypeID;
            _Test = clsTest.FindByTestAppointmentID(_TestAppointment.TestAppointmentID);
        }




        private void txtNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!_IsTestAbailable)
                e.Handled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_Test != null)
                return;

            _Test = new clsTest();
            _Test.TestAppointmentID = _TestAppointment.TestAppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text;
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_Test.Add())
            {
                MessageBox.Show("Test Added Successfully");
                _TestAppointment.IsLocked = true;
                _TestAppointment.Save();

                if (_Test.TestResult)
                {
                    clsApplication Application = clsApplication.FindByApplicationID(_TestAppointment.RetakeTestApplicationID);
                    Application.Status = clsApplication.enStatus.Complete;
                    Application.Save();  
                }

            }
            else
                MessageBox.Show("Test Adding Failed");
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _TestTypeID;
            ctrlScheduledTest1.LoadData(_TestAppointment , _Test);

            if (_Test != null)
            {
                _IsTestAbailable = false;

                rbPass.Checked = rbPass.Enabled = _Test.TestResult;
                rbFail.Checked = rbFail.Enabled = !_Test.TestResult;
                txtNotes.Text = _Test.Notes;
            }
            else
            {
                if (_TestAppointment.IsLocked)  // Test appointment is timepassed
                {
                    _IsTestAbailable = false;

                    rbPass.Checked = false;
                    rbFail.Checked = false;
                    txtNotes.Text = "Test Wasn't Taken";
                }
                else
                {
                    _IsTestAbailable = true;

                    txtNotes.Clear();
                }
                rbPass.Enabled = _IsTestAbailable;
                rbFail.Enabled = _IsTestAbailable;
            }
            btnSave.Enabled = _IsTestAbailable;
        }
    }
}
