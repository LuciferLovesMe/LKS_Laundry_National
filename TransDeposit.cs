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
    public partial class TransDeposit : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int id, id_package;

        public TransDeposit()
        {
            InitializeComponent();
            loadpackage();
            loadgrid("");

            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        void loadpackage()
        {
            string com = "select * from service";
            comboBox1.DataSource = Command.getData(com);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";

            getprice();
        }

        void getprice()
        {
            command = new SqlCommand("select * from service where id = " + comboBox1.SelectedValue, connection);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                textBox1.Text = reader.GetInt32(4).ToString();
                connection.Close();
            }
            else
            {
                textBox1.Text = "";
                connection.Close();
            }
        }

        void loadgrid(string s)
        {
            string com = "select * from package_view" + s;
            dataGridView1.DataSource = Command.getData(com);
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            id = 0;
        }

        bool val()
        {
            if (id == 0)
            {
                MessageBox.Show("Please select a customer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (comboBox1.Text.Length < 1)
            {
                MessageBox.Show("Please select a package", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            command = new SqlCommand("select top(1) * from customer where phoneNumber like '%" + textBox2.Text + "%'", connection);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                textBox3.Text = reader.GetString(1);
                textBox4.Text = reader.GetString(3);
                id = reader.GetInt32(0);
                connection.Close();
            }
            else
            {
                connection.Close();
                textBox3.Text = "";
                textBox4.Text = "";
                id = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddCustomer add = new AddCustomer();
            this.Hide();
            add.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getprice();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            loadgrid(" where service_name like '%" + textBox5.Text + "%' or customer_name like '%" + textBox5.Text + "%'");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val())
            {
                string com = "insert into prepaidPackage values(" + id + ", " + comboBox1.SelectedValue + ", " + Convert.ToInt32(textBox1.Text) + ", getdate(), null)";
                try
                {
                    Command.exec(com);
                    connection.Close();

                    command = new SqlCommand("select top(1) * from prepaidPackage order by id desc", connection);
                    connection.Open();
                    reader = command.ExecuteReader();
                    reader.Read();
                    id_package = reader.GetInt32(0);
                    connection.Close();

                    string c = "insert into detailDeposit values(null, " + comboBox1.SelectedValue + ", " + id_package + ", " + Convert.ToInt32(textBox1.Text) + ", " + numericUpDown1.Value + ", null)";
                    try
                    {
                        Command.exec(c);

                        MessageBox.Show("Successfully inserted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        loadgrid("");
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
