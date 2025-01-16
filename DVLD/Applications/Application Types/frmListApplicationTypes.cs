using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;

namespace DVLD.Applications.ApplicationTypes
{
    public partial class frmListApplicationTypes : Form
    {
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }


        private void _LoadData()
        {
            dgvApplicationTypes.DataSource = clsApplicationType.ListApplicationTypes();
            lblRecordsCount.Text = dgvApplicationTypes.RowCount.ToString();
            
            dgvApplicationTypes.Columns[1].Width = 350;
            dgvApplicationTypes.Columns[2].Width = 150;
        }


        //--------------------------------------


        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;
            Form frm = new frmEditApplicationType(ID);
            frm.ShowDialog();
            _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}
