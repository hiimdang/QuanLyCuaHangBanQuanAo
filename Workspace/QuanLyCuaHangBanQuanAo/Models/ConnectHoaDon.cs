using Project_QuanAo.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class ConnectHoaDon
    {
        private DBConnection dbConnect;

        public ConnectHoaDon(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }

        // Thêm hóa đơn
        public void ThemHoaDon(HoaDon hoaDon)
        {
            try
            {
                dbConnect.OpenConnect();
                string query = @"
                INSERT INTO HoaDon (NgayDat, MaKH, DiaChiGiaoHang, MaTTD, TongTien, MaNV)
                VALUES (@NgayDat, @MaKH, @DiaChiGiaoHang, @MaTTD, @TongTien, @MaNV);
                SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                cmd.Parameters.AddWithValue("@NgayDat", hoaDon.NgayDat);
                cmd.Parameters.AddWithValue("@MaKH", hoaDon.MaKH);
                cmd.Parameters.AddWithValue("@DiaChiGiaoHang", hoaDon.DiaChiGiaoHang);
                cmd.Parameters.AddWithValue("@MaTTD", hoaDon.MaTTD);
                cmd.Parameters.AddWithValue("@TongTien", hoaDon.TongTien);
                // Kiểm tra nếu MaNV không phải là null thì mới thêm vào tham số
                if (hoaDon.MaNV.HasValue)
                {
                    cmd.Parameters.AddWithValue("@MaNV", hoaDon.MaNV.Value);
                }
                else
                {
                    // Nếu MaNV là null, truyền giá trị DBNull để tránh lỗi
                    cmd.Parameters.AddWithValue("@MaNV", DBNull.Value);
                }

                // Lấy ID của hóa đơn mới thêm
                hoaDon.MaHD = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }
        }

        // Lấy danh sách hóa đơn
        public List<HoaDon> LayDanhSachHoaDon()
        {
            List<HoaDon> ds = new List<HoaDon>();
            string query = "SELECT * FROM HoaDon ORDER BY NgayDat DESC";

            try
            {
                dbConnect.OpenConnect();
                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HoaDon hoaDon = new HoaDon
                    {
                        MaHD = (int)reader["MaHD"],
                        NgayDat = (DateTime)reader["NgayDat"],
                        MaKH = (int)reader["MaKH"],
                        DiaChiGiaoHang = reader["DiaChiGiaoHang"].ToString(),
                        MaTTD = (int)reader["MaTTD"],
                        TongTien = Convert.ToDouble(reader["TongTien"]),
                        MaNV = reader["MaNV"] is DBNull ? (int?)null : (int)reader["MaNV"]
                    };
                    ds.Add(hoaDon);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }

            return ds;
        }

        public List<HoaDon> LayDanhSachHoaDon(string maTTD)
        {
            List<HoaDon> ds = new List<HoaDon>();
            string query = "SELECT * FROM HoaDon WHERE MaTTD = '" + maTTD + "' ORDER BY NgayDat DESC";

            try
            {
                dbConnect.OpenConnect();
                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HoaDon hoaDon = new HoaDon
                    {
                        MaHD = (int)reader["MaHD"],
                        NgayDat = (DateTime)reader["NgayDat"],
                        MaKH = (int)reader["MaKH"],
                        DiaChiGiaoHang = reader["DiaChiGiaoHang"].ToString(),
                        MaTTD = (int)reader["MaTTD"],
                        TongTien = Convert.ToDouble(reader["TongTien"]),
                        MaNV = reader["MaNV"] is DBNull ? (int?)null : (int)reader["MaNV"]
                    };
                    ds.Add(hoaDon);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }

            return ds;
        }

        public List<HoaDon> LayDanhSachHoaDon(int maKH)
        {
            List<HoaDon> ds = new List<HoaDon>();
            string query = "SELECT * FROM HoaDon WHERE MaKH = '" + maKH + "' ORDER BY NgayDat DESC";

            try
            {
                dbConnect.OpenConnect();
                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HoaDon hoaDon = new HoaDon
                    {
                        MaHD = (int)reader["MaHD"],
                        NgayDat = (DateTime)reader["NgayDat"],
                        MaKH = (int)reader["MaKH"],
                        DiaChiGiaoHang = reader["DiaChiGiaoHang"].ToString(),
                        MaTTD = (int)reader["MaTTD"],
                        TongTien = Convert.ToDouble(reader["TongTien"]),
                        MaNV = reader["MaNV"] is DBNull ? (int?)null : (int)reader["MaNV"]
                    };
                    ds.Add(hoaDon);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }

            return ds;
        }

        // Sửa hóa đơn
        public void SuaHoaDon(HoaDon hoaDon)
        {
            try
            {
                dbConnect.OpenConnect();
                string query = @"
                UPDATE HoaDon
                SET NgayDat = @NgayDat, 
                    MaKH = @MaKH, 
                    DiaChiGiaoHang = @DiaChiGiaoHang, 
                    MaTTD = @MaTTD, 
                    TongTien = @TongTien, 
                    MaNV = @MaNV
                WHERE MaHD = @MaHD";

                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                cmd.Parameters.AddWithValue("@NgayDat", hoaDon.NgayDat);
                cmd.Parameters.AddWithValue("@MaKH", hoaDon.MaKH);
                cmd.Parameters.AddWithValue("@DiaChiGiaoHang", hoaDon.DiaChiGiaoHang);
                cmd.Parameters.AddWithValue("@MaTTD", hoaDon.MaTTD);
                cmd.Parameters.AddWithValue("@TongTien", hoaDon.TongTien);
                cmd.Parameters.AddWithValue("@MaNV", hoaDon.MaNV);
                cmd.Parameters.AddWithValue("@MaHD", hoaDon.MaHD);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sửa hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }
        }

        // Xóa hóa đơn
        public void XoaHoaDon(int maHD)
        {
            try
            {
                dbConnect.OpenConnect();
                string query = "DELETE FROM HoaDon WHERE MaHD = @MaHD";
                SqlCommand cmd = new SqlCommand(query, dbConnect.SConn);
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa hóa đơn: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnect();
            }
        }
        public string LayTenTrangThaiDon(int MaTTD)
        {
            try
            {
                string cauTruyVan = "SELECT TenTTD FROM TrangThaiDon WHERE MaTTD = '" + MaTTD + "'";
                return dbConnect.GetString(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tên trạng thái đơn: " + ex.Message);
            }
        }

        public void DuyetHoaDon(int MaHD, int trinhTrang, int MaNV)
        {
            try
            {
                int maTTD = 2;
                if (trinhTrang == 1) maTTD = 3;
                string cauTruyVan = "UPDATE HoaDon SET MaTTD = '" + maTTD + "', MaNV = '" + MaNV + "' WHERE MaHD = '" + MaHD + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi duyệt / hủy hóa đơn: " + ex.Message);
            }
            
        }
    }
}