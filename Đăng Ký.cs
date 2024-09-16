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
    public partial class frmĐăngKý : Form
    {
        readonly DataAccess dataAccess = new DataAccess();
        public frmĐăngKý()
        {
            InitializeComponent();
        }

        private void btnĐăngKý_Click(object sender, EventArgs e)
        {
            string maNV = txtMãNhânViên.Text;
            string matKhau = txtMatKhau.Text;
            string matKhauNhapLai = txtMatKhauNhapLai.Text;
            if (maNV.Trim() == "") { MessageBox.Show("Vui lòng nhập mã nhân viên!"); }
            else if (matKhau.Trim() == "") { MessageBox.Show("Vui lòng nhập mật khẩu!"); }
            else
            {
                string _query = "select * from NhanVien where MaNV = '" + maNV + "'and MatKhau is null";
                DataTable taiKhoan = dataAccess.GetDataTable(_query);
                if (taiKhoan.Rows.Count != 0)
                {
                    if (matKhau != matKhauNhapLai)
                    {
                        MessageBox.Show("Mật khẩu không trùng khớp!");
                    }
                    else
                    {
                        dataAccess.UpdateData(string.Format("update NhanVien set MatKhau = {0} where MaNV = {1}", matKhau, maNV));
                    }
                }
                else { MessageBox.Show("Mã nhân viên không hợp lệ!"); }
            }
        }
    }
}
