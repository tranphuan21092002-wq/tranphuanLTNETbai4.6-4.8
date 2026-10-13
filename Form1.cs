
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Bai46_QuanLySinhVien
{
    public partial class Form1 : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            // Khoi tao form khi mo chuong trinh
        }
        // Danh sach sinh vien co kha nang thong bao thay doi
        private BindingList<Student> danhSachSV =
            new BindingList<Student>();

        // Duong dan anh dang chon de them hoac cap nhat
        private string duongDanAnh = "";

        public Form1()
        {
            InitializeComponent();

            KhoiTaoGiaoDien();
            KhoiTaoBinding();
            GanSuKien();
        }

        private void KhoiTaoGiaoDien()
        {
            // Danh sach gioi tinh
            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nu");
            cboGioiTinh.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // Cau hinh anh dai dien
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;

            // Cau hinh bang
            dgvSinhVien.AutoGenerateColumns = true;
            dgvSinhVien.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvSinhVien.MultiSelect = false;
            dgvSinhVien.ReadOnly = true;
            dgvSinhVien.AllowUserToAddRows = false;
            dgvSinhVien.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void KhoiTaoBinding()
        {
            // Gan BindingSource voi danh sach sinh vien
            bsSinhVien.DataSource = danhSachSV;

            // Gan BindingSource vao DataGridView
            dgvSinhVien.DataSource = bsSinhVien;

            // Lien ket hai chieu giua TextBox va du lieu
            txtMaSV.DataBindings.Add(
                "Text", bsSinhVien, "StudentId",
                true, DataSourceUpdateMode.OnPropertyChanged);

            txtHoTen.DataBindings.Add(
                "Text", bsSinhVien, "FullName",
                true, DataSourceUpdateMode.OnPropertyChanged);

            txtTuoi.DataBindings.Add(
                "Text", bsSinhVien, "Age",
                true, DataSourceUpdateMode.OnPropertyChanged);

            cboGioiTinh.DataBindings.Add(
                "Text", bsSinhVien, "Gender",
                true, DataSourceUpdateMode.OnPropertyChanged);

            // Hien thi anh khi doi sinh vien dang chon
            bsSinhVien.CurrentChanged += (s, e) =>
            {
                HienThiAnhSinhVienDangChon();
            };
        }

        private void GanSuKien()
        {
            btnThem.Click += btnThem_Click;
            btnXoa.Click += btnXoa_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnChonAnh.Click += btnChonAnh_Click;
            btnXuatCSV.Click += btnXuatCSV_Click;

            // Khi chon dong khac trong bang
            dgvSinhVien.SelectionChanged +=
                dgvSinhVien_SelectionChanged;
        }

        private void HienThiAnh(string duongDan)
        {
            // Giai phong anh cu de tranh khoa file
            if (picAvatar.Image != null)
            {
                Image anhCu = picAvatar.Image;
                picAvatar.Image = null;
                anhCu.Dispose();
            }

            if (!string.IsNullOrWhiteSpace(duongDan)
                && File.Exists(duongDan))
            {
                // Doc anh vao bo nho, khong khoa file goc
                using (FileStream fs = new FileStream(
                    duongDan, FileMode.Open, FileAccess.Read))
                using (Image anhTam = Image.FromStream(fs))
                {
                    picAvatar.Image = new Bitmap(anhTam);
                }
            }
        }

        private void HienThiAnhSinhVienDangChon()
        {
            Student sv = bsSinhVien.Current as Student;

            if (sv == null)
            {
                HienThiAnh("");
                return;
            }

            HienThiAnh(sv.AvatarPath);
        }

        private void dgvSinhVien_SelectionChanged(
            object sender, EventArgs e)
        {
            // Dong hien tai cua BindingSource se tu dong doi
            HienThiAnhSinhVienDangChon();
        }

        private bool KiemTraDuLieu()
        {
            int tuoi;

            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui long nhap ma sinh vien!");
                txtMaSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui long nhap ho ten!");
                txtHoTen.Focus();
                return false;
            }

            if (!int.TryParse(txtTuoi.Text, out tuoi)
                || tuoi < 1 || tuoi > 120)
            {
                MessageBox.Show("Tuoi phai la so tu 1 den 120!");
                txtTuoi.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu())
                return;

            string maSV = txtMaSV.Text.Trim();

            foreach (Student sv in danhSachSV)
            {
                if (string.Equals(
                    sv.StudentId?.Trim(),
                    maSV,
                    StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Ma sinh vien da ton tai!");
                    return;
                }
            }

            Student sinhVienMoi = new Student
            {
                StudentId = maSV,
                FullName = txtHoTen.Text.Trim(),
                Age = int.Parse(txtTuoi.Text),
                Gender = cboGioiTinh.Text,
                AvatarPath = duongDanAnh
            };

            danhSachSV.Add(sinhVienMoi);

            bsSinhVien.Position = danhSachSV.Count - 1;

            MessageBox.Show("Them sinh vien thanh cong!");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            Student sv = bsSinhVien.Current as Student;

            if (sv == null)
            {
                MessageBox.Show("Vui long chon sinh vien can xoa!");
                return;
            }

            DialogResult ketQua = MessageBox.Show(
                "Ban co chac muon xoa sinh vien nay?",
                "Xac nhan xoa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                danhSachSV.Remove(sv);
                HienThiAnhSinhVienDangChon();
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            Student sv = bsSinhVien.Current as Student;

            if (sv == null)
            {
                MessageBox.Show("Vui long chon sinh vien can cap nhat!");
                return;
            }

            if (!KiemTraDuLieu())
                return;

            string maSV = txtMaSV.Text.Trim();

            foreach (Student item in danhSachSV)
            {
                if (item != sv && string.Equals(
                    item.StudentId, maSV,
                    StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Ma sinh vien da ton tai!");
                    return;
                }
            }

            sv.StudentId = maSV;
            sv.FullName = txtHoTen.Text.Trim();
            sv.Age = int.Parse(txtTuoi.Text);
            sv.Gender = cboGioiTinh.Text;
            sv.AvatarPath = duongDanAnh;

            // Lam moi danh sach hien thi
            bsSinhVien.ResetCurrentItem();
            HienThiAnhSinhVienDangChon();

            MessageBox.Show("Cap nhat sinh vien thanh cong!");
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog hopThoai = new OpenFileDialog())
            {
                hopThoai.Title = "Chon anh dai dien";
                hopThoai.Filter =
                    "File hinh anh|*.jpg;*.jpeg;*.png;*.bmp";

                if (hopThoai.ShowDialog() == DialogResult.OK)
                {
                    duongDanAnh = hopThoai.FileName;

                    HienThiAnh(duongDanAnh);
                }
            }
        }

        private void btnXuatCSV_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog hopThoai = new SaveFileDialog())
            {
                hopThoai.Title = "Luu danh sach sinh vien";
                hopThoai.Filter = "CSV (*.csv)|*.csv";
                hopThoai.FileName = "DanhSachSinhVien.csv";
                hopThoai.DefaultExt = "csv";
                hopThoai.AddExtension = true;

                if (hopThoai.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    // UTF-8 co BOM giup Excel nhan dien tieng Viet
                    using (StreamWriter writer = new StreamWriter(
                        hopThoai.FileName, false,
                        new UTF8Encoding(true)))
                    {
                        writer.WriteLine(
                            "MaSV,HoTen,Tuoi,GioiTinh,AvatarPath");

                        foreach (Student sv in danhSachSV)
                        {
                            writer.WriteLine(
                                ChuyenCSV(sv.StudentId) + "," +
                                ChuyenCSV(sv.FullName) + "," +
                                sv.Age + "," +
                                ChuyenCSV(sv.Gender) + "," +
                                ChuyenCSV(sv.AvatarPath));
                        }
                    }

                    MessageBox.Show("Xuat file CSV thanh cong!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Khong the xuat file: " + ex.Message);
                }
            }
        }

        private string ChuyenCSV(string giaTri)
        {
            if (giaTri == null)
                giaTri = "";

            // Bao gia tri trong dau nhay kep,
            // dong thoi nhan doi dau nhay kep ben trong
            return "\"" + giaTri.Replace("\"", "\"\"") + "\"";
        }
    }
}
