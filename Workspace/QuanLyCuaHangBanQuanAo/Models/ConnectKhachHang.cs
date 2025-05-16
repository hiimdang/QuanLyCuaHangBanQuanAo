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
    public class ConnectKhachHang
    {
        private DBConnection dbConnect;
        public ConnectKhachHang(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }
        public List<KhachHang> LayDanhSachKhachHang()
        {
            string cauTruyVan = "SELECT * FROM KhachHang";

            DataSet dataSet = new DataSet();

            SqlDataAdapter dataEmp = new SqlDataAdapter(cauTruyVan, dbConnect.StrConnect);
            dataEmp.Fill(dataSet, "KhachHang");

            List<KhachHang> ds = new List<KhachHang>();
            for (int i = 0; i < dataSet.Tables["KhachHang"].Rows.Count; i++)
            {
                KhachHang temp = new KhachHang();

                temp.MaKH = int.Parse(dataSet.Tables["KhachHang"].Rows[i]["MaKH"].ToString());
                temp.Ho = dataSet.Tables["KhachHang"].Rows[i]["Ho"].ToString();
                temp.Ten = dataSet.Tables["KhachHang"].Rows[i]["Ten"].ToString();
                temp.NgaySinh = DateTime.Parse(dataSet.Tables["KhachHang"].Rows[i]["NgaySinh"].ToString());
                temp.DiaChi = dataSet.Tables["KhachHang"].Rows[i]["DiaChi"].ToString();
                temp.MaTK = int.Parse(dataSet.Tables["KhachHang"].Rows[i]["MaTK"].ToString());


                ds.Add(temp);
            }
            return ds;
        }
        public List<KhachHang> TimKiemKhachHangTheoTuKhoa(string tuKhoa)
        {
            List<KhachHang> ds = new List<KhachHang>();
            string cauTruyVan = "SELECT * FROM KhachHang WHERE Ten LIKE @tuKhoa";

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
                KhachHang temp = new KhachHang
                {
                    MaKH = (int)reader["MaKH"],
                    Ho = reader["Ho"].ToString(),
                    Ten = reader["Ten"].ToString(),
                    NgaySinh = (DateTime)reader["NgaySinh"],
                    DiaChi = reader["DiaChi"].ToString(),
                    MaTK = (int)reader["MaTK"],
                };
                ds.Add(temp);
            }

            //dbConnect.CloseConnect();
            connection.Close();
            return ds;
        }
        public KhachHang LayKhachHangTheoMa(int MaKH)
        {
            string query = "SELECT * FROM KhachHang WHERE MaKH = @MaKH";
            using (SqlConnection conn = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", MaKH);

                conn.Open(); // Mở kết nối tại đây
                SqlDataReader reader = cmd.ExecuteReader();

                KhachHang khachHang = null;
                if (reader.Read())
                {
                    khachHang = new KhachHang
                    {
                        MaKH = (int)reader["MaKH"],
                        Ho = reader["Ho"].ToString(),
                        Ten = reader["Ten"].ToString(),
                        NgaySinh = (DateTime)reader["NgaySinh"],
                        DiaChi = reader["DiaChi"].ToString(),
                        MaTK = (int)reader["MaTK"],
                    };
                }
                reader.Close();
                return khachHang;
            }
        }
        public KhachHang LayKhachHangTheoMaTK(int MaTK)
        {
            string query = "SELECT * FROM KhachHang WHERE MaTK = @MaTK";
            using (SqlConnection conn = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTK", MaTK);

                conn.Open(); // Mở kết nối tại đây
                SqlDataReader reader = cmd.ExecuteReader();

                KhachHang khachHang = null;
                if (reader.Read())
                {
                    khachHang = new KhachHang
                    {
                        MaKH = (int)reader["MaKH"],
                        Ho = reader["Ho"].ToString(),
                        Ten = reader["Ten"].ToString(),
                        NgaySinh = (DateTime)reader["NgaySinh"],
                        DiaChi = reader["DiaChi"].ToString(),
                        MaTK = (int)reader["MaTK"],
                    };
                }
                reader.Close();
                return khachHang;
            }
        }
        public void ThemKhachHang(KhachHang khachHang)
        {
            try
            {
                string cauTruyVan = "INSERT INTO KhachHang VALUES ('" + khachHang.Ho + "','" + khachHang.Ten + "','" + khachHang.NgaySinh + "','" + khachHang.DiaChi + "','" + khachHang.MaTK + "')";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi thêm khách hàng: " + ex.Message);
            }
        }
        public void SuaKhachHang(KhachHang khachHang)
        {
            try
            {
                string cauTruyVan = "UPDATE KhachHang " +
                     "SET Ho = N'" + khachHang.Ho + "', " +
                     "Ten = N'" + khachHang.Ten + "', " +
                     "NgaySinh = N'" + khachHang.NgaySinh + "'," +
                     "DiaChi = N'" + khachHang.DiaChi + "', " +
                     "MaTK = N'" + khachHang.MaTK + "'" +
                     " WHERE MaKH = '" + khachHang.MaKH + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi sửa khách hàng: " + ex.Message);
            }
        }
        public void XoaKhachHang(int maKH)
        {
            try
            {
                string cauTruyVan = "DELETE FROM KhachHang WHERE MaKH = '" + maKH + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa khách hàng: " + ex.Message);
            }
        }
    }
}