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
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MainLogin()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand command = new SqlCommand("select * from employee where email = @email and password = @pass", connection);
                command.Parameters.AddWithValue("@email", textBox1.Text);
                command.Parameters.AddWithValue("@pass", textBox2.Text);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Model.id = reader.GetInt32(0);
                    Model.name = reader.GetString(2);
                    Model.jobId = reader.GetInt32(7);

                    connection.Close();
                    MainForm main = new MainForm();
                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    connection.Close();
                    MessageBox.Show("User can't be found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
