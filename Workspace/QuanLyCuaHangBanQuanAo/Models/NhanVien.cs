using Project_QuanAo.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class NhanVien
    {
        private int maNV;
        private string ho;
        private string ten;
        private DateTime ngaySinh;
        private int maTK;

        public int MaNV { get=>maNV; set=>maNV=value; }
        public string Ho { get=>ho; set=>ho=value; }
        public string Ten { get=>ten; set=>ten=value; }
        public DateTime NgaySinh { get => ngaySinh; set => ngaySinh = value; }
        public int MaTK { get=>maTK; set=>maTK=value; }
        public NhanVien() { }
        public NhanVien(int MaNV, string Ho, string Ten, DateTime NgaySinh, int MaTK)
        {
            this.MaNV = MaNV;
            this.Ho = Ho;
            this.Ten = Ten;
            this.NgaySinh = NgaySinh;
            this.MaTK = MaTK;
        }
    }
}