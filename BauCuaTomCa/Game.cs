using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;


namespace BauCuaTomCa
{
    public partial class Game : Form
    {
        private decimal currentUserMoney;
        private readonly Random random = new Random();
        private string[] imageNames = { "bau", "ca", "cua", "ga", "nai", "tom" };
        private int countMatchingNames = 0;
        private List<string> pictureBoxNames = new List<string>();
        private SoundPlayer backgroundMusicPlayer;
        private SoundPlayer successSoundPlayer;
        private SoundPlayer failSoundPlayer;


        public Game(decimal money)
        {
            InitializeComponent();

            // loading money usser
            currentUserMoney = money;

            // Display the current user money
            txtMoney.Text = FormatMoney(currentUserMoney);
            comboBoxSelect.Items.AddRange(new string[] { "bau", "ca", "cua", "ga", "nai", "tom" });

            // Khởi tạo đối tượng SoundPlayer từ tài nguyên nhúng
            backgroundMusicPlayer = new SoundPlayer(Properties.Resources.soundtrack);
            // Khởi tạo đối tượng SoundPlayer cho âm thanh chức mừng
            successSoundPlayer = new SoundPlayer(Properties.Resources.success);

            // Khởi tạo đối tượng SoundPlayer cho âm thanh thất bại
            failSoundPlayer = new SoundPlayer(Properties.Resources.fail);

            // Khởi tạo hình ảnh ban đầu khi form được tạo
            UpdatePictureBoxes();
        }

        private string FormatMoney(decimal money)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN"); // Vietnamese culture
            return money.ToString("C2", cultureInfo); // Format money as currency in Vietnamese culture
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private int GetRandomIndex()
        {
            return random.Next(imageNames.Length);
        }

        private void UpdatePictureBoxes()
        {
            pictureBoxNames.Clear();
            // Tạo một giá trị ngẫu nhiên duy nhất cho cả ba PictureBox
            int randomIndex = GetRandomIndex();

            // Cập nhật pictureBox2 với hình ảnh được chọn ngẫu nhiên và lấy tên hình ảnh
            string namePictureBox2 = UpdatePictureBox(pictureBox2, imageNames, randomIndex);

            // Cập nhật pictureBox3 với hình ảnh không cần kiểm soát tính duy nhất
            string namePictureBox3 = UpdatePictureBox(pictureBox3, imageNames, GetRandomIndex());

            // Cập nhật pictureBox4 với hình ảnh không cần kiểm soát tính duy nhất
            string namePictureBox4 = UpdatePictureBox(pictureBox4, imageNames, GetRandomIndex());

            // Reset biến đếm về 0 sau mỗi lần chơi
            countMatchingNames = 0;

            // Kiểm tra từng tên trong danh sách
            if (comboBoxSelect.SelectedItem != null)
            {
                string selectedValue = comboBoxSelect.SelectedItem.ToString();

                // Kiểm tra từng tên trong danh sách
                foreach (string pictureBoxName in pictureBoxNames)
                {
                    // Nếu tên giống với giá trị đã chọn, tăng biến đếm
                    if (pictureBoxName.Equals(selectedValue, StringComparison.OrdinalIgnoreCase))
                    {
                        countMatchingNames++;
                    }
                }
            }

            // Hiển thị thông báo với tên hình ảnh của từng PictureBox
            // MessageBox.Show($"Tên hình PictureBox2: {namePictureBox2}\nTên hình PictureBox3: {namePictureBox3}\nTên hình PictureBox4: {namePictureBox4}");
        }

        private string UpdatePictureBox(PictureBox pictureBox, string[] imageNames, int randomIndex)
        {
            // Tạo tên hình ảnh từ sự chọn ngẫu nhiên
            string selectedImageName = imageNames[randomIndex];

            // Cập nhật tên của PictureBox tương ứng với hình ảnh mới
            pictureBox.Name = selectedImageName;

            // Thêm tên của PictureBox vào danh sách
            pictureBoxNames.Add(selectedImageName);

            // Tải hình ảnh được chọn từ tài nguyên
            System.Drawing.Image image = Properties.Resources.ResourceManager.GetObject(selectedImageName) as System.Drawing.Image;

            // Gán hình ảnh cho PictureBox
            pictureBox.Image = image;

            // Trả về tên hình ảnh đã chọn
            return selectedImageName;
        }

        private void txtEnterMoney_TextChanged(object sender, EventArgs e)
        {
            string enteredValue = txtEnterMoney.Text;

            // Kiểm tra từng ký tự trong chuỗi nhập vào
            foreach (char c in enteredValue)
            {
                // Nếu ký tự không phải là số từ 0 đến 9, không thực hiện gì cả và thoát khỏi vòng lặp
                if (c < '0' || c > '9')
                {
                    txtEnterMoney.Text = ""; // Gán giá trị là rỗng nếu có ký tự không hợp lệ
                    return;
                }
            }

            // Nếu chuỗi nhập vào chứa chỉ số từ 0 đến 9, hiển thị nó trong txtMoneyResult
            if (decimal.TryParse(enteredValue, out decimal numericValue))
            {
                txtMoneyResult.Text = numericValue.ToString("N0");
            }
            else
            {
                txtMoneyResult.Text = "";
            }
        }

        private void txtMoneyResult_TextChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem giá trị nhập vào có phải là số không
            if (decimal.TryParse(txtMoneyResult.Text, out decimal resultValue))
            {
                // Định dạng số theo định dạng tiền tệ Việt Nam và hiển thị lại
                txtMoneyResult.Text = resultValue.ToString("N0");

                // Hiển thị item đã chọn từ comboBoxSelect và txtMoneyResult trong TextSelect
                TextSelect.Text = $"{comboBoxSelect.SelectedItem} {txtMoneyResult.Text}";
            }
            else
            {
                // Nếu không phải là số, có thể thực hiện các xử lý khác tùy vào yêu cầu của bạn
                // Ở đây, chỉ đơn giản là gán giá trị là rỗng
                txtMoneyResult.Text = "";
                TextSelect.Text = "";
            }
        }

        private void txtMoney_TextChanged(object sender, EventArgs e)
        {

        }

        private void nạpTiềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deposit deposit = new Deposit();
            deposit.Show();
        }

        private void rútTiềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Withdraw withdraw = new Withdraw();
            withdraw.Show();
        }

        private void hướngDẫnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guide guide = new Guide();
            guide.Show();
        }

        private void hỗTrợToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Support support = new Support();
            support.Show();
        }

        private bool comboBoxSelected = false;

        private void comboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

           
            // Lấy giá trị đã chọn từ comboBoxSelect
            if (comboBoxSelect.SelectedItem != null)
            {
                string selectedValue = comboBoxSelect.SelectedItem.ToString();

             


                // Hiển thị thông báo tùy thuộc vào kết quả

                /*
                if (countMatchingNames > 0)
                {
                    MessageBox.Show($"Có {countMatchingNames} cái PictureBox giống với giá trị {selectedValue} từ comboBoxSelect.");
                }
                else
                {
                    MessageBox.Show($"Không có PictureBox nào giống với giá trị {selectedValue} từ comboBoxSelect.");
                }
                */


                // Kiểm tra xem đã chọn giá trị hay chưa
                if (!string.IsNullOrEmpty(selectedValue))
                {
                    comboBoxSelected = true;
                }
                else
                {
                    comboBoxSelected = false;
                    MessageBox.Show("Vui lòng chọn một giá trị từ comboBoxSelect.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn giá trị từ comboBoxSelect hay chưa
            if (!comboBoxSelected)
            {
                MessageBox.Show("Vui lòng chọn một con vật trước khi chơi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật hình ảnh trước khi kiểm tra
            UpdatePictureBoxes();
      
            // Kiểm tra nếu người dùng đã nhập một số tiền hợp lệ
            if (decimal.TryParse(txtMoneyResult.Text, out decimal betAmount))
            {
                // Kiểm tra số tiền cược không được lớn hơn số tiền hiện có
                if (betAmount > currentUserMoney)
                {
                    MessageBox.Show("Số tiền cược không được lớn hơn số tiền hiện có.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra số tiền cược phải lớn hơn 0
                if (betAmount <= 0)
                {
                    MessageBox.Show("Số tiền cược phải lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lưu lại số tiền hiện có trước khi chơi để kiểm tra sau cùng
                decimal previousMoney = currentUserMoney;

                // Kiểm tra xem 3 hình đã cập nhật có giống với giá trị đã chọn từ comboBoxSelect hay không
                bool matchingImages = countMatchingNames > 0;

                // Nếu 3 hình giống, cộng tiền, ngược lại trừ tiền
                if (matchingImages)
                {
                    currentUserMoney += betAmount * countMatchingNames;
                   
                }
                else
                {
                    currentUserMoney -= betAmount;
                }

                // Hiển thị số tiền hiện có mới sau khi chơi
                txtMoney.Text = FormatMoney(currentUserMoney);
                // Cập nhật số tiền vào cơ sở dữ liệu
                UpdateUserMoneyInDatabase(currentUserMoney);

                // Hiển thị thông báo về kết quả
                if (matchingImages)
                {
                    // Phát âm thanh chức mừng
                    PlaySuccessSound();
                    MessageBox.Show($"Chúc mừng! Bạn đã thắng {betAmount * countMatchingNames:C}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    backgroundMusicPlayer.PlayLooping();
                }
                else
                {
                    PlayFailSound();
                    MessageBox.Show($"Rất tiếc! Bạn đã thua {betAmount:C}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    backgroundMusicPlayer.PlayLooping();
                }

                // Xóa ô đã chọn và cược tiền
                comboBoxSelect.SelectedIndex = -1;
                txtEnterMoney.Text = "";
                TextSelect.Text = "";
              
            }
            else
            {
                MessageBox.Show("Vui lòng nhập một số tiền cược hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }
        private void UpdateUserMoneyInDatabase(decimal newMoney)
        {
            // Thực hiện kết nối đến cơ sở dữ liệu
            string connectionString = "Data Source=KHUONGVIETTAI;Initial Catalog=BAUCUATOMCA;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Tạo câu truy vấn cập nhật số tiền cho người dùng đã đăng nhập
                    string updateQuery = "UPDATE USERS SET Money = @NewMoney WHERE Username = @Username";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        // Thêm các tham số để tránh tình trạng SQL Injection
                        updateCommand.Parameters.AddWithValue("@NewMoney", newMoney);
                        updateCommand.Parameters.AddWithValue("@Username", ((Login)Application.OpenForms["Login"]).LoggedInUsername); // Sử dụng username đã lưu từ trước

                        // Thực hiện câu truy vấn cập nhật
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                           
                        }
                        else
                        {
                            // Không có dòng nào được cập nhật (có thể do username không tồn tại)
                            MessageBox.Show("Hệ thông đang gặp lỗi. Vui lòng thử lại sau hoặc liên hệ hỗ trợ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật số tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void TextSelect_Click(object sender, EventArgs e)
        {

        }

        private void lịchSửNạpTiềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DepositHistory depositHistory = new DepositHistory();
            depositHistory.Show();
        }

        private void lịchSửRútTiềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WithdrawalHistory withdrawalHistory = new WithdrawalHistory();
            withdrawalHistory.Show();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            backgroundMusicPlayer.PlayLooping();
        }
        private void PlaySuccessSound()
        {
            // Tạm dừng phát âm thanh nền để tránh xung đột
            backgroundMusicPlayer.Stop();

            // Kiểm tra xem âm thanh chưa được phát
            if (!successSoundPlayer.IsLoadCompleted)
            {
                // Bắt đầu phát âm thanh chức mừng
                successSoundPlayer.Play();
            }
            
        }

        private void PlayFailSound()
        {
            // Tạm dừng phát âm thanh nền để tránh xung đột
            backgroundMusicPlayer.Stop();

            // Kiểm tra xem âm thanh chưa được phát
            if (!failSoundPlayer.IsLoadCompleted)
            {
                // Bắt đầu phát âm thanh thất bại
                failSoundPlayer.Play();
            }

           
            
        }
    }
}
