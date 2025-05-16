using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class LoaiSanPham
    {
        int maLoaiSP;
        string tenLoaiSP;
        public int MaLoaiSP { get => maLoaiSP; set => maLoaiSP = value; }
        public string TenLoaiSP { get => tenLoaiSP; set => tenLoaiSP = value; }
        public LoaiSanPham() { }
        public LoaiSanPham(int MaLoaiSP, string TenLoaiSP)
        {
            this.MaLoaiSP = MaLoaiSP;
            this.TenLoaiSP = TenLoaiSP;
        }
    }
}