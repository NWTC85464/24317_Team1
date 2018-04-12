using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat1._0
{
    //User class for storing logged in user details
    public class User
    {
        public int user_ID { set; get; }
        public string user_name { set; get; }
        public bool user_online { set; get; }
        public string fname { set; get; }
        public string lname { set; get; }
        public string email { set; get; }
        public string password { set; get; }
        public string state { set; get; }
        public string address1 { set; get; }
        public string city { set; get; }
        public int zip_code { set; get; }
    }
}
