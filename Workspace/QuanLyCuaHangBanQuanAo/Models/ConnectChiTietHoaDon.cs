using Project_QuanAo.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class ConnectChiTietHoaDon
    {
        private DBConnection dbConnect;

        public ConnectChiTietHoaDon(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }

        // Thêm chi tiết hóa đơn
        public void ThemChiTietHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
            try
            {
                dbConnect.OpenConnect();
                string query = @"
                INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, GiaTien)
                VALUES (@MaHD, @MaSP, @SoLuong, @DonGia)";

                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                cmd.Parameters.AddWithValue("@MaHD", chiTietHoaDon.MaHD);
                cmd.Parameters.AddWithValue("@MaSP", chiTietHoaDon.MaSP);
                cmd.Parameters.AddWithValue("@SoLuong", chiTietHoaDon.SoLuong);
                cmd.Parameters.AddWithValue("@DonGia", chiTietHoaDon.DonGia);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }
        }

        // Lấy danh sách chi tiết hóa đơn theo mã hóa đơn
        public List<ChiTietHoaDon> LayDanhSachChiTietHoaDon(int maHD)
        {
            List<ChiTietHoaDon> ds = new List<ChiTietHoaDon>();
            string query = "SELECT * FROM ChiTietHoaDon WHERE MaHD = @MaHD";

            try
            {
                dbConnect.OpenConnect();
                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                cmd.Parameters.AddWithValue("@MaHD", maHD);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon
                    {
                        MaHD = (int)reader["MaHD"],
                        MaSP = (int)reader["MaSP"],
                        SoLuong = (int)reader["SoLuong"],
                        DonGia = Convert.ToDouble(reader["DonGia"])
                    };
                    ds.Add(chiTietHoaDon);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }

            return ds;
        }

        // Sửa chi tiết hóa đơn
        public void SuaChiTietHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
            try
            {
                dbConnect.OpenConnect();
                string query = @"
                UPDATE ChiTietHoaDon
                SET SoLuong = @SoLuong, 
                    GiaTien = @DonGia
                WHERE MaHD = @MaHD AND MaSP = @MaSP";

                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                cmd.Parameters.AddWithValue("@SoLuong", chiTietHoaDon.SoLuong);
                cmd.Parameters.AddWithValue("@DonGia", chiTietHoaDon.DonGia);
                cmd.Parameters.AddWithValue("@MaHD", chiTietHoaDon.MaHD);
                cmd.Parameters.AddWithValue("@MaSP", chiTietHoaDon.MaSP);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sửa chi tiết hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }
        }

        // Xóa chi tiết hóa đơn
        public void XoaChiTietHoaDon(int maHD, int maSP)
        {
            try
            {
                dbConnect.OpenConnect();
                string query = "DELETE FROM ChiTietHoaDon WHERE MaHD = @MaHD AND MaSP = @MaSP";

                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                cmd.Parameters.AddWithValue("@MaSP", maSP);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }
        }
    }
}
