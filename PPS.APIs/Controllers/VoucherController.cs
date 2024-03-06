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
    public class VoucherController : ApiController
    {

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetVouchers([FromBody] GeneralRequestBE inParams)
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
                    DateTime startDate = Convert.ToDateTime("2022-01-01"); // DateTime.Now.AddDays(-30);
                    DateTime endDate = DateTime.Now;
                    int pageNo = 0;
                    if(inParams.StartDate != null && inParams.StartDate.Year != 1)
                    {
                        startDate = Convert.ToDateTime(inParams.StartDate);
                    }
                    if(inParams.EndDate != null && inParams.EndDate.Year != 1)
                    {
                        endDate = Convert.ToDateTime(inParams.EndDate);
                    }
                    if(inParams.PageNo > 0)
                    {
                        pageNo = Convert.ToInt32(inParams.PageNo);
                    }
                    int pageSize = 5;
                    List<GeneralVoucherBE> ListofVouchers = GeneralVoucherDAL.GetAllVoucherByPumpID(currentUser.PumpID,startDate,endDate);

                    if(inParams.AccountID > 0)
                    {
                        ListofVouchers = ListofVouchers.Where(p => p.Credit_Account_ID == inParams.AccountID || p.Debit_Account_ID == inParams.AccountID).ToList();
                    }
                    if(inParams.Debit_Account_ID > 0)
                    {
                        ListofVouchers = ListofVouchers.Where(p => p.Debit_Account_ID == inParams.Debit_Account_ID).ToList();
                    }
                    if (inParams.Credit_Account_ID > 0)
                    {
                        ListofVouchers = ListofVouchers.Where(p => p.Credit_Account_ID == inParams.Credit_Account_ID).ToList();
                    }

                    if (ListofVouchers != null && ListofVouchers.Count > 0)
                    {
                        ListofVouchers = ListofVouchers.OrderByDescending(a => a.VoucherNo).ToList();
                    }
                    if(inParams.OrderBy != "")
                    {
                        if(inParams.OrderBy == "1")
                        {
                            ListofVouchers = ListofVouchers.OrderByDescending(a => a.VoucherNo).ToList();
                        }
                        else if (inParams.OrderBy == "2")
                        {
                            ListofVouchers = ListofVouchers.OrderBy(a => a.VoucherNo).ToList();
                        }
                        else if (inParams.OrderBy == "3")
                        {
                            ListofVouchers = ListofVouchers.OrderByDescending(a => a.Dated).ToList();
                        }
                        else if (inParams.OrderBy == "4")
                        {
                            ListofVouchers = ListofVouchers.OrderBy(a => a.Dated).ToList();
                        }
                        else if (inParams.OrderBy == "5")
                        {
                            ListofVouchers = ListofVouchers.OrderByDescending(a => a.Created_Date).ToList();
                        }
                        else if (inParams.OrderBy == "6")
                        {
                            ListofVouchers = ListofVouchers.OrderBy(a => a.Created_Date).ToList();
                        }
                    }
                    if(inParams.VoucherType != "")
                    {
                        if(inParams.VoucherType == "Expenses")
                        {
                            ListofVouchers = ListofVouchers.Where(p => p.Debit_Account.Account_Type_BE.ID == 8).ToList();
                        }
                        else if (inParams.VoucherType == "Received Amount")
                        {
                            ListofVouchers = ListofVouchers.Where(p => p.Debit_Account.Account_Type_BE.ID == 1).ToList();
                        }
                        else if (inParams.VoucherType == "Paid Amount")
                        {
                            ListofVouchers = ListofVouchers.Where(p => p.Credit_Account.Account_Type_BE.ID == 1 && p.Debit_Account.Account_Type_BE.ID != 8).ToList();
                        }

                    }
                    int TotalVouchers = 0;
                    int DebitCount = 0;
                    int CreditCount = 0;
                    decimal Total = 0M;

                    if(ListofVouchers != null && ListofVouchers.Count > 0)
                    {
                        TotalVouchers = ListofVouchers.Count();
                        DebitCount = ListofVouchers.Where(a => a.Debit_Account_ID > 0).Count();
                        CreditCount = ListofVouchers.Where(a => a.Credit_Account_ID > 0).Count();
                        Total = ListofVouchers.Sum(a => a.Amount);
                    }

                    ListofVouchers = ListofVouchers.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<GeneralVoucherBE>();
                    

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofVouchers = ListofVouchers,
                        TotalVouchers = TotalVouchers,
                        DebitCount = DebitCount,
                        CreditCount = CreditCount,
                        Total = Total,
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
        public HttpResponseMessage GetVoucherByID([FromBody] UserBE inParams)
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
                    int voucherID = inParams.ID;
                    GeneralVoucherBE gv = GeneralVoucherDAL.GetVoucherByID(voucherID);
                    if(gv.PumpID != currentUser.PumpID)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Not able to return data",
                        });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning Voucher",
                        ID = gv.ID,
                        VoucherNo = gv.VoucherNo,
                        Dated = gv.Dated,
                        Description = gv.Description,
                        Debit_Account_ID = gv.Debit_Account_ID,
                        Credit_Account_ID = gv.Credit_Account_ID,
                        Amount = gv.Amount,
                        AddedByUserID = gv.AddedByUserID,
                        AddedByUser = gv.AddedByUser,
                        UpdatedByUser = gv.UpdatedByUser,

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
        public HttpResponseMessage AddNewVoucher([FromBody] GeneralVoucherBE inParams)
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

                    AccountBE debitAccount = AccountDAL.GetAccountByID(inParams.Debit_Account_ID,loggedinUser.PumpID);
                    if(debitAccount == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Debit Account."
                        });
                    }

                    AccountBE creditAccount = AccountDAL.GetAccountByID(inParams.Credit_Account_ID, loggedinUser.PumpID);
                    if (debitAccount == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Credit Account."
                        });
                    }
                    GeneralVoucherBE objV = new GeneralVoucherBE();
                    objV.VoucherNo = GeneralVoucherDAL.GetNextVoucherNo(loggedinUser.PumpID);
                    objV.PumpID = loggedinUser.PumpID;
                    objV.Dated = inParams.Dated;
                    objV.Description = inParams.Description;
                    objV.Debit_Account_ID = inParams.Debit_Account_ID;
                    objV.Credit_Account_ID = inParams.Credit_Account_ID;
                    objV.Amount = inParams.Amount;
                    objV.AddedByUserID = loggedinUser.ID;
                    objV.AddedByUser = loggedinUser.User_Email;
                    objV.Created_Date = DateTime.Now;
                    objV.Updated_Date = DateTime.Now;
                    objV.Is_Active = true;
                    objV.Is_Deleted = false;
                    int voucherID  = GeneralVoucherDAL.Save(objV);
                    if(voucherID > 0)
                    {
                        //update the ledger
                        GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, objV.Credit_Account_ID, "Credit", objV.Amount, voucherID, "Voucher", objV.Description, objV.Dated, "", "");
                        GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, objV.Debit_Account_ID, "Debit", objV.Amount, voucherID, "Voucher", objV.Description, objV.Dated, "", "");
                        AccountDAL.updateAccountLedger(objV.Credit_Account_ID);
                        AccountDAL.updateAccountLedger(objV.Debit_Account_ID);
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Voucher Added Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to add voucher!",
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
        public HttpResponseMessage GetNextVoucherNo([FromBody] UserBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    string voucherNO = GeneralVoucherDAL.GetNextVoucherNo(loggedinUser.PumpID).ToString();
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning VoucherNo",
                        NextVoucherNo = voucherNO,
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
        public HttpResponseMessage DeleteVoucher([FromBody] GeneralVoucherBE inParams)
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
                    GeneralVoucherBE acBE = GeneralVoucherDAL.GetVoucherByID(inParams.ID);
                    if (acBE != null)
                    {
                        if (acBE.PumpID == currentUser.PumpID)
                        {
                            if(acBE.Created_Date  < DateTime.Now.AddDays(-2))
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new
                                {
                                    status_code = 0,
                                    status_message = "Two Days Old Voucher Can not be Deleted",
                                });
                            }
                            else
                            {
                                GeneralVoucherDAL.DeleteCompleteVoucher(acBE.ID,currentUser.PumpID);
                                return Request.CreateResponse(HttpStatusCode.OK, new
                                {
                                    status_code = 1,
                                    status_message = "Voucher Deleted Successfullly!",
                                });
                            }
                            

                         
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Restriction on to delete voucher!",
                            });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Voucher Not Found!",
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
        public HttpResponseMessage UpdateVoucher([FromBody] GeneralVoucherBE inParams)
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

                    AccountBE debitAccount = AccountDAL.GetAccountByID(inParams.Debit_Account_ID, loggedinUser.PumpID);
                    if (debitAccount == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Debit Account."
                        });
                    }

                    AccountBE creditAccount = AccountDAL.GetAccountByID(inParams.Credit_Account_ID, loggedinUser.PumpID);
                    if (debitAccount == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Credit Account."
                        });
                    }
                    GeneralVoucherBE objV = GeneralVoucherDAL.GetVoucherByID(inParams.ID);
                    if(objV != null)
                    {
                        int oldDebitID = objV.Debit_Account_ID;
                        int oldCreditID = objV.Credit_Account_ID;

                        objV.Dated = inParams.Dated;
                        objV.Description = inParams.Description;
                        objV.Debit_Account_ID = inParams.Debit_Account_ID;
                        objV.Credit_Account_ID = inParams.Credit_Account_ID;
                        objV.Amount = inParams.Amount;
                        objV.UpdatedByUser = loggedinUser.User_Email;
                        objV.Updated_Date = DateTime.Now;
                        int voucherID = GeneralVoucherDAL.Save(objV);

                        //delete existing entries in ledger and add new entries in ledger. 
                        GeneralLedgerBE glDebit = GeneralLedgerDAL.GetLedgerEntrybyAccountRefType(oldDebitID, objV.ID, "Voucher");
                        GeneralLedgerBE glCredit = GeneralLedgerDAL.GetLedgerEntrybyAccountRefType(oldCreditID, objV.ID, "Voucher");
                        if (glDebit != null)
                        {
                            glDebit.Account_ID = objV.Debit_Account_ID;
                            glDebit.Debit = objV.Amount;
                            glDebit.Description = objV.Description;
                            glDebit.Transaction_Date = objV.Dated;
                            glDebit.Credit = 0;
                            int glID = GeneralLedgerDAL.Save(glDebit);
                            if (objV.Debit_Account_ID != oldDebitID)
                            {
                                int recordEffected = AccountDAL.updateAccountLedger(oldDebitID);
                            }
                            else
                            {
                                int recordEffected = AccountDAL.updateAccountLedger(objV.Debit_Account_ID);
                            }

                        }
                        else
                        {
                            //no previous entry exists add now. 
                            // update the ledger
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, objV.Debit_Account_ID, "Debit", objV.Amount, voucherID, "Voucher", objV.Description, objV.Dated, "", "");
                            int recordEffected = AccountDAL.updateAccountLedger(objV.Debit_Account_ID);
                        }
                        //GeneralLedgerBE glCredit = GeneralLedgerDAL.GetLedgerEntrybyAccountRefType(oldCreditID, objV.ID, "Voucher");
                        if (glCredit != null)
                        {
                            glCredit.Account_ID = objV.Credit_Account_ID;
                            glCredit.Credit = objV.Amount;
                            glCredit.Description = objV.Description;
                            glCredit.Transaction_Date = objV.Dated;
                            glCredit.Debit = 0;
                            int glID = GeneralLedgerDAL.Save(glCredit);
                            if (objV.Credit_Account_ID != oldCreditID)
                            {
                                int recordEffected = AccountDAL.updateAccountLedger(oldCreditID);
                            }
                            else
                            {
                                int recordEffected = AccountDAL.updateAccountLedger(objV.Credit_Account_ID);
                            }

                        }
                        else
                        {
                            //add entry in credit
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, objV.Credit_Account_ID, "Credit", objV.Amount, voucherID, "Voucher", objV.Description, objV.Dated, "", "");
                            int recordEffected = AccountDAL.updateAccountLedger(objV.Credit_Account_ID);
                        }


                        if (voucherID > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 1,
                                status_message = "Voucher Updated Successfully!",
                            });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Failed to Update voucher!",
                            });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Update voucher!",
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
    }
}
