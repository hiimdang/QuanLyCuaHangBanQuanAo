using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_QuanAo.Helpers
{
    public class DBConnection
    {
        private SqlConnection sConn;
        private string strConnect, strServerName, strDBName, strUserID, strPassword;

        public SqlConnection SConn { get => sConn; set => sConn = value; }
        public string StrConnect { get => strConnect; set => strConnect = value; }
        public string StrServerName { get => strServerName; set => strServerName = value; }
        public string StrDBName { get => strDBName; set => strDBName = value; }
        public string StrUserID { get => strUserID; set => strUserID = value; }
        public string StrPassword { get => strPassword; set => strPassword = value; }

        public DBConnection(string StrServerName, string StrDBName)
        {
            this.StrServerName = StrServerName;
            this.StrDBName = StrDBName;

            strConnect = @"Data Source=" + StrServerName + ";Initial Catalog=" + strDBName + ";Integrated Security=True";
            sConn = new SqlConnection(strConnect);
        }
        public void OpenConnect()
        {
            if (sConn.State == ConnectionState.Closed)
                sConn.Open();
        }

        public void CloseConnect()
        {
            if (sConn.State == ConnectionState.Open)
                sConn.Close();
        }

        public void DisposeConnect()
        {
            if (sConn.State == ConnectionState.Open)
                sConn.Close();
            sConn.Dispose();
        }

        public bool UpdateToDatabase(string strSQL)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sConn;
            cmd.CommandText = strSQL;
            int result = cmd.ExecuteNonQuery();
            CloseConnect();
            return result > 0;
        }

        public SqlDataReader ExecuteReader(string cauTruyVan)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand(cauTruyVan, sConn);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public int GetInt(string strSQL)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sConn;
            cmd.CommandText = strSQL;
            int output = (int)cmd.ExecuteScalar();
            CloseConnect();
            return output;
        }

        public string GetString(string strSQL)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sConn;
            cmd.CommandText = strSQL;
            string output = cmd.ExecuteScalar().ToString();
            CloseConnect();
            return output;
        }

        public object GetObject(string strSQL)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sConn;
            cmd.CommandText = strSQL;
            object output = (object)cmd.ExecuteScalar();
            CloseConnect();
            return output;
        }

        public bool CheckExist(string tenBang, string tenCot, string giaTriCot)
        {
            string strSQL = "SELECT COUNT(*) FROM " + tenBang + " WHERE " + tenCot + " = '" + giaTriCot + "'";
            return GetInt(strSQL) > 0;
        }

        public bool CheckExist(string tenBang, string tenCot, string giaTriCot, string tenCotMa, string ma)
        {
            string strSQL = "SELECT COUNT(*) FROM " + tenBang +
                            " WHERE " + tenCot + " = N'" + giaTriCot +
                            "' AND " + tenCotMa + " = N'" + ma + "'"; ;
            return GetInt(strSQL) > 0;
        }

        public bool CheckExist(string tableName, List<string> fieldNames, List<string> fieldValues)
        {
            if (fieldNames.Count != fieldValues.Count)
            {
                throw new Exception("fieldNames và fieldValues phải có cùng độ dài");
            }

            string strSQL = "SELECT COUNT(*) FROM " + tableName + " WHERE ";

            for (int i = 0; i < fieldNames.Count; i++)
            {
                strSQL += fieldNames[i] + " = '" + fieldValues[i] + "'";
                if (i < fieldNames.Count - 1)
                {
                    strSQL += " AND ";
                }
            }

            return GetInt(strSQL) > 0;
        }

        public SqlDataReader ExecuteReader(string query, object parameters = null)
        {
            OpenConnect();
            SqlCommand cmd = new SqlCommand(query, sConn);

            if (parameters != null)
            {
                // Add parameters dynamically (nếu cần thiết)
                var props = parameters.GetType().GetProperties();
                foreach (var prop in props)
                {
                    cmd.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(parameters));
                }
            }

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

    }
}