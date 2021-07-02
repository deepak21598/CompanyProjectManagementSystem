using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CompanyProjectManagementSystem.DBConnection
{
    public class DataBase
    {
        public SqlConnection conn = null;
        public SqlCommand cmd = null;
        public DataTable dt = null;

        public SqlConnection OpenConnection()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ToString());
            conn.Open();
            return conn;
        }
        public void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public DataTable GetData(string query)
        {
            OpenConnection();
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public int ExecuteNonQuery(string query)
        {
            OpenConnection();
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                int i = cmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}