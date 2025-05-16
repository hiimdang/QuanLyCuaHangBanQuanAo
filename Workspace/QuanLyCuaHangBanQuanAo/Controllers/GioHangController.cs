using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_QuanAo.Helpers;
using Project_QuanAo.Models;

namespace QuanLyCuaHangBanQuanAo.Controllers
{
    public class GioHangController : Controller
    {

        private DBConnection dbConnect = DatabaseManager.Instance.GetDBConnection();

        [HttpGet]

        public ActionResult Index()
        {
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int msp, string strURL)
        {
            SanPham sanPham = new ConnectSanPham(dbConnect).LaySanPhamTheoMa(msp);

            List<GioHang> gioHangList = LayGioHang();

            GioHang existingProduct = gioHangList.SingleOrDefault(sp => sp.MaSP == msp);

            if (existingProduct != null)
            {
                existingProduct.SoLuong += 1;
                TempData["Message"] = "Tăng số lượng cho sản phẩm thành công!";
            }
            else
            {
                GioHang newProduct = new GioHang
                {
                    MaSP = sanPham.MaSP,
                    TenSP = sanPham.TenSP,
                    GiaBan = sanPham.GiaBan,
                    DuongDanAnh = sanPham.DuongDanAnh,
                    SoLuong = 1,
                };

                gioHangList.Add(newProduct);
                TempData["Message"] = "Thêm vào giỏ hàng thành công!";
            }

            Session["GioHang"] = gioHangList;
            return Redirect(strURL);
        }
        private int TongSoLuong()
        {
            int tSl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tSl = lstGioHang.Sum(sp => sp.SoLuong);
            }
            return tSl;
        }
        private double TongThanhTien()
        {
            double tTT = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tTT += lstGioHang.Sum(sp => sp.ThanhTien);
            }
            return tTT;
        }
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(lstGioHang);

        }


        public ActionResult CapNhatGioHang(int MaSP, int soLuongMoi)
        {
            // Lấy giỏ hàng từ session
            List<GioHang> lstGioHang = LayGioHang();

            // Tìm sản phẩm trong giỏ hàng theo mã sản phẩm
            GioHang sp = lstGioHang.SingleOrDefault(s => s.MaSP == MaSP);

            if (sp != null)
            {
                // Cập nhật số lượng mới và tính lại thành tiền (ThanhTien sẽ tự động cập nhật khi thay đổi SoLuong)
                sp.SoLuong = soLuongMoi;
            }

            // Cập nhật giỏ hàng vào session
            Session["GioHang"] = lstGioHang;

            // Quay lại trang giỏ hàng
            return RedirectToAction("GioHang", "GioHang");
        }
        [HttpPost]
        public ActionResult CapNhatSoLuong(int MaSP, int SoLuong)
        {
            // Lấy giỏ hàng từ session
            List<GioHang> gioHangList = (List<GioHang>)Session["GioHang"];

            // Tìm sản phẩm trong giỏ hàng
            var item = gioHangList.SingleOrDefault(sp => sp.MaSP == MaSP);
            if (item != null)
            {
                // Cập nhật số lượng
                item.SoLuong = Math.Max(item.SoLuong + SoLuong, 1);  // Đảm bảo số lượng không nhỏ hơn 1
            }

            // Cập nhật giỏ hàng vào session
            Session["GioHang"] = gioHangList;

            // Trả về giỏ hàng đã cập nhật
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult XoaThanhPhanGioHang(int id)
        {
            // Lấy giỏ hàng từ session
            List<GioHang> gioHangList = (List<GioHang>)Session["GioHang"];

            if (gioHangList != null)
            {
                // Tìm và xóa sản phẩm trong giỏ hàng
                var productToRemove = gioHangList.SingleOrDefault(sp => sp.MaSP == id);
                if (productToRemove != null)
                {
                    gioHangList.Remove(productToRemove);
                }
            }

            // Cập nhật lại giỏ hàng trong session
            Session["GioHang"] = gioHangList;

            // Quay lại trang giỏ hàng
            return RedirectToAction("Index");
        }

        public ActionResult ThanhToan()
        {
            int maTK = (Session["MaTK"] as int?) ?? -1;
            if(maTK == -1)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            ConnectKhachHang cKH = new ConnectKhachHang(dbConnect);
            ConnectTaiKhoan cTK = new ConnectTaiKhoan(dbConnect);

            Session["Email"] = cTK.LayEmailTheoMaTK(maTK);
            KhachHang kh = cKH.LayKhachHangTheoMaTK(maTK);
            Session["KhachHang"] = kh;
            List<GioHang> gioHangList = LayGioHang();

            // Kiểm tra xem giỏ hàng có sản phẩm không
            if (gioHangList == null || gioHangList.Count == 0)
            {
                TempData["Error"] = "Không thể thanh toán vì giỏ hàng trống!";
                return RedirectToAction("Index", "Home"); // Nếu giỏ hàng trống, chuyển về trang chủ
            }

            // Tính tổng tiền
            double tongTien = gioHangList.Sum(sp => sp.ThanhTien);

            // Gửi giỏ hàng và tổng tiền sang view để hiển thị
            ViewBag.TongTien = tongTien;
            return View(gioHangList);
        }
        [HttpPost]
        public ActionResult ThanhToan(FormCollection form)
        {
            // Lấy thông tin giỏ hàng từ session
            List<GioHang> gioHangList = LayGioHang();

            // Kiểm tra nếu giỏ hàng trống
            if (gioHangList == null || gioHangList.Count == 0)
            {
                TempData["Error"] = "Không thể thanh toán vì giỏ hàng trống!";
                return RedirectToAction("Index", "Home"); // Nếu giỏ hàng trống, chuyển về trang chủ
            }


            // Lấy thông tin địa chỉ từ form
            string address = form["Address"];
            string ward = form["Ward"];       // Phường xã
            string district = form["District"]; // Quận huyện
            string location = form["Location"]; // Tỉnh thành

            // Nối các phần địa chỉ lại với nhau
            string fullAddress = address + ", " + ward + ", " + district + ", " + location;
            double totalAmount = gioHangList.Sum(sp => sp.ThanhTien);

            int maTK = (Session["MaTK"] as int?) ?? -1;
            if (maTK == -1) return View("Index",LayGioHang());
            ConnectKhachHang cKH = new ConnectKhachHang(dbConnect);
            KhachHang kh = cKH.LayKhachHangTheoMaTK(maTK);

            HoaDon hoaDon = new HoaDon
            {
                NgayDat = DateTime.Now,
                DiaChiGiaoHang = fullAddress,
                MaTTD = 1, // Chờ duyệt
                TongTien = totalAmount,
                MaNV = null, // Mã nhân viên mặc định
                MaKH = kh.MaKH // Mã khách hàng
            };
            ConnectHoaDon cn = new ConnectHoaDon(dbConnect);
            cn.ThemHoaDon(hoaDon);


            ConnectChiTietHoaDon connnectChiTietHoaDon = new ConnectChiTietHoaDon(dbConnect);
            foreach (var item in gioHangList)
            {
                ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon
                {
                    MaHD = hoaDon.MaHD,
                    MaSP = item.MaSP,
                    SoLuong = item.SoLuong,
                    DonGia = item.GiaBan
                };

                // Thêm chi tiết hóa đơn vào database
                connnectChiTietHoaDon.ThemChiTietHoaDon(chiTietHoaDon);
            }
            // Sau khi thanh toán thành công, xóa giỏ hàng trong session
            Session["GioHang"] = null;

            // Trả về view thông báo thành công và chuyển hướng về trang chủ
            TempData["Message"]  = "Tạo đơn hàng thành công!.";
            return RedirectToAction("Index", "Home"); // Chuyển về trang chủ
        }
        //        DiaChiGiaoHang = $"{Location} {District} {Ward}, {Address}",

        public ActionResult LichSuDonHang()
        {
            int maTK = (Session["MaTK"] as int?) ?? -1;
            ConnectKhachHang cKH = new ConnectKhachHang(dbConnect);
            KhachHang kh = cKH.LayKhachHangTheoMaTK(maTK);

            ConnectHoaDon cHD = new ConnectHoaDon(dbConnect);
            List<HoaDon> ds = cHD.LayDanhSachHoaDon(kh.MaKH);
            ViewBag.dbConnect = dbConnect;
            return View(ds);
        }
    }
}