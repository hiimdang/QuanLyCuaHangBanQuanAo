using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Project_QuanAo.Helpers;

namespace Project_QuanAo.Models
{
    public class ConnectLoaiSanPham
    {
        private DBConnection dbConnect;
        public ConnectLoaiSanPham(DBConnection dbConnect)
        {
            this.dbConnect = dbConnect;
        }
        public List<LoaiSanPham> LayDanhSachLoaiSanPham()
        {
            string cauTruyVan = "SELECT * FROM LoaiSanPham";

            DataSet dataSet = new DataSet();

            SqlDataAdapter dataEmp = new SqlDataAdapter(cauTruyVan, dbConnect.StrConnect);
            dataEmp.Fill(dataSet, "LoaiSanPham");

            List<LoaiSanPham> ds = new List<LoaiSanPham>();
            for (int i = 0; i < dataSet.Tables["LoaiSanPham"].Rows.Count; i++)
            {
                LoaiSanPham temp = new LoaiSanPham();

                temp.MaLoaiSP = int.Parse(dataSet.Tables["LoaiSanPham"].Rows[i]["MaLoaiSP"].ToString());
                temp.TenLoaiSP = dataSet.Tables["LoaiSanPham"].Rows[i]["TenLoaiSP"].ToString();

                ds.Add(temp);
            }
            return ds;
        }

        public string LayTenLoaiSPTheoMa(int MaLoaiSP)
        {
            string query = "SELECT * FROM LoaiSanPham WHERE MaLoaiSP = @MaLoaiSP";
            using (SqlConnection conn = new SqlConnection(dbConnect.StrConnect))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLoaiSP", MaLoaiSP);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                LoaiSanPham loaiSP = null;
                if (reader.Read())
                {
                    loaiSP = new LoaiSanPham
                    {
                        MaLoaiSP = (int)reader["MaLoaiSP"],
                        TenLoaiSP = reader["TenLoaiSP"].ToString(),
                    };
                }
                reader.Close();
                return loaiSP.TenLoaiSP;
            }
        }
    }
}