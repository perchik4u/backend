using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace proj
{
    class dataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-47R1RQI\SQLEXPRESS;Initial Catalog=bend_bd;Integrated Security=True;Encrypt=False");

        public void openConnection()
        {
            if(sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if(sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
