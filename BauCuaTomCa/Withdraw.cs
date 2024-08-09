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

namespace BauCuaTomCa
{
    public partial class Withdraw : Form
    {
        public Withdraw()
        {
            InitializeComponent();
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMoney_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            // Get the username from the login form
            string username = ((Login)Application.OpenForms["Login"]).LoggedInUsername;

            // Get other values from your form controls
            string phone = txtPhone.Text;
            string name = txtName.Text;

            // Parse txtMoney.Text to decimal, handle exceptions appropriately
            decimal withdrawAmount;
            if (decimal.TryParse(txtMoney.Text, out withdrawAmount))
            {
                // Check if the user has enough balance to withdraw
                decimal currentBalance = GetCurrentUserBalance(username);

                if (currentBalance >= withdrawAmount)
                {
                    // Insert data into the database
                    using (SqlConnection connection = new SqlConnection("Data Source=KHUONGVIETTAI;Initial Catalog=BAUCUATOMCA;Integrated Security=True;"))
                    {
                        connection.Open();

                        string query = "INSERT INTO Withdraws (Username, Status, Phone, Name, Money) " +
                                       "VALUES (@Username, @Status, @Phone, @Name, @Money)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@Status", "Pending"); // You might want to set a default status
                            command.Parameters.AddWithValue("@Phone", phone);
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Money", withdrawAmount);

                            command.ExecuteNonQuery();
                        }
                    }

                    // Display a success message
                    MessageBox.Show("Rút tiền thành công! Có thể mất vài giờ để tiền về tài khoản");

                    // You can perform any additional actions after the data is inserted
                }
                else
                {
                    MessageBox.Show("Không đủ tiền. Vui lòng nhập số tiền rút hợp lệ.");
                }
            }
            else
            {
                MessageBox.Show("Định dạng tiền không hợp lệ. Vui lòng nhập một số hợp lệ.");
            }
        }

        // Helper method to get the current balance of the user
        private decimal GetCurrentUserBalance(string username)
        {
            decimal currentBalance = 0;

            using (SqlConnection connection = new SqlConnection("Data Source=KHUONGVIETTAI;Initial Catalog=BAUCUATOMCA;Integrated Security=True;"))
            {
                connection.Open();

                string query = "SELECT Money FROM Users WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    // ExecuteScalar is used to retrieve a single value (in this case, the Money column)
                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        currentBalance = Convert.ToDecimal(result);
                    }
                }
            }

            return currentBalance;
        }

    }
}
