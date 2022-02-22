using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Laundry_National
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        private void panel_employee_Click(object sender, EventArgs e)
        {
            MasterEmployee master = new MasterEmployee();
            this.Hide();
            master.ShowDialog();
        }

        private void panel_service_Click(object sender, EventArgs e)
        {
            MasterService service = new MasterService();
            this.Hide();
            service.ShowDialog();
        }

        private void panel_package_Click(object sender, EventArgs e)
        {
            MasterPackage master = new MasterPackage();
            this.Hide();
            master.ShowDialog();
        }

        private void panel_depo_Click(object sender, EventArgs e)
        {
            TransDeposit trans = new TransDeposit();
            this.Hide();
            trans.ShowDialog();
        }

        private void panel_prepaid_Click(object sender, EventArgs e)
        {
            PrepaidPackage prepaid = new PrepaidPackage();
            this.Hide();
            prepaid.ShowDialog();
        }

        private void panel_view_Click(object sender, EventArgs e)
        {
            ViewTransaction view = new ViewTransaction();
            this.Hide();
            view.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
            }
        }
    }
}
