using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLDIEM_NguyenTuyetQuynh
{
    public partial class Form1 : Form
    {
        public string State = "";
        public string ConnectionString = @"Data Source=ADMIN\SQLEXPRESS; initial catalog = QLDIEM_NGUYEN TUYET QUYNH; user id = sa; password = 312210";
        public Form1()
        {
            InitializeComponent();
            State = "Reset";
            SetControl(State);
            Getdata();
        }
        public void Getdata()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "SELECT * FROM tblDiemSV";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if(ds != null)
            {
                DataColumn newCol = new DataColumn("VIEW");
                ds.Tables[0].Columns.Add(newCol);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["GioiTinh"].ToString() == "1")
                    {
                        ds.Tables[0].Rows[i]["VIEW"] = "Nam";
                    }
                    else
                    {
                        ds.Tables[0].Rows[i]["VIEW"] = "Nu";
                    }
                }
                dgvDiem.AutoGenerateColumns = false;
                dgvDiem.DataSource = ds.Tables[0];

                //fill data
                txtID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                txtMaSV.Text = ds.Tables[0].Rows[0]["MaSV"].ToString();
                txtTen.Text = ds.Tables[0].Rows[0]["TenSV"].ToString();
                txtCCan.Text = ds.Tables[0].Rows[0]["DiemChuyenCan"].ToString();
                txtGiua.Text = ds.Tables[0].Rows[0]["DiemGiuaKy"].ToString();
                txtCuoi.Text = ds.Tables[0].Rows[0]["DiemCuoiKy"].ToString();
                dtpNgaysinh.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["NgaySinh"]);

                if (ds.Tables[0].Rows[0]["GioiTinh"].ToString() == "1")
                {
                    rdoNam.Checked = true;
                }
                else
                {
                    rdoNam.Checked = false;
                }
            }
            conn.Close();
        }
        public void SetControl(string State)
        {
            switch (State)
            {
                case "Reset":

                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnGhi.Enabled = false;
                    btnHuy.Enabled = false;

                    txtID.Enabled = false;
                    txtMaSV.Enabled = false;
                    txtTen.Enabled = false;
                    txtCCan.Enabled = false;
                    txtGiua.Enabled = false;
                    txtCuoi.Enabled = false;

                    dtpNgaysinh.Enabled = false;
                    rdoNam.Enabled = false;
                    rdoNu.Enabled = false;
                    break;
                case "Insert":
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnGhi.Enabled = true;
                    btnHuy.Enabled = true;

                    txtID.Enabled = true;
                    txtMaSV.Enabled = true;
                    txtTen.Enabled = true;
                    txtCCan.Enabled = true;
                    txtGiua.Enabled = true;
                    txtCuoi.Enabled = true;

                    dtpNgaysinh.Enabled = true;
                    rdoNam.Enabled = true;
                    rdoNu.Enabled = true;

                    txtTen.Focus();
                    break;
                case "Edit":
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnGhi.Enabled = true;
                    btnHuy.Enabled = true;

                    txtID.Enabled = true;
                    txtMaSV.Enabled = true;
                    txtTen.Enabled = true;
                    txtCCan.Enabled = true;
                    txtGiua.Enabled = true;
                    txtCuoi.Enabled = true;

                    dtpNgaysinh.Enabled = true;
                    rdoNam.Enabled = true;
                    rdoNu.Enabled = true;

                    break;
                default:
                    break;
            }
        }
        
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dgvDiem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = dgvDiem.CurrentCell.RowIndex;
                //khai bao item chinh la dòng đc ấn
                DataGridViewRow item = dgvDiem.Rows[index];

                //lay gtri tương ứng với các cell ở trên dòng đc click 
                txtID.Text = item.Cells["ID"].Value.ToString();
                txtMaSV.Text = item.Cells["MaSV"].Value.ToString();
                txtTen.Text = item.Cells["TenSV"].Value.ToString();
                txtCCan.Text = item.Cells["DiemChuyenCan"].Value.ToString();
                txtGiua.Text = item.Cells["DiemGiuaKy"].Value.ToString();
                txtCuoi.Text = item.Cells["DiemCuoiKy"].Value.ToString();
                
                dtpNgaysinh.Value = Convert.ToDateTime(item.Cells["NgaySinh"].Value);
                if (item.Cells["GTT"].Value.ToString() == "1")
                {
                    rdoNam.Checked = true;
                    rdoNu.Checked = false;
                }
                else
                {
                    rdoNam.Checked = false;
                    rdoNu.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            State = "Insert";
            SetControl(State);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            State = "Edit";
            SetControl(State);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            State = "Reset";
            SetControl(State);
        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            try
            {
                if (State == "Insert")
                {
                    //them du lieu
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    string GioiTinh = "1";
                    if (rdoNam.Checked == false) { GioiTinh = "0"; }
                    string query = "INSERT INTO tblDiemSV VALUES (N'" + txtMaSV.Text + "', N'" + txtTen.Text + "', '" + dtpNgaysinh.Value.ToString("MM/dd/yyyy") + "'," +
                        "'" + GioiTinh + "', '" + txtCCan.Text + "', '" + txtGiua.Text + "', N'" + txtCuoi.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Ghi du lieu thanh cong", "Thong bao");
                    State = "Reset";
                    SetControl(State);
                    Getdata();
                }
                else
                {
                    //Sua
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    string GioiTinh = "1";
                    if (rdoNam.Checked == false) { GioiTinh = "0"; }
                    string query = "UPDATE tblDiemSV SET MaSV= N'" + txtMaSV.Text.Trim() + "',TenSV= N'" + txtTen.Text.Trim() + "',NgaySinh='" + dtpNgaysinh.Value.ToString("MM/dd/yyyy") + "'," +
                        "GioiTinh='" + GioiTinh + "',DiemChuyenCan=N'" + txtCCan.Text + "', DiemGiuaKy=N'" + txtGiua.Text + "',DiemCuoiKy=N'" + txtCuoi.Text + "'WHERE ID='" + txtID.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cap nhat du lieu thanh cong", "Thong bao");
                    State = "Reset";
                    SetControl(State);
                    Getdata();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ban co muon xoa k?", "Thong bao", MessageBoxButtons.YesNo) == DialogResult.No) { return; }
                SqlConnection conn = new SqlConnection(ConnectionString);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "DELETE FROM tblDiemSV WHERE ID='" + txtID.Text + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Xoa su lieu thanh cong");
                State = "Reset";
                SetControl(State);
                Getdata();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
