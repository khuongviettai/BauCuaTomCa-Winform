using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BauCuaTomCa
{
    public partial class Login : Form
    {
      
        public Login()
        {
            InitializeComponent();
        }
        public string LoggedInUsername { get; private set; }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            decimal currentUserMoney = 0;
            // Lấy thông tin từ các ô nhập liệu
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Kiểm tra xem các ô nhập liệu có trống hay không
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thực hiện kết nối đến cơ sở dữ liệu
            string connectionString = "Data Source=KHUONGVIETTAI;Initial Catalog=BAUCUATOMCA;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Tạo câu truy vấn kiểm tra thông tin đăng nhập
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm các tham số để tránh tình trạng SQL Injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Thực hiện câu truy vấn
                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                        {
                            LoggedInUsername = username;
                            // Lấy số tiền khi đăng nhập thành công
                            query = "SELECT Money FROM Users WHERE Username = @Username AND Password = @Password";
                            using (SqlCommand moneyCommand = new SqlCommand(query, connection))
                            {
                                moneyCommand.Parameters.AddWithValue("@Username", username);
                                moneyCommand.Parameters.AddWithValue("@Password", password);

                                currentUserMoney = Convert.ToDecimal(moneyCommand.ExecuteScalar());
                            }

                            this.Hide();
                            Game form1 = new Game(currentUserMoney);
                            form1.Show();
                        }
                        else
                        {
                            MessageBox.Show("Sai username hoặc password. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChangeToRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register register = new Register();
            register.Show();
        }

        
    }
}
