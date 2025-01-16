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

namespace DVLD.Tests
{
    public partial class frmScheduleTest : Form
    {
        private int _TestAppointmentID;
        private int _LDLApplicationID;
        private int _TestTypeID;

        public frmScheduleTest(int TestAppointmentID, int LDLApplicationID , int TestTypeID)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID;
            _TestAppointmentID = TestAppointmentID;
            _LDLApplicationID = LDLApplicationID;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.LoadAppointmentInfo(_TestAppointmentID, _LDLApplicationID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
