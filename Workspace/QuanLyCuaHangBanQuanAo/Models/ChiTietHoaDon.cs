using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class ChiTietHoaDon
    {
        private int maHD;
        private int maSP;
        private int soLuong;
        private double donGia;

        public int MaHD { get => maHD; set => maHD = value; }
        public int MaSP { get => maSP; set => maSP = value; }
        public int SoLuong { get => soLuong; set => soLuong = value; }
        public double DonGia { get => donGia; set => donGia = value; }

        public ChiTietHoaDon()
        {

        }
        public ChiTietHoaDon(int mahd,int masp,int soluong,double dongia)
        {
            this.MaHD = mahd;
            this.MaSP = masp;
            this.SoLuong = soluong;
            this.DonGia = dongia;
        }
    }
}