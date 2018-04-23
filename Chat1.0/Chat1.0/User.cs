using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat1._0
{
    public class User
    {
        //field that holds username
        private string username;

        //get username
        public string Username
        {
            get { return username; }
        }


        //constructor that creates user after successful login
        public User(string username)
        {
            this.username = username;
        }

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
