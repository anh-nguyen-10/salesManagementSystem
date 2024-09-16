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
    public partial class frmMainForm : Form
    {
        bool chonChucNang;
        bool chonHeThong;
        private Form chucNangChon;
        public frmMainForm()
        {
            InitializeComponent();
        }

        private void tmrThanhChứcNăng_Tick(object sender, EventArgs e)
        {
            if (chonChucNang)
            {
                flpThanhChứcNăng.Width -= 10;
                if (flpThanhChứcNăng.Width == flpThanhChứcNăng.MinimumSize.Width)
                {
                    chonChucNang = false;
                    tmrThanhChứcNăng.Stop();
                }
            }
            else
            {
                flpThanhChứcNăng.Width += 10;
                if (flpThanhChứcNăng.Width == flpThanhChứcNăng.MaximumSize.Width)
                {
                    chonChucNang = true;
                    tmrThanhChứcNăng.Stop();
                }
            }
        }
        
        private void moChucNang (Form frmChon)
        {
            if (chucNangChon != null)
            {
                chucNangChon.Close();
            }
            chucNangChon = frmChon;
            frmChon.TopLevel = false;
            frmChon.FormBorderStyle = FormBorderStyle.None;
            frmChon.Dock = DockStyle.Fill;
            pnlChon.Controls.Add(frmChon);
            pnlChon.Tag = frmChon;
            frmChon.BringToFront();
            frmChon.Show();
        }

        private void picThanhChứcNăng_Click(object sender, EventArgs e)
        {
            tmrThanhChứcNăng.Start();
        }

        private void btnĐơnHàng_Click(object sender, EventArgs e)
        {
            tmrThanhChứcNăng.Start();
            moChucNang(new frmĐơnHàng());
        }

        private void btnHệThống_Click(object sender, EventArgs e)
        {
            if (pnlHeThong.Height == pnlHeThong.MaximumSize.Height)
            {
                pnlHeThong.Height = pnlHeThong.MinimumSize.Height;
            }
            else
            {
                pnlHeThong.Height = pnlHeThong.MaximumSize.Height;
            }
        }

        private void btnDanhMục_Click(object sender, EventArgs e)
        {
            tmrThanhChứcNăng.Start();
            moChucNang(new frmDanhMục());            
        }

        private void btnKhoHàng_Click(object sender, EventArgs e)
        {
            tmrThanhChứcNăng.Start();
            moChucNang(new frmKhoHàng());
        }

        private void btnThốngKê_Click(object sender, EventArgs e)
        {
            tmrThanhChứcNăng.Start();
            moChucNang(new frmThốngKê());
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            moChucNang(new frmĐăngKý());
        }

        private void btnThayDoi_Click(object sender, EventArgs e)
        {
            moChucNang(new frmThayĐổiThôngTin());
        }
    }
}
