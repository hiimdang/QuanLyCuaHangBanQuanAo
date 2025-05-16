using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_QuanAo.Models
{
    public class HoaDon
    {
        private int maHD;
        private DateTime ngayDat;
        private int maKH;
        private string diaChiGiaoHang;
        private int maTTD;
        private double tongTien;
        private int? maNV;

        public int MaHD { get => maHD; set => maHD = value; }
        public DateTime NgayDat { get => ngayDat; set => ngayDat = value; }
        public int MaKH { get => maKH; set => maKH = value; }
        public string DiaChiGiaoHang { get => diaChiGiaoHang; set => diaChiGiaoHang = value; }
        public int MaTTD { get => maTTD; set => maTTD = value; }
        public double TongTien { get => tongTien; set => tongTien = value; }
        public int? MaNV { get => maNV; set => maNV = value; }

        public HoaDon()
        {

        }
        public HoaDon(int mahd,DateTime ngaydat,int makh,string diachi,int mattd,double tongtien,int? manv)
        {
            this.MaHD = mahd;
            this.NgayDat = ngaydat;
            this.MaKH = makh;
            this.DiaChiGiaoHang = diachi;
            this.MaTTD = mattd;
            this.TongTien = tongtien;
            this.MaNV = manv;
            
        }
    }
}