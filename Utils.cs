using System;
using System.Collections.Generic;
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


}
