using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_QuanAo.Helpers;
using Project_QuanAo.Models;

namespace QuanLyCuaHangBanQuanAo.Controllers
{
    public class AdminController : Controller
    {
        private DBConnection dbConnect;
        List<LoaiSanPham> dsLoaiSanPham;
        List<NhaCungCap> dsNhaCungCap;
        List<SanPham> dsSP;
        List<KhachHang> dsKhachHang;
        List<NhanVien> dsNhanVien;
        List<HoaDon> dsHoaDon;
        ConnectSanPham cSP;
        ConnectNhaCungCap cNCC;
        ConnectKhachHang cKH;
        ConnectNhanVien cNV;
        ConnectHoaDon cHD;
        
        public AdminController()
        {
            dbConnect = DatabaseManager.Instance.GetDBConnection();

            if (dsLoaiSanPham == null || dsNhaCungCap == null)
            {
                ConnectLoaiSanPham cLSP = new ConnectLoaiSanPham(dbConnect);
                cNCC = new ConnectNhaCungCap(dbConnect);
                cKH = new ConnectKhachHang(dbConnect);
                cNV = new ConnectNhanVien(dbConnect);
                cHD = new ConnectHoaDon(dbConnect);

                dsLoaiSanPham = cLSP.LayDanhSachLoaiSanPham();
                dsNhaCungCap = cNCC.LayDanhSachNhaCungCap();
                cSP = new ConnectSanPham(dbConnect);
                dsSP = cSP.LayDanhSachSanPham();
                dsKhachHang = cKH.LayDanhSachKhachHang();
                dsNhanVien = cNV.LayDanhSachNhanVien();
                dsHoaDon = cHD.LayDanhSachHoaDon();
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLySanPham()
        {
            //--
            ViewBag.DBConnection = dbConnect;
            return View(dsSP);
        }
        public ActionResult FormThemSanPham()
        {
            ViewBag.LoaiSanPham = dsLoaiSanPham;
            ViewBag.NhaCungCap = dsNhaCungCap;
            return View();
        }
        [HttpPost]
        public ActionResult ThemSanPham(SanPham dt, HttpPostedFileBase fileAnh)
        {
            try
            {
                if (fileAnh != null && fileAnh.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(fileAnh.FileName));
                    fileAnh.SaveAs(path);
                    dt.DuongDanAnh =  Path.GetFileName(fileAnh.FileName);

                    //Phần Minh: tự tạo Model rồi gọi Model tương tác với db để thêm

                    cSP.ThemSanPham(dt);

       
                    ViewBag.Message = "Thêm sản phẩm thành công!";
                }
                else
                {
                    ViewBag.Message = "Vui lòng chọn một file ảnh hợp lệ.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi thêm sản phẩm: " + ex.Message;
            }

            ViewBag.LoaiSanPham = dsLoaiSanPham;
            ViewBag.NhaCungCap = dsNhaCungCap;
            return View("FormThemSanPham");
        }
        public ActionResult FormSuaSanPham(int id)
        {
            SanPham sp = dsSP.FirstOrDefault(x => x.MaSP == id);
            ViewBag.LoaiSanPham = dsLoaiSanPham;
            ViewBag.NhaCungCap = dsNhaCungCap;
            ViewBag.SanPham = sp;
            return View();
        }
        [HttpPost]
        public ActionResult SuaSanPham(SanPham sp, HttpPostedFileBase fileAnh)
        {
            try
            {
                // Lấy sản phẩm từ cơ sở dữ liệu dựa trên MaSP
                SanPham sanPhamHienTai = cSP.LaySanPhamTheoMa(sp.MaSP);
              
                // Cập nhật thông tin sản phẩm
                sanPhamHienTai.TenSP = sp.TenSP;
                sanPhamHienTai.MaLoaiSP = sp.MaLoaiSP;
                sanPhamHienTai.MaNCC = sp.MaNCC;
                sanPhamHienTai.GiaBan = sp.GiaBan;
                sanPhamHienTai.MoTa = sp.MoTa;

                // Cập nhật ảnh nếu có file mới
                if (fileAnh != null && fileAnh.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(fileAnh.FileName));
                    fileAnh.SaveAs(path);
                    sanPhamHienTai.DuongDanAnh = Path.GetFileName(fileAnh.FileName);
                }

                // Lưu sản phẩm
                cSP.SuaSanPham(sanPhamHienTai);
                ViewBag.Message = "Sửa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra: " + ex.Message;
            }

            // Reload dữ liệu và trả về view
            ViewBag.SanPham = sp;
            ViewBag.LoaiSanPham = dsLoaiSanPham;
            ViewBag.NhaCungCap = dsNhaCungCap;
            return View("FormSuaSanPham");
        }
        public ActionResult XoaSanPham(int id)
        {
            try
            {
                // Kiểm tra xem sản phẩm có tồn tại không
                var sp = cSP.LaySanPhamTheoMa(id);
                if (sp == null)
                {
                    ViewBag.Message = "Sản phẩm không tồn tại.";
                    return RedirectToAction("QuanLySanPham");
                }

                // Thực hiện xóa sản phẩm
                cSP.XoaSanPham(id);

                // Thông báo thành công
                ViewBag.Message = "Xóa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                ViewBag.Message = "Có lỗi xảy ra khi xóa sản phẩm: " + ex.Message;
            }

            // Quay lại trang quản lý sản phẩm
            return RedirectToAction("QuanLySanPham");
        }
        public ActionResult TimKiemSanPham(string tuKhoa)
        {
            try
            {
                ViewBag.DBConnection = dbConnect;
                // Kiểm tra từ khóa
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    ViewBag.Message = "Vui lòng nhập từ khóa để tìm kiếm.";
                    return View("QuanLySanPham", dsSP);
                }

                // Gọi model để tìm kiếm
                List<SanPham> ketQua = cSP.TimKiemSanPhamTheoTuKhoa(tuKhoa);

                if (ketQua.Count == 0)
                {
                    ViewBag.Message = "Không tìm thấy sản phẩm nào phù hợp.";
                }

                return View("QuanLySanPham", ketQua);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi tìm kiếm sản phẩm: " + ex.Message;
                return View("QuanLySanPham", dsSP);
            }
        }
        public ActionResult QuanLyNhaCungCap()
        {
            //--
            ViewBag.DBConnection = dbConnect;
            return View(dsNhaCungCap);
        }
        public ActionResult FormThemNhaCungCap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNhaCungCap(NhaCungCap dt)
        {
            try
            {
                    cNCC.ThemNhaCungCap(dt);

                    ViewBag.Message = "Thêm nhà cung cấp thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi thêm nhà cung cấp: " + ex.Message;
            }

            return View("FormThemNhaCungCap");
        }
        public ActionResult FormSuaNhaCungCap(int id)
        {
            NhaCungCap ncc = dsNhaCungCap.FirstOrDefault(x => x.MaNCC == id);
            ViewBag.NhaCungCap = ncc;
            return View();
        }
        [HttpPost]
        public ActionResult SuaNhaCungCap(NhaCungCap ncc)
        {
            try
            {
                cNCC.SuaNhaCungCap(ncc);
                ViewBag.Message = "Sửa nhà cung cấp thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra: " + ex.Message;
            }

            // Reload dữ liệu và trả về view
            ViewBag.NhaCungCap = ncc;
            return View("FormSuaNhaCungCap");
        }
        public ActionResult XoaNhaCungCap(int id)
        {
            try
            {
                cNCC.XoaNhaCungCap(id);

                ViewBag.Message = "Xóa nhà cung cấp thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi xóa nhà cung cấp: " + ex.Message;
            }

            return RedirectToAction("QuanLyNhaCungCap");
        }
        public ActionResult TimKiemNhaCungCap(string tuKhoa)
        {
            try
            {
                ViewBag.DBConnection = dbConnect;
                // Kiểm tra từ khóa
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    ViewBag.Message = "Vui lòng nhập từ khóa để tìm kiếm.";
                    return View("QuanLyNhaCungCap", dsNhaCungCap);
                }

                // Gọi model để tìm kiếm
                List<NhaCungCap> ketQua = cNCC.TimKiemNhaCungCapTheoTuKhoa(tuKhoa);

                if (ketQua.Count == 0)
                {
                    ViewBag.Message = "Không tìm thấy nhà cung cấp nào phù hợp.";
                }

                return View("QuanLyNhaCungCap", ketQua);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi tìm kiếm nhà cung cấp: " + ex.Message;
                return View("QuanLyNhaCungCap", dsNhaCungCap);
            }
        }
        public ActionResult QuanLyKhachHang()
        {
            //--
            ViewBag.DBConnection = dbConnect;
            return View(dsKhachHang);
        }
        public ActionResult FormThemKhachHang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemKhachHang(KhachHang dt)
        {
            try
            {
                cKH.ThemKhachHang(dt);

                ViewBag.Message = "Thêm nhà cung cấp thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi thêm nhà cung cấp: " + ex.Message;
            }

            return View("FormThemKhachHang");
        }
        public ActionResult FormSuaKhachHang(int id)
        {
            KhachHang ncc = dsKhachHang.FirstOrDefault(x => x.MaKH == id);
            ViewBag.KhachHang = ncc;
            return View();
        }
        [HttpPost]
        public ActionResult SuaKhachHang(KhachHang kh)
        {
            try
            {
                cKH.SuaKhachHang(kh);
                ViewBag.Message = "Sửa khách hàng thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra: " + ex.Message;
            }

            // Reload dữ liệu và trả về view
            ViewBag.KhachHang = kh;
            return View("FormSuaKhachHang");
        }
        public ActionResult XoaKhachHang(int id)
        {
            try
            {
                cKH.XoaKhachHang(id);

                ViewBag.Message = "Xóa khách hàng thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi xóa khách hàng: " + ex.Message;
            }

            return RedirectToAction("QuanLyKhachHang");
        }
        public ActionResult TimKiemKhachHang(string tuKhoa)
        {
            try
            {
                ViewBag.DBConnection = dbConnect;
                // Kiểm tra từ khóa
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    ViewBag.Message = "Vui lòng nhập từ khóa để tìm kiếm.";
                    return View("QuanLyKhachHang", dsKhachHang);
                }

                // Gọi model để tìm kiếm
                List<KhachHang> ketQua = cKH.TimKiemKhachHangTheoTuKhoa(tuKhoa);

                if (ketQua.Count == 0)
                {
                    ViewBag.Message = "Không tìm thấy khách hàng nào phù hợp.";
                }

                return View("QuanLyKhachHang", ketQua);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi tìm kiếm khách hàng: " + ex.Message;
                return View("QuanLyKhachHang", dsKhachHang);
            }
        }
        public ActionResult QuanLyNhanVien()
        {
            //--
            ViewBag.DBConnection = dbConnect;
            return View(dsNhanVien);
        }
        public ActionResult FormThemNhanVien()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNhanVien(NhanVien dt)
        {
            try
            {
                cNV.ThemNhanVien(dt);

                ViewBag.Message = "Thêm nhân viên thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi thêm nhân viên: " + ex.Message;
            }

            return View("FormThemNhanVien");
        }
        public ActionResult FormSuaNhanVien(int id)
        {
            NhanVien nv = dsNhanVien.FirstOrDefault(x => x.MaNV == id);
            ViewBag.NhanVien = nv;
            return View();
        }
        [HttpPost]
        public ActionResult SuaNhanVien(NhanVien nv)
        {
            try
            {
                cNV.SuaNhanVien(nv);
                ViewBag.Message = "Sửa nhân viên thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra: " + ex.Message;
            }

            // Reload dữ liệu và trả về view
            ViewBag.NhanVien = nv;
            return View("FormSuaNhanVien");
        }
        public ActionResult XoaNhanVien(int id)
        {
            try
            {
                cNV.XoaNhanVien(id);

                ViewBag.Message = "Xóa nhân viên thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi xóa nhân viên: " + ex.Message;
            }

            return RedirectToAction("QuanLyNhanVien");
        }
        public ActionResult TimKiemNhanVien(string tuKhoa)
        {
            try
            {
                ViewBag.DBConnection = dbConnect;
                // Kiểm tra từ khóa
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    ViewBag.Message = "Vui lòng nhập từ khóa để tìm kiếm.";
                    return View("QuanLyNhanVien", dsNhanVien);
                }

                // Gọi model để tìm kiếm
                List<NhanVien> ketQua = cNV.TimKiemNhanVienTheoTuKhoa(tuKhoa);

                if (ketQua.Count == 0)
                {
                    ViewBag.Message = "Không tìm thấy nhân viên nào phù hợp.";
                }

                return View("QuanLyNhanVien", ketQua);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi tìm kiếm nhân viên: " + ex.Message;
                return View("QuanLyNhanVien", dsNhanVien);
            }
        }
        public ActionResult QuanLyDonHang(string trangThaiDon = null)
        {
            ViewBag.DBConnection = dbConnect;
            //1 tất 2 chờ
            if(trangThaiDon == null ||trangThaiDon == "0")
            {
                return View(cHD.LayDanhSachHoaDon());
            }

            return View(cHD.LayDanhSachHoaDon(trangThaiDon));    
        }
        public ActionResult DuyetDonHang(int id)
        {
            int maTK = (Session["MaTK"] as int?) ?? -1;
            if (maTK == -1) return View("QuanLyDonHang", dsHoaDon);
            NhanVien nv = cNV.LayNhanVienTheoMaTK(maTK);
            cHD.DuyetHoaDon(id, 1, nv.MaNV);
            ViewBag.DBConnection = dbConnect;
            dsHoaDon = cHD.LayDanhSachHoaDon();
            return View("QuanLyDonHang", dsHoaDon);
        }
        public ActionResult HuyDonHang(int id)
        {
            int maTK = (Session["MaTK"] as int?) ?? -1;
            if (maTK == -1) return View("QuanLyDonHang", dsHoaDon);
            NhanVien nv = cNV.LayNhanVienTheoMaTK(maTK);
            cHD.DuyetHoaDon(id, 2, nv.MaNV);
            ViewBag.DBConnection = dbConnect;
            dsHoaDon = cHD.LayDanhSachHoaDon();
            return View("QuanLyDonHang", dsHoaDon);
        }
    }
}