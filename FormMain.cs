
using Bai46_QuanLySinhVien;
using System;
using System.Windows.Forms;

namespace Bai47_QuanLyMDI
{
    public partial class FormMain : Form
    {
        private Timer timerDongHo = new Timer();

        public FormMain()
        {
            InitializeComponent();

            IsMdiContainer = true;

            // Menu he thong
            mnuDangXuat.Click += (s, e) => DangXuat();
            mnuThoat.Click += (s, e) => Close();

            // Menu danh muc
            mnuQuanLySinhVien.Click +=
                (s, e) => MoFormSinhVien();

            mnuQuanLySanPham.Click +=
                (s, e) => MoFormSanPham();

            // Menu cua so
            mnuCascade.Click += (s, e) =>
                LayoutMdi(MdiLayout.Cascade);

            mnuTileHorizontal.Click += (s, e) =>
                LayoutMdi(MdiLayout.TileHorizontal);

            mnuTileVertical.Click += (s, e) =>
                LayoutMdi(MdiLayout.TileVertical);

            // StatusStrip
            tslNguoiDung.Text = "Nguoi dung: Admin";
            tslTrangThai.Text = "San sang";

            timerDongHo.Interval = 1000;
            timerDongHo.Tick += (s, e) =>
            {
                tslThoiGian.Text =
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            };
            timerDongHo.Start();
        }


        private void MoFormSinhVien()
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is Bai46_QuanLySinhVien.Form1)
                {
                    f.WindowState = FormWindowState.Maximized;
                    f.Activate();
                    return;
                }
            }

            Bai46_QuanLySinhVien.Form1 form =
                new Bai46_QuanLySinhVien.Form1();

            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }


        private void MoFormSanPham()
        {
            foreach (Form f in MdiChildren)
            {
                if (f is FormProduct)
                {
                    f.Activate();
                    CapNhatTrangThai(
                        "Da kich hoat Form san pham");
                    return;
                }
            }

            FormProduct form = new FormProduct();
            form.MdiParent = this;
            form.Show();

            CapNhatTrangThai("Da mo Form san pham");
        }

        public void CapNhatTrangThai(string thongBao)
        {
            tslTrangThai.Text = thongBao;
        }

        private void DangXuat()
        {
            foreach (Form f in MdiChildren)
            {
                f.Close();
            }

            CapNhatTrangThai("Da dang xuat");
        }
    }
}
