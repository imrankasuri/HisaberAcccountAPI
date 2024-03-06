using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAccounts.APIs.BasicAuth
{
    public class ApiSecurity
    {
        public static bool VaidateUser(string username, string password)
        {
            // Check if it is valid credential  
            if (username.Equals("awais") && password.Equals("zafar"))//CheckUserInDB(username, password))  
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}