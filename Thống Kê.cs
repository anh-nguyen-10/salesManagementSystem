using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace qlbh1234
{
    public partial class frmThốngKê : Form
    {
        readonly DataAccess dataAccess = new DataAccess();
        public frmThốngKê()
        {
            InitializeComponent();
            cbbTenSP.DataSource = dataAccess.GetDataTable("select MaSP, TenSP from SanPham");
            cbbTenSP.DisplayMember = "TenSP";
            cbbTenSP.ValueMember = "MaSP";

            cbbTenKH.DataSource = dataAccess.GetDataTable("select MaKH, HoTen from KhachHang");
            cbbTenKH.DisplayMember = "HoTen";
            cbbTenKH.ValueMember = "MaKH";
        }


        private void dtpNgày_ValueChanged(object sender, EventArgs e)
        {
            dgvThốngKêĐơnHàng.DataSource = dataAccess.GetDataTable(string.Format("select * from HoaDon where cast(ThoiGian as date) = '{0}'" +
                " and TrangThai = N'Thành Công'", dtpNgày.Value.ToString("yyyy-MM-dd")));
            txtSốLượngĐơn.Text = dataAccess.GetDataTable(string.Format("select count(MaHD) from HoaDon where cast(ThoiGian as date) = '{0}'" +
                " and TrangThai = N'Thành Công'", dtpNgày.Value.ToString("yyyy-MM-dd"))).Rows[0][0].ToString();
        }

        private void dtpThongKeDoanhThu_ValueChanged(object sender, EventArgs e)
        {
            dgvDoanhThu.DataSource = dataAccess.GetDataTable(string.Format("select MaHD, TongTien, ThoiGian from HoaDon where format(ThoiGian, 'MM/yyyy') = '{0}'" +
                "and TrangThai = N'Thành Công'", dtpThongKeDoanhThu.Value.ToString("MM/yyyy")));
            txtDoanhThu.Text = dataAccess.GetDataTable(string.Format("select sum(TongTien) from HoaDon where format(ThoiGian, 'MM/yyyy') = '{0}'" +
            "and TrangThai = N'Thành Công'", dtpThongKeDoanhThu.Value.ToString("MM/yyyy"))).Rows[0][0].ToString();
        }

        private void cbbTenSP_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvLuotMua.DataSource = dataAccess.GetDataTable(string.Format("select * from CTHoaDon where MaSP = '{0}'", cbbTenSP.SelectedValue));
            txtLuotMua.Text = dataAccess.GetDataTable(string.Format("select sum(SL) from CTHoaDon where MaSP = '{0}'", cbbTenSP.SelectedValue)).Rows[0][0].ToString();
        }

        private void cbbTenKH_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            dgvChiTieu.DataSource = dataAccess.GetDataTable(string.Format("select * from HoaDon where MaKH = {0}", cbbTenKH.SelectedValue));
            txtTongChiTieu.Text = dataAccess.GetDataTable(string.Format("select sum(TongTien) from HoaDon where MaKH = {0}", cbbTenKH.SelectedValue)).Rows[0][0].ToString();
        }
    }
}