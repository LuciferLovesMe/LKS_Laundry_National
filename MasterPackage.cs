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
    public partial class MasterPackage : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        int cond, id;
        SqlCommand command;
        SqlDataReader reader;

        public MasterPackage()
        {
            InitializeComponent();
            dis();
            loadgrid("");
            loadserv();
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        void dis()
        {
            btn_insert.Enabled = true;
            btn_update.Enabled = true;
            btn_delete.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            comboBox1.Enabled = false;
            textBox3.Enabled = false;
            numericUpDown1.Enabled = false;
        }

        void enable()
        {
            btn_insert.Enabled = false;
            btn_update.Enabled = false;
            btn_delete.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            comboBox1.Enabled = true;
            textBox3.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        void loadgrid(string s)
        {
            string com = "select package.*, service.name as Service_name from package join service on package.idService = service.id" + s;
            dataGridView1.DataSource = Command.getData(com);
        }

        void loadserv()
        {
            string com = "select * from service";
            comboBox1.DataSource = Command.getData(com);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
        }

        void clear()
        {
            textBox3.Text = "";
            numericUpDown1.Value = 0;
        }

        bool val()
        {
            if(comboBox1.Text.Length < 1 || textBox3.TextLength < 1 || numericUpDown1.Value < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_insert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                cond = 2;
                enable();
            }
            else
            {
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result== DialogResult.Yes)
                {
                    string com = "delete from package where id = " + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid("");
                        clear();
                        dis();
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
            else
            {
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(cond == 1 && val())
            {
                string com = "insert into package values(" + comboBox1.SelectedValue + ", " + numericUpDown1.Value + ", " + Convert.ToInt32(textBox3.Text) + ")";
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid("");
                    clear();
                    dis();
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
            else if(cond == 2 && val())
            {
                string com = "update package set idService = " + comboBox1.SelectedValue + ", totalUnit = " + numericUpDown1.Value + ", price = " + Convert.ToInt32(textBox3.Text) + " where id = " + id;
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid("");
                    clear();
                    dis();
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid(" where service.name like '%" + textBox1.Text + "%'");
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            dis();
            clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[2].Value);
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
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
