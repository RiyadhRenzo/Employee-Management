using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
namespace WindowsFormsAppEntelect
{
    public partial class Form1 : Form
    {
        string mysqlCon = "server=127.0.0.1; port=3307; user=root; database=itep finals;";
        public Form1()
        {
            InitializeComponent();
            MySqlConnection mySqlConnection = new MySqlConnection(mysqlCon);
            mySqlConnection.Open();
        }

        BindingSource bs = new BindingSource();
        DataTable Table = new DataTable();
        public int rowIndex;
        private void Form1_Load(object sender, EventArgs e)
        {
            Table.Columns.Add("Employee ID   ", typeof(string));
            Table.Columns.Add("First Name    ", typeof(string));
            Table.Columns.Add("Last Name     ", typeof(string));
            Table.Columns.Add("Department     ", typeof(string));
            Table.Columns.Add("Salary (R)     ", typeof(double));


            bs.DataSource = Table;
            dgvEmployee.DataSource = bs;

            Table.Rows.Add("577669", "Lewan", "Staden", "Maintanance", 32000.95);
            Table.Rows.Add("600669", "Thabiso", "Moloyi", "Developer", 39000.05);
            Table.Rows.Add("565845", "Delight", "Chipiro", "HR", 15000.95);
            Table.Rows.Add("577669", "Thuli", "Phogolo", "Finance", 48000.95);
        }
        //-------------------------------------------------------------------------
        public void refresh()
        {
            txteID.Focus();
            txteID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtDep.Clear();
            txtSalary.Clear();
        }
        //------------------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)//input data
        {
            string employeeID = txteID.Text;
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string department = txtDep.Text;
            double salary;
            if (!double.TryParse(txtSalary.Text, out salary))
            {
                MessageBox.Show("Invalid salary amount. Please enter a valid number.");
                return;
            }
            InsertData(employeeID, firstName, lastName, department, salary);
            Table.Rows.Add(employeeID, firstName, lastName, department, salary);

            refresh();
        }
        //--------------------------------------------------------------------------------
        private void InsertData(string employeeID, string firstName, string lastName, string department, double salary)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlCon))
                {
                    string query = "INSERT INTO Employee (EmployeeID, FirstName, LastName, Department, Salary) VALUES (@EmployeeID, @FirstName, @LastName, @Department, @Salary)";

                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@EmployeeID", employeeID);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Salary", salary);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        //------------------------------------------------------------------------------

        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;

            

            DataGridViewRow row = dgvEmployee.Rows[rowIndex];

            txteID.Text = row.Cells[0].Value.ToString();
            txtFirstName.Text = row.Cells[1].Value.ToString();
            txtLastName.Text = row.Cells[2].Value.ToString();
            txtDep.Text = row.Cells[3].Value.ToString();
            txtSalary.Text = row.Cells[4].Value.ToString();
        }
        //-------------------------------------------------------------------------
        private void btnEdit_Click(object sender, EventArgs e)//update
        {
            // Get the data from the textboxes
            string employeeID = txteID.Text;
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string department = txtDep.Text;
            double salary;

            DataGridViewRow row = dgvEmployee.Rows[rowIndex];

            row.Cells[0].Value = txteID.Text;
            row.Cells[1].Value = txtFirstName.Text;
            row.Cells[2].Value = txtLastName.Text;
            row.Cells[3].Value = txtDep.Text;
            row.Cells[4].Value = txtSalary.Text;
            refresh();
            if (!double.TryParse(txtSalary.Text, out salary))
            {
                MessageBox.Show("Invalid salary amount. Please enter a valid number.");
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlCon))
                {
                    string query = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Department = @Department, Salary = @Salary WHERE EmployeeID = @EmployeeID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Salary", salary);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No rows updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string employeeID = dgvEmployee.CurrentRow.Cells[0].Value.ToString();
            DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlCon))
                    {
                        string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        int row = dgvEmployee.CurrentCell.RowIndex;
                        dgvEmployee.Rows.RemoveAt(row);

                        refresh();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No rows deleted.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                refresh();
            }
        }
        //-------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e) //exit
        {
            Environment.Exit(0);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bs.MoveFirst();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bs.MovePrevious();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bs.MoveNext();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bs.MoveLast();
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
