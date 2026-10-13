using System;
using System.Windows.Forms;

namespace AsyncProgressBarApp
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Tự động mở Form con danh sách sinh viên khi chạy ứng dụng
            FrmStudentList childForm = new FrmStudentList();
            childForm.MdiParent = this;
            childForm.Show();
        }

        /// <summary>
        /// Phương thức hỗ trợ Form con cập nhật StatusStrip ở Form cha
        /// </summary>
        public void UpdateStatusProgress(int percent, string message)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new Action(() => UpdateStatusProgress(percent, message)));
                return;
            }

            toolStripProgressBar1.Value = Math.Min(100, Math.Max(0, percent));
            toolStripStatusLabel1.Text = $"{message} ({percent}%)";
        }
    }
}