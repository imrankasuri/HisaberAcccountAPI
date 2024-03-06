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
    public class PurchaseController : ApiController
    {

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetPurchases([FromBody] UserBE inParams)
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
                    DateTime startDate = Convert.ToDateTime("2022-01-01");
                    DateTime endDate = DateTime.Now;
                    int pageNo = 0;
                    int pageSize = 10;
                    int totalRecords = 0;
                    if (inParams.StartDate != null)
                    {
                        startDate = Convert.ToDateTime(inParams.StartDate);
                    }
                    if(inParams.EndDate != null)
                    {
                        endDate = Convert.ToDateTime(inParams.EndDate);
                    }
                    if(inParams.PageNo != null)
                    {
                        pageNo = Convert.ToInt32(inParams.PageNo);
                    }
                   
                    decimal CashTotal = 0m;
                    decimal CreditTotal = 0m;
                    Decimal TotalPurchases = 0m;
                    
                    List<PurchaseInvoiceHeadBE> ListofPurchase = PurchaseInvoiceHeadDAL.GetPurchaseInvoiceHeadBEs(startDate,endDate, currentUser.PumpID);
                    if(ListofPurchase != null && ListofPurchase.Count > 0)
                    {
                        totalRecords = ListofPurchase.Count();
                        CashTotal = ListofPurchase.Sum(a => a.Cash_Total);
                        CreditTotal = ListofPurchase.Sum(a => a.Credit_Total);
                        TotalPurchases = ListofPurchase.Sum(a => a.TotalPurchase);
                    }
                    
                    ListofPurchase = ListofPurchase.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<PurchaseInvoiceHeadBE>();
                 

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofPurchase = ListofPurchase,
                        CashTotal = CashTotal,
                        CreditTotal = CreditTotal,
                        TotalPurchases = TotalPurchases,
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
        public HttpResponseMessage GetPurchaseDetail([FromBody] UserBE inParams)
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

                    List<PurchaseInvoiceDetailBE> ListofPurchaseDetail = PurchaseInvoiceDetailDAL.GetPurchaseInvoiceDetailBEs(Convert.ToInt32(inParams.ID));

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofPurchase = ListofPurchaseDetail,
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
        public HttpResponseMessage AddPurchase([FromBody] PurchaseInvoiceHeadBE inParams)
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
                    AccountBE cashAccount = AccountDAL.GetAccountByName("Cash", loggedinUser.PumpID);
                    AccountBE purchaseAccount = AccountDAL.GetAccountByName("Purchase", loggedinUser.PumpID);

                    foreach (PurchaseInvoiceDetailBE pid in inParams.ListofPurchaseDetails)
                    {
                        if (pid.Account_ID == cashAccount.ID)
                        {
                            pid.Is_Cash = true;
                        }
                    }
                    decimal cashTotal = inParams.ListofPurchaseDetails.Where(a => a.Is_Cash == true).Sum(a => a.Quantity * a.Price);
                    decimal creditTotal = inParams.ListofPurchaseDetails.Where(a => a.Is_Cash == false).Sum(a => a.Quantity * a.Price);

                    PurchaseInvoiceHeadBE pinvoicebe = new PurchaseInvoiceHeadBE();
                    pinvoicebe.PumpID = loggedinUser.PumpID;
                    pinvoicebe.PumpCode = loggedinUser.PumpCode;
                    pinvoicebe.Reference_No = inParams.Reference_No;
                    pinvoicebe.Dated = inParams.Dated;
                    pinvoicebe.Description = inParams.Description;
                    pinvoicebe.Updated_Date = DateTime.Now;
                    pinvoicebe.Created_Date = DateTime.Now;
                    pinvoicebe.Is_Deleted = false;
                    pinvoicebe.Is_Active = true;
                    pinvoicebe.Cash_Total = cashTotal;
                    pinvoicebe.Credit_Total = creditTotal;
                    pinvoicebe.AddedBy = loggedinUser.User_Email;
                    pinvoicebe.InvoiceNo = PurchaseInvoiceHeadDAL.GetNextPurchaseInvoiceNumber(loggedinUser.PumpID); 
                    int invoiceid;
                    invoiceid = PurchaseInvoiceHeadDAL.Save(pinvoicebe);
                    //delete previous records first
                    PurchaseInvoiceDetailDAL.DeletePurchaseInvoiceDetailByInvoice(invoiceid);
                    //save the invoice details.
                    List<PurchaseInvoiceDetailBE> ListofpDetail = inParams.ListofPurchaseDetails;
                    

                    foreach (PurchaseInvoiceDetailBE pid in ListofpDetail)
                    {
                        pid.Invoice_ID = invoiceid;
                        pid.PumpID = loggedinUser.PumpID;
                        pid.PumpCode = loggedinUser.PumpCode;
                        pid.Created_Date = DateTime.Now;
                        pid.Updated_Date = DateTime.Now;
                        pid.Is_Active = true;
                        pid.Is_Deleted = false;
                        pid.Account_BE = AccountDAL.GetAccountByID(pid.Account_ID, loggedinUser.PumpID);
                        pid.Product_BE = ProductDAL.GetProductByID(pid.Product_ID);
                        PurchaseInvoiceDetailDAL.Save(pid);
                        //update last purchase rate
                        ProductBE p = ProductDAL.GetProductByID(pid.Product_ID);

                        //add credit ledger entry in the account of purchaser
                        string desc = "Purchased " + pid.Quantity.ToString("0.00") + " " + p.Measure_Unit_BE.Name + " " + pid.Product_BE.Name + " @" + pid.Price.ToString("0.00") + " from " + pid.Account_BE.Name;

                        if (p != null)
                        {
                            p.Last_Purchase_Price = pid.Price;
                            ProductDAL.Save(p);
                            GeneralFunctions.AddEntryInProductLedger(p.ID, loggedinUser.PumpID, "Debit", pid.Quantity, pid.Invoice_ID, "Purchase", desc, pinvoicebe.Dated, pid.Vehicle_No, pinvoicebe.Reference_No);
                            ProductDAL.updateProductLedger(p.ID);
                        }
                        //add debit ledger entry into purchases account. 
                        //get the purchase account for this user

                        if (purchaseAccount != null)
                        {
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, purchaseAccount.ID, "Debit", pid.Amount, pid.Invoice_ID, "Purchase", desc, pinvoicebe.Dated, pid.Vehicle_No, pinvoicebe.Reference_No);
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, pid.Account_ID, "Credit", pid.Amount, pid.Invoice_ID, "Purchase", desc, pinvoicebe.Dated, pid.Vehicle_No, pinvoicebe.Reference_No);
                            AccountDAL.updateAccountLedger(purchaseAccount.ID);
                            AccountDAL.updateAccountLedger(pid.Account_ID);
                        }

                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Purchase Successfully Saved",
                        InvoiceNo = invoiceid.ToString(),
                        InvoiceID = pinvoicebe.InvoiceNo.ToString(),
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
        public HttpResponseMessage GetNextPurchaseInvoiceNo([FromBody] UserBE inParams)
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
                    int invoiceNo = PurchaseInvoiceHeadDAL.GetNextPurchaseInvoiceNumber(currentUser.PumpID);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning Invoice No",
                        InvoiceNo = invoiceNo,
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
        public HttpResponseMessage DeletePurchase([FromBody] GeneralRequestBE inParams)
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
                    UserBE loggedInUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    PurchaseInvoiceHeadBE purchaseHead = PurchaseInvoiceHeadDAL.GetPIHeadByID(Convert.ToInt32(inParams.ID));

                    if (purchaseHead != null)
                    {
                        int invoiceID = purchaseHead.ID;
                        if(purchaseHead.PumpID != loggedInUser.PumpID)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Not Allowed to Delete",
                            });
                        }

                        AccountBE purchaseAccount = AccountDAL.GetAccountByPumpIDTypeID(loggedInUser.PumpID, 4);

                        if (purchaseHead.Created_Date < DateTime.Now.AddDays(-2))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Two Days Old Purchase Can not be Deleted",
                            });
                        }
                        else
                        {
                            List<PurchaseInvoiceDetailBE> listofdetails = PurchaseInvoiceDetailDAL.GetPurchaseInvoiceDetailBEs(invoiceID);
                            if (listofdetails != null && listofdetails.Count > 0)
                            {
                                foreach (PurchaseInvoiceDetailBE sid in listofdetails)
                                {
                                    PurchaseInvoiceDetailDAL.DeleteEntryfromProductLedger(loggedInUser.PumpID, sid.Product_ID, sid.Invoice_ID, "Purchase");
                                }
                            }
                            PurchaseInvoiceDetailDAL.DeletePurchaseInvoiceDetailByInvoice(invoiceID);
                            PurchaseInvoiceHeadDAL.DeletePurchaseInvoiceByInvoice(invoiceID);
                            PurchaseInvoiceDetailDAL.DeletePurchasefromLedger(purchaseAccount.ID, invoiceID, "Purchase");
                            AccountDAL.updateAccountLedger(purchaseAccount.ID);

                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 1,
                                status_message = "Purchase Deleted Successfullly!",
                            });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Purchase Not Found!",
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
        public HttpResponseMessage UpdatePurchase([FromBody] PurchaseInvoiceHeadBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey) && !string.IsNullOrEmpty(Convert.ToString(inParams.ID)))
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
                    AccountBE cashAccount = AccountDAL.GetAccountByName("Cash", loggedinUser.PumpID);

                    foreach (PurchaseInvoiceDetailBE pid in inParams.ListofPurchaseDetails)
                    {
                        if (pid.Account_ID == cashAccount.ID)
                        {
                            pid.Is_Cash = true;
                        }
                    }

                    decimal cashTotal = inParams.ListofPurchaseDetails.Where(a => a.Is_Cash == true).Sum(a => a.Quantity * a.Price);
                    decimal creditTotal = inParams.ListofPurchaseDetails.Where(a => a.Is_Cash == false).Sum(a => a.Quantity * a.Price);

                    int invoiceid = inParams.ID;
                    AccountBE purchaseAccount = AccountDAL.GetAccountByName("Purchase", loggedinUser.PumpID);
                    PurchaseInvoiceHeadBE pinvoicebe = PurchaseInvoiceHeadDAL.GetPIHeadByID(invoiceid);
                  
                    pinvoicebe.PumpID = loggedinUser.PumpID;
                    pinvoicebe.PumpCode = loggedinUser.PumpCode;
                    pinvoicebe.Reference_No = inParams.Reference_No;
                    pinvoicebe.Dated = inParams.Dated;
                    pinvoicebe.Description = inParams.Description;
                    pinvoicebe.Updated_Date = DateTime.Now;
                    pinvoicebe.Is_Deleted = false;
                    pinvoicebe.Is_Active = true;
                    pinvoicebe.Cash_Total = cashTotal;
                    pinvoicebe.Credit_Total = creditTotal;
                    invoiceid = PurchaseInvoiceHeadDAL.Save(pinvoicebe);
                   
                    //save detail

                    //delete previous purchase detail entries
                    PurchaseInvoiceDetailDAL.DeletePurchaseInvoiceDetailByInvoice(invoiceid);
                    PurchaseInvoiceDetailDAL.DeletePurchasefromLedger(purchaseAccount.ID, invoiceid, "Purchase");
                    AccountDAL.updateAccountLedger(purchaseAccount.ID);
                    PurchaseInvoiceDetailDAL.DeleteEntryfromProductLedger(loggedinUser.PumpID, invoiceid, "Purchase");


                    //save the invoice details.
                    List<PurchaseInvoiceDetailBE> ListofpDetail = inParams.ListofPurchaseDetails;
                    foreach (PurchaseInvoiceDetailBE pid in ListofpDetail)
                    {
                        pid.Invoice_ID = invoiceid;
                        pid.PumpID = loggedinUser.PumpID;
                        pid.PumpCode = loggedinUser.PumpCode;
                        pid.Created_Date = DateTime.Now;
                        pid.Updated_Date = DateTime.Now;
                        pid.Is_Active = true;
                        pid.Is_Deleted = false;
                        pid.Account_BE = AccountDAL.GetAccountByID(pid.Account_ID, loggedinUser.PumpID);
                        pid.Product_BE = ProductDAL.GetProductByID(pid.Product_ID);
                        PurchaseInvoiceDetailDAL.Save(pid);


                        //update last purchase rate
                        ProductBE p = ProductDAL.GetProductByID(pid.Product_ID);

                        //add credit ledger entry in the account of purchaser
                        string desc = "Purchased " + pid.Quantity.ToString("0.00") + " " + p.Measure_Unit_BE.Name + " " + pid.Product_BE.Name + " @" + pid.Price.ToString("0.00") + " from " + pid.Account_BE.Name;

                        if (p != null)
                        {
                            p.Last_Purchase_Price = pid.Price;
                            ProductDAL.Save(p);
                            GeneralFunctions.AddEntryInProductLedger(p.ID, loggedinUser.PumpID, "Debit", pid.Quantity, pid.Invoice_ID, "Purchase", desc, pinvoicebe.Dated, pid.Vehicle_No, pinvoicebe.Reference_No);
                        }
                        //add debit ledger entry into purchases account. 
                        //get the purchase account for this user

                        if (purchaseAccount != null)
                        {
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, purchaseAccount.ID, "Debit", pid.Amount, pid.Invoice_ID, "Purchase", desc, pinvoicebe.Dated, pid.Vehicle_No, pinvoicebe.Reference_No);
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, pid.Account_ID, "Credit", pid.Amount, pid.Invoice_ID, "Purchase", desc, pinvoicebe.Dated, pid.Vehicle_No, pinvoicebe.Reference_No);
                            AccountDAL.updateAccountLedger(purchaseAccount.ID);
                            AccountDAL.updateAccountLedger(pid.Account_ID);
                        }

                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Purchase Successfully Updated",
                        InvoiceNo = invoiceid.ToString(),
                        InvoiceID = pinvoicebe.InvoiceNo.ToString(),
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
        public HttpResponseMessage GetPurchaseByID([FromBody] UserBE inParams)
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

                    PurchaseInvoiceHeadBE pInvoice = PurchaseInvoiceHeadDAL.GetPIHeadByID(inParams.ID);
                    if(pInvoice != null)
                    {
                        if(pInvoice.PumpID != currentUser.PumpID)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Invalid User Access."
                            });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invoice does not exists"
                        });
                    }
                    List<PurchaseInvoiceDetailBE> ListofPurchaseDetail = PurchaseInvoiceDetailDAL.GetPurchaseInvoiceDetailBEs(Convert.ToInt32(inParams.ID));
                    var reducedList = ListofPurchaseDetail.Select(e => new
                    {
                        e.ID,
                        e.Invoice_ID,
                        e.Product_ID,
                        e.Product_BE,
                        e.Account_ID,
                        e.Account_BE,
                        e.Price,
                        e.Quantity,
                        e.Is_Cash,
                        e.Vehicle_No,
                        e.Serial_No,
                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning Purchase",
                        ID = pInvoice.ID,
                        InvoiceNo = pInvoice.InvoiceNo,
                        PumpID = pInvoice.PumpID,
                        PumpCode = pInvoice.PumpCode,
                        Dated = pInvoice.Dated,
                        Cash_Total = pInvoice.Cash_Total,
                        Credit_Total = pInvoice.Credit_Total,
                        Description = pInvoice.Description,
                        AddedBy = pInvoice.AddedBy,
                        Reference_No= pInvoice.Reference_No,
                        listofPurchase = reducedList,
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
    }
}
