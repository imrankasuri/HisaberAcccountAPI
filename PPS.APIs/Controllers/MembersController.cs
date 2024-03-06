using HAccounts.APIs.Models;
using HAccounts.APIs.Utils;
using HAccounts.BE;
using HAccounts.DAL;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using ZXing;

namespace HAccounts.APIs.Controllers
{
    public class MembersController : ApiController
    {

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SignUp([FromBody] CompanyInfoBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.Name) && !String.IsNullOrEmpty(inParams.Email) && !String.IsNullOrEmpty(inParams.Mobile) && !String.IsNullOrEmpty(inParams.Password) )
            {
                try
                {
                    //check email address already exists
                    if (GeneralFunctions.IsValidEmail(inParams.Email.ToLower()) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Email Address Entered"
                        });

                    }

                    if (inParams.Password == "")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Please Provide Password. Operation Cancelled"
                        });

                    }

                    if (inParams.Password.Length < 8)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Small passwords are not accepted! Minimum password length is 8 characters. We Recommend a Strong Password for your security!"
                        });

                    }
                    if (inParams.Mobile.Length < 11)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Enter a valid Mobile Number to continue!"
                        });
                    }
                    if (inParams.Mobile.Trim().Substring(0, 2) != "92")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Mobile Number must start with 92!"
                        });
                    }
                    if (inParams.Mobile.Trim().Length != 12)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Mobile Number is invalid"
                        });
                    }
                    if (inParams.Mobile.Trim() != "")
                    {
                        Boolean result = GeneralFunctions.IsDecimalValue(inParams.Mobile.Trim());
                        if (result == false)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Mobile Number is invalid"
                            });
                        }
                    }

                    CompanyInfoBE cMobileNo = CompanyInfoDAL.GetCompanyInfoByMobileNo(inParams.Mobile);
                    if (cMobileNo != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Mobile No Already Exists."
                        });
                    }

                    CompanyInfoBE cemailAddress = CompanyInfoDAL.GetCompanyInfoByEmail(inParams.Email);
                    if (cemailAddress != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Email Address is Already Exists."
                        });
                    }
                    UserBE u = UserDAL.GetUserByEmailAddress(inParams.Email);
                    if(u != null )
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Email Address is Already Exists."
                        });
                    }

                    CompanyInfoBE companyInfo = new CompanyInfoBE();
                    companyInfo.Created_Date = DateTime.Now;
                    companyInfo.Email = inParams.Email;
                    companyInfo.Is_Active = true;
                    companyInfo.Is_Deleted = false;
                    companyInfo.Mobile = inParams.Mobile;
                    companyInfo.Name = inParams.Name;
                    companyInfo.Updated_Date = DateTime.Now;
                    companyInfo.Show_Pumps = true;
                    companyInfo.Logo_Login = "PSO_LOGIN.png";
                    companyInfo.Logo_Reports = "PSO_REPORT.png";
                    companyInfo.Logo_Title = "PSO_TITLE.PNG";
                    companyInfo.PackageName = "Trial";
                    companyInfo.PackageExpiry = DateTime.Now.AddMonths(2);

                    int pumpID = CompanyInfoDAL.Save(companyInfo);
                    if (pumpID > 0)
                    {

                        companyInfo = CompanyInfoDAL.GetCompanyInfoByID(pumpID);
                        //generate pumpcode
                        CompanyInfoBE previousCompany = CompanyInfoDAL.GetPreviousCompanyInfo(pumpID);
                        if (previousCompany == null)
                        {
                            companyInfo.PumpCode = "00001";
                        }
                        else
                        {
                            int previousCode = Convert.ToInt32(previousCompany.PumpCode);
                            companyInfo.PumpCode = (previousCode + 1).ToString("00000");
                        }
                        CompanyInfoDAL.Save(companyInfo);

                        //create one user in the users table. 
                        UserBE loggedinUser = new UserBE();
                        loggedinUser.Created_Date = DateTime.Now;
                        loggedinUser.Is_Active = true;
                        loggedinUser.Is_Deleted = false;
                        loggedinUser.Password = GeneralFunctions.Encrypt(inParams.Password);
                        loggedinUser.Updated_Date = DateTime.Now;
                        loggedinUser.User_Email = inParams.Email;
                        loggedinUser.User_Type = "Admin";
                        loggedinUser.PumpID = pumpID;
                        loggedinUser.User_Mobile = companyInfo.Mobile;
                        loggedinUser.PumpCode = companyInfo.PumpCode;
                        loggedinUser.FullName = "Manager";
                        loggedinUser.LastPasswordChange = DateTime.Now;
                        int loggedId = UserDAL.Save(loggedinUser);
                        if (loggedId > 0)
                        {
                            //generate the default accounts for this new company. 
                            List<AccountTypeBE> listofAccountTypes = AccountTypeDAL.GetAccountTypeBEs();
                            if (listofAccountTypes != null && listofAccountTypes.Count > 0)
                            {
                                foreach (AccountTypeBE accountType in listofAccountTypes)
                                {
                                    AccountBE acBE = new AccountBE();
                                    acBE.Created_Date = DateTime.Now;
                                    acBE.Description = "";
                                    acBE.Email_Address = "";
                                    acBE.Is_Active = true;
                                    acBE.Is_Deleted = false;
                                    acBE.Mobile_No = "";
                                    acBE.Phone_No = "";
                                    acBE.Name = accountType.Name;
                                    acBE.Updated_Date = DateTime.Now;
                                    acBE.PumpID = pumpID;
                                    acBE.Is_Default = true;
                                    acBE.Account_Type_ID = accountType.ID;
                                    AccountDAL.Save(acBE);
                                }
                            }

                            //add default products into new account
                            List<DefaultProductBE> listofDefaultProducts = DefaultProductDAL.GetDefaultProducts();
                            if (listofDefaultProducts != null && listofDefaultProducts.Count > 0)
                            {
                                foreach (DefaultProductBE defaultProduct in listofDefaultProducts)
                                {
                                    ProductBE p = new ProductBE();
                                    p.Created_Date = DateTime.Now;
                                    p.Description = defaultProduct.Description;
                                    p.Is_Active = true;
                                    p.Is_Default = true;
                                    p.Is_Deleted = false;
                                    p.Last_Purchase_Price = 0;
                                    p.MeasureUnitID = defaultProduct.MeasureUnitID;
                                    p.Name = defaultProduct.Name;
                                    p.ProductCode = defaultProduct.ProductCode;
                                    p.Profit = 0;
                                    p.PumpID = pumpID;
                                    p.Sale_Price = 0;
                                    p.Updated_Date = DateTime.Now;
                                    p.Amount = 0;
                                    ProductDAL.Save(p);
                                }
                            }

                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 1,
                                status_message = "Account Created Successfully!",
                                PumpCode = companyInfo.PumpCode,
                                EmailAddress= companyInfo.Email,
                            });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Failed to Create User!",
                                PumpCode = companyInfo.PumpCode,
                                EmailAddress = companyInfo.Email,
                            });
                        }

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Create account, Please Contact  aliimran@maimasoft.com!",
                        });

                    }

                }
                catch (Exception ex)
                {
                    //log exception in database
                    //ExceptionBE exp = new ExceptionBE();
                    //exp.Created_Date = DateTime.Now;
                    //exp.Dated = DateTime.Now;
                    //exp.Event_Name = "SignUp";
                    //exp.Exception_Details = ex.Message;
                    //exp.Page_URL = "Signup API Call";
                    //exp.Updated_Date = DateTime.Now;
                    //ExceptionDAL.Save(exp);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to complete registration. please try again later."
                    });
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                status_code = 0,
                status_message = "Invalid Request Parameters"
            });
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
                        if(m.PumpCode != inParams.PumpCode)
                        {
                            InsertLoginLogEntry(false, inParams.User_Email, inParams.Password,m.User_Type);
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "User not authorized for this Petrol Pump."
                            });
                        }
                        InsertLoginLogEntry(true, inParams.User_Email, GeneralFunctions.Encrypt(inParams.Password),m.User_Type);
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
                       

                        CompanyInfoBE c = CompanyInfoDAL.GetCompanyInfoByID(m.PumpID);

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
                            pumpLogo = c.Logo_Title,
                            pumpCode = m.PumpCode,
                            PumpName = c.Name,
                            PackageName = c.PackageName,
                            PackageExpiry = c.PackageExpiry
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

            InsertLoginLogEntry(false, "null", "Null","");
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                status_code = 0,
                status_message = "Invalid Request Parameters"
            });

        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetDashboardData([FromBody] UserBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Access Key."
                        });
                    }

                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    CompanyInfoBE c = CompanyInfoDAL.GetCompanyInfoByID(loggedinUser.PumpID);

                    List<AccountBE> ListofAllAccounts = AccountDAL.GetAccountByPumpID(loggedinUser.PumpID);
                    decimal cashInHand = 0;
                    string balanceType = "";
                    decimal totalSales = 0;
                    decimal totalPurchase = 0;
                    decimal totalExpenses = 0;
                    decimal monthlyTotalSales = 0;
                    if (ListofAllAccounts != null && ListofAllAccounts.Count > 0)
                    {
                        cashInHand = ListofAllAccounts.Where(a => a.Account_Type_ID == 1).Sum(a => a.Balance);
                        balanceType = ListofAllAccounts.Where(a => a.Account_Type_ID == 1).FirstOrDefault().BalanceType;
                        AccountBE cashAccountBE = ListofAllAccounts.Where(a => a.Account_Type_ID == 1).FirstOrDefault();
                        totalSales = ListofAllAccounts.Where(a => a.Account_Type_ID == 2).Sum(a => a.Balance);
                        totalPurchase = ListofAllAccounts.Where(a => a.Account_Type_ID == 4).Sum(a => a.Balance);
                        totalExpenses = ListofAllAccounts.Where(a => a.Account_Type_ID == 8).Sum(a => a.Balance);
                    }
                    DateTime startDate = Convert.ToDateTime("2022-01-01");

                    DateTime startingDate = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
                    DateTime endingDate = startingDate.AddMonths(1).AddDays(-1);


                    decimal petrolSaleAmount = 0;
                    decimal dieselSaleAmount = 0;
                    decimal petrolSaleQuantity = 0;
                    decimal dieselSaleQuantity = 0;
                    int noofAccounts = ListofAllAccounts.Count();

                    List<SaleInvoiceDetailBE> ListofSales = SaleInvoiceDetailDAL.GetSaleInvoiceDetailByPumpDates(loggedinUser.PumpID, startingDate, endingDate);
                    if (ListofSales != null && ListofSales.Count > 0)
                    {
                         petrolSaleAmount = ListofSales.Where(a => a.Product_BE.ProductCode == "PET").Sum(b => b.Amount);
                         dieselSaleAmount = ListofSales.Where(a => a.Product_BE.ProductCode == "HSD").Sum(b => b.Amount);
                         petrolSaleQuantity = ListofSales.Where(a => a.Product_BE.ProductCode == "PET").Sum(b => b.Quantity);
                         dieselSaleQuantity = ListofSales.Where(a => a.Product_BE.ProductCode == "HSD").Sum(b => b.Quantity);
                         monthlyTotalSales = ListofSales.Sum(b => b.Quantity * b.Price);
                    }
                    List<ProductBE> listofProducts = ProductDAL.GetProductsByPumpID(loggedinUser.PumpID);
                    if (listofProducts != null && listofProducts.Count > 0)
                    {
                        listofProducts = listofProducts.OrderByDescending(a => a.Balance).ToList();
                        if (listofProducts.Count > 8)
                        {
                            listofProducts = listofProducts.Take(8).ToList();
                        }
                       
                    }
                    //hand negative stock
                    if(listofProducts != null && listofProducts.Count > 0)
                    {
                        foreach (ProductBE p in listofProducts)
                        {
                            if(p.BalanceType == "Credit")
                            {
                                p.Balance = p.Balance * -1;
                            }
                        }
                    }

                    List<ProductPumpBE> ListofProductPumps = ProductPumpDAL.GetProductPumpByPumpID(loggedinUser.PumpID);

                    List<PumpReadingBE> ListofReadings = new List<PumpReadingBE>();
                    int previousinvoiceno = SaleInvoiceHeadDAL.GetLastInvoiceID(loggedinUser.PumpID);
                    if (previousinvoiceno > 0)
                    {
                        List<PumpReadingBE> ListofPreviousReading = new List<PumpReadingBE>();
                        if (previousinvoiceno != 0)
                        {
                            ListofPreviousReading = PumpReadingDAL.GetPumpReadingByInvoiceID(previousinvoiceno).OrderBy(p => p.Pump_No).ToList();
                        }
                        foreach (ProductPumpBE p in ListofProductPumps)
                        {
                            PumpReadingBE prb = new PumpReadingBE();
                            prb.Created_Date = DateTime.Now;
                            prb.Dated = DateTime.Now;
                            prb.Is_Active = true;
                            prb.Is_Deleted = false;
                            prb.Current_Reading = 0;
                            prb.Last_Reading = 0;
                            prb.Pump_No = p.Pump_No;
                            prb.PumpMachineID = p.ID;
                            ProductBE prod = ProductDAL.GetProductByID(p.Product_ID);
                            prb.ProductName = prod.Name;
                            prb.ProductCode = prod.ProductCode;
                            ListofReadings.Add(prb);
                        }
                        if (ListofPreviousReading != null && ListofPreviousReading.Count > 0)
                        {
                            if (ListofReadings != null)
                            {
                                foreach (PumpReadingBE prb in ListofPreviousReading)
                                {
                                    foreach (PumpReadingBE prbcurrent in ListofReadings)
                                    {
                                        if (prbcurrent.Pump_No == prb.Pump_No)
                                        {
                                            prbcurrent.Last_Reading = prb.Current_Reading;
                                            prbcurrent.Current_Reading = prb.Current_Reading;
                                        }
                                    }
                                }
                            }
                        }
                        ListofReadings = ListofReadings.OrderBy(p => p.Pump_No).ToList();
                       
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully Returning dashboard data",
                        NoOfAccounts = noofAccounts,
                        CashInHand = cashInHand.ToString("0,0") + " " + balanceType,
                        TotalSales = totalSales.ToString("0,0"),
                        MonthlyTotalSales = monthlyTotalSales.ToString("0,0"),
                        PetrolSaleQuantity = petrolSaleQuantity.ToString("0,0"),
                        DieselSaleQuantity = dieselSaleQuantity.ToString("0,0"),
                        TotalPurchases = totalPurchase.ToString("0,0"),
                        TotalExpenses = totalExpenses.ToString("0,0"),
                        ListofStockDetails = listofProducts,
                        ListofReadings = ListofReadings,
                        PackageName = c.PackageName,
                        PackageExpiry = c.PackageExpiry,

                    });


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

            InsertLoginLogEntry(false, "null", "Null","");
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                status_code = 0,
                status_message = "Invalid Request Parameters"
            });

        }

        //[System.Web.Http.HttpPost]
        //public HttpResponseMessage ForgotPassword([FromBody] BinaryTree_NodeBE inParams)
        //{
        //    if (inParams != null && !String.IsNullOrEmpty(inParams.Email_Address))
        //    {
        //        try
        //        {
        //            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //            BinaryTree_NodeBE m = BinaryTree_NodeDAL.GetPersonalBEByEmail(inParams.Email_Address);
        //            if (m != null)
        //            {
        //                Boolean emailStatus = SendForgotEmail(m);
        //                if(emailStatus == true)
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.OK, new
        //                    {
        //                        status_code = 1,
        //                        status_message = "Email Sent to your provided email address!",
        //                    });
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.OK, new
        //                    {
        //                        status_code = 0,
        //                        status_message = "Failed to Generate Forgot Email!",
        //                    });
        //                }

        //            }
        //            else
        //            {
        //                //InsertLoginLogEntry(false, inParams.userName, inParams.password);
        //                return Request.CreateResponse(HttpStatusCode.OK, new
        //                {
        //                    status_code = 0,
        //                    status_message = "You have entered Invalid Email Address."
        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //InsertLoginLogEntry(false, inParams.userName, inParams.password);
        //            return Request.CreateResponse(HttpStatusCode.OK, new
        //            {
        //                status_code = 0,
        //                status_message = "Sorry, unable to complete transaction please try again later." + ex.Message
        //            });
        //        }
        //    }

        //    InsertLoginLogEntry(false, "null", "Null");
        //    return Request.CreateResponse(HttpStatusCode.OK, new
        //    {
        //        status_code = 0,
        //        status_message = "Invalid Request Parameters"
        //    });

        //}

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ChangePassword([FromBody] ChangePasswordBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.ID) && !String.IsNullOrEmpty(inParams.AccessKey) && !String.IsNullOrEmpty(inParams.OldPassword) && !String.IsNullOrEmpty(inParams.NewPassword))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.ID), inParams.AccessKey) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Access Key."
                        });
                    }
                    if (inParams.OldPassword == inParams.NewPassword)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid New Password."
                        });
                    }


                    UserBE m = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.ID));
                    if (m != null)
                    {
                        if (inParams.NewPassword.Length < 8)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "New Password Length must be at least 8 characters!",
                            });
                        }

                        if (inParams.OldPassword != GeneralFunctions.Decrypt(m.Password))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Invalid Old Password."
                            });
                        }

                        m.Password = GeneralFunctions.Encrypt(inParams.NewPassword);
                        UserDAL.Save(m);
                        AccessKeyDAL.DeleteAccessKey(m.ID);
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Password Changed Successfully. Please Re-Login Again!",
                        });

                    }
                    else
                    {
                        //InsertLoginLogEntry(false, inParams.userName, inParams.password);
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Request."
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
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters."
                });
            }

        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage LogoutUser([FromBody] UserBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Access Key."
                        });
                    }

                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));

                    AccessKeyDAL.DeleteAccessKey(loggedinUser.ID);
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully Logged Out",
                    }); ;


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
        public HttpResponseMessage GetFuelProviders([FromBody] UserBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Access Key."
                        });
                    }

                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));

                    List<FuelProviderBE> ListofFuelProviders = FuelProviderDAL.GetFuelProviderss();
                    var reducedList = ListofFuelProviders.Select(e => new { e.ID, e.Name, e.Logo_Login, e.Logo_Title, e.Logo_Reports}).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully Returning fuel providers",
                        ListofFuelProviders = reducedList,
                    }); ;


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
        public HttpResponseMessage GetCompanyProfile([FromBody] UserBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Access Key."
                        });
                    }

                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                   

                    if (loggedinUser.User_Type == "Admin")
                    {
                        CompanyInfoBE c = CompanyInfoDAL.GetCompanyInfoByID(loggedinUser.PumpID);
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Successfully Returning Company Profile",
                            ID = c.ID,
                            PumpCode = c.PumpCode,
                            Name = c.Name,
                            Address = c.Address,
                            Phone = c.Phone,
                            Fax = c.Fax,
                            Email = c.Email,
                            Mobile = c.Mobile,
                            Website = c.Website,
                            FuelProviderID = c.FuelProviderID,
                            Show_Pumps = c.Show_Pumps
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Not authorized for profile change",
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
        public HttpResponseMessage UpdateCompanyProfile([FromBody] CompanyInfoBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Access Key."
                        });
                    }

                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));


                    if (loggedinUser.User_Type == "Admin")
                    {
                        CompanyInfoBE c = CompanyInfoDAL.GetCompanyInfoByID(loggedinUser.PumpID);
                        c.Name = Convert.ToString(inParams.Name);
                        c.Address = Convert.ToString(inParams.Address);
                        int fuleId = Convert.ToInt32(inParams.FuelProviderID);
                        if(fuleId != c.FuelProviderID)
                        {
                            c.FuelProviderID = fuleId;
                            FuelProviderBE fpBE = FuelProviderDAL.GetFuelProviderByID(fuleId);
                            c.Logo_Login = fpBE.Logo_Login;
                            c.Logo_Reports = fpBE.Logo_Reports;
                            c.Logo_Title = fpBE.Logo_Title;
                        }
                        c.Phone = inParams.Phone;
                        c.Fax = inParams.Fax;
                        c.Email = inParams.Email;
                        c.Mobile = inParams.Mobile;
                        c.Show_Pumps = inParams.Show_Pumps;
                        c.Website = inParams.Website;

                        CompanyInfoDAL.Save(c);
                        
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Successfully Updated Profile",
                            
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Not authorized for profile change",
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

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ForgotPassword([FromBody] UserBE inParams)
        {
            if (inParams != null && !String.IsNullOrEmpty(inParams.User_Email))
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    UserBE m = UserDAL.GetUserBEByUserName(inParams.User_Email);
                    if (m != null)
                    {
                        Boolean emailStatus = SendForgotEmail(m);
                        if (emailStatus == true)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 1,
                                status_message = "Email Sent to your provided email address!",
                            });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Failed to Generate Forgot Email!",
                            });
                        }

                    }
                    else
                    {
                        //InsertLoginLogEntry(false, inParams.userName, inParams.password);
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "You have entered Invalid Email Address."
                        });
                    }
                }
                catch (Exception ex)
                {
                    //InsertLoginLogEntry(false, inParams.userName, inParams.password);
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 2,
                        status_message = "Sorry, unable to complete transaction please try again later." + ex.Message
                    });
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                status_code = 2,
                status_message = "Invalid Request Parameters"
            });

        }

        private Boolean SendForgotEmail(UserBE mem)
        {
            Boolean status = false;
            string tempString = "";
            try
            {
                // D:\Inetpub\vhosts\ICTraderorg.org\ICTradermaster.com\Email_Template\Forgot_Personal.htm
                //path used in website

                string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/");
                //string filepath = "D:\\Inetpub\\vhosts\\goldpayexchange.com\\ictrader.co\\";
                filepath = filepath + "Email_Template\\Forgot_Personal.htm";
                tempString = filepath;
                System.IO.StreamReader myFile = new System.IO.StreamReader(filepath);
                string myString = myFile.ReadToEnd();
                myFile.Close();
                myString = myString.Replace("{#password#}", GeneralFunctions.Decrypt(mem.Password));
                myString = myString.Replace("{#Name#}", mem.FullName);


                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();
                message.To.Add(new MailAddress(mem.User_Email));
                message.Subject = "Your password at petrol-pump.com";
                message.IsBodyHtml = true;

                message.Body = myString;
                string hostname = ConfigurationManager.AppSettings.Get("MailServerName").ToString().Trim();
                client.Host = hostname;
                string username = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
                string password = ConfigurationManager.AppSettings.Get("Password").ToString().Trim();
                message.From = new MailAddress(username);
                System.Net.NetworkCredential basicCredentials = new System.Net.NetworkCredential(username, password);
                client.Credentials = basicCredentials;
                client.Send(message);
                status = true;

            }
            catch (Exception ex)
            {
                ExceptionBE exp = new ExceptionBE();
                exp.Created_Date = DateTime.Now;
                exp.Dated = DateTime.Now;
                exp.Event_Name = "Send Forgot Email";
                exp.Exception_Details = tempString + " " + ex.Message;
                exp.Module_Name = "Forgot Email";
                exp.Page_URL = "Forgot Email";
                exp.Updated_Date = DateTime.Now;
                ExceptionDAL.Save(exp);
            }
            finally
            {

            }
            #region EmailLogEntry
            //add entry in email log
            EmailLogBE elBE = new EmailLogBE();
            elBE.Created_Date = DateTime.Now;
            elBE.Updated_Date = DateTime.Now;
            elBE.Dated = DateTime.Now.Date;
            if (status == true)
            {
                elBE.Delivery_Status = "Success";
            }
            else
            {
                elBE.Delivery_Status = "Failure";
            }

            elBE.Email_From = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
            elBE.Email_To = mem.User_Email;
            elBE.Event_Type = "Forgot Password";
            elBE.Reference = "Personal";
            elBE.Subject = "Forgot Password Information at petrol-pump.com";
            elBE.Updated_Date = DateTime.Now;
            EmailLogDAL.Save(elBE);
            #endregion

            return status;

        }
    }
}
