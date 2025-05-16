using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class NhaCungCap
    {
        private int maNCC;
        private string tenNCC;
        private string diaChi;
        private string sdt;

        public int MaNCC { get => maNCC; set => maNCC = value; }
        public string TenNCC { get => tenNCC; set => tenNCC = value; }
        public string DiaChi { get => diaChi; set => diaChi = value; }
        public string Sdt { get => sdt; set => sdt = value; }

        public NhaCungCap() { }
        public NhaCungCap(int MaNCC, string TenNCC, string DiaChi, string Sdt)
        {
            this.MaNCC = MaNCC;
            this.TenNCC = TenNCC;
            this.DiaChi = DiaChi;
            this.Sdt = Sdt;
        }
    }
}