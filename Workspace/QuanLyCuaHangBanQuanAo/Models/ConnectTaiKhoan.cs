using Project_QuanAo.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class ConnectTaiKhoan
    {
        private DBConnection dbConnect;
        public ConnectTaiKhoan(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }
        public int DangNhap(string TenDangNhap, string MatKhau, ref int MaLoaiTK)
        {
            if (!dbConnect.CheckExist("TaiKhoan", "TenDangNhap", TenDangNhap)) throw new Exception("Không tồn tại người dùng với tên đăng nhập '" + TenDangNhap +"'");
            if (!dbConnect.CheckExist("TaiKhoan","TenDangNhap", TenDangNhap, "MatKhau", MatKhau)) throw new Exception("Sai thông tin đăng nhập");
            string cauTruyVan = "SELECT MaTK From TaiKhoan WHERE TenDangNhap = '" + TenDangNhap + "'";
            MaLoaiTK = dbConnect.GetInt("SELECT MaLoaiTK FROM TaiKhoan WHERE TenDangNhap = '" + TenDangNhap + "'");
            return dbConnect.GetInt(cauTruyVan);
        }
        public void DangKy(string TenDangNhap, string MatKhau, string Email, string SoDienThoai)
        {
            if (dbConnect.CheckExist("TaiKhoan", "TenDangNhap", TenDangNhap)) throw new Exception("Đã tồn tại người dùng với tên đăng nhập '" + TenDangNhap + "'");
            string cauTruyVan = $@"INSERT INTO TaiKhoan VALUES('{TenDangNhap}','{MatKhau}','{Email}','{SoDienThoai}',1)";
            dbConnect.UpdateToDatabase(cauTruyVan);
        }
        public int ThemTaiKhoan(TaiKhoan tk)
        {
            try
            {
                string cauTruyVan = $@"INSERT INTO TaiKhoan VALUES('{tk.TenDangNhap}','{tk.MatKhau}','{tk.Email}','{tk.SoDienThoai}',1); SELECT SCOPE_IDENTITY();";
                return int.Parse(dbConnect.GetObject(cauTruyVan).ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi thêm khách hàng: " + ex.Message);
            }
        }
        public string LayEmailTheoMaTK(int MaTK)
        {
            string cauTruyVan = "SELECT Email FROM TaiKhoan WHERE MaTK = '" + MaTK + "'";
            return dbConnect.GetString(cauTruyVan);
        }
    }
}