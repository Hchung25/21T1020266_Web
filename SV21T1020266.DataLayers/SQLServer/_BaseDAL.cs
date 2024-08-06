using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020266.DataLayers.SQLServer
{
    /// <summary>
    /// Lớp đóng vai trò là lớp cha cho các lớp cài đặt
    /// các phép xử lý dữ liệu
    /// </summary>
    public abstract class _BaseDAL
    {
        protected String _conectionString = "";
        public _BaseDAL(String conectionString)
        {
            _conectionString = conectionString;
        }
        /// <summary>
        /// Tạo và mở kết nối đến CSDL SQL Server
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(_conectionString);
            connection.Open();
            return connection;
        }
    }
}
