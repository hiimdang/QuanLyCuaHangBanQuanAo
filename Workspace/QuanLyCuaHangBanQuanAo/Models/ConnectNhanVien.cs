using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Project_QuanAo.Helpers;

namespace Project_QuanAo.Models
{
    public class ConnectNhanVien
    {
        private DBConnection dbConnect;
        public ConnectNhanVien(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }
        public List<NhanVien> LayDanhSachNhanVien()
        {
            string cauTruyVan = "SELECT * FROM NhanVien";

            DataSet dataSet = new DataSet();

            SqlDataAdapter dataEmp = new SqlDataAdapter(cauTruyVan, dbConnect.StrConnect);
            dataEmp.Fill(dataSet, "NhanVien");

            List<NhanVien> ds = new List<NhanVien>();
            for (int i = 0; i < dataSet.Tables["NhanVien"].Rows.Count; i++)
            {
                NhanVien temp = new NhanVien();

                temp.MaNV = int.Parse(dataSet.Tables["NhanVien"].Rows[i]["MaNV"].ToString());
                temp.Ho = dataSet.Tables["NhanVien"].Rows[i]["Ho"].ToString();
                temp.Ten = dataSet.Tables["NhanVien"].Rows[i]["Ten"].ToString();
                temp.NgaySinh = DateTime.Parse(dataSet.Tables["NhanVien"].Rows[i]["NgaySinh"].ToString());
                temp.MaTK = int.Parse(dataSet.Tables["NhanVien"].Rows[i]["MaTK"].ToString());

                ds.Add(temp);
            }
            return ds;
        }
        public List<NhanVien> TimKiemNhanVienTheoTuKhoa(string tuKhoa)
        {
            List<NhanVien> ds = new List<NhanVien>();
            string cauTruyVan = "SELECT * FROM NhanVien WHERE Ten LIKE @tuKhoa";

            //SqlCommand cmd = new SqlCommand(cauTruyVan, new SqlConnection(dbConnect.StrConnect));
            //cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");

            SqlConnection connection = new SqlConnection(dbConnect.StrConnect);
            SqlCommand cmd = new SqlCommand(cauTruyVan, connection);
            cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");

            //dbConnect.OpenConnect();
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                NhanVien temp = new NhanVien
                {
                    MaNV = (int)reader["MaNV"],
                    Ho = reader["Ho"].ToString(),
                    Ten = reader["Ten"].ToString(),
                    NgaySinh = (DateTime)reader["NgaySinh"],
                    MaTK = (int)reader["MaTK"],
                };
                ds.Add(temp);
            }

            //dbConnect.CloseConnect();
            connection.Close();
            return ds;
        }
        public NhanVien LayNhanVienTheoMa(int MaNV)
        {
            string query = "SELECT * FROM NhanVien WHERE MaNV = @MaNV";
            using (SqlConnection conn = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", MaNV);

                conn.Open(); // Mở kết nối tại đây
                SqlDataReader reader = cmd.ExecuteReader();

                NhanVien nhanVien = null;
                if (reader.Read())
                {
                    nhanVien = new NhanVien
                    {
                        MaNV = (int)reader["MaNV"],
                        Ho = reader["Ho"].ToString(),
                        Ten = reader["Ten"].ToString(),
                        NgaySinh = (DateTime)reader["NgaySinh"],
                        MaTK = (int)reader["MaTK"],
                    };
                }
                reader.Close();
                return nhanVien;
            }
        }
        public NhanVien LayNhanVienTheoMaTK(int MaTK)
        {
            string query = "SELECT * FROM NhanVien WHERE MaTK = @MaTK";
            using (SqlConnection conn = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTK", MaTK);

                conn.Open(); // Mở kết nối tại đây
                SqlDataReader reader = cmd.ExecuteReader();

                NhanVien nhanVien = null;
                if (reader.Read())
                {
                    nhanVien = new NhanVien
                    {
                        MaNV = (int)reader["MaNV"],
                        Ho = reader["Ho"].ToString(),
                        Ten = reader["Ten"].ToString(),
                        NgaySinh = (DateTime)reader["NgaySinh"],
                        MaTK = (int)reader["MaTK"],
                    };
                }
                reader.Close();
                return nhanVien;
            }
        }
        public void ThemNhanVien(NhanVien nhanVien)
        {
            try
            {
                string cauTruyVan = "INSERT INTO NhanVien VALUES ('" + nhanVien.Ho + "','" + nhanVien.Ten + "','" + nhanVien.NgaySinh + "','" + nhanVien.MaTK + "')";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi thêm nhân viên: " + ex.Message);
            }
        }
        public void SuaNhanVien(NhanVien nhanVien)
        {
            try
            {
                string cauTruyVan = "UPDATE NhanVien " +
                     "SET Ho = N'" + nhanVien.Ho + "', " +
                     "Ten = N'" + nhanVien.Ten + "', " +
                     "NgaySinh = N'" + nhanVien.NgaySinh + "'," +
                     "MaTK = N'" + nhanVien.MaTK + "'" +
                     " WHERE MaNV = '" + nhanVien.MaNV + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi sửa nhân viên: " + ex.Message);
            }
        }
        public void XoaNhanVien(int maNV)
        {
            try
            {
                string cauTruyVan = "DELETE FROM NhanVien WHERE MaNV = '" + maNV + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa nhân viên: " + ex.Message);
            }
        }
    }
}