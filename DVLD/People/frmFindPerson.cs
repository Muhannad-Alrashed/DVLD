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
    public partial class frmFindPerson : Form
    {
        public frmFindPerson()
        {
            InitializeComponent();
        }

        private void ctrlPersonCardWithFilter1_OnFindAddClicked(clsPerson obj)
        {
            clsPerson Person = ctrlPersonCardWithFilter1._RetrievedPerson;

            if (Person != null)
                lblTitle.Text = Person.FirstName+ " " + Person.LastName;
        }
    }
}
