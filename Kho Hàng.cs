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
    public partial class frmKhoHàng : Form
    {
        readonly DataAccess dataAccess = new DataAccess();
        DataTable cTHoaDonNhap = new DataTable();
        
        public frmKhoHàng()
        {
            InitializeComponent();
            
            txtMaHDN.Text = dataAccess.GetDataTable("select max(MaHDN) + 1 from HoaDonNhap").Rows[0][0].ToString();
            
            dtpThờiGianNhập.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            cbbTenNCC.DataSource = dataAccess.GetDataTable("select MaNCC, TenNCC from NhaCungCap");
            cbbTenNCC.DisplayMember = "TenNCC";
            cbbTenNCC.ValueMember = "MaNCC";

            cbbTenNV.DataSource = dataAccess.GetDataTable("select MaNV, TenNV from NhanVien where ChucVu = N'Xử lý kho'");
            cbbTenNV.DisplayMember = "TenNV";
            cbbTenNV.ValueMember = "MaNV";

            cbbTenSP.DataSource = dataAccess.GetDataTable("select MaSP, TenSP from SanPham");
            cbbTenSP.DisplayMember = "TenSP";
            cbbTenSP.ValueMember = "MaSP";

            cTHoaDonNhap.Columns.Add("MaSP", typeof(string));
            cTHoaDonNhap.Columns.Add("TenSP", typeof(string));
            cTHoaDonNhap.Columns.Add("SL", typeof(int));
            cTHoaDonNhap.Columns.Add("Gia1SP", typeof(float));

            cbbNVKiemKe.DataSource = dataAccess.GetDataTable("select MaNV, TenNV from NhanVien where ChucVu = N'Xử lý kho'");
            cbbNVKiemKe.DisplayMember = "TenNV";
            cbbNVKiemKe.ValueMember = "MaNV";

            cbbTenSPKiemKe.DataSource = dataAccess.GetDataTable("select MaSP, TenSP from SanPham");
            cbbTenSPKiemKe.DisplayMember = "TenSP";
            cbbTenSPKiemKe.ValueMember = "MaSP";

            cbbNhanVienNhap.DataSource = dataAccess.GetDataTable("select MaNV, TenNV from NhanVien where ChucVu = N'Xử lý kho'");
            cbbNhanVienNhap.DisplayMember = "TenNV";
            cbbNhanVienNhap.ValueMember = "MaNV";

            LoadData();
        }

        void LoadData()
        {
            dgvCTHoaDonNhap.DataSource = cTHoaDonNhap;
            dgvSLTrongKho.DataSource = dataAccess.GetDataTable("select MaSP, TenSP, SLTrongKho from SanPham");
            dgvHDNTraCuu.DataSource = dataAccess.GetDataTable("select * from HoaDonNhap");
        }

        private void btnThêm_Click(object sender, EventArgs e)
        {
            if (nupSL.Value > 0 && float.Parse(txtGia1SPNhap.Text) > 0)
            {
                DataTable tTSP = dataAccess.GetDataTable(string.Format("select * from SanPham where MaSP = '{0}'", cbbTenSP.SelectedValue));
                DataRow _dr = cTHoaDonNhap.NewRow();
                _dr["MaSP"] = cbbTenSP.SelectedValue;
                _dr["TenSP"] = tTSP.Rows[0]["TenSP"];
                _dr["SL"] = nupSL.Value;
                _dr["Gia1SP"] = txtGia1SPNhap.Text;
                cTHoaDonNhap.Rows.Add(_dr);
                LoadData();

                float tongTien = cTHoaDonNhap.AsEnumerable()                
                .Sum(r => r.Field<int>("SL") * r.Field<float>("Gia1SP"));
                lblTongGia.Text = tongTien.ToString();
            }
            else
            {
                MessageBox.Show("Thông tin nhập hàng không hợp lệ!");
            }
        }

        private void btnXóa_Click(object sender, EventArgs e)
        {
             int i = dgvCTHoaDonNhap.SelectedRows[0].Index;
            cTHoaDonNhap.Rows.Remove(cTHoaDonNhap.Rows[i]);
            LoadData();

            float tongTien = cTHoaDonNhap.AsEnumerable()
            .Sum(r => r.Field<int>("SL") * r.Field<float>("Gia1SP"));
            lblTongTienTinhDuoc.Text = tongTien.ToString();
        }

        private void btnXácNhận_Click(object sender, EventArgs e)
        {

            string queryHDN = string.Format("insert into HoaDonNhap values ({0},{1},{2},{3},'{4}')",
                txtMaHDN.Text, cbbTenNCC.SelectedValue, cbbTenNV.SelectedValue, lblTongTienTinhDuoc.Text, dtpThờiGianNhập.Value);
            dataAccess.UpdateData(queryHDN);

            for (int i = 0; i < cTHoaDonNhap.Rows.Count; i++)
            {
                string queryCTHDN = string.Format("insert into CTHoaDonNhap values ({0},'{1}',N'{2}',{3},{4})",
                txtMaHDN.Text, cTHoaDonNhap.Rows[i]["MaSP"], cTHoaDonNhap.Rows[i]["TenSP"], cTHoaDonNhap.Rows[i]["SL"], cTHoaDonNhap.Rows[i]["Gia1SP"]);
                string queryCapNhatSP = string.Format("update SanPham set SLTrong Kho += {0} where MaSP = {1}", cTHoaDonNhap.Rows[i]["SL"], cTHoaDonNhap.Rows[i]["MaSP"]);

                dataAccess.UpdateData(queryCTHDN);
                dataAccess.UpdateData(queryCapNhatSP);
            }

            MessageBox.Show("Tạo Hóa Đơn Nhập Thành Công!");
        }

        private void btnXacNhanKiemKe_Click(object sender, EventArgs e)
        {
            string queryCapNhatSP = string.Format("update SanPham set SLTrong Kho = {0} where MaSP = {1}", nudSLDoi.Value, cbbTenSPKiemKe.SelectedValue);
            dgvSLTrongKho.DataSource = dataAccess.GetDataTable(queryCapNhatSP);
        }

        private void btnTìmKiếm_Click(object sender, EventArgs e)
        {
            string queryTimKiemHDN = "";
            if (txtMaHDNTraCuu.Text != "")
            {
                queryTimKiemHDN = string.Format("select * from HoaDonNhap where MaHDN = {0}", txtMaHDNTraCuu.Text);
            }
            else if (cbbNhanVienNhap.SelectedValue.ToString() !="")
            {
                queryTimKiemHDN = string.Format("select * from HoaDonNhap where MaNV = {0}", cbbNhanVienNhap.SelectedValue.ToString());
            }
            dgvHDNTraCuu.DataSource = dataAccess.GetDataTable(queryTimKiemHDN);
        }

        private void dgvHDNTraCuu_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow dr = dgvHDNTraCuu.SelectedCells[0].OwningRow;
            string maHDN = dr.Cells["MaHDN"].Value.ToString();
            string maNVN = dr.Cells["MaNV"].Value.ToString();

            txtMaHDNTraCuu.Text = maHDN;
            cbbNhanVienNhap.SelectedValue = maNVN;

            dgvHDNTraCuu.DataSource = dataAccess.GetDataTable(string.Format("select * from CTHoaDonNhap where MaHDN = {0}", maHDN));
        }

        private void ptbQuayLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
