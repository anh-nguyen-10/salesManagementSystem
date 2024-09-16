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
    public partial class frmThayĐổiThôngTin : Form
    {
        readonly DataAccess dataAccess = new DataAccess();
        public frmThayĐổiThôngTin()
        {
            InitializeComponent();
        }

        private void btnThayĐổi_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNhanVien.Text;
            string matKhau = txtMatKhau.Text;
            string matKhauMoi = txtMatKhauMoi.Text;
            string matKhauNhapLai = txtMatKhauNhapLai.Text;
            if (maNV.Trim() == "") { MessageBox.Show("Vui lòng nhập mã nhân viên!"); }
            else if (matKhau.Trim() == "") { MessageBox.Show("Vui lòng nhập mật khẩu!"); }
            else
            {
                string _query = "select * from NhanVien where MaNV = '" + maNV + "'and MatKhau = '" + matKhau + "'";
                DataTable taiKhoan = dataAccess.GetDataTable(_query);
                if (taiKhoan.Rows.Count != 0)
                {
                    if (matKhauMoi != matKhauNhapLai)
                    {
                        MessageBox.Show("Mật khẩu không trùng khớp!");
                    }
                    else
                    {
                        dataAccess.UpdateData(string.Format("update NhanVien set MatKhau = {0} where MaNV = {1}", matKhauMoi, maNV));
                    }
                }
                else { MessageBox.Show("Thông tin tài khoản không hợp lệ!"); }
            }
        }
    }
}
