using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace database
{
    public class database
    {
        public string con;

        public void openConnection(string connection)
        {
            con = $"Server = MSI;User ID= {_login};Password={_password};Initial Catalog = yakovlev; Integrated Security = false";

            using (SqlConnection connect = new SqlConnection(con))
            {
                connect.Open();
            }
        }

        public void closeConnection(string _login, string _password)
        {

            con = $"Server = MSI;User ID= {_login};Password={_password};Initial Catalog = yakovlev;Integrated Security = false";
            using (SqlConnection connect = new SqlConnection(con))
            {
                connect.Close();
            }

        }

        public SqlConnection getConnection(string _login, string _password)
        {

            con = $"Server = MSI;User ID= {_login};Password={_password};Initial Catalog = yakovlev; Integrated Security = false";
            using (SqlConnection connect = new SqlConnection(con))
            {
                return connect;
            }

        }

        //public void openConnection()
        //{

        //    using (SqlConnection connect = new SqlConnection(con))
        //    {
        //        if (connect.State == System.Data.ConnectionState.Closed)
        //        {
        //            connect.Open();
        //        }
        //    }
        //}


        //public void closeConnection()
        //{
        //    using (SqlConnection connect = new SqlConnection(con))
        //    {
        //        if (connect.State == System.Data.ConnectionState.Open)
        //        {
        //            connect.Close();
        //        }
        //    }
        //}

        //public SqlConnection getConnection()
        //{
        //    using (SqlConnection connect = new SqlConnection(con))
        //    {
        //        return connect;
        //    }
        //}










    }
}
