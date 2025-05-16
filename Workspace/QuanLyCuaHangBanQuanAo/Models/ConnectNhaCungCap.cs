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
    public class ConnectNhaCungCap
    {
        private DBConnection dbConnect;
        public ConnectNhaCungCap(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }
        public List<NhaCungCap> LayDanhSachNhaCungCap()
        {
            string cauTruyVan = "SELECT * FROM NhaCungCap";

            DataSet dataSet = new DataSet();

            SqlDataAdapter dataEmp = new SqlDataAdapter(cauTruyVan, dbConnect.StrConnect);
            dataEmp.Fill(dataSet, "NhaCungCap");

            List<NhaCungCap> ds = new List<NhaCungCap>();
            for (int i = 0; i < dataSet.Tables["NhaCungCap"].Rows.Count; i++)
            {
                NhaCungCap temp = new NhaCungCap();

                temp.MaNCC = int.Parse(dataSet.Tables["NhaCungCap"].Rows[i]["MaNCC"].ToString());
                temp.TenNCC = dataSet.Tables["NhaCungCap"].Rows[i]["TenNCC"].ToString();
                temp.Sdt = dataSet.Tables["NhaCungCap"].Rows[i]["Sdt"].ToString();
                temp.DiaChi = dataSet.Tables["NhaCungCap"].Rows[i]["DiaChi"].ToString();

                ds.Add(temp);
            }
            return ds;
        }
        public List<NhaCungCap> TimKiemNhaCungCapTheoTuKhoa(string tuKhoa)
        {
            List<NhaCungCap> ds = new List<NhaCungCap>();
            string cauTruyVan = "SELECT * FROM NhaCungCap WHERE TenNCC LIKE @tuKhoa";

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
                NhaCungCap temp = new NhaCungCap
                {
                    MaNCC = (int)reader["MaNCC"],
                    TenNCC = reader["TenNCC"].ToString(),
                    DiaChi = reader["DiaChi"].ToString(),
                    Sdt = reader["SDT"].ToString(),
                };
                ds.Add(temp);
            }

            //dbConnect.CloseConnect();
            connection.Close();
            return ds;
        }
        public NhaCungCap LayNhaCungCapTheoMa(int MaNCC)
        {
            string query = "SELECT * FROM NhaCungCap WHERE MaNCC = @MaNCC";
            using (SqlConnection conn = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNCC", MaNCC);

                conn.Open(); // Mở kết nối tại đây
                SqlDataReader reader = cmd.ExecuteReader();

                NhaCungCap nhaCungCap = null;
                if (reader.Read())
                {
                    nhaCungCap = new NhaCungCap
                    {
                        MaNCC = (int)reader["MaNCC"],
                        TenNCC = reader["TenNCC"].ToString(),
                        DiaChi = reader["DiaChi"].ToString(),
                        Sdt = reader["SDT"].ToString(),
                    };
                }
                reader.Close();
                return nhaCungCap;
            }
        }
        public void ThemNhaCungCap( NhaCungCap nhaCungCap)
        {
            try
            {
                    string cauTruyVan = "INSERT INTO NhaCungCap VALUES ('" + nhaCungCap.TenNCC + "','" + nhaCungCap.DiaChi + "','" + nhaCungCap.Sdt + "')";
                    dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi thêm nhà cung cấp: " + ex.Message);
            }
        }
        public void SuaNhaCungCap(NhaCungCap nhaCungCap)
        {
            try
            {
                string cauTruyVan = "UPDATE NhaCungCap " +
                     "SET TenNCC = N'" + nhaCungCap.TenNCC + "', " +
                     "DiaChi = N'" + nhaCungCap.DiaChi + "', " +
                     "SDT = N'" + nhaCungCap.Sdt + "'" +
                     " WHERE MaNCC = '" + nhaCungCap.MaNCC + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi sửa nhà cung cấp: " + ex.Message);
            }
        }
        public void XoaNhaCungCap(int maNCC)
        {
            try
            {
                string cauTruyVan = "DELETE FROM NhaCungCap WHERE MaNCC = '" + maNCC + "'";
                dbConnect.UpdateToDatabase(cauTruyVan);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa nhà cung cấp: " + ex.Message);
            }
        }
    }
}