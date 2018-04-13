using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat1._0
{
    class User
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
    }
}
