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
using System.IO;
using DVLD.Core;

namespace DVLD
{
    public partial class frmListPeople : Form
    {
        private string _FilterBy = "None";

        public frmListPeople()
        {
            InitializeComponent();
        }


        private void _LoadPeopleData( string Token ="")
        {

            DataTable PeopleDataTable = clsPerson.ListPeople();

            if (_FilterBy != "None" && Token != "")
            {
                string Condition ="";
                if (_FilterBy == "PersonID")
                    Condition = $"{_FilterBy} = {Token}";
                else
                    Condition = $"{_FilterBy} like '{Token}%'";

                DataRow[] ResultRows;
                ResultRows = PeopleDataTable.Select(Condition);

                DataTable filteredTable = PeopleDataTable.Clone(); 
                foreach (DataRow row in ResultRows)
                    filteredTable.ImportRow(row);

                dgvPeople.DataSource = filteredTable;
                lblRecordsCount.Text = ResultRows.Count().ToString();
            }
            else
            {
                dgvPeople.DataSource = PeopleDataTable;
                lblRecordsCount.Text = PeopleDataTable.Rows.Count.ToString();
            }

            if (dgvPeople.RowCount > 0)
                dgvPeople.Columns[10].Width = 225;
        }

        private void _ShowAddUpdateForm(int ID)
        {
            Form frm = new frmAddUpdatePerson(ID);
            frm.ShowDialog();
            this.Refresh();
        }


        //-------------------------------------------


        private void dgvPeople_DoubleClick(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            _LoadPeopleData();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            _LoadPeopleData();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _ShowAddUpdateForm(-1);
            _LoadPeopleData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            _ShowAddUpdateForm(ID);
            _LoadPeopleData();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            clsPerson Person = clsPerson.FindByPersonID(ID);

            if (MessageBox.Show($"Are you sure you want to delete {Person.FirstName} {Person.LastName} from the system?'",
                "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            if (clsPerson.Delete(ID))
            {
                clsGlobal.DeleteFile(Person.ImagePath);
                MessageBox.Show("Perosn Deleted Successfully");
            }
            else 
                MessageBox.Show("Deletion Failed");

            _LoadPeopleData();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _LoadPeopleData(txtFilterValue.Text.Trim());
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
               if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = true;
            txtFilterValue.Clear();

            switch(cbFilterBy.SelectedItem.ToString())
            {
                case "None":
                    {
                        txtFilterValue.Visible = false;
                    }
                    break;
                case "Person ID":
                    {
                        _FilterBy = "PersonID";
                    }
                    break;
                case "National No.":
                    {
                        _FilterBy = "NationalNo";
                    }   
                    break;
                case "First Name":
                    {
                        _FilterBy = "FirstName";
                    }
                    break;
                case "Second Name":
                    {
                        _FilterBy = "SecondName";
                    }
                    break;
                case "Third Name":
                    {
                        _FilterBy = "ThirdName";
                    }
                    break;
                case "Last Name":
                    {
                        _FilterBy = "LastName";
                    }
                    break;
                case "Nationality":
                    {
                        _FilterBy = "CountryName";
                    }
                    break;
                case "Gender":
                    {
                        _FilterBy = "Gender_Caption";
                    }
                    break;
                case "Phone":
                    {
                        _FilterBy = "Phone";
                    }
                    break;
                case "Email":
                    {
                        _FilterBy = "Email";
                    }
                    break;
            }
               _LoadPeopleData();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            _ShowAddUpdateForm(-1);
            _LoadPeopleData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _LoadPeopleData();
        }

        private void findPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmFindPerson();
            frm.ShowDialog();
            _LoadPeopleData();
        }
    }
}
