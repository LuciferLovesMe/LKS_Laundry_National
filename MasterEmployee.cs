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
    public partial class MasterEmployee : Form
    {
        int id, cond;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public MasterEmployee()
        {
            InitializeComponent();
            dis();
            loadgrid("");
            loadcombo();

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
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            dateTimePicker1.Enabled = false;
            comboBox1.Enabled = false;
        }

        void enable()
        {
            btn_insert.Enabled = false;
            btn_update.Enabled = false;
            btn_delete.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBox1.Enabled = true;
        }

        void loadgrid(string s)
        {
            string com = "select employee.*, job.name as Job_Name from employee join job on job.id = employee.idJob" + s;
            dataGridView1.DataSource = Command.getData(com);
            dataGridView1.Columns[1].Visible = false;
        }

        void loadcombo()
        {
            string com = "select * from job";
            comboBox1.DataSource = Command.getData(com);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || textBox6.TextLength < 1 || textBox7.TextLength < 1 || textBox8.TextLength < 1 || dateTimePicker1.Value == null|| comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBox5.Text != textBox4.Text)
            {
                MessageBox.Show("Confirm password doesn't correct!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from employee where email = '" + textBox3.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Email already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        bool val_up()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox6.TextLength < 1 || textBox7.TextLength < 1 || textBox8.TextLength < 1 || dateTimePicker1.Value == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from employee where email = '" + textBox3.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && reader.GetInt32(0) != id)
            {
                connection.Close();
                MessageBox.Show("Email already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            dateTimePicker1.Text = "";
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
                if (result == DialogResult.Yes)
                {
                    string com = "delete from employee where id = " + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Successfully deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid("");
                        clear();
                        dis();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                command = new SqlCommand("insert into employee values (@password, @name, @email, @address, '" + textBox6.Text + "', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', " + comboBox1.SelectedValue + ", " + Convert.ToInt32(textBox8.Text) + ")", connection);
                command.Parameters.AddWithValue("@password", textBox4.Text.Replace("'", "`"));
                command.Parameters.AddWithValue("@name", textBox2.Text.Replace("'", "`"));
                command.Parameters.AddWithValue("@email", textBox3.Text.Replace("'", "`"));
                command.Parameters.AddWithValue("@address", textBox7.Text.Replace("'", "`"));

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid("");
                    dis();
                    clear();
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
            else if (cond == 2 && val_up())
            {
                command = new SqlCommand("update employee set password = @password, name = @name, email = @email, address = @address, phoneNumber = '" + textBox6.Text + "', dateOfBirth = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', idJob = " + comboBox1.SelectedValue + ", salary = " + Convert.ToInt32(textBox8.Text) + " where id = " + id, connection);
                command.Parameters.AddWithValue("@password", textBox4.Text.Replace("'", "`"));
                command.Parameters.AddWithValue("@name", textBox2.Text.Replace("'", "`"));
                command.Parameters.AddWithValue("@email", textBox3.Text.Replace("'", "`"));
                command.Parameters.AddWithValue("@address", textBox7.Text.Replace("'", "`"));
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid("");
                    dis();
                    clear();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid(" where name like '%" + textBox1.Text + "%'");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox4.PasswordChar = '\0';
            }
            else
            {
                textBox4.PasswordChar = '*';
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBox7.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox8.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            clear();
            dis();
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
