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
    public partial class frmListTestTypes : Form
    {
        public frmListTestTypes()
        {
            InitializeComponent();
        }


        private void _LoadData()
        {
            dgvTestTypes.DataSource = clsTestType.ListTestTypes();
            lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();
            
            dgvTestTypes.Columns[1].Width = 200;
            dgvTestTypes.Columns[2].Width = 600;
            dgvTestTypes.Columns[3].Width = 150;
        }


        //------------------------------------


        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvTestTypes.CurrentRow.Cells[0].Value;
            Form frm = new frmEditTestTypes(ID);
            frm.ShowDialog();
            _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}
