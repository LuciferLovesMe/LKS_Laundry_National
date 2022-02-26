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
    public partial class AddCustomer : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;

        public AddCustomer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Where.id == 1)
            {
                PrepaidPackage prepaidPackage = new PrepaidPackage();
                this.Hide();
                prepaidPackage.ShowDialog();
            }
            else
            {
                TransDeposit trans = new TransDeposit();
                this.Hide();
                trans.ShowDialog();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string c = "select * from customer where phoneNumber = '+62" + textBox2.Text + "'";
                command = new SqlCommand(c, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    MessageBox.Show("Phone Number already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                }
                else
                {
                    connection.Close();
                    string com = "insert into customer values('" + textBox1.Text.Replace("'", "`") + "', '+62" + textBox2.Text + "', '" + textBox3.Text.Replace("'", "`") + "')";
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Successfully add customer", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if(Where.id == 1)
                        {
                            PrepaidPackage prepaidPackage = new PrepaidPackage();
                            this.Hide();
                            prepaidPackage.ShowDialog();
                        }
                        else
                        {
                            TransDeposit trans = new TransDeposit();
                            this.Hide();
                            trans.ShowDialog();
                        }
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsSymbol(e.KeyChar) || e.KeyChar == 8);
        }
    }
}
