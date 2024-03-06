using HAccounts.APIs.Utils;
using HAccounts.BE;
using HAccounts.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;

namespace HAccounts.APIs.Controllers
{
    public class UsersController : ApiController
    {

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetAllUsers([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                if(AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID),inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    UserBE currentUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                   
                    int pageNo = 0;
                    int totalRecords = 0;
                    if(inParams.PageNo > 0)
                    {
                        pageNo = Convert.ToInt32(inParams.PageNo);
                    }
                    int pageSize = 5;
                    List<UserBE> ListofUsers = UserDAL.GetAllUserBEs();

                    totalRecords = ListofUsers.Count();
                    ListofUsers = ListofUsers.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<UserBE>();
                    

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofUsers = ListofUsers,
                        totalRecords = totalRecords
                    });


                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }


                
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetUserByID([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.ID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    UserBE currentUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    int userID = Convert.ToInt32(inParams.ID);
                    UserBE user = UserDAL.GetUserBEByID(userID);
                    
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning Voucher",
                        User = user,

                    });


                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }



            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SignUp([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    //check debit and credit accounts exists
                   


                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));


                   
                    UserBE user = new UserBE();
                    user.FullName = inParams.FullName;
                    user.User_Mobile = inParams.User_Mobile;
                    user.User_Email = inParams.User_Name;
                    user.Password = GeneralFunctions.Encrypt(inParams.Password);
                    user.LastPasswordChange = DateTime.Now;
                    user.User_Type = inParams.User_Type;
                    user.Created_Date = DateTime.Now;
                    user.Updated_Date = DateTime.Now;
                    user.Is_Active = true;
                    user.Is_Deleted = false;
                    if(inParams.Android_Token != null)
                    {
                        user.Android_Token = inParams.Android_Token;
                    }
                   

                    //check for already exist user
                    UserBE userEmailAlready = UserDAL.GetUserByEmailAddress(inParams.User_Name);
                    if(userEmailAlready != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Email Address Already Registered!",
                        });
                    }
                    UserBE userMobileAlready = UserDAL.GetUserBEByMobileNo(inParams.User_Mobile);
                    if (userMobileAlready != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Mobile Number Already Registered!",
                        });
                    }

                    if (userEmailAlready != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Email Address Already Registered!",
                        });
                    }


                    int userID  = UserDAL.Save(user);
                    if(userID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "User Added Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to add user!",
                        });
                    }
                    


                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }



            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SignIn([FromBody] UserBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.User_Email) && !String.IsNullOrEmpty(inParams.Password))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    UserBE m = UserDAL.GetUserBEByEmailPassword(inParams.User_Email, GeneralFunctions.Encrypt(inParams.Password));
                    if (m != null)
                    {
                        
                        InsertLoginLogEntry(true, inParams.User_Email, GeneralFunctions.Encrypt(inParams.Password), m.User_Type);
                        System.Guid guid = System.Guid.NewGuid();
                        String apiAccessKey = guid.ToString();

                        AccessKeyBE acAlready = AccessKeyDAL.GetAccessKeyByUserID(m.ID);
                        if (acAlready != null)
                        {
                            if (acAlready.ExpiryDate > DateTime.Now)
                            {
                                //accesskey is valid
                                apiAccessKey = acAlready.AccessKey;
                            }
                            else
                            {
                                //delete previous access key
                                AccessKeyDAL.DeleteAccessKey(m.ID);

                                AccessKeyBE akBE = new AccessKeyBE();
                                akBE.UserID = m.ID;
                                akBE.ExpiryDate = DateTime.Now.AddDays(30);
                                akBE.Is_Active = true;
                                akBE.Is_Deleted = false;
                                akBE.Created_Date = DateTime.Now;
                                akBE.Updated_Date = DateTime.Now;
                                akBE.AccessKey = apiAccessKey;
                                AccessKeyDAL.Save(akBE);
                            }
                        }
                        else
                        {
                            AccessKeyBE akBE = new AccessKeyBE();
                            akBE.UserID = m.ID;
                            akBE.ExpiryDate = DateTime.Now.AddDays(30);
                            akBE.Is_Active = true;
                            akBE.Is_Deleted = false;
                            akBE.Created_Date = DateTime.Now;
                            akBE.Updated_Date = DateTime.Now;
                            akBE.AccessKey = apiAccessKey;
                            AccessKeyDAL.Save(akBE);
                        }


                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Successfully logged in as " + m.User_Email,
                            AccessKey = apiAccessKey,
                            ID = m.ID.ToString(),
                            Email_Address = m.User_Email,
                            Full_Name = m.FullName,
                            Mobile_No = m.User_Mobile,
                            Is_Active = m.Is_Active,
                            Member_Type = m.User_Type,
                            Updated_Date = m.Updated_Date,
                        });

                    }
                    else
                    {
                        //InsertLoginLogEntry(false, inParams.userName, inParams.password);
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "You have entered Invalid Email Address OR Password."
                        });
                    }
                }
                catch (Exception ex)
                {
                    //InsertLoginLogEntry(false, inParams.userName, inParams.password);
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to complete transaction please try again later." + ex.Message
                    });
                }
            }

            InsertLoginLogEntry(false, "null", "Null", "");
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                status_code = 0,
                status_message = "Invalid Request Parameters"
            });

        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteUser([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    UserBE currentUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    UserBE user = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.ID));
                    if (user != null)
                    {

                        if (user.User_Type == "Admin")
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Adminr Can not be Deleted!",
                            });
                        }
                        user.Is_Active = false;
                        user.Is_Deleted = true;
                        user.Updated_Date = DateTime.Now;
                        UserDAL.Save(user);

                        AccessKeyDAL.DeleteAccessKey(user.ID);

                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "User Deleted Successfullly!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "User Can not be deleted!",
                        });
                    }



                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }



            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage DisableUser([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    UserBE currentUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    UserBE user = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.ID));
                    if (user != null)
                    {
                        
                            if(user.User_Type == "Admin")
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new
                                {
                                    status_code = 0,
                                    status_message = "Adminr Can not be Disabled!",
                                });
                            }
                            user.Is_Deleted = true;
                            user.Updated_Date = DateTime.Now;
                            UserDAL.Save(user);

                            //delete user access keys
                            AccessKeyDAL.DeleteAccessKey(user.ID);
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 1,
                                status_message = "User Disabled Successfullly!",
                            });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "User Can not be Disabled!",
                            });
                        }
                    


                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }



            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateUser([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey) && !String.IsNullOrEmpty(Convert.ToString(inParams.ID)))
            {
                if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    //check debit and credit accounts exists



                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));

                    UserBE user = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.ID));
                    if(user != null)
                    {
                       
                        user.FullName = inParams.FullName;
                        user.User_Mobile = inParams.User_Mobile;
                        user.Password = GeneralFunctions.Encrypt(inParams.Password);
                        user.LastPasswordChange = DateTime.Now;
                        user.User_Type = inParams.User_Type;
                        user.Updated_Date = DateTime.Now;
                         int userID = UserDAL.Save(user);
                        if (userID > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 1,
                                status_message = "User updated Successfully!",
                            });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Failed to update user!",
                            });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Update user!",
                        });
                    }
                    



                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }



            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetLoginLogs([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    UserBE currentUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));

                    int pageNo = 0;
                    int totalRecords = 0;
                    if (inParams.PageNo > 0)
                    {
                        pageNo = Convert.ToInt32(inParams.PageNo);
                    }
                    int pageSize = 5;
                    List<LoginLogBE> ListofLogs = LoginLogDAL.GetTopLoginLogBEs(currentUser.User_Type,currentUser.User_Email);

                    totalRecords = ListofLogs.Count();
                    ListofLogs = ListofLogs.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<LoginLogBE>();


                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofLogs = ListofLogs,
                        totalRecords = totalRecords
                    });


                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }



            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }


        private void InsertLoginLogEntry(bool isSuccessfulLogin, string mobile, string password, string userType)
        {
            try
            {
                LoginLogBE llBE = new LoginLogBE();
                llBE.Created_Date = DateTime.Now;
                llBE.IP_Address = GetIPAddress();
                llBE.Is_Active = true;
                llBE.Is_Deleted = false;
                if (isSuccessfulLogin)
                {
                    llBE.Is_Success = true;
                }
                else
                {
                    llBE.Is_Success = false;
                }
                llBE.Password = password; // GeneralFunctions.Encrypt(password);
                llBE.Updated_Date = DateTime.Now;
                llBE.User_Name = mobile;
                llBE.Login_Source = "API";
                llBE.User_Type = userType;
                long idreturned = LoginLogDAL.Save(llBE);
            }
            catch (Exception ex)
            {

                //throw;
            }

        }

        private string GetIPAddress()
        {

            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }
    }
}
