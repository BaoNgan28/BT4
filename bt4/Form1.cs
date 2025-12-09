using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace bt4
{
    public partial class bt4 : Form
    {
        public bt4()
        {
            InitializeComponent();
        }

        // 1. Tạo dữ liệu mẫu
        private List<Student> add_MauData()
        {
            return new List<Student>
            {
                new Student { maSinhVien = "SV001", hoTen = "Nguyễn Thanh Bảo Ngân", gioiTinh = 1, diemTB = 8.5f, chuyenNganh = "Công Nghệ Thông Tin" },
                new Student { maSinhVien = "SV002", hoTen = "Trần Thị Thảo", gioiTinh = 0, diemTB = 7.2f, chuyenNganh = "Quản trị kinh doanh" },
                new Student { maSinhVien = "SV003", hoTen = "Hoàng Tuấn Cường", gioiTinh = 1, diemTB = 9.0f, chuyenNganh = "Công Nghệ Thông Tin" },
                new Student { maSinhVien = "SV004", hoTen = "Đào Thị Trang", gioiTinh = 0, diemTB = 6.5f, chuyenNganh = "Quản trị kinh doanh" },
                new Student { maSinhVien = "SV005", hoTen = "Lê Hoàng Tú ", gioiTinh = 1, diemTB = 5.8f, chuyenNganh = "Ngôn Ngữ Anh" }
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbb_ChuyenNganh.SelectedIndex = 1;
            rdo_Nu.Checked = true;

            dataGrv.Rows.Clear();
            List<Student> students = add_MauData();
            foreach (var item in students)
            {
                int index = dataGrv.Rows.Add();
                dataGrv.Rows[index].Cells[0].Value = item.maSinhVien;
                dataGrv.Rows[index].Cells[1].Value = item.hoTen;
                dataGrv.Rows[index].Cells[2].Value = (item.gioiTinh == 1) ? "Nam" : "Nữ";
                dataGrv.Rows[index].Cells[3].Value = item.diemTB;
                dataGrv.Rows[index].Cells[4].Value = item.chuyenNganh;
            }

            UpdateNoLogin();

            dataGrv.CellClick += DataGrv_CellClick;
        }
        private void UpdateNoLogin()
        {
            int countNam = 0;
            int countNu = 0;

            foreach (DataGridViewRow row in dataGrv.Rows)
            {
                if (row.Cells[0].Value == null) continue;

                string gioitinh = row.Cells[2].Value.ToString();
                if (gioitinh == "Nam")
                    countNam++;
                else
                    countNu++;
            }

            lbl_Nam.Text = countNam.ToString();
            lbl_Nu.Text = countNu.ToString();
        }

        private int GetSelectedRow(string studentID)
        {
            for (int i = 0; i < dataGrv.Rows.Count; i++)
            {
                if (dataGrv.Rows[i].Cells[0].Value != null &&
                    dataGrv.Rows[i].Cells[0].Value.ToString() == studentID)
                {
                    return i;
                }
            }
            return -1;
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txb_msv.Text) || string.IsNullOrEmpty(txb_HoTen.Text) || string.IsNullOrEmpty(txb_DiemTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!float.TryParse(txb_DiemTB.Text, out float diemTB))
                {
                    MessageBox.Show("Điểm trung bình phải là số!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string msv = txb_msv.Text;
                string hoten = txb_HoTen.Text;
                string gioitinh = rdo_Nam.Checked ? "Nam" : "Nữ";
                string khoa = cbb_ChuyenNganh.Text;

                int index = GetSelectedRow(msv);

                if (index == -1) 
                {
                    index = dataGrv.Rows.Add();
                    MessageBox.Show("Thêm mới sinh viên thành công!", "Thông báo");
                }
                else 
                {
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo");
                }

                dataGrv.Rows[index].Cells[0].Value = msv;
                dataGrv.Rows[index].Cells[1].Value = hoten;
                dataGrv.Rows[index].Cells[2].Value = gioitinh;
                dataGrv.Rows[index].Cells[3].Value = diemTB;
                dataGrv.Rows[index].Cells[4].Value = khoa;

                ResetForm();
                UpdateNoLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                
                int index = GetSelectedRow(txb_msv.Text);

                if (index == -1)
                {
                    MessageBox.Show("Không tìm thấy sinh viên có mã: " + txb_msv.Text, "Thông báo");
                    return;
                }

                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {txb_msv.Text}?","Xác nhận xóa", MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dataGrv.Rows.RemoveAt(index);
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                    ResetForm();
                    UpdateNoLogin();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}");
            }
        }

        private void DataGrv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            DataGridViewRow row = dataGrv.Rows[e.RowIndex];
            if (row.Cells[0].Value != null)
            {
                txb_msv.Text = row.Cells[0].Value.ToString();
                txb_HoTen.Text = row.Cells[1].Value.ToString();

                string gt = row.Cells[2].Value.ToString();
                if (gt == "Nam") rdo_Nam.Checked = true;
                else rdo_Nu.Checked = true;

                txb_DiemTB.Text = row.Cells[3].Value.ToString();
                cbb_ChuyenNganh.Text = row.Cells[4].Value.ToString();
            }
        }

        private void ResetForm()
        {
            txb_msv.Clear();
            txb_HoTen.Clear();
            txb_DiemTB.Clear();
            rdo_Nam.Checked = true;
            cbb_ChuyenNganh.SelectedIndex = 0;
            txb_msv.Focus();
        }

        private void btn_test_Click(object sender, EventArgs e) { }
    }

}
