using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace qlbh1234
{
    public partial class frmĐăngNhập : Form
    {
        readonly DataAccess dataAccess = new DataAccess();
        public frmĐăngNhập()
        {
            InitializeComponent();
        }

        private void btnĐăngNhập_Click(object sender, EventArgs e)
        {
            string maNV = txtMãNhânViên.Text;
            string matKhau = txtMậtKhẩu.Text;
            if (maNV.Trim() == "") { MessageBox.Show("Vui lòng nhập mã nhân viên!"); }
            else if (matKhau.Trim() == "") { MessageBox.Show("Vui lòng nhập mật khẩu!"); }
            else
            {
                string _query = "select * from NhanVien where MaNV = '" + maNV + "'and MatKhau = '" + matKhau + "'";
                DataTable taiKhoan = dataAccess.GetDataTable(_query);
                if (taiKhoan.Rows.Count != 0)
                {
                    frmMainForm mainForm = new frmMainForm();
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else { MessageBox.Show("Sai thông tin đăng nhập!"); }
            }
        }

        private void lblĐăngKý_Click(object sender, EventArgs e)
        {
            frmĐăngKý dangKy = new frmĐăngKý();
            dangKy.ShowDialog();
        }
    }
}
