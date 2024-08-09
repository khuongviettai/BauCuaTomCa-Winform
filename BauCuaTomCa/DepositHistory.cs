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
    public partial class DepositHistory : Form
    {
        public DepositHistory()
        {
            InitializeComponent();
        }

        private void DepositHistory_Load(object sender, EventArgs e)
        {
            // Đặt username của người dùng đã đăng nhập vào biến username
            string username = ((Login)Application.OpenForms["Login"]).LoggedInUsername;

            // Chuỗi truy vấn SQL để lấy lịch sử nạp tiền dựa trên username
            string query = "SELECT * FROM Deposits WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection("Data Source=KHUONGVIETTAI;Initial Catalog=BAUCUATOMCA;Integrated Security=True;"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số để tránh tình trạng SQL injection
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Tạo một DataTable để chứa dữ liệu từ SQL Server
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataTable.Columns.Remove("ID");

                        // Gán DataTable làm DataSource cho DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }
    }
}
