using Project_QuanAo.Helpers;
using Project_QuanAo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCuaHangBanQuanAo.Controllers
{
    public class DangNhapController : Controller
    {
        private DBConnection dbConnect = DatabaseManager.Instance.GetDBConnection();
        // GET: DangNhap
        ConnectTaiKhoan cTK;
        public DangNhapController()
        {
            cTK = new ConnectTaiKhoan(dbConnect);
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(TaiKhoan taiKhoan)
        {
            try
            {
                int LoaiTK = 1;
                int maTK = cTK.DangNhap(taiKhoan.TenDangNhap, taiKhoan.MatKhau, ref LoaiTK);
                Session["MaTK"] = maTK;
                Session["MaLoaiTK"] = LoaiTK;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult DangKy(TaiKhoan taiKhoan)
        //{
        //    try
        //    {
        //        cTK.DangKy(taiKhoan.TenDangNhap,taiKhoan.MatKhau,taiKhoan.Email,taiKhoan.SoDienThoai);
        //        ViewBag.Message = "Đăng ký thành công! Vui lòng đăng nhập";
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = ex.Message;
        //    }
        //    return View("DangNhap");
        //}
        [HttpPost]
        public ActionResult DangKy(FormCollection form)
        {
            try
            {
                string hoVaTen = form["HoVaTen"];
                string tenDangNhap = form["TenDangNhap"];
                //--Xử lý tên
                int lastSpaceIndex = hoVaTen.LastIndexOf(' ');

                string ho, ten;
                if (lastSpaceIndex != -1)
                {
                    ten = hoVaTen.Substring(lastSpaceIndex + 1);
                    ho = hoVaTen.Substring(0, lastSpaceIndex);
                }
                else
                {
                    ten = hoVaTen;
                    ho = "";
                }
                //--
                string email = form["Email"];
                string sdt = form["SoDienThoai"];
                string matKhau = form["MatKhau"];
                DateTime ngaySinh = DateTime.Parse(form["NgaySinh"]);

                //--
                TaiKhoan taikhoan = new TaiKhoan
                {
                    TenDangNhap = tenDangNhap,
                    MatKhau = matKhau,
                    Email = email,
                    SoDienThoai = sdt,
                };
                ConnectTaiKhoan cTK = new ConnectTaiKhoan(dbConnect);
                int maTK = cTK.ThemTaiKhoan(taikhoan);

                ConnectKhachHang cKH = new ConnectKhachHang(dbConnect);

                KhachHang khachHang = new KhachHang
                {
                    Ho = ho,
                    Ten = ten,
                    NgaySinh = ngaySinh,
                    MaTK = maTK,
                };

                // Thêm chi tiết hóa đơn vào database
                cKH.ThemKhachHang(khachHang);
                //--
                ViewBag.Message = "Đăng ký thành công! Vui lòng đăng nhập";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("DangNhap");
        }
        public ActionResult DangXuat()
        {
            Session["MaTK"] = null;
            Session["MaLoaiTK"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}