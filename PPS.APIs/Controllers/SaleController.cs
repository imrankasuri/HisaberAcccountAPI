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
    public class SaleController : ApiController
    {

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetSales([FromBody] GeneralRequestBE inParams)
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
                    #region PageNumberDetails
                    int PageNo = 1;
                    int PageSize = 10;
                    int totalRecords = 0;

                    if (inParams.PageNo > 0)
                    {
                        PageNo = Convert.ToInt32(inParams.PageNo);
                    }
                    if (inParams.PageSize > 0)
                    {
                        PageSize = Convert.ToInt32(inParams.PageSize);
                    }
                    if (PageNo == 0)
                    {
                        PageNo = 1;
                    }
                    if (PageSize == 0)
                    {
                        PageSize = 1000;
                    }
                    #endregion

                    UserBE currentUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    DateTime startDate = Convert.ToDateTime("2022-01-01");
                    DateTime endDate = DateTime.Now;
                  

                    if (inParams.StartDate != null && inParams.StartDate.Year > 1)
                    {
                        startDate = Convert.ToDateTime(inParams.StartDate);
                    }
                    if(inParams.EndDate != null && inParams.EndDate.Year > 1)
                    {
                        endDate = Convert.ToDateTime(inParams.EndDate);
                    }
                    if(inParams.PageNo > 0)
                    {
                        PageNo = Convert.ToInt32(inParams.PageNo);
                    }
                    
                    List<SaleInvoiceHeadBE> ListofSale = SaleInvoiceHeadDAL.GetSaleInvoiceHeadBEs(startDate, endDate, currentUser.PumpID);

                    totalRecords = ListofSale.Count;
                    decimal CashTotal = 0m;
                    decimal CreditTotal = 0m;
                    Decimal TotalSales = 0m;
                    if(ListofSale != null && ListofSale.Count > 0)
                    {
                        totalRecords = ListofSale.Count();
                        CashTotal = ListofSale.Sum(a => a.Cash_Total);
                        CreditTotal = ListofSale.Sum(a => a.Credit_Total);
                        TotalSales = ListofSale.Sum(a => a.TotalSales);
                    }

                    ListofSale = ListofSale.Skip((PageNo - 1) * PageSize).Take(PageSize).ToList<SaleInvoiceHeadBE>();
                    
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofSale = ListofSale,
                        CashTotal= CashTotal,
                        CreditTotal = CreditTotal,
                        TotalSales = TotalSales,
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
        public HttpResponseMessage GetSaleDetail([FromBody] UserBE inParams)
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

                    List<SaleInvoiceDetailBE> ListofSaleDetail = SaleInvoiceDetailDAL.GetSaleInvoiceDetailBEs(Convert.ToInt32(inParams.ID));
                    List<PumpReadingBE> listofReadings = PumpReadingDAL.GetPumpReadingByInvoiceID(Convert.ToInt32(inParams.ID));
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofSale = ListofSaleDetail,
                        listofReading = listofReadings,
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
        public HttpResponseMessage GetSaleByID([FromBody] UserBE inParams)
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

                    SaleInvoiceHeadBE sInvoice = SaleInvoiceHeadDAL.GetSIHeadByID(currentUser.PumpID, inParams.ID);
                    List<SaleInvoiceDetailBE> ListofSaleDetail = SaleInvoiceDetailDAL.GetSaleInvoiceDetailBEs(Convert.ToInt32(inParams.ID));
                    List<PumpReadingBE> ListofReadings = PumpReadingDAL.GetPumpReadingByInvoiceID(sInvoice.ID);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        ID = sInvoice.ID,
                        InvoiceNo = sInvoice.InvoiceNo,
                        PumpID = sInvoice.PumpID,
                        PumpCode = sInvoice.PumpCode,
                        Dated = sInvoice.Dated,
                        Cash_Total = sInvoice.Cash_Total,
                        Credit_Total = sInvoice.Credit_Total,
                        Description = sInvoice.Description,
                        AddedBy = sInvoice.AddedBy,
                        listofSale = ListofSaleDetail,
                        listofReadings = ListofReadings,
                    }) ;
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
        public HttpResponseMessage GetProductRate([FromBody] GeneralRequestBE inParams)
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

                    ProductBE p = ProductDAL.GetProductByID(Convert.ToInt32(inParams.ProductID));
                    if (p != null)
                    {
                        int CustomerID = Convert.ToInt32(inParams.AccountID);
                        CustomerRateBE crBE = CustomerRateDAL.GetCustomerRateByCustomerIDProductID(CustomerID, p.ID, currentUser.PumpID);
                        if (crBE != null)
                        {
                            if (crBE.Selling_Rate > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new
                                {
                                    status_code = 1,
                                    status_message = "Successfully returning Rate",
                                    Rate = crBE.Selling_Rate,
                                });
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new
                                {
                                    status_code = 0,
                                    status_message = "Invalid rate",
                                    Rate = 0M,
                                });
                            }
                        }
                        else
                        {
                            SaleInvoiceDetailBE sid = SaleInvoiceDetailDAL.GetSIDetailByProductID(Convert.ToInt32(inParams.ProductID));
                            if (sid != null)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new
                                {
                                    status_code = 1,
                                    status_message = "Successfully returning Rate",
                                    Rate = sid.Price,
                                });
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new
                                {
                                    status_code = 1,
                                    status_message = "Successfully returning Rate",
                                    Rate = p.Sale_Price,
                                });
                            }
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "No Product Exists",
                            Rate = 0M,
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
        public HttpResponseMessage UpdateCustomerRate([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey) && !String.IsNullOrEmpty(inParams.SalePrice))
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
                    ProductBE p = ProductDAL.GetProductByID(Convert.ToInt32(inParams.ProductID));
                    int productID = Convert.ToInt32(inParams.ProductID);
                    int CustomerID = Convert.ToInt32(inParams.AccountID);
                    decimal salePrice = Convert.ToDecimal(inParams.SalePrice);
                    AccountBE selectedCustomer = AccountDAL.GetAccountByID(CustomerID, currentUser.PumpID);

                    CustomerRateBE crBE = CustomerRateDAL.GetCustomerRateByCustomerIDProductID(CustomerID, productID, currentUser.PumpID);
                    if (crBE != null)
                    {
                        if (crBE.Selling_Rate != salePrice)
                        {
                            crBE.Selling_Rate = salePrice;
                            CustomerRateDAL.Save(crBE);
                        }
                    }
                    else
                    {
                        //save rate for this customer
                        CustomerRateBE crNewBE = new CustomerRateBE();
                        crNewBE.Product_ID = productID;
                        crNewBE.Created_Date = DateTime.Now;
                        crNewBE.Is_Deleted = false;
                        crNewBE.Is_Active = true;
                        crNewBE.Is_Deleted = false;
                        crNewBE.PumpID = currentUser.PumpID;
                        crNewBE.PumpCode = currentUser.PumpCode;
                        crNewBE.Selling_Rate = salePrice;
                        crNewBE.Updated_Date = DateTime.Now;
                        crNewBE.Customer_ID = CustomerID;
                        CustomerRateDAL.Save(crNewBE);
                    }
                    //if the select account is cash, then change price in main product table.
                    if (selectedCustomer.Account_Type_ID == 1)
                    {
                        if (p.Sale_Price != salePrice)
                        {
                            //change the price in main table. 
                            p.Sale_Price = salePrice;
                            ProductDAL.Save(p);
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Price Updated Successfully!",
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
        public HttpResponseMessage AddSale([FromBody] SaleInvoiceHeadBE inParams)
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
                  

                    foreach (SaleInvoiceDetailBE sid in inParams.ListofSaleDetails)
                    {
                        if(sid.Account_ID == cashAccount.ID)
                        {
                            sid.Is_Cash = true;
                        }
                    }

                    decimal cashTotal = inParams.ListofSaleDetails.Where(a => a.Is_Cash == true).Sum(a => a.Quantity * a.Price);
                    decimal creditTotal = inParams.ListofSaleDetails.Where(a => a.Is_Cash == false).Sum(a => a.Quantity * a.Price);

                    SaleInvoiceHeadBE sinvoicebe = new SaleInvoiceHeadBE();
                    sinvoicebe.PumpID = loggedinUser.PumpID;
                    sinvoicebe.PumpCode = loggedinUser.PumpCode;
                    //pinvoicebe.Reference_No = inParams.Reference_No;
                    sinvoicebe.Dated = inParams.Dated;
                    sinvoicebe.Description = inParams.Description;
                    sinvoicebe.Updated_Date = DateTime.Now;
                    sinvoicebe.Created_Date = DateTime.Now;
                    sinvoicebe.Is_Deleted = false;
                    sinvoicebe.Is_Active = true;
                    sinvoicebe.Cash_Total = cashTotal;
                    sinvoicebe.Credit_Total = creditTotal;
                    sinvoicebe.AddedBy = loggedinUser.User_Email;
                    sinvoicebe.InvoiceNo = SaleInvoiceHeadDAL.GetNextSaleInvoiceNumber(loggedinUser.PumpID); 
                    int invoiceid;
                    invoiceid = SaleInvoiceHeadDAL.Save(sinvoicebe);
                    sinvoicebe.ID = invoiceid;
                    //delete previous records first
                    SaleInvoiceDetailDAL.DeleteSaleInvoiceDetailByInvoice(invoiceid);
                    //save the invoice details.
                    
                    List<PumpReadingBE> ListofReading = inParams.ListofReadings;

                    AccountBE saleAccount = AccountDAL.GetAccountByName("Sales", loggedinUser.PumpID);

                    if(ListofReading != null && ListofReading.Count > 0)
                    {
                        foreach (PumpReadingBE prb in ListofReading)
                        {
                            ProductPumpBE ppBE = ProductPumpDAL.GetProductPumpBEByID(prb.PumpMachineID);
                            if(ppBE != null)
                            {
                                prb.Invoice_ID = invoiceid;
                                prb.Dated = sinvoicebe.Dated;
                                prb.Created_Date = DateTime.Now;
                                prb.Updated_Date = DateTime.Now;
                                prb.Is_Deleted = false;
                                prb.Is_Active = true;
                                prb.PumpID = loggedinUser.PumpID;
                                prb.PumpMachineID = ppBE.ID;
                                ProductPumpBE productPUmpBE = ProductPumpDAL.GetProductPumpBEByID(ppBE.ID);
                                prb.Pump_No = productPUmpBE.Pump_No;
                                prb.ProductCode = ppBE.Selected_Product.ProductCode;
                                prb.ProductName = ppBE.Selected_Product.Name;
                                PumpReadingDAL.Save(prb);
                            }
                            
                        }
                    }

                    foreach (SaleInvoiceDetailBE sid in inParams.ListofSaleDetails)
                    {
                        sid.Invoice_ID = invoiceid;
                        sid.PumpID = loggedinUser.PumpID;
                        sid.PumpCode = loggedinUser.PumpCode;
                        sid.Created_Date = DateTime.Now;
                        sid.Updated_Date = DateTime.Now;
                        sid.Is_Active = true;
                        sid.Is_Deleted = false;
                        sid.Account_BE = AccountDAL.GetAccountByID(sid.Account_ID, loggedinUser.PumpID);
                        SaleInvoiceDetailDAL.Save(sid);
                        //update last purchase rate
                        ProductBE p = ProductDAL.GetProductByID(sid.Product_ID);

                        //add credit ledger entry in the account of purchaser
                        string desc = "Sold " + sid.Quantity.ToString("0.00") + " " + p.Measure_Unit_BE.Name + " " + sid.Product_BE.Name + " " + p.Name + " @" + sid.Price.ToString("0.00") + " to " + sid.Account_BE.Name;

                        if (p != null)
                        {
                            if(p.Sale_Price != sid.Price)
                            {
                                p.Sale_Price = sid.Price;
                                ProductDAL.Save(p);
                            }
                            GeneralFunctions.AddEntryInProductLedger(p.ID, loggedinUser.PumpID, "Credit", sid.Quantity, sid.Invoice_ID, "Sale", desc, sinvoicebe.Dated, sid.Vehicle_No, sinvoicebe.ID.ToString());
                            ProductDAL.updateProductLedger(p.ID);
                        }
                        //add debit ledger entry into purchases account. 
                        //get the purchase account for this user

                        if (saleAccount != null)
                        {
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, saleAccount.ID, "Credit", sid.Amount, sid.Invoice_ID, "Sale", desc, sinvoicebe.Dated, sid.Vehicle_No, sinvoicebe.ID.ToString());
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, sid.Account_ID, "Debit", sid.Amount, sid.Invoice_ID, "Sale", desc, sinvoicebe.Dated, sid.Vehicle_No, sinvoicebe.ID.ToString());
                            AccountDAL.updateAccountLedger(saleAccount.ID);
                            AccountDAL.updateAccountLedger(sid.Account_ID);
                        }
                    }

                    
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Sale Successfully Saved",
                        InvoiceNo = sinvoicebe.InvoiceNo.ToString(),
                        InvoiceID = invoiceid.ToString(),
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
        public HttpResponseMessage GetNextSaleInvoiceNo([FromBody] UserBE inParams)
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
                    CompanyInfoBE companyInfo = CompanyInfoDAL.GetCompanyInfoByID(loggedinUser.PumpID);

                    string invoiceNo = SaleInvoiceHeadDAL.GetNextSaleInvoiceNumber(loggedinUser.PumpID).ToString();
                    List<ProductPumpBE> ListofProductPumps = ProductPumpDAL.GetProductPumpByPumpID(loggedinUser.PumpID);

                    List<PumpReadingBE> ListofReadings = new List<PumpReadingBE>();
                    int previousinvoiceno = SaleInvoiceHeadDAL.GetPreviousInvoiceNumber(loggedinUser.PumpID, Convert.ToInt32(invoiceNo));
                    List<PumpReadingBE> ListofPreviousReading = new List<PumpReadingBE>();
                    if (previousinvoiceno != 0)
                    {
                        SaleInvoiceHeadBE siPrevious = SaleInvoiceHeadDAL.GetSIHeadByInvoiceNo(loggedinUser.PumpID, previousinvoiceno);
                        ListofPreviousReading = PumpReadingDAL.GetPumpReadingByInvoiceID(siPrevious.ID).OrderBy(p => p.Pump_No).ToList();
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
                   
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning InvoiceNO",
                        NextInvoiceNo = invoiceNo,
                        ListofReading = ListofReadings.OrderBy(p => p.Pump_No).ToList(),
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
        public HttpResponseMessage DeleteSales([FromBody] GeneralRequestBE inParams)
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
                    SaleInvoiceHeadBE saleHead = SaleInvoiceHeadDAL.GetSIHeadByID(loggedInUser.PumpID, Convert.ToInt32(inParams.ID));
                   
                    if (saleHead != null)
                    {
                        int invoiceID = saleHead.ID;
                        AccountBE saleAccount = AccountDAL.GetAccountByPumpIDTypeID(loggedInUser.PumpID, 2);

                        if (saleHead.Created_Date < DateTime.Now.AddDays(-2))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Two Days Old Sale Can not be Deleted",
                            });
                        }
                        else
                        {
                            SaleInvoiceDetailDAL.DeleteCompleteSaleInvoice(invoiceID, loggedInUser.PumpID);

                            AccountDAL.updateAccountLedger(saleAccount.ID);
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 1,
                                status_message = "Sale Deleted Successfullly!",
                            });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Sale Not Found!",
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
        public HttpResponseMessage UpdateSale([FromBody] SaleInvoiceHeadBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));

                    AccountBE saleAccount = AccountDAL.GetAccountByName("Sales", loggedinUser.PumpID);
                    AccountBE cashAccount = AccountDAL.GetAccountByName("Cash", loggedinUser.PumpID);
                    int invoiceid = inParams.ID;

                    List<SaleInvoiceDetailBE> ListofsDetail = inParams.ListofSaleDetails;
                    foreach (SaleInvoiceDetailBE sid in ListofsDetail)
                    {
                        if (sid.Account_ID == cashAccount.ID)
                        {
                            sid.Is_Cash = true;
                        }
                    }

                    decimal cashTotal = inParams.ListofSaleDetails.Where(a => a.Is_Cash == true).Sum(a => a.Quantity * a.Price);
                    decimal creditTotal = inParams.ListofSaleDetails.Where(a => a.Is_Cash == false).Sum(a => a.Quantity * a.Price);

                    SaleInvoiceHeadBE sinvoicebe = SaleInvoiceHeadDAL.GetSIHeadByID(loggedinUser.PumpID, invoiceid);
                    sinvoicebe.Dated = inParams.Dated;
                    sinvoicebe.Description = inParams.Description;
                    sinvoicebe.Updated_Date = DateTime.Now;
                    sinvoicebe.Cash_Total = cashTotal;
                    sinvoicebe.Credit_Total = creditTotal;
                    invoiceid = SaleInvoiceHeadDAL.Save(sinvoicebe);
                    sinvoicebe.ID = invoiceid;
                    //delete previous records first
                    SaleInvoiceDetailDAL.DeleteEntryfromProductLedger(loggedinUser.PumpID, invoiceid, "Sale");
                    SaleInvoiceDetailDAL.DeleteSaleInvoiceDetailByInvoice(invoiceid);
                    SaleInvoiceDetailDAL.DeleteSalefromLedger(saleAccount.ID, invoiceid, "Sale");
                    PumpReadingDAL.DeletePumpReadingInvoice(invoiceid, loggedinUser.PumpID);

                    //save the invoice details.

                   
                    List<PumpReadingBE> ListofReading = inParams.ListofReadings;

                    if (ListofReading != null && ListofReading.Count > 0)
                    {
                        foreach (PumpReadingBE prb in ListofReading)
                        {
                            ProductPumpBE ppBE = ProductPumpDAL.GetProductPumpBEByID(prb.PumpMachineID);
                            if (ppBE != null)
                            {
                                prb.Invoice_ID = invoiceid;
                                prb.Dated = sinvoicebe.Dated;
                                prb.Created_Date = DateTime.Now;
                                prb.Updated_Date = DateTime.Now;
                                prb.Is_Deleted = false;
                                prb.Is_Active = true;
                                prb.PumpID = loggedinUser.PumpID;
                                prb.PumpMachineID = ppBE.ID;
                                ProductPumpBE productPUmpBE = ProductPumpDAL.GetProductPumpBEByID(ppBE.ID);
                                prb.Pump_No = productPUmpBE.Pump_No;
                                prb.ProductCode = ppBE.Selected_Product.ProductCode;
                                prb.ProductName = ppBE.Selected_Product.Name;
                                PumpReadingDAL.Save(prb);
                            }

                        }
                    }

                    foreach (SaleInvoiceDetailBE sid in ListofsDetail)
                    {
                        sid.Invoice_ID = invoiceid;
                        sid.PumpID = loggedinUser.PumpID;
                        sid.PumpCode = loggedinUser.PumpCode;
                        sid.Created_Date = DateTime.Now;
                        sid.Updated_Date = DateTime.Now;
                        sid.Is_Active = true;
                        sid.Is_Deleted = false;
                        sid.Account_BE = AccountDAL.GetAccountByID(sid.Account_ID, loggedinUser.PumpID);
                        SaleInvoiceDetailDAL.Save(sid);
                        //update last purchase rate
                        ProductBE p = ProductDAL.GetProductByID(sid.Product_ID);

                        //add credit ledger entry in the account of purchaser
                        string desc = "Sold " + sid.Quantity.ToString("0.00") + " " + p.Measure_Unit_BE.Name + " " + sid.Product_BE.Name + " @" + sid.Price.ToString("0.00") + " to " + sid.Account_BE.Name;

                        if (p != null)
                        {
                            if (p.Sale_Price != sid.Price)
                            {
                                p.Sale_Price = sid.Price;
                                ProductDAL.Save(p);
                            }
                            GeneralFunctions.AddEntryInProductLedger(p.ID, loggedinUser.PumpID, "Debit", sid.Quantity, sid.Invoice_ID, "Sale", desc, sinvoicebe.Dated, sid.Vehicle_No, sinvoicebe.ID.ToString());
                        }
                        //add debit ledger entry into purchases account. 
                        //get the purchase account for this user

                        if (saleAccount != null)
                        {
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, saleAccount.ID, "Credit", sid.Amount, sid.Invoice_ID, "Sale", desc, sinvoicebe.Dated, sid.Vehicle_No, sinvoicebe.ID.ToString());
                            GeneralFunctions.AddEntryInLedger(loggedinUser.PumpID, sid.Account_ID, "Debit", sid.Amount, sid.Invoice_ID, "Sale", desc, sinvoicebe.Dated, sid.Vehicle_No, sinvoicebe.ID.ToString());
                            AccountDAL.updateAccountLedger(saleAccount.ID);
                            AccountDAL.updateAccountLedger(sid.Account_ID);
                        }
                    }


                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Sale Successfully Updated",
                        InvoiceNo = sinvoicebe.InvoiceNo.ToString(),
                        InvoiceID = invoiceid.ToString(),
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
