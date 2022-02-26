using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Laundry_National
{
    public partial class ViewTransaction : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        int id;
        public ViewTransaction()
        {
            InitializeComponent();
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        void loadgrid()
        {
            if (comboBox1.Text.ToLower() == "service")
            {
                string com = "select detailDeposit.id, customer.name as Customer_name, service.name as Service_Name, detailDeposit.priceUnit, detailDeposit.totalUnit, employee.name as Employee_Name from headerDeposit join detailDeposit on detailDeposit.idDeposit = headerDeposit.id join customer on headerDeposit.idCustomer = customer.id join service on detailDeposit.idService = service.id join employee on headerDeposit.idEmployee = employee.id where completeDatetime is null";
                string c = "select detaildeposit.id, customer.name as Customer_name, service.name as Service_Name, detailDeposit.priceUnit, detailDeposit.totalUnit, employee.name as Employee_Name,detailDeposit.completeDatetime from headerDeposit join detailDeposit on detailDeposit.idDeposit = headerDeposit.id join customer on headerDeposit.idCustomer = customer.id join service on detailDeposit.idService = service.id join employee on headerDeposit.idEmployee = employee.id where completeDatetime is not null";
                dataGridView1.DataSource = Command.getData(c);
                dataGridView2.DataSource = Command.getData(com);
            }
            else if (comboBox1.Text.ToLower() == "package")
            {
                string com = "select prepaidPackage.id, customer.name as Customer_Name, service.name as Service_Name, package.price, prepaidPackage.startDateTime from prepaidPackage join customer on prepaidPackage.idCustomer = customer.id join package on prepaidPackage.idPackage = package.id join service on package.idService = service.id where prepaidPackage.completedDateTime is null";
                string c = "select prepaidPackage.id, customer.name as Customer_Name, service.name as Service_Name, package.price, prepaidPackage.startDateTime, prepaidPackage.completedDateTime from prepaidPackage join customer on prepaidPackage.idCustomer = customer.id join package on prepaidPackage.idPackage = package.id join service on package.idService = service.id where prepaidPackage.completedDateTime is not null";
                dataGridView1.DataSource = Command.getData(c);
                dataGridView2.DataSource = Command.getData(com);
            }
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
            if (result == DialogResult.Yes)
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

        private void button3_Click(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Selected)
            {
                if(comboBox1.Text.ToLower() == "service")
                {
                    string com = "update detailDeposit set completeDatetime = getdate() where id =" + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Successfully completed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else if (comboBox1.Text.ToLower() == "package")
                {
                    string com = "update prepaidPackage set completedDatetime = getdate() where id =" + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Successfully completed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
            if(dataGridView1.RowCount < 1)
            {
                MessageBox.Show("There are no data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                application.Workbooks.Add(Type.Missing);
                for(int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    application.Cells[i + 1] = dataGridView1.Columns[i].HeaderText;
                }

                for(int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for(int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        application.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                application.Visible = true;
            }
        }
    }
}
