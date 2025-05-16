using Project_QuanAo.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class SanPham
    {
        private int maSP;
        private string tenSP;
        private int maLoaiSP;
        private int maNCC;
        private double giaBan;
        private string duongDanAnh;
        private string moTa;

        public int MaSP { get => maSP; set => maSP = value; }
        public string TenSP { get => tenSP; set => tenSP = value; }
        public int MaLoaiSP { get => maLoaiSP; set => maLoaiSP = value; }
        public int MaNCC { get => maNCC; set => maNCC = value; }
        public double GiaBan { get => giaBan; set => giaBan = value; }
        public string DuongDanAnh { get => duongDanAnh; set => duongDanAnh = value; }
        public string MoTa { get => moTa; set => moTa = value; }
        public SanPham() { }
        public SanPham(int MaSP, string TenSP, int MaLoaiSP, int MaNCC, double GiaBan, string DuongDanAnh, string MoTa)
        {
            this.MaSP = MaSP;
            this.TenSP = TenSP;
            this.MaLoaiSP = MaLoaiSP;
            this.MaNCC = MaNCC;
            this.GiaBan = GiaBan;
            this.DuongDanAnh = DuongDanAnh;
            this.MoTa = MoTa;
        }

        public string LayTenCatNgan(string TenSP)
        {
            return TenSP.Length > 40 ? TenSP.Substring(0, 37) + "..." : TenSP;
        }
    }
}