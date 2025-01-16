using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmListUsers : Form
    {
        private string _FilterBy = "";

        public frmListUsers()
        {
            InitializeComponent();
        }

        private void LoadUsersData(string FilterValue = "")
        {
            DataTable UsersDataTable = clsUser.ListUsers();

            if (_FilterBy != "" && FilterValue != "")
            {
                string condition = string.Empty;

                if (cbFilterBy.Text != "User Name" || cbFilterBy.Text != "Full Name")
                    condition = $"{_FilterBy} = {FilterValue}";
                else
                    condition = $"{_FilterBy} like '{FilterValue}%'";

                DataRow[] ResultRows;
                ResultRows = UsersDataTable.Select(condition);

                DataTable filteredTable = UsersDataTable.Clone();
                foreach (DataRow row in ResultRows)
                    filteredTable.ImportRow(row);

                dgvUsers.DataSource = filteredTable;
                lblRecordsCount.Text = filteredTable.Rows.Count.ToString();
            }
            else
            {
            dgvUsers.DataSource = UsersDataTable;
            lblRecordsCount.Text = UsersDataTable.Rows.Count.ToString();
            }

            if (dgvUsers.RowCount > 0)
            {
                dgvUsers.Columns[3].Width = 400;
                dgvUsers.Columns[4].Width = 125;
            }

        }

        private void LoadAddUpdateForm(int ID)
        {
            Form frm = new frmAddUpdateUser(ID);
            frm.ShowDialog();
            this.Refresh();
        }


        //---------------------------------------


        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            txtFilterValue.Visible = true;
            cbIsActive.Visible = false;
            cbIsActive.SelectedItem = "All";

            switch (cbFilterBy.Text)
            {
                case "None":
                    {
                        txtFilterValue.Visible = false;
                    }
                    break;
                case "User ID":
                    {
                        _FilterBy = "UserID";
                    }
                    break;
                case "UserName":
                    {
                        _FilterBy = "UserName";
                    }
                    break;
                case "Person ID":
                    {
                        _FilterBy = "PersonID";
                    }
                    break;
                case "Full Name":
                    {
                        _FilterBy = "Full_Name";
                    }
                    break;
                case "Is Active":
                    {
                        _FilterBy = "Active_State";
                        txtFilterValue.Visible = false;
                        cbIsActive.Visible = true;
                    }
                    break;
            }
            LoadUsersData();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            LoadUsersData(txtFilterValue.Text);
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "User ID" || cbFilterBy.Text == "Person ID")
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadAddUpdateForm(-1);
            LoadUsersData();
        }

        private void dgvUsers_CellContentDoubleClick(object sender, EventArgs e)
        {
            int ID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            Form frm = new frmShowUserInfo(ID);
            frm.ShowDialog();
            LoadUsersData();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            Form frm = new frmShowUserInfo(ID);
            frm.ShowDialog();
            LoadUsersData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            LoadAddUpdateForm(ID);
            LoadUsersData();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            clsUser User = clsUser.Find(ID);
            if (MessageBox.Show($"Are you sure you want to delete {User.PersonInfo.FirstName} {User.PersonInfo.LastName} from the system?'",
                "Confirm Delete", MessageBoxButtons.OKCancel ,MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            if (clsUser.Find(ID).Delete())
                MessageBox.Show("User Deleted Successfully");
            else
                MessageBox.Show("Deletion Failed");
            LoadUsersData();
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            Form frm = new frmChangePassword(ID);
            frm.ShowDialog();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
           string SearchToken = "";

            switch (cbIsActive.Text)
            {
                case "All":
                    SearchToken = "";
                    break;
                case "Active":
                    SearchToken = "1";
                    break;
                case "InActive":
                    SearchToken = "0";
                    break;
            }
            LoadUsersData(SearchToken);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            LoadAddUpdateForm(-1);
            LoadUsersData();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            LoadUsersData();
        }
    }
}
