using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Project_QuanAo.Helpers;

namespace Project_QuanAo.Models
{
    public class ConnectSanPham {
        private DBConnection dbConnect;
        public ConnectSanPham(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }


        public List<SanPham> TimKiemSanPhamTheoTuKhoa(string tuKhoa)
        {
            List<SanPham> ds = new List<SanPham>();
            string cauTruyVan = "SELECT * FROM SanPham WHERE TenSP LIKE @tuKhoa";

            //SqlCommand cmd = new SqlCommand(cauTruyVan, new SqlConnection(dbConnect.StrConnect));
            //cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");

            SqlConnection connection = new SqlConnection(dbConnect.StrConnect);
            SqlCommand cmd = new SqlCommand(cauTruyVan, connection);
            cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");

            //dbConnect.OpenConnect();
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                SanPham temp = new SanPham
                {
                    MaSP = (int)reader["MaSP"],
                    TenSP = reader["TenSP"].ToString(),
                    MaNCC = (int)reader["MaNCC"],
                    MaLoaiSP = (int)reader["MaLoaiSP"],
                    GiaBan = double.Parse(reader["GiaBan"].ToString()),
                    MoTa = reader["MoTa"].ToString(),
                    DuongDanAnh = reader["DuongDanAnh"].ToString()
                };
                ds.Add(temp);
            }

            //dbConnect.CloseConnect();
            connection.Close();
            return ds;
        }
        public List<SanPham> TimKiemSanPhamTheoTuKhoaCoPhanTrang(string tuKhoa, int pageSize, int offset)
        {
            List<SanPham> ds = new List<SanPham>();
            string cauTruyVan = @"
        SELECT * FROM SanPham 
        WHERE TenSP LIKE @tuKhoa
        ORDER BY TenSP 
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            
            SqlConnection connection = new SqlConnection(dbConnect.StrConnect);
            SqlCommand cmd = new SqlCommand(cauTruyVan, connection);
            cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");
            cmd.Parameters.AddWithValue("@PageSize", pageSize);  // số lượng sản phẩm mỗi trang
            cmd.Parameters.AddWithValue("@Offset", offset);      // số lượng sản phẩm đã bỏ qua

            
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            
            while (reader.Read())
            {
                SanPham temp = new SanPham
                {
                    MaSP = (int)reader["MaSP"],
                    TenSP = reader["TenSP"].ToString(),
                    MaNCC = (int)reader["MaNCC"],
                    MaLoaiSP = (int)reader["MaLoaiSP"],
                    GiaBan = double.Parse(reader["GiaBan"].ToString()),
                    MoTa = reader["MoTa"].ToString(),
                    DuongDanAnh = reader["DuongDanAnh"].ToString()
                };
                ds.Add(temp);
            }  
            connection.Close();
            return ds;
        }
        public int TimKiemSanPhamTheoTuKhoaCount(string tuKhoa)
        {
            string query = "SELECT COUNT(*) FROM SanPham WHERE TenSP LIKE @TuKhoa";

            using (SqlConnection connection = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

                connection.Open();
                var count = cmd.ExecuteScalar();  // Trả về số lượng sản phẩm tìm được
                return Convert.ToInt32(count);  // Chuyển kết quả về kiểu int
            }
        }

        public List<SanPham> LayDanhSachSanPhamTheoLoai(string loai)
        {
            var danhSachSanPham = new List<SanPham>();



            string query = @"
        SELECT sp.MaSP, sp.TenSP, sp.MaLoaiSP, sp.GiaBan, sp.DuongDanAnh
        FROM SanPham sp
        JOIN LoaiSanPham ls ON sp.MaLoaiSP = ls.MaLoaiSP
        WHERE ls.TenLoaiSP LIKE @Loai";

            using (SqlConnection connection = new SqlConnection(dbConnect.StrConnect))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Loai", "%" + loai + "%");
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachSanPham.Add(new SanPham
                            {
                                MaSP = Convert.ToInt32(reader["MaSP"]),
                                TenSP = reader["TenSP"].ToString(),
                                MaLoaiSP = Convert.ToInt32(reader["MaLoaiSP"]),
                                GiaBan = double.Parse(reader["GiaBan"].ToString()),
                                DuongDanAnh = reader["DuongDanAnh"].ToString()
                            });
                        }
                    }
                }
            }

            return danhSachSanPham;
        }


        public List<SanPham> LayDanhSachSanPhamTheoLoaiExclude(string exclude1, string exclude2)
        {
            var danhSachSanPham = new List<SanPham>();



            string query = @"
        SELECT sp.MaSP, sp.TenSP, sp.MaLoaiSP, sp.GiaBan, sp.DuongDanAnh
        FROM SanPham sp
        JOIN LoaiSanPham ls ON sp.MaLoaiSP = ls.MaLoaiSP
        WHERE ls.TenLoaiSP NOT LIKE @Exclude1 AND ls.TenLoaiSP NOT LIKE @Exclude2";

            using (SqlConnection connection = new SqlConnection(dbConnect.StrConnect))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Exclude1", "%" + exclude1 + "%");
                    command.Parameters.AddWithValue("@Exclude2", "%" + exclude2 + "%");
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachSanPham.Add(new SanPham
                            {
                                MaSP = Convert.ToInt32(reader["MaSP"]),
                                TenSP = reader["TenSP"].ToString(),
                                MaLoaiSP = Convert.ToInt32(reader["MaLoaiSP"]),
                                GiaBan = double.Parse(reader["GiaBan"].ToString()),
                                DuongDanAnh = reader["DuongDanAnh"].ToString()
                            });
                        }
                    }
                }
            }

            return danhSachSanPham;
        }



        public List<SanPham> LayDanhSachSanPham()
        {
            string cauTruyVan = "select * from SanPham";

            DataSet dataSet = new DataSet();

            SqlDataAdapter dataEmp = new SqlDataAdapter(cauTruyVan, dbConnect.StrConnect);
            dataEmp.Fill(dataSet,"SanPham");

            List<SanPham> ds = new List<SanPham>();
            for (int i = 0; i < dataSet.Tables["SanPham"].Rows.Count; i++)
            {
                SanPham temp = new SanPham();

                temp.MaSP = int.Parse(dataSet.Tables["SanPham"].Rows[i]["MaSP"].ToString());
                temp.TenSP = dataSet.Tables["SanPham"].Rows[i]["TenSP"].ToString();
                temp.MaNCC = int.Parse(dataSet.Tables["SanPham"].Rows[i]["MaNCC"].ToString());
                temp.MaLoaiSP = int.Parse(dataSet.Tables["SanPham"].Rows[i]["MaLoaiSP"].ToString());
                temp.GiaBan = double.Parse(dataSet.Tables["SanPham"].Rows[i]["GiaBan"].ToString());
                temp.MoTa = dataSet.Tables["SanPham"].Rows[i]["MoTa"].ToString();
                temp.DuongDanAnh = dataSet.Tables["SanPham"].Rows[i]["DuongDanAnh"].ToString();

                ds.Add(temp);
            }
            return ds;
        }
        public string LayTenLoaiSPTheoMa(int MaLoaiSP)
        {
            return dbConnect.GetString("Select TenLoaiSP From LoaiSanPham Where MaLoaiSP = '" + MaLoaiSP + "'");
        }
        public string LayTenNCCTheoMa(int MaNCC)
        {
            return dbConnect.GetString("Select TenNCC From NhaCungCap Where MaNCC = '" + MaNCC + "'");
        }
        public SanPham LaySanPhamTheoMa(int MaSP)
        {
            string query = "SELECT * FROM SanPham WHERE MaSP = @MaSP";
            using (SqlConnection conn = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSP", MaSP);

                conn.Open(); // Mở kết nối tại đây
                SqlDataReader reader = cmd.ExecuteReader();

                SanPham sanPham = null;
                if (reader.Read())
                {
                    sanPham = new SanPham
                    {
                        MaSP = (int)reader["MaSP"],
                        TenSP = reader["TenSP"].ToString(),
                        MaLoaiSP = int.Parse(reader["MaLoaiSP"].ToString()),
                        MaNCC = int.Parse(reader["MaNCC"].ToString()),
                        GiaBan = double.Parse(reader["GiaBan"].ToString()),
                        DuongDanAnh = reader["DuongDanAnh"].ToString(),
                        MoTa = reader["MoTa"].ToString(),
                    };
                }
                reader.Close();
                return sanPham;
            }
        }
        public void ThemSanPham(SanPham sanPham)
        {
            try
            {
                string cauTruyVan = @"INSERT INTO SanPham VALUES
                                    (N'" + sanPham.TenSP + "'," + sanPham.MaLoaiSP + "," + sanPham.MaNCC + "," + sanPham.GiaBan + ",'" + sanPham.DuongDanAnh + "',N'" + sanPham.MoTa + "')";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi thêm sản phẩm: " + ex.Message);
            }
        }
        public void SuaSanPham(SanPham sanPham)
        {
            try
            {
                    string cauTruyVan = $@"UPDATE SanPham
                             SET TenSP = N'{sanPham.TenSP}', 
                                 MaLoaiSP = N'{sanPham.MaLoaiSP}', 
                                 MaNCC = N'{sanPham.MaNCC}', 
                                 GiaBan = N'{sanPham.GiaBan}', 
                                 DuongDanAnh = N'{sanPham.DuongDanAnh}', 
                                 MoTa = N'{sanPham.MoTa}'
                             WHERE MaSP = N'{sanPham.MaSP}'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi sửa sản phẩm: " + ex.Message);
            }
        }
        public void XoaSanPham(int maSP)
        {
            try
            {
                string cauTruyVan = "DELETE FROM SanPham WHERE MaSP = '"+ maSP + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }
    }
}