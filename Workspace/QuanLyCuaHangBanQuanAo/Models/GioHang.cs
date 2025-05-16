using Project_QuanAo.Helpers;
using Project_QuanAo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class GioHang
    {
        private int maSP;
        private string tenSP;
        private double giaBan;
        private string duongDanAnh;
        private int soLuong;
        public double ThanhTien
        {
            get { return SoLuong * GiaBan; }
        }

        public int MaSP { get => maSP; set => maSP = value; }
        public string TenSP { get => tenSP; set => tenSP = value; }
        public double GiaBan { get => giaBan; set => giaBan = value; }
        public string DuongDanAnh { get => duongDanAnh; set => duongDanAnh = value; }
        public int SoLuong { get => soLuong; set => soLuong = value; }

        public GioHang()
        {

        }
        //public GioHang(int MaSP)
        //{
        //    //this.MaSP = MaSP;
        //    //this.TenSP = TenSP;
        //    //this.GiaBan = GiaBan;
        //    //this.DuongDanAnh = DuongDanAnh;
        //    //this.SoLuong = 1;

        //}

        public GioHang(int MaSP, string TenSP, double GiaBan, string DuongDanAnh)
        {
            this.MaSP = MaSP;
            this.TenSP = TenSP;
            this.GiaBan = GiaBan;
            this.DuongDanAnh = DuongDanAnh;
            this.SoLuong = 1;
        }
    }
}