using HAccounts.APIs.Models;
using HAccounts.APIs.Utils;
using HAccounts.BE;
using HAccounts.DAL;
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
    public class ProductController : ApiController
    {
        //#region Member Sign Up

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetMeasureUnit([FromBody] UserBE inParams)
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

                    List<MeasureUnitBE> listofAccountType = MeasureUnitDAL.GetMeasureUnitBEs();
                    var reducedList = listofAccountType.Select(e => new { e.ID, e.Name }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofUnits = reducedList,
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
        public HttpResponseMessage AddNewProduct([FromBody] ProductBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(inParams.UserID);
                    ProductBE productBE = new ProductBE();
                    ProductBE productAlreadyCode = ProductDAL.GetProductByPumpIDProductCode(loggedinUser.PumpID, inParams.ProductCode);
                    if (productAlreadyCode != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "This Product Code already Exists."
                        });
                    }
                    ProductBE productAlreadyName = ProductDAL.GetProductByPumpIDProductName(loggedinUser.PumpID, inParams.Name);
                    if (productAlreadyName != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "This Product Name already Exists."
                        });
                    }

                    productBE.ProductCode = inParams.ProductCode;
                    productBE.PumpID = loggedinUser.PumpID;
                    productBE.Name = inParams.Name;
                    productBE.MeasureUnitID = inParams.MeasureUnitID;
                    productBE.Description = inParams.Description;
                    productBE.Sale_Price = Convert.ToDecimal(inParams.Sale_Price);
                    productBE.Last_Purchase_Price = Convert.ToDecimal(inParams.Last_Purchase_Price);
                    productBE.Is_Default = false;
                    productBE.Is_Active = true;
                    productBE.Is_Deleted = false;
                    productBE.Updated_Date = DateTime.Now;
                    productBE.Created_Date = DateTime.Now;
                    int productID = ProductDAL.Save(productBE);
                    if (productID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Product Added Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to add product!",
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
        public HttpResponseMessage UpdateProduct([FromBody] ProductBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(inParams.UserID);
                    ProductBE productBE = ProductDAL.GetProductByID(Convert.ToInt32(inParams.ID));
                    if(productBE.PumpID != loggedinUser.PumpID)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Product Can not be Updated."
                        });
                    }
                    //if(productBE.Is_Default == true)
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.OK, new
                    //    {
                    //        status_code = 0,
                    //        status_message = "Default Product Can not be Updated."
                    //    });
                    //}
                    productBE.PumpID = loggedinUser.PumpID;
                    productBE.Description = inParams.Description;
                    productBE.Sale_Price = Convert.ToDecimal(inParams.Sale_Price);
                    productBE.Last_Purchase_Price = Convert.ToDecimal(inParams.Last_Purchase_Price);
                    productBE.Is_Default = false;
                    productBE.Is_Active = true;
                    productBE.Is_Deleted = false;
                    productBE.Updated_Date = DateTime.Now;
                    productBE.Created_Date = DateTime.Now;
                    int productID = ProductDAL.Save(productBE);
                    if (productID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Product Updated Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Update product!",
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
        public HttpResponseMessage GetProducts([FromBody] UserBE inParams)
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

                    List<ProductBE> ListofProducts = ProductDAL.GetProductsByPumpID(currentUser.PumpID);
                    decimal TotalStockValueDebit = ListofProducts.Where(a=> a.BalanceType == "Debit").Sum(a => a.Balance * a.Last_Purchase_Price);
                    decimal TotalStockValueCredit = ListofProducts.Where(a => a.BalanceType == "Credit").Sum(a => a.Balance * a.Last_Purchase_Price);

                    var reducedList = ListofProducts.Select(e => new { e.ID, e.ProductCode, e.Name, e.Balance, e.BalanceType, e.Description, e.Last_Purchase_Price, e.Sale_Price, e.MeasureUnitID, e.Measure_Unit_BE,  e.PumpID }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        ListofProducts = ListofProducts,
                        TotalStockValue = TotalStockValueDebit-TotalStockValueCredit,
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
        public HttpResponseMessage GetProductByID([FromBody] UserBE inParams)
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
                    int productID = inParams.ID;

                    ProductBE Products = ProductDAL.GetProductByID(productID);
                    if (Products != null)
                    {
                        if(Products.PumpID != currentUser.PumpID)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                status_code = 0,
                                status_message = "Invalid Product",
                            });
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning Product",
                        Product = Products,
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
        public HttpResponseMessage GetProductLedger([FromBody] UserBE inParams)
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

                    int productID = Convert.ToInt32(inParams.ID);
                    ProductBE ac = ProductDAL.GetProductByID(productID, currentUser.PumpID);
                    if (ac == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Sorry, Invalid Product ID."
                        });
                    }

                    int pageSize = 5;
                    List<ProductLedgerBE> ListofLedger = ProductLedgerDAL.GetProductLedger(productID, startDate, endDate);
                    ListofLedger = ListofLedger.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<ProductLedgerBE>();

                    var reducedList = ListofLedger.Select(e => new { e.ID, e.Product_ID, e.Transaction_Date, e.Description, e.Receipt_No, e.Reference_Type, e.Reference_ID, e.Vehicle_No, e.Debit, e.Credit, e.Balance, e.BalanceType }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofProductLedger = reducedList,
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
        public HttpResponseMessage AddNewNozzle([FromBody] ProductPumpBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(inParams.UserID);
                    ProductPumpBE pumpNozzle = new ProductPumpBE();
                    ProductPumpBE productAlreadyCode = ProductPumpDAL.GetProductPumpByPumpNumber(inParams.Pump_No, loggedinUser.PumpID);
                    if (productAlreadyCode != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "This Nozzle Number already Exists."
                        });
                    }
                    

                    pumpNozzle.Product_ID = inParams.Product_ID;
                    pumpNozzle.PumpID = loggedinUser.PumpID;
                    pumpNozzle.Pump_No = inParams.Pump_No;
                    pumpNozzle.Is_Active = true;
                    pumpNozzle.Is_Deleted = false;
                    pumpNozzle.Updated_Date = DateTime.Now;
                    pumpNozzle.Created_Date = DateTime.Now;
                    int productID = ProductPumpDAL.Save(pumpNozzle);
                    if (productID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Machine Nozzle Added Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to add Nozzle!",
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
        public HttpResponseMessage GetPumpNozzls([FromBody] UserBE inParams)
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

                    List<ProductPumpBE> ListofProducts = ProductPumpDAL.GetProductPumpByPumpID(currentUser.PumpID);
                   if(ListofProducts != null && ListofProducts.Count > 0)
                    {
                        ListofProducts = ListofProducts.OrderBy(a => a.Pump_No).ToList();
                    }
                    var reducedList = ListofProducts.Select(e => new { e.ID, e.Pump_No, e.Product_ID, e.Selected_Product.Name, e.PumpID }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        ListofNozzels = ListofProducts,
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
        public HttpResponseMessage GetReadings([FromBody] UserBE inParams)
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

                    int pumpMachineID = Convert.ToInt32(inParams.ID);
                    ProductPumpBE ppmID = ProductPumpDAL.GetProductPumpBEByID(pumpMachineID);
                    if (ppmID == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Sorry, Invalid Pump Machine ID."
                        });
                    }
                    if(ppmID.PumpID != currentUser.PumpID)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Sorry, Invalid Pump Machine ID."
                        });
                    }
                    int pageSize = 50;
                    List<PumpReadingBE> ListofLedger = PumpReadingDAL.GetPumpReadingBEs(currentUser.PumpID, pumpMachineID, startDate, endDate);

                    ListofLedger = ListofLedger.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<PumpReadingBE>();

                    var reducedList = ListofLedger.Select(e => new { e.ID, e.PumpMachineID, e.Pump_No, e.ProductCode, e.Dated, e.Last_Reading, e.Current_Reading, e.Returned, e.UsedQuantity, e.Invoice_ID, e.Created_Date }).ToList();

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

        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddNewTank([FromBody] TankDefinationBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(inParams.UserID);
                    TankDefinationBE tankDef = new TankDefinationBE();
                    TankDefinationBE tankAlready = TankDefinationDAL.GetTankByTankNumber(inParams.TankNo, loggedinUser.PumpID);
                    if (tankAlready != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "This Tank Number already Exists."
                        });
                    }

                    tankDef.PumpID = loggedinUser.PumpID;
                    tankDef.TankNo = inParams.TankNo;
                    tankDef.ProductID = inParams.ProductID;
                    tankDef.TankFullCapacity = inParams.TankFullCapacity;
                    tankDef.UseableCapacity = inParams.UseableCapacity;
                    tankDef.TankSizeDetails = inParams.TankSizeDetails;
                    tankDef.TankShape = inParams.TankShape;
                    tankDef.Is_Active = true;
                    tankDef.Is_Deleted = false;
                    tankDef.Updated_Date = DateTime.Now;
                    tankDef.Created_Date = DateTime.Now;
                    int tankID = TankDefinationDAL.Save(tankDef);
                    if (tankID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Tank Added Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to add Tank!",
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
        public HttpResponseMessage GetTanks([FromBody] UserBE inParams)
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

                    List<TankDefinationBE> listofTanks = TankDefinationDAL.GetTankDefinationsBEs(currentUser.PumpID);
                    if (listofTanks != null && listofTanks.Count > 0)
                    {
                        listofTanks = listofTanks.OrderBy(a => a.TankNo).ToList();
                    }
                    var reducedList = listofTanks.Select(e => new { e.ID, e.PumpID, e.TankNo, e.ProductID, e.SelectedProduct.Name,  e.TankFullCapacity, e.UseableCapacity, e.TankSizeDetails, e.TankShape }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        ListofTanks = listofTanks,
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
        public HttpResponseMessage UpdateTank([FromBody] TankDefinationBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(inParams.UserID);
                    TankDefinationBE tankDef = TankDefinationDAL.GetTankDefinationByID(inParams.ID, loggedinUser.PumpID);
                    if (tankDef == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Tank Does not exists."
                        });
                    }

                    tankDef.PumpID = loggedinUser.PumpID;
                    tankDef.TankNo = inParams.TankNo;
                    tankDef.ProductID = inParams.ProductID;
                    tankDef.TankFullCapacity = inParams.TankFullCapacity;
                    tankDef.UseableCapacity = inParams.UseableCapacity;
                    tankDef.TankSizeDetails = inParams.TankSizeDetails;
                    tankDef.TankShape = inParams.TankShape;
                    tankDef.Is_Active = true;
                    tankDef.Is_Deleted = false;
                    tankDef.Updated_Date = DateTime.Now;
                    tankDef.Created_Date = DateTime.Now;
                    int tankID = TankDefinationDAL.Save(tankDef);
                    if (tankID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Tank Updated Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Update Tank!",
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
        public HttpResponseMessage UpdateNozzle([FromBody] ProductPumpBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(inParams.UserID);
                    ProductPumpBE pumpNozzle = ProductPumpDAL.GetProductPumpBEByID(Convert.ToInt32(inParams.ID));
                    if(pumpNozzle == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "This Nozzle Number does not Exists."
                        });
                    }
                   if(pumpNozzle.PumpID != loggedinUser.PumpID)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "This Nozzle Number does not Exists."
                        });
                    }

                    pumpNozzle.Product_ID = inParams.Product_ID;
                    pumpNozzle.PumpID = loggedinUser.PumpID;
                    pumpNozzle.Pump_No = inParams.Pump_No;
                    pumpNozzle.Is_Active = true;
                    pumpNozzle.Is_Deleted = false;
                    pumpNozzle.Updated_Date = DateTime.Now;
                    pumpNozzle.Created_Date = DateTime.Now;
                    int productID = ProductPumpDAL.Save(pumpNozzle);
                    if (productID > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Machine Nozzle Updated Successfully!",
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Failed to Update Nozzle!",
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
        public HttpResponseMessage DeleteNozzle([FromBody] GeneralRequestBE inParams)
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
                    ProductPumpBE productPump = ProductPumpDAL.GetProductPumpBEByID(Convert.ToInt32(inParams.ID));

                    if (productPump != null)
                    {
                        ProductPumpDAL.DeleteNozzle(loggedInUser.PumpID, productPump.ID);

                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Nozzle Deleted Successfully",
                        });


                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Nozzle Not Found!",
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
        public HttpResponseMessage GetPumpMachineByID([FromBody] GeneralRequestBE inParams)
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
                    ProductPumpBE productPump = ProductPumpDAL.GetProductPumpBEByID(Convert.ToInt32(inParams.ID));
                    if(productPump.PumpID != loggedInUser.PumpID)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Invalid Request."
                        });
                    }
                    if (productPump != null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 1,
                            status_message = "Returning Pump Machine/ Nozzle Successfully",
                            PumpMachine = productPump, 
                        });


                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            status_code = 0,
                            status_message = "Nozzle Not Found!",
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
        public HttpResponseMessage AdjustDIP([FromBody] AdjustDIPBE inParams)
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
                    UserBE loggedinUser = UserDAL.GetUserBEByID(inParams.UserID);
                    List<ProductBE> ListofProducts = ProductDAL.GetProductsByPumpID(loggedinUser.PumpID);

                    AdjustDIPBE adb = new AdjustDIPBE();
                    adb.Date = inParams.Date;
                    adb.ListofProductDIP = new List<ProductDIP>();
                    if (inParams.ListofProductDIP != null && inParams.ListofProductDIP.Count > 0)
                    {
                        foreach (ProductDIP pdip in inParams.ListofProductDIP)
                        {
                            ProductBE selectedProduct = ListofProducts.Where(a => a.ID == pdip.ProductID).FirstOrDefault();

                            decimal totalQTY = 0;

                            foreach (DIPReading rptTankItems in pdip.ListofDipReading)
                            {
                                DipReadingBE dp = new DipReadingBE();
                                dp.Created_Date = DateTime.Now;
                                dp.Dated = inParams.Date;
                                dp.DIP = Convert.ToDecimal(rptTankItems.Reading);
                                dp.IsPosted = false;
                                dp.Is_Active = true;
                                dp.Is_Deleted = false;
                                dp.ProductCode = selectedProduct.ProductCode;
                                dp.ProductID = selectedProduct.ID;
                                dp.ProductName = selectedProduct.Name;
                                dp.PumpID = loggedinUser.PumpID;
                                dp.StockLtr = Convert.ToDecimal(rptTankItems.StockInLtr);
                                dp.TankID = Convert.ToInt32(rptTankItems.TankID);
                                TankDefinationBE tnkDef = TankDefinationDAL.GetTankDefinationByID(rptTankItems.TankID, loggedinUser.PumpID);
                                dp.TankNo = Convert.ToInt32(tnkDef.TankNo);
                                dp.Updated_Date = DateTime.Now;
                                DipReadingDAL.Save(dp);
                                totalQTY = totalQTY + dp.StockLtr;
                            }
                            DIPAdjustmentBE dipAdj = new DIPAdjustmentBE();
                            dipAdj.PumpID = loggedinUser.PumpID;
                            dipAdj.ProductID = selectedProduct.ID;
                            dipAdj.TotalPhysicalStock = pdip.ListofDipReading.Sum(a => a.StockInLtr);

                            dipAdj.TotalSystemStock = selectedProduct.Balance;
                            dipAdj.AdjustmentRate = pdip.AdjustmentRate;
                            if (dipAdj.TotalPhysicalStock > dipAdj.TotalSystemStock)
                            {
                                dipAdj.DifferenceQuantity = dipAdj.TotalPhysicalStock - dipAdj.TotalSystemStock;
                            }
                            else
                            {
                                dipAdj.DifferenceQuantity = dipAdj.TotalPhysicalStock - dipAdj.TotalSystemStock;
                            }

                            dipAdj.IsPosted = true;
                            dipAdj.Is_Active = true;
                            dipAdj.Is_Deleted = false;
                            dipAdj.Created_Date = DateTime.Now;
                            dipAdj.Updated_Date = DateTime.Now;
                            dipAdj.Dated = inParams.Date;
                            int idadjusted = DipAdjustmentDAL.Save(dipAdj);
                            dipAdj.ID = idadjusted;
                            //add entry in stock ledger
                            ProductLedgerBE pl = new ProductLedgerBE();
                            pl.Product_ID = dipAdj.ProductID;
                            pl.Created_Date = DateTime.Now;
                            if (dipAdj.DifferenceQuantity > 0)
                            {
                                pl.Debit = dipAdj.DifferenceQuantity;
                                pl.Description = "Stock Increased during Dip";
                                pl.BalanceType = "Debit";
                            }
                            else
                            {
                                pl.Credit = (dipAdj.DifferenceQuantity * -1);
                                pl.Description = "Stock Decreased during Dip";
                                pl.BalanceType = "Credit";
                            }
                            pl.Is_Active = true;
                            pl.Is_Deleted = false;
                            pl.PumpID = loggedinUser.PumpID;
                            pl.Reference_ID = dipAdj.ID;
                            pl.Reference_Type = "DIP";
                            pl.Transaction_Date = dipAdj.Dated;
                            pl.Updated_Date = DateTime.Now;
                            ProductLedgerDAL.Save(pl);
                            //dipAdj.IsPosted = true;
                            //DipAdjustmentDAL.Save(dipAdj);
                        }

                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "DIP Posted Successfully!",
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
        public HttpResponseMessage GetDipReadings([FromBody] UserBE inParams)
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
                    List<ListofDates> listofDates = new List<ListofDates>();
                    List<DIPAdjustmentBE> adjustmentsList = DipAdjustmentDAL.GetDipAdjustmentBEs(currentUser.PumpID, startDate,endDate);
                    if(adjustmentsList != null && adjustmentsList.Count > 0)
                    {
                        foreach (DIPAdjustmentBE item in adjustmentsList)
                        {
                            ListofDates lst = new ListofDates();
                            lst.Date = item.Dated;
                            if(listofDates.Where(a=> a.Date == lst.Date).FirstOrDefault() == null)
                            {
                                listofDates.Add(lst);
                            }
                            
                        }
                    }

                    
                    //if(endDate > startDate)
                    //{
                    //    while (startDate < endDate)
                    //    {
                    //        ListofDates lst = new ListofDates();
                    //        lst.Date = startDate;
                    //        startDate = startDate.AddDays(1);
                    //        listofDates.Add(lst);
                    //    }
                    //}
                    int i = 1;
                    if(listofDates != null && listofDates.Count > 0)
                    {
                        foreach (ListofDates lst in listofDates)
                        {
                            lst.SerialNo = i;
                            lst.ListofDips = new List<ProductDIP>();

                            List<DIPAdjustmentBE> listofDipAdjustment = DipAdjustmentDAL.GetDipAdjustmentBEs(currentUser.PumpID, lst.Date);
                            List<DipReadingBE> lsitofDIPReadings = DipReadingDAL.GetDipReadingBEs(currentUser.PumpID, lst.Date);
                            if(listofDipAdjustment != null && listofDipAdjustment.Count > 0)
                            {
                                foreach (DIPAdjustmentBE dp in listofDipAdjustment)
                                {
                                    ProductDIP productDIP = new ProductDIP();
                                    productDIP.AdjustmentRate = dp.AdjustmentRate;
                                    productDIP.DifferenceQuantity = dp.DifferenceQuantity;
                                    productDIP.TotalSystemStock = dp.TotalSystemStock;
                                    productDIP.TotalPhysicalStock = dp.TotalPhysicalStock;
                                    productDIP.ProductID = dp.ProductID;
                                    ProductBE selectedProduct = ProductDAL.GetProductByID(dp.ProductID);
                                    productDIP.ProductCode = selectedProduct.ProductCode;
                                    productDIP.ProductName = selectedProduct.Name;
                                    productDIP.ListofDipReading = new List<DIPReading>();
                                    if(lsitofDIPReadings != null && lsitofDIPReadings.Count > 0)
                                    {
                                        List<DipReadingBE> lsitofProductDIPReadings = lsitofDIPReadings.Where(a => a.ProductID == dp.ProductID).ToList();
                                        foreach (DipReadingBE pumpReadingBE in lsitofProductDIPReadings)
                                        {
                                            DIPReading dr = new DIPReading();
                                            dr.Reading = pumpReadingBE.DIP;
                                            dr.StockInLtr = pumpReadingBE.StockLtr;
                                            dr.TankID = pumpReadingBE.TankID;
                                            dr.TankNo = pumpReadingBE.TankNo;
                                            productDIP.ListofDipReading.Add(dr);
                                        }
                                    }
                                    lst.ListofDips.Add(productDIP);
                                }
                            }
                            i = i + 1;

                        }
                    }

                    int pageSize = 5;
                    if(listofDates != null && listofDates.Count > 0)
                    {
                        listofDates = listofDates.Where(a => a.ListofDips.Count > 0).ToList();
                        //reset serial number
                        if(listofDates != null && listofDates.Count > 0)
                        {
                            i = 1;
                            foreach (ListofDates item in listofDates)
                            {
                                item.SerialNo = i;
                                i = i + 1;
                            }
                        }
                    }
                    listofDates = listofDates.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList<ListofDates>();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning list",
                        listofDates = listofDates,
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
        public HttpResponseMessage DeleteDIPRecord([FromBody] GeneralRequestBE inParams)
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

                    ProductDAL.DeleteDIPRecord(loggedInUser.PumpID, inParams.Dated);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "DIP Deleted Successfully",
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
