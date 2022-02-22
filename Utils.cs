using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKS_Laundry_National
{
    class Utils
    {
        public static string conn = @"Data Source=asmodeus;Initial Catalog=LKS_Laundry;Integrated Security=True";
    }

    class Model
    {
        public static int id { set; get; }
        public static string name { set; get; }
        public static int jobId { set; get; }
    }

    class Command
    {
        public static DataTable getData(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(com, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public static void exec(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlCommand command = new SqlCommand(com, connection);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
