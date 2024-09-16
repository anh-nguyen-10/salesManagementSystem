using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qlbh1234
{
    public partial class frmDanhMục : Form
    {
        readonly DataAccess dataAccess = new DataAccess();
        public frmDanhMục()
        {
            InitializeComponent();
            LoadData();
            cbbChucVu.DataSource = dataAccess.GetDataTable("select distinct ChucVu from NhanVien");
            cbbChucVu.DisplayMember = "ChucVu";
            cbbChucVu.ValueMember = "ChucVu";

            cbbPhanLoaiSP.DataSource = dataAccess.GetDataTable("select MaPL, TenPL from PhanLoai");
            cbbPhanLoaiSP.DisplayMember = "TenPL";
            cbbPhanLoaiSP.ValueMember = "MaPL";
        }
  
        void LoadData()
        {
            dgvNV.DataSource = dataAccess.GetDataTable("select MaNV, TenNV, NgaySinh, QueQuan, SDT, ChucVu from NhanVien");
            dgvKH.DataSource = dataAccess.GetDataTable("select * from KhachHang");
            dgvPhanLoaiSP.DataSource = dataAccess.GetDataTable("select * from PhanLoai");
            dgvSanPham.DataSource = dataAccess.GetDataTable("select * from SanPham");
            dgvNhaCungCap.DataSource = dataAccess.GetDataTable("select * from NhaCungCap");
        }
      
        private void dgvNV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvNV.SelectedCells[0].OwningRow;
            string maNV = dr.Cells["MaNV"].Value.ToString();
            string tenNV = dr.Cells["TenNV"].Value.ToString();
            string sDT = dr.Cells["SDT"].Value.ToString();
            string chucVu = dr.Cells["ChucVu"].Value.ToString();
            string ngaySinh = dr.Cells["NgaySinh"].Value.ToString();
            string queQuan = dr.Cells["QueQuan"].Value.ToString();

            txtTenNhanVien.Text = tenNV;
            txtMaNhanVien.Text = maNV;
            txtSDTNV.Text = sDT;
            cbbChucVu.Text = chucVu;
            dtpNgaySinh.Value = DateTime.Parse(ngaySinh);
            cbbQueQuan.Text = queQuan;
        }

        private void dgvKH_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvKH.SelectedCells[0].OwningRow;
            string maKH = dr.Cells["MaKH"].Value.ToString();
            string tenKH = dr.Cells["HoTen"].Value.ToString();
            string sDTKH = dr.Cells["SDT"].Value.ToString();
            string diaChiKH = dr.Cells["DiaChi"].Value.ToString();

            txtMaKH.Text = maKH;
            txtTenKH.Text = tenKH;
            txtSDTKH.Text = sDTKH;
            txtDiaChiKH.Text = diaChiKH;
        }

        private void btnThêmNhânViên_Click(object sender, EventArgs e)
        {
            string maNV = dataAccess.GetDataTable("select max(MaNV) + 1 from NhanVien").Rows[0][0].ToString();
            string queryThemNV = string.Format("insert into NhanVien (MaNV, TenNV, NgaySinh, QueQuan, SDT, ChucVu)" +
                " values ({0},N'{1}','{2}',N'{3}','{4}', N'{5}')",
                maNV, txtTenNhanVien.Text, dtpNgaySinh.Value.ToString("yyyy-MM-dd"), cbbQueQuan.Text, txtSDTNV.Text, cbbChucVu.SelectedValue);
            dataAccess.UpdateData(queryThemNV);
            txtMaNhanVien.Text = maNV;
            LoadData();
        }

        private void btnSửaNhânViên_Click(object sender, EventArgs e)
        {
            string querySuaNV = string.Format("update NhanVien set TenNV = N'{0}', NgaySinh = '{1}', QueQuan = N'{2}', SDT = '{3}', ChucVu = N'{4}'" +
                " where MaNV = {5}", txtTenNhanVien.Text, dtpNgaySinh.Value.ToString("yyyy-MM-dd"), cbbQueQuan.Text, txtSDTNV.Text, cbbChucVu.SelectedValue, txtMaNhanVien.Text);
            dataAccess.UpdateData(querySuaNV);
            LoadData();
        }

        private void btnXóaNhânViên_Click(object sender, EventArgs e)
        {
            string queryXoaNV = string.Format("delete from NhanVien where MaNV = {0}", txtMaNhanVien.Text);
            dataAccess.UpdateData(queryXoaNV);
            LoadData();
        }
        private void btnTìmKiếmNhânViên_Click(object sender, EventArgs e)
        {
            string queryTimKiemNV = "";
            if (txtTenNhanVien.Text != "")
            {
                queryTimKiemNV = string.Format("select MaNV, TenNV, NgaySinh, QueQuan, SDT, ChucVu from NhanVien where" +
                    " TenNV like N'%{0}%'", txtTenNhanVien.Text);
            }
            else if (txtMaNhanVien.Text != "")
            {
                queryTimKiemNV = string.Format("select MaNV, TenNV, NgaySinh, QueQuan, SDT, ChucVu from NhanVien where" +
                    " MaNV = {0}", txtMaNhanVien.Text);
            }
            else if (txtSDTNV.Text != "")
            {
                queryTimKiemNV = string.Format("select MaNV, TenNV, NgaySinh, QueQuan, SDT, ChucVu from NhanVien where" +
                    " SDT = {0}", txtMaNhanVien.Text);
            }
            else if (cbbChucVu.SelectedValue.ToString() != "")
            {
                queryTimKiemNV = string.Format("select MaNV, TenNV, NgaySinh, QueQuan, SDT, ChucVu from NhanVien where" +
                   " ChucVu = N'{0}'", cbbChucVu.SelectedValue.ToString());
            }
            else if (cbbQueQuan.SelectedValue.ToString() != "")
            {
                queryTimKiemNV = string.Format("select MaNV, TenNV, NgaySinh, QueQuan, SDT, ChucVu from NhanVien where" +
                   " QueQuan = N'{0}'", cbbQueQuan.SelectedValue.ToString());
            }
            dgvNV.DataSource = dataAccess.GetDataTable(queryTimKiemNV);
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            string maKH = dataAccess.GetDataTable("select max(MaKH) + 1 from KhachHang").Rows[0][0].ToString();
            string queryThemKH = string.Format("insert into KhachHang values ({0},N'{1}','{2}',N'{3}')",
                txtMaKH.Text, txtTenKH.Text, txtSDTKH.Text, txtDiaChiKH.Text);
            dataAccess.UpdateData(queryThemKH);
            txtMaKH.Text = maKH;
            LoadData();
        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            string querySuaKH = string.Format("update KhachHang set HoTen = N'{0}', SDT = '{1}', DiaChi = N'{2}' where MaKH = {3}", 
                txtTenKH.Text, txtSDTKH.Text, txtDiaChiKH.Text, txtMaKH.Text);
            dataAccess.UpdateData(querySuaKH);
            LoadData();
        }

        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            string queryXoaKH = string.Format("delete from KhachHang where MaKH = {0}", txtMaKH.Text);
            dataAccess.UpdateData(queryXoaKH);
            LoadData();
        }

        private void btnTimKiemKH_Click(object sender, EventArgs e)
        {
            string queryTimKiemKH = "";
            if (txtTenKH.Text != "")
            {
                queryTimKiemKH = string.Format("select * from KhachHang where TenKH like N'%{0}%'", txtTenKH.Text);
            }
            else if (txtMaKH.Text != "")
            {
                queryTimKiemKH = string.Format("select * from KhachHang where MaKH = {0}", txtMaKH.Text);
            }
            else if (txtSDTKH.Text != "")
            {
                queryTimKiemKH = string.Format("select * from KhachHang where SDT = {0}", txtSDTKH.Text);
            }
            dgvNV.DataSource = dataAccess.GetDataTable(queryTimKiemKH);
        }

        private void dgvPhanLoaiSP_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvPhanLoaiSP.SelectedCells[0].OwningRow;
            string maPL = dr.Cells["MaPL"].Value.ToString();
            string tenPL = dr.Cells["TenPL"].Value.ToString();

            txtMaPL.Text = maPL;
            txtTenPL.Text = tenPL;
        }

        private void btnThemPL_Click(object sender, EventArgs e)
        {            
            string queryThemPL = string.Format("insert into PhanLoai values ({0},N'{1}')", txtMaPL.Text, txtTenPL.Text);
            dataAccess.UpdateData(queryThemPL);
            LoadData();
        }

        private void btnSuaPL_Click(object sender, EventArgs e)
        {
            string querySuaPL = string.Format("update PhanLoai set TenPL = N'{0}' where MaPL = {1}",
                txtTenPL.Text, txtMaPL.Text);
            dataAccess.UpdateData(querySuaPL);
            LoadData();
        }

        private void btnXoaPL_Click(object sender, EventArgs e)
        {
            string queryXoaPL = string.Format("delete from PhanLoai where MaPL = {0}", txtMaPL.Text);
            dataAccess.UpdateData(queryXoaPL);
            LoadData();
        }

        private void btnTimKiemPL_Click(object sender, EventArgs e)
        {
            string queryTimKiemPL = "";
            if (txtTenPL.Text != "")
            {
                queryTimKiemPL = string.Format("select * from PhanLoai where TenPL like N'%{0}%'", txtTenPL.Text);
            }
            else if (txtMaPL.Text != "")
            {
                queryTimKiemPL = string.Format("select * from PhanLoai where MaPL = {0}", txtMaPL.Text);
            }            
            dgvNV.DataSource = dataAccess.GetDataTable(queryTimKiemPL);
        }

        private void dgvSanPham_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvSanPham.SelectedCells[0].OwningRow;
            string maSP = dr.Cells["MaSP"].Value.ToString();
            string tenSP = dr.Cells["TenSP"].Value.ToString();
            string maPL = dr.Cells["MaPL"].Value.ToString();
            string gia = dr.Cells["Gia"].Value.ToString();
            string sLTrongKho = dr.Cells["SLTrongKho"].Value.ToString();

            txtMaSP.Text = maSP;
            txtTenSP.Text = tenSP;
            cbbPhanLoaiSP.SelectedValue = maPL;
            txtGia.Text = gia;
            nudSLTrongKho.Value = int.Parse(sLTrongKho);
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {           
            string queryThemSP = string.Format("insert into SanPham values ('{0}',N'{1}',{2},{3},0)", 
                txtMaSP.Text, txtTenSP.Text, cbbPhanLoaiSP.SelectedValue, txtGia.Text);
            dataAccess.UpdateData(queryThemSP);
            nudSLTrongKho.Value = 0;
            LoadData();
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            string querySuaSP = string.Format("update SanPham set TenSP = N'{0}', MaPL = {1}, Gia = {2} where MaSP = {3}",
                txtTenSP.Text, cbbPhanLoaiSP.SelectedValue, txtGia.Text, txtMaSP.Text);
            dataAccess.UpdateData(querySuaSP);
            LoadData();
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            string queryXoaSP = string.Format("delete from SanPham where MaSP = {0}", txtMaSP.Text);
            dataAccess.UpdateData(queryXoaSP);
            LoadData();
        }

        private void btnTimKiemSP_Click(object sender, EventArgs e)
        {
            string queryTimKiemSP = "";
            if (txtTenSP.Text != "")
            {
                queryTimKiemSP = string.Format("select * from SanPham where TenSP like N'%{0}%'", txtTenSP.Text);
            }
            else if (txtMaSP.Text != "")
            {
                queryTimKiemSP = string.Format("select * from SanPham where MaSP = {0}", txtMaSP.Text);
            }
            else if (cbbPhanLoaiSP.SelectedValue.ToString() != "")
            {
                queryTimKiemSP = string.Format("select * from SanPham where MaPL = {0}", cbbPhanLoaiSP.SelectedValue);
            }
            dgvNV.DataSource = dataAccess.GetDataTable(queryTimKiemSP);
        }

        private void dgvNhaCungCap_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvNhaCungCap.SelectedCells[0].OwningRow;
            string maNCC = dr.Cells["MaNCC"].Value.ToString();
            string tenNCC = dr.Cells["TenNCC"].Value.ToString();
            string sDTNCC = dr.Cells["SDT"].Value.ToString();
            string diaChiNCC = dr.Cells["DiaChi"].Value.ToString();
            
            txtMaNCC.Text = maNCC;
            txtTenNCC.Text = tenNCC;
            txtSDTNCC.Text = sDTNCC;
            txtDiaChiNCC.Text = diaChiNCC;
        }

        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            string maNCC = dataAccess.GetDataTable("select max(MaNCC) + 1 from NhaCungCap").Rows[0][0].ToString();
            string queryThemNCC = string.Format("insert into NhaCungCap values ({0},N'{1}','{2}',N'{3}')",
                txtMaNCC.Text, txtTenNCC.Text, txtSDTNCC.Text, txtDiaChiNCC);
            dataAccess.UpdateData(queryThemNCC);
            txtMaNCC.Text = maNCC;
            LoadData();
        }

        private void btnSuaNCC_Click(object sender, EventArgs e)
        {
            string querySuaNCC = string.Format("update NhaCungCap set TenNCC = N'{0}', SDT = '{1}', DiaChi = N'{2}' where MaNCC = {3}",
                txtTenNCC.Text, txtSDTNCC.Text, txtDiaChiNCC, txtMaNCC.Text);
            dataAccess.UpdateData(querySuaNCC);
            LoadData();
        }

        private void btnXoaNCC_Click(object sender, EventArgs e)
        {
            string queryXoaNCC = string.Format("delete from NhaCungCap where MaNCC = {0}", txtMaNCC.Text);
            dataAccess.UpdateData(queryXoaNCC);
            LoadData();
        }

        private void btnTimKiemNCC_Click(object sender, EventArgs e)
        {
            string queryTimKiemNCC = "";
            if (txtTenNCC.Text != "")
            {
                queryTimKiemNCC = string.Format("select * from NhaCungCap where TenNCC like N'%{0}%'", txtTenNCC.Text);
            }
            else if (txtMaNCC.Text != "")
            {
                queryTimKiemNCC = string.Format("select * from NhaCungCap where MaSP = {0}", txtMaNCC.Text);
            }
            else if (txtSDTNCC.Text != "")
            {
                queryTimKiemNCC = string.Format("select * from NhaCungCap where SDT = {0}", txtSDTNCC.Text);
            }
            dgvNV.DataSource = dataAccess.GetDataTable(queryTimKiemNCC);
        }                
    }
}
