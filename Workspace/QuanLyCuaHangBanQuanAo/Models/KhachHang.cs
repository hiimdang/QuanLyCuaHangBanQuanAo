using Project_QuanAo.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class KhachHang
    {
        private int maKH;
        private string ho;
        private string ten;
        private DateTime ngaySinh;
        private string diaChi;
        private int maTK;

        public int MaKH { get => maKH; set => maKH = value; }
        public string Ho { get => ho; set => ho = value; }
        public string Ten { get => ten; set => ten = value; }
        public DateTime NgaySinh { get => ngaySinh; set => ngaySinh = value; }
        public string DiaChi { get => diaChi; set => diaChi = value; }
        public int MaTK { get => maTK; set => maTK = value; }
        public KhachHang() { }
        public KhachHang(int MaNV, string Ho, string Ten, DateTime NgaySinh, string DiaChi, int MaTK)
        {
            this.MaKH = MaNV;
            this.Ho = Ho;
            this.Ten = Ten;
            this.NgaySinh = NgaySinh;
            this.DiaChi = DiaChi;
            this.MaTK = MaTK;
        }
    }
}