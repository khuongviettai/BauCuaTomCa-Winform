using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BauCuaTomCa
{
    public partial class Deposit : Form
    {
        public Deposit()
        {
            InitializeComponent();
        }

        private void OptionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void MoneySelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSeri_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            // Get the selected options
            // Get the selected options
            string option = OptionSelect.SelectedItem.ToString();

            // Extract the selected amount without commas
            decimal money = Convert.ToDecimal(MoneySelect.SelectedItem.ToString().Replace(".", ""));
            string seri = txtSeri.Text;
            string number = txtNumber.Text;

            // Get the username from the login form (assuming it's stored in a variable named 'username')
            string username = ((Login)Application.OpenForms["Login"]).LoggedInUsername;

            // Check if all required fields are filled
            if (string.IsNullOrWhiteSpace(option) || string.IsNullOrWhiteSpace(seri) || string.IsNullOrWhiteSpace(number))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin nạp tiền.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Perform database operation to insert a new deposit record
            string connectionString = "Data Source=KHUONGVIETTAI;Initial Catalog=BAUCUATOMCA;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Insert a new deposit record with 'Pending' status
                    string query = "INSERT INTO Deposits (Username, Status, Options, Seri, Number, Money) VALUES (@Username, @Status, @Options, @Seri, @Number, @Money)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Status", "Pending");
                        command.Parameters.AddWithValue("@Options", option);
                        command.Parameters.AddWithValue("@Seri", seri);
                        command.Parameters.AddWithValue("@Number", number);
                        command.Parameters.AddWithValue("@Money", money);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Nạp tiền thành công! Đang chờ xác nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       
    }
}
