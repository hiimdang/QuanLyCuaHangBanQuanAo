using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Project_QuanAo.Helpers;
using Project_QuanAo.Models;

namespace QuanLyCuaHangBanQuanAo.Controllers
{
    public class SanPhamController : Controller
    {
        private DBConnection dbConnect = DatabaseManager.Instance.GetDBConnection();
        private ConnectSanPham cSP;

        public SanPhamController()
        {
            //DBConnection dbConnect = DatabaseManager.Instance.GetDBConnection();
            cSP = new ConnectSanPham(dbConnect);
        }

        [HttpGet]
        public ActionResult TimKiem(string tuKhoa, int page = 1) //Phần này t đang lấy danh sách sản phẩm tạm để hiện thông tin thôi, tự điều chỉnh lấy thông tin tìm kiếm mà tìm đi, sửa trong model ConnectSanPham
        {

            if (string.IsNullOrEmpty(tuKhoa))
            {
                return RedirectToAction("Index", "Home");
            }

            
            int pageSize = 4;
            
            int offset = (page - 1) * pageSize;

            // Lấy danh sách sản phẩm từ database với LIMIT và OFFSET
            List<SanPham> result = cSP.TimKiemSanPhamTheoTuKhoaCoPhanTrang(tuKhoa, pageSize, offset);
            
            int totalProducts = cSP.TimKiemSanPhamTheoTuKhoaCount(tuKhoa);

            
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            
            ViewBag.TuKhoa = tuKhoa;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalProducts = totalProducts;

            return View(result);
        }
        public ActionResult DanhMuc(string loai = null, string sortOrder = null)
        {
            List<SanPham> dsSP;
            if (string.IsNullOrEmpty(loai))
            {
                dsSP = cSP.LayDanhSachSanPham(); 
            }
            else if (loai == "phu-kien")
            {
                dsSP = cSP.LayDanhSachSanPhamTheoLoaiExclude("áo", "quần"); 
            }
            else
            {
                dsSP = cSP.LayDanhSachSanPhamTheoLoai(loai);
            }

            switch (sortOrder)
            {
                case "1":
                    // Tên A-Z
                    dsSP = dsSP.OrderBy(sp => sp.TenSP).ToList();
                    break;
                case "2":
                    // Tên Z-A
                    dsSP = dsSP.OrderByDescending(sp => sp.TenSP).ToList();
                    break;
                case "3":
                    // Giá tăng dần
                    dsSP = dsSP.OrderBy(sp => sp.GiaBan).ToList();
                    break;
                case "4":
                    // Giá giảm dần
                    dsSP = dsSP.OrderByDescending(sp => sp.GiaBan).ToList();
                    break;
                default:
                    break;
            }

            return View(dsSP);
        }

        public ActionResult XemChiTiet(int id)
        {
            SanPham sp = cSP.LaySanPhamTheoMa(id);
            ViewBag.SanPham = sp;
            return View();
        }
    }
}