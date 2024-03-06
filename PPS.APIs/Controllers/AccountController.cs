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
    public class AccountController : ApiController
    {
        //#region Member Sign Up

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetAccounts([FromBody] GeneralRequestBE inParams)
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
                    int PageNo = 0;
                    int PageSize = 1000;

                    DateTime Dated = DateTime.Now;

                    if (inParams.Dated != null)
                    {
                        Dated = Convert.ToDateTime(inParams.Dated);
                    }
                    if (inParams.PageNo > 0)
                    {
                        PageNo = Convert.ToInt32(inParams.PageNo);
                    }
                    if (inParams.PageSize > 0)
                    {
                        PageSize = inParams.PageSize;
                    }


                    List<AccountBE> listofAccounts = new List<AccountBE>();
                    if (inParams.Dated != null)
                    {
                        listofAccounts = AccountDAL.GetAccountByPumpIDOnDate(currentUser.PumpID, Dated);
                    }
                    else
                    {
                        listofAccounts = AccountDAL.GetAccountByPumpID(currentUser.PumpID).ToList();
                    }

                    if (inParams.Account_Type_ID > 0 && listofAccounts.Count > 0)
                    {
                        listofAccounts = listofAccounts.Where(a => a.Account_Type_ID == inParams.Account_Type_ID).ToList();
                    }
                    if (inParams.BalanceType == "Debit" || inParams.BalanceType == "Credit")
                    {
                        listofAccounts = listofAccounts.Where(a => a.BalanceType == inParams.BalanceType).ToList();
                    }
                    if(inParams.Name != null)
                    {
                        if (inParams.Name != "")
                        {
                            listofAccounts = listofAccounts.Where(a => a.Name.ToLower() == inParams.Name.ToLower()).ToList();

                        }
                    }
                   

                    if (inParams.OrderBy != "")
                    {
                        if (inParams.OrderBy == "1")
                        {
                            //Name Asc
                            listofAccounts = listofAccounts.OrderBy(a => a.Name).ToList();
                        }
                        else if (inParams.OrderBy == "2")
                        {
                            listofAccounts = listofAccounts.OrderByDescending(a => a.Name).ToList();
                        }
                        else if (inParams.OrderBy == "3")
                        {
                            listofAccounts = listofAccounts.OrderBy(a => a.Balance).ToList();
                        }
                        else if (inParams.OrderBy == "4")
                        {
                            listofAccounts = listofAccounts.OrderByDescending(a => a.Balance).ToList();
                        }
                        else if (inParams.OrderBy == "5")
                        {
                            listofAccounts = listofAccounts.OrderBy(a => a.Created_Date).ToList();
                        }
                        else if (inParams.OrderBy == "6")
                        {
                            listofAccounts = listofAccounts.OrderByDescending(a => a.Created_Date).ToList();
                        }
                    }

                    decimal DebitSum = 0M;
                    decimal CreditSum = 0M;
                    if(listofAccounts != null && listofAccounts.Count > 0)
                    {
                        DebitSum = listofAccounts.Where(a => a.BalanceType == "Debit").Sum(a => a.Balance);
                        CreditSum = listofAccounts.Where(a => a.BalanceType == "Credit").Sum(a => a.Balance);
                    }

                    var reducedList = listofAccounts.Select(e => new { e.ID, e.Name, e.Balance, e.BalanceType, e.Description, e.Email_Address, e.Mobile_No, e.Phone_No, e.Account_Type_ID, e.Account_Type_BE, e.PumpID, e.Is_Default }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofAccounts = reducedList,
                        DebitSum = DebitSum,
                        CreditSum = CreditSum,
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
        public HttpResponseMessage AddNewAccount([FromBody] AccountBE inParams)
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

                UserBE currentUser = UserDAL.GetUserBEByID(inParams.UserID);


                AccountBE acAlreadyExits = AccountDAL.GetAccountByName(inParams.Name, currentUser.PumpID);
                if (acAlreadyExits != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Account Already Exists with same Name!",
                    });
                }

                try
                {

                    AccountBE accountBE = new AccountBE();
                    accountBE.Name = inParams.Name;
                    accountBE.Description = inParams.Description;
                    accountBE.Mobile_No = inParams.Mobile_No;
                    accountBE.Phone_No = inParams.Phone_No;
                    accountBE.Email_Address = inParams.Email_Address;
                    accountBE.Updated_Date = DateTime.Now;
                    accountBE.Created_Date = DateTime.Now;
                    accountBE.Is_Deleted = false;
                    accountBE.Is_Active = true;
                    accountBE.PumpID = currentUser.PumpID;
                    accountBE.Account_Type_ID = inParams.Account_Type_ID;
                    int accountID;
                    accountID = AccountDAL.Save(accountBE);

                    AccountTypeBE acType = AccountTypeDAL.GetAccountTypeByID(inParams.Account_Type_ID);
                    string openingBalanceType = inParams.OpeningBalanceType;
                    //get capital account for the selected pump
                    DateTime voucherDate = Convert.ToDateTime(inParams.OpeningDate);
                    if (inParams.OpeningBalance != 0)
                    {
                        AccountBE capitalAccount = AccountDAL.GetAccountByName("Capital", currentUser.PumpID);
                        if (capitalAccount != null)
                        {
                            // add voucher of the opening balance
                            if (acType.Name.ToLower() == "customer" || acType.Name.ToLower() == "debitors")
                            {
                                //add voucher
                                int voucherNo = GeneralVoucherDAL.GetNextVoucherNo(currentUser.PumpID);
                                GeneralVoucherBE gv = new GeneralVoucherBE();
                                gv.Amount = Convert.ToDecimal(inParams.OpeningBalance);
                                gv.Dated = voucherDate;
                                gv.Created_Date = DateTime.Now;
                                if(openingBalanceType == "Debit")
                                {
                                    gv.Credit_Account_ID = capitalAccount.ID;
                                    gv.Debit_Account_ID = accountID;
                                }
                                else
                                {
                                    gv.Credit_Account_ID = accountID;
                                    gv.Debit_Account_ID = capitalAccount.ID;
                                }
                                gv.Description = "Opening Balance Voucher";
                                gv.Is_Active = true;
                                gv.Is_Deleted = false;
                                gv.Updated_Date = DateTime.Now;
                                gv.VoucherNo = voucherNo;
                                gv.PumpID = currentUser.PumpID;
                                gv.AddedByUserID = currentUser.ID;
                                gv.AddedByUser = currentUser.User_Email;
                                int gvID = GeneralVoucherDAL.Save(gv);
                                if(gvID > 0)
                                {
                                    if(openingBalanceType == "Debit")
                                    {
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, accountID, "Debit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, capitalAccount.ID, "Credit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                    }
                                   else
                                    {
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, capitalAccount.ID, "Debit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, accountID, "Credit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                    }
                                }
                                
                            }
                            if (acType.Name.ToLower() == "creditors" || acType.Name.ToLower() == "supplier")
                            {
                                //add voucher
                                int voucherNo = GeneralVoucherDAL.GetNextVoucherNo(currentUser.PumpID);
                                GeneralVoucherBE gv = new GeneralVoucherBE();
                                gv.Amount = Convert.ToDecimal(inParams.OpeningBalance);
                                gv.Created_Date = DateTime.Now;
                                gv.Dated = voucherDate;
                                if(openingBalanceType == "Credit")
                                {
                                    gv.Credit_Account_ID = accountID;
                                    gv.Debit_Account_ID = capitalAccount.ID;
                                }
                                else
                                {
                                    gv.Credit_Account_ID = capitalAccount.ID;
                                    gv.Debit_Account_ID = accountID;
                                }
                                gv.Description = "Opening Balance Voucher";
                                gv.Is_Active = true;
                                gv.Is_Deleted = false;
                                gv.Updated_Date = DateTime.Now;
                                gv.VoucherNo = voucherNo;
                                gv.PumpID = currentUser.PumpID;
                                gv.AddedByUserID = currentUser.ID;
                                gv.AddedByUser = currentUser.User_Email;
                                int gvID = GeneralVoucherDAL.Save(gv);
                                if(gvID > 0)
                                {
                                    if(openingBalanceType == "Credit")
                                    {
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, accountID, "Credit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, capitalAccount.ID, "Debit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                    }
                                    else
                                    {
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, capitalAccount.ID, "Credit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                        GeneralFunctions.AddEntryInLedger(currentUser.PumpID, accountID, "Debit", gv.Amount, gvID, "Voucher", gv.Description, gv.Dated, "", "");
                                    }
                                }
                                
                            }
                        }

                    }
                    if (accountID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Account Opened Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Open Account!",
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
        public HttpResponseMessage UpdateNewAccount([FromBody] AccountBE inParams)
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

                UserBE currentUser = UserDAL.GetUserBEByID(inParams.UserID);

                int ID = Convert.ToInt32(inParams.ID);
                AccountBE accountBE = AccountDAL.GetAccountByID(ID, currentUser.PumpID);
                if (accountBE == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Account does not exists!",
                    });
                }

                try
                {

                    accountBE.Name = inParams.Name;
                    accountBE.Description = inParams.Description;
                    accountBE.Mobile_No = inParams.Mobile_No;
                    accountBE.Phone_No = inParams.Phone_No;
                    accountBE.Email_Address = inParams.Email_Address;
                    accountBE.Updated_Date = DateTime.Now;
                    accountBE.Account_Type_ID = inParams.Account_Type_ID;
                    int accountID;
                    accountID = AccountDAL.Save(accountBE);
                   
                    if (accountID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Account Updated Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Update Account!",
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
        public HttpResponseMessage GetAccountbyID([FromBody] UserBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey) && !String.IsNullOrEmpty(inParams.ID.ToString()))
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
                    int ID = Convert.ToInt32(inParams.ID);
                    AccountBE selectedAccount = AccountDAL.GetAccountByID(ID, currentUser.PumpID);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning Details",
                        ID = selectedAccount.ID,
                        Account_Type_ID = selectedAccount.Account_Type_ID,
                        PumpID = selectedAccount.PumpID,
                        Name = selectedAccount.Name,
                        Description = selectedAccount.Description,
                        Mobile_No = selectedAccount.Mobile_No,
                        Email_Address = selectedAccount.Email_Address,
                        Phone_No = selectedAccount.Phone_No,
                        Is_Default = selectedAccount.Is_Default,
                        AccountTypeName = selectedAccount.Account_Type_BE.Name,
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
        public HttpResponseMessage GetAccountbyIDWithBalance([FromBody] UserBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey) && !String.IsNullOrEmpty(inParams.ID.ToString()))
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
                    int ID = Convert.ToInt32(inParams.ID);
                    AccountBE selectedAccount = AccountDAL.GetAccountByIDPumpID(currentUser.PumpID,ID);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning Details",
                        ID = selectedAccount.ID,
                        Account_Type_ID = selectedAccount.Account_Type_ID,
                        PumpID = selectedAccount.PumpID,
                        Name = selectedAccount.Name,
                        Description = selectedAccount.Description,
                        Mobile_No = selectedAccount.Mobile_No,
                        Email_Address = selectedAccount.Email_Address,
                        Phone_No = selectedAccount.Phone_No,
                        Is_Default = selectedAccount.Is_Default,
                        AccountTypeName = selectedAccount.Account_Type_BE.Name,
                        Balance = selectedAccount.Balance,
                        BalanceType = selectedAccount.BalanceType,
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
        public HttpResponseMessage GetAccountLedger([FromBody] UserBE inParams)
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
                    DateTime startDate = Convert.ToDateTime("2022-01-01");
                    DateTime endDate = DateTime.Now;
                    int pageNo = 0;
                    if (inParams.StartDate != null)
                    {
                        startDate = Convert.ToDateTime(inParams.StartDate);
                    }
                    if (inParams.EndDate != null)
                    {
                        endDate = Convert.ToDateTime(inParams.EndDate);
                    }
                    if (inParams.PageNo != null)
                    {
                        pageNo = Convert.ToInt32(inParams.PageNo);
                    }

                    int accoutnID = Convert.ToInt32(inParams.ID);
                    AccountBE ac = AccountDAL.GetAccountByID(accoutnID, currentUser.PumpID);
                    if(ac == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Sorry, Invalid Acocunt ID."
                        });
                    }

                    int pageSize = 5;
                    List<GeneralLedgerBE> ListofLedger = GeneralLedgerDAL.GetLedgerbyDate(accoutnID, startDate, endDate);
                    ListofLedger = ListofLedger.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<GeneralLedgerBE>();

                    var reducedList = ListofLedger.Select(e => new { e.ID, e.Account_ID, e.Transaction_Date, e.Description,e.Reference_No,e.Reference_Type, e.Vehicle_No, e.Receipt_No, e.Debit, e.Credit, e.Balance, e.BalanceType }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofLedger = reducedList,
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

        //[System.Web.Http.HttpPost]
        //public HttpResponseMessage DeleteAccount([FromBody] AccountBE inParams)
        //{

        //    if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
        //    {
        //        if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, new
        //            {
        //                status_code = 0,
        //                status_message = "Invalid Access Key."
        //            });
        //        }

        //        try
        //        {
        //            UserBE currentUser = UserDAL.GetUserBEByID(inParams.UserID);
        //            AccountBE acBE = AccountDAL.GetAccountByID(inParams.ID);
        //            if (acBE != null)
        //            {
        //                if (acBE.PumpID == currentUser.PumpID)
        //                {
        //                    acBE.Is_Active = false;
        //                    acBE.Is_Deleted = true;
        //                    acBE.Updated_Date = DateTime.Now;
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.OK, new
        //                    {
        //                        status_code = 0,
        //                        status_message = "Restriction on to delete voucher!",
        //                    });
        //                }
        //            }

        //            int voucherID = AccountDAL.Save(acBE);
        //            if (voucherID > 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new
        //                {
        //                    status_code = 1,
        //                    status_message = "Account Deleted Successfully!",
        //                });
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new
        //                {
        //                    status_code = 0,
        //                    status_message = "Failed to delete Account!",
        //                });
        //            }



        //        }
        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, new
        //            {
        //                status_code = 0,
        //                status_message = "Sorry, unable to reply."
        //            });
        //        }



        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            status_code = 0,
        //            status_message = "Invalid Request Parameters"
        //        });
        //    }
        //}


        

    }
}
