using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class TaiKhoan
    {
        private int maTK;
        private string tenDangNhap;
        private string matKhau;
        private string email;
        private string soDienThoai;
        private int maLoaiTK;

        public int MaTK { get => maTK; set => maTK = value; }
        public string TenDangNhap { get => tenDangNhap; set => tenDangNhap = value; }
        public string MatKhau { get => matKhau; set => matKhau = value; }
        public string Email { get => email; set => email = value; }
        public int MaLoaiTK { get => maLoaiTK; set => maLoaiTK = value; }
        public string SoDienThoai { get => soDienThoai; set => soDienThoai = value; }
    }
}