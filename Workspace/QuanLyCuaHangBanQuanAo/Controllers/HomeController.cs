using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_QuanAo.Helpers;
using Project_QuanAo.Models;

namespace QuanLyCuaHangBanQuanAo.Controllers
{
    public class HomeController : Controller
    {
        private DBConnection dbConnect = DatabaseManager.Instance.GetDBConnection();
        public ActionResult Index()
        {
            ConnectSanPham cSP = new ConnectSanPham(dbConnect);
            List<SanPham> dsSP = cSP.LayDanhSachSanPham();
            return View(dsSP);
        }
    }
}