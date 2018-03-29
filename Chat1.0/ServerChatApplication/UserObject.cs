using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    // This class creates objects that hold user information
    public class UserObject
    {
        // Holds the user IP
        private IPEndPoint userIP;

        // Holds the unique identifer connected to the user.
        private string userID;

        // Get and set for the user IP 
        public IPEndPoint UserIp
        {
            get { return userIP; }
            set { userIP = value; }
        }

        public string UserId
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}
