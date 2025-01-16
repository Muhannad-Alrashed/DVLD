using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        //Custom Events
        public event Action<clsPerson> OnFindClicked; // Define a custom event handler
        protected virtual void PersonFound(clsPerson Person) // Create a protected method to be executed
        {
            Action<clsPerson> handler = OnFindClicked;
            if (handler != null)
                handler(Person);
        }

        //Custom Events
        public event Action<clsPerson> OnAddClicked; // Define a custom event handler
        protected virtual void PersonAdded(clsPerson Person) // Create a protected method to be executed
        {
            Action<clsPerson> handler = OnAddClicked;
            if (handler != null)
                handler(Person);
        }

        public clsPerson _RetrievedPerson { get; set; }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }


        private void GetFoundPerson(object sender, clsPerson Person)    //Delegated function to get the found person
        {
            _RetrievedPerson = Person;
        }

        private void GetAddedPerson(object sender, int PersonID)    //Delegated function to get the added person
        {
            Find(PersonID);
            _RetrievedPerson = clsPerson.FindByPersonID(PersonID);
        }

        public void Find(int PersonID)
        {
            if(txtFilterValue.Text != "")
            {

                if (cbFilterBy.Text == "Person ID")
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                else
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
            }
            else
                ctrlPersonCard1.LoadPersonInfo(PersonID);
        }


        //-------------------------------------------------


        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            frmAddUpdatePerson frm = new frmAddUpdatePerson(-1);
            frm.AddedPersonDataBack += GetAddedPerson;   // Subscribe to delegation
            frm.ShowDialog();

            if (OnAddClicked != null)   //  Raise the Add event 
                PersonAdded(_RetrievedPerson);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            ctrlPersonCard1.FoundPersonDataBack += GetFoundPerson;    // Subscribe to delegation
            Find(-1);

            if (OnFindClicked != null)   //  Raise the find event 
                PersonFound(_RetrievedPerson);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
        }
    }
}
