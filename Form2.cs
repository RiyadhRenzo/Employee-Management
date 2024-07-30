using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsAppEntelect
{
    public partial class Form2 : Form
    {
        string mysqlCon = "server=127.0.0.1; port=3307; user=root; database=itep finals;";
        MySqlConnection mySqlConnection;
        public Form2()
        {
            InitializeComponent();
            mySqlConnection = new MySqlConnection(mysqlCon);
            mySqlConnection.Open();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            try
            {
                string query = "SELECT COUNT(*) FROM Admin WHERE Name = @Name AND Password = @Password";

                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@Name", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int count = Convert.ToInt32(command.ExecuteScalar());


                    MessageBox.Show(count > 0 ? "Login successful!" : "Invalid username or password.", "Result", MessageBoxButtons.OK, count > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    if (count > 0)
                    {
                        Form1 f1 = new Form1();
                        this.Hide();
                        f1.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
