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
    public partial class frmĐơnHàng : Form
    {     
        readonly DataAccess dataAccess = new DataAccess();
        DataTable cTHoaDon = new DataTable();

        public frmĐơnHàng()

        {
            InitializeComponent();

            txtMãHóaĐơn.Text = dataAccess.GetDataTable("select max(MaHD) + 1 from HoaDon").Rows[0][0].ToString();

            dtpThờiGian.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            cbbTenKH.DataSource = dataAccess.GetDataTable("select MaKH, HoTen from KhachHang");
            cbbTenKH.DisplayMember = "HoTen";
            cbbTenKH.ValueMember = "MaKH";

            cbbTenNV.DataSource = dataAccess.GetDataTable("select MaNV, TenNV from NhanVien where ChucVu = N'Bán Hàng'");
            cbbTenNV.DisplayMember = "TenNV";
            cbbTenNV.ValueMember = "MaNV";

            cbbTenSP.DataSource = dataAccess.GetDataTable("select MaSP, TenSP from SanPham");
            cbbTenSP.DisplayMember = "TenSP";
            cbbTenSP.ValueMember = "MaSP";

            cTHoaDon.Columns.Add("MaSP", typeof(string));
            cTHoaDon.Columns.Add("TenSP", typeof(string));
            cTHoaDon.Columns.Add("SL", typeof(int));
            cTHoaDon.Columns.Add("Gia1SP", typeof(float));

            cbbTenKHDoi.DataSource = dataAccess.GetDataTable("select MaKH, HoTen from KhachHang");
            cbbTenKHDoi.DisplayMember = "HoTen";
            cbbTenKHDoi.ValueMember = "MaKH";

            cbbTenKHHD.DataSource = dataAccess.GetDataTable("select MaKH, HoTen from KhachHang");
            cbbTenKHHD.DisplayMember = "HoTen";
            cbbTenKHHD.ValueMember = "MaKH";

            LoadData();
            
        }

        void LoadData()
        {
            dgvCTHoaDon.DataSource = cTHoaDon;
            dgvDonHang.DataSource = dataAccess.GetDataTable("select * from HoaDon");
        }

        private void btnThêm_Click(object sender, EventArgs e)
        {
            if (nudSL.Value > 0)
            {
                DataTable tTSP = dataAccess.GetDataTable(string.Format("select * from SanPham where MaSP = '{0}'", cbbTenSP.SelectedValue));
                DataRow _dr = cTHoaDon.NewRow();
                _dr["MaSP"] = cbbTenSP.SelectedValue;
                _dr["TenSP"] = tTSP.Rows[0]["TenSP"];
                _dr["SL"] = nudSL.Value;
                _dr["Gia1SP"] = tTSP.Rows[0]["Gia"];
                cTHoaDon.Rows.Add(_dr);
                LoadData();

                float tongTien = cTHoaDon.AsEnumerable()
                .Sum(r => r.Field<int>("SL") * r.Field<float>("Gia1SP"));
                lblTongGia.Text = tongTien.ToString();
            }
        }

        private void btnXóa_Click(object sender, EventArgs e)
        {
            int i = dgvCTHoaDon.SelectedRows[0].Index;
            cTHoaDon.Rows.Remove(cTHoaDon.Rows[i]);
            LoadData();

            float tongTien = cTHoaDon.AsEnumerable()
            .Sum(r => r.Field<int>("SL") * r.Field<float>("Gia1SP"));
            lblTongTienTinhDuoc.Text = tongTien.ToString();
        }

        private void btnXácNhận_Click(object sender, EventArgs e)
        {
            string queryHD = string.Format("insert into HoaDon values ({0},{1},{2},{3},'{4}',N'Thành Công')",
                txtMãHóaĐơn.Text, cbbTenKH.SelectedValue, cbbTenNV.SelectedValue, lblTongTienTinhDuoc.Text, dtpThờiGian.Value);
            dataAccess.UpdateData(queryHD);

            for (int i = 0; i < cTHoaDon.Rows.Count; i++)
            {
                string queryCTHD = string.Format("insert into CTHoaDon values ({0},'{1}',N'{2}',{3},{4})",
                txtMãHóaĐơn.Text, cTHoaDon.Rows[i]["MaSP"], cTHoaDon.Rows[i]["TenSP"], cTHoaDon.Rows[i]["SL"], cTHoaDon.Rows[i]["Gia1SP"]);
                string queryCapNhatSP = string.Format("update SanPham set SLTrong Kho -= {0} where MaSP = {1}", cTHoaDon.Rows[i]["SL"], cTHoaDon.Rows[i]["MaSP"]);
                
                dataAccess.UpdateData(queryCTHD);
                dataAccess.UpdateData(queryCapNhatSP);
            }

            MessageBox.Show("Tạo Hóa Đơn Thành Công!");
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string queryTimKiemHD = "";
            if (txtMaHDTraCuu.Text != "")
            {
                queryTimKiemHD = string.Format("select * from CTHoaDon where MaHD = {0}", txtMaHDTraCuu.Text);
            }            
            else if (cbbTenKHHD.SelectedValue.ToString() != "")
            {
                queryTimKiemHD = string.Format("select * from HoaDon where MaKH = {0}", cbbTenKHHD.SelectedValue);
            }
            dgvDonHang.DataSource = dataAccess.GetDataTable(queryTimKiemHD);
        }

        private void dgvDonHang_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvDonHang.SelectedCells[0].OwningRow;
            string maHD = dr.Cells["MaHD"].Value.ToString();
            string maKH = dr.Cells["MaKH"].Value.ToString();  

            txtMaHDTraCuu.Text = maHD;       
            cbbTenKHHD.SelectedValue = maKH;

            dgvDonHang.DataSource = dataAccess.GetDataTable(string.Format("select * from CTHoaDon where MaHD = {0}", maHD));
        }

        private void ptbQuayLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnHuyDon_Click(object sender, EventArgs e)
        {
            cTHoaDon = dataAccess.GetDataTable(string.Format("select * from CTHoaDon where MaHD = {0}", txtMaHDTraCuu.Text));

            dataAccess.UpdateData(string.Format("update HoaDon set TrangThai = N'Hủy' where MaHD = {0}", txtMaHDTraCuu.Text));
            dataAccess.UpdateData(string.Format("delete from CTHoaDon where MaHD = {0}", txtMaHDTraCuu.Text));
            
            for (int i = 0; i < cTHoaDon.Rows.Count; i++)
            {                
                string queryCapNhatSP = string.Format("update SanPham set SLTrong Kho += {0} where MaSP = {1}", cTHoaDon.Rows[i]["SL"], cTHoaDon.Rows[i]["MaSP"]);

                dataAccess.UpdateData(queryCapNhatSP);
            }
        }

        private void txtXacNhan_Click(object sender, EventArgs e)
        {
            string queryCapNhatSP = string.Format("update SanPham set SLTrong Kho -= {0} where MaSP = {1}", nudSLDoi, cbbTenSPDoi.SelectedValue);
            dataAccess.UpdateData(queryCapNhatSP);
        }

        private void cbbTenKHDoi_SelectedValueChanged(object sender, EventArgs e)
        {
            string queryTraCuuHDDoi = string.Format("select * from HoaDon where MaKH = {0}", cbbTenKHDoi.SelectedValue);
            dgvHoaDonDoi.DataSource = dataAccess.GetDataTable(queryTraCuuHDDoi);
        }

        private void dgvHoaDonDoi_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvHoaDonDoi.SelectedCells[0].OwningRow;
            string maHD = dr.Cells["MaHD"].Value.ToString();
            
            string queryLocSP = string.Format("select * from CTHoaDon where MaHD = {0}", maHD);           
           
            cbbTenSPDoi.DataSource = dataAccess.GetDataTable(queryLocSP);
            cbbTenSPDoi.DisplayMember = "TenSP";
            cbbTenSPDoi.ValueMember = "MaSP";
            
            txtMaHoaDonDoi.Text = maHD;            
        }

        private void txtMaHoaDonDoi_TextChanged(object sender, EventArgs e)
        {
            string queryTraCuuHDDoi = string.Format("select * from CTHoaDon where MaHD = {0}", txtMaHoaDonDoi.Text);
            dgvHoaDonDoi.DataSource = dataAccess.GetDataTable(queryTraCuuHDDoi);
        }

        private void cbbTenKHHD_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string queryTraCuuHD = string.Format("select * from HoaDon where MaKH = {0}", cbbTenKHHD.SelectedValue);
            dgvHoaDonDoi.DataSource = dataAccess.GetDataTable(queryTraCuuHD);
        }
    }
}
