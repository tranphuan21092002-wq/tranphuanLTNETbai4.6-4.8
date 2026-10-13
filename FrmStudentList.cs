using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AsyncProgressBarApp
{
    public partial class FrmStudentList : Form
    {
        private readonly StudentDataService _studentService;

        public FrmStudentList()
        {
            InitializeComponent();
            _studentService = new StudentDataService();
        }

        private async void btnLoadData_Click(object sender, EventArgs e)
        {
            // 1. Tự động khóa nút bấm tránh kích hoạt trùng lặp
            btnLoadData.Enabled = false;
            progressBar1.Value = 0;
            lblPercent.Text = "0%";

            // 2. Khởi tạo IProgress<int> cập nhật giao diện theo thời gian thực
            IProgress<int> progress = new Progress<int>(percent =>
            {
                // Cập nhật trên Form con
                progressBar1.Value = percent;
                lblPercent.Text = $"{percent}%";

                // Đồng bộ lên StatusStrip của Form cha MDI
                if (this.MdiParent is FrmMain mainForm)
                {
                    mainForm.UpdateStatusProgress(percent, "Đang nạp dữ liệu sinh viên...");
                }
            });

            try
            {
                // 3. Gọi phương thức Async/Await
                List<Student> students = await _studentService.LoadStudentsAsync(progress);

                // Gán nguồn dữ liệu cho DataGridView
                dgvStudents.DataSource = students;

                MessageBox.Show("Nạp dữ liệu hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 4. Mở lại nút bấm khi tác vụ kết thúc hoặc có lỗi
                btnLoadData.Enabled = true;

                if (this.MdiParent is FrmMain mainForm)
                {
                    mainForm.UpdateStatusProgress(100, "Sẵn sàng.");
                }
            }
        }
    }
}