using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace HAccounts.DAL
{
    public static class SaleInvoiceDetailDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(SaleInvoiceDetailBE  saleInvoiceDetailBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
               tblSaleInvoiceDetail clinq = null;
                
                clinq  = ConvertToLinqObject(saleInvoiceDetailBE);

                try
                {
                    // Update Updated Date
                   
                    if (saleInvoiceDetailBE.ID == 0)
                    {

                        context.tblSaleInvoiceDetails.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblSaleInvoiceDetails.Attach(clinq, true);
                    }

                    // Save changes to the database
                    context.SubmitChanges();

                    // Retrieve ID of saved object
                    result = clinq.ID;
                }
                catch (Exception ex)
                {
                    // pass error back to calling method
                    throw ex;
                }
                finally
                {
                    // Clean up
                    context.Dispose();
                }

                scope.Complete();
            }

            return result;
        }

        public static SaleInvoiceDetailBE GetSIDetailByID(int id)
        {
            // Declare variables
            SaleInvoiceDetailBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblSaleInvoiceDetails

                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.ID == id
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              }

                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }

            return result;
        }

        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailByPumpID(int pumpID)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceDetails
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Is_Active== true && objEntity.Is_Deleted== false && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              },
                              Sale_Invoice_Head_BE = new SaleInvoiceHeadBE()
                              {
                                  Dated = saleHead.Dated,
                              }
                          }).ToList<SaleInvoiceDetailBE>();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }

            return result;
        }

        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailByPumpDates(int pumpID, DateTime startDate, DateTime endDate)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceDetails
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Is_Active == true && objEntity.Is_Deleted == false && objEntity.PumpID == pumpID && saleHead.Dated >= startDate && saleHead.Dated < endDate.AddDays(1)
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  ProductCode = products.ProductCode,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              },
                              Sale_Invoice_Head_BE = new SaleInvoiceHeadBE()
                              {
                                  Dated = saleHead.Dated,
                              }
                          }).ToList<SaleInvoiceDetailBE>();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }

            return result;
        }
        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailBEs(int Invoice_Id)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceDetails 
                         
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Is_Active == true && objEntity.Is_Deleted == false && objEntity.Invoice_ID == Invoice_Id
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  ProductCode = products.ProductCode,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              }
                          }).ToList<SaleInvoiceDetailBE>();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }

            return result;
        }

        public static tblSaleInvoiceDetail ConvertToLinqObject(SaleInvoiceDetailBE objEntity)
        {
            // Declare variables
            tblSaleInvoiceDetail  result = new tblSaleInvoiceDetail();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.PumpCode = objEntity.PumpCode;
                result.Invoice_ID = objEntity.Invoice_ID;
                result.Product_ID = objEntity.Product_ID;
                result.Account_ID = objEntity.Account_ID;
                result.Quantity = objEntity.Quantity;
                result.Price = objEntity.Price;
                result.Is_Cash = objEntity.Is_Cash;
                result.Vehicle_No = objEntity.Vehicle_No;
                result.Receipt_No = objEntity.Receipt_No;
                result.Purchase_Price = objEntity.Purchase_Price;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;
                if (objEntity.TimeStamp != null)
                {
                    result.TimeStamp = new System.Data.Linq.Binary(Convert.FromBase64String(objEntity.TimeStamp.ToString()));
                }
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }

            return result;
        }

        public static void DeleteSaleInvoiceDetailByInvoice(int id)
        {
            List<SaleInvoiceDetailBE> listofSaleDetails = SaleInvoiceDetailDAL.GetSaleInvoiceDetailBEs(id);
            if(listofSaleDetails != null && listofSaleDetails.Count > 0)
            {
                foreach (SaleInvoiceDetailBE sbe in listofSaleDetails)
                {
                    int accountID = sbe.Account_ID;

                    DeleteSalefromLedger(sbe.Account_ID, sbe.Invoice_ID, "Sale");
                    //AccountDAL.updateAccountLedger(accountID);
                }
            }
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblSaleInvoiceDetails.DeleteAllOnSubmit(context.tblSaleInvoiceDetails.Where(c => c.Invoice_ID == id));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static void DeleteSaleInvoiceDetailByInvoiceID(int invoiceID, int pumpID)
        {
            int result = 0;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("DeleteSaleInvoiceDetail", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters.Add("@Invoice_ID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = invoiceID;
            try
            {
                sqlCon.Open();
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        public static void DeleteCompleteSaleInvoice(int invoiceID, int pumpID)
        {
            int result = 0;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("DeleteSaleInvoice", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters.Add("@Invoice_ID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = invoiceID;
            try
            {
                sqlCon.Open();
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        public static void DeleteSaleInvoiceDetailByID(int id)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SaleInvoiceDetailBE sbe = SaleInvoiceDetailDAL.GetSIDetailByID(id);

                    //DeleteSalefromLedger(sbe.Account_ID, sbe.Invoice_ID, "Sale");
                    context.tblGeneralLedgers.DeleteAllOnSubmit(context.tblGeneralLedgers.Where(c => c.Account_ID == sbe.Account_ID && c.Reference_No == sbe.Invoice_ID && c.Reference_Type == "Sale"));
                    //AccountDAL.updateAccountLedger(sbe.Account_ID);
                    context.tblSaleInvoiceDetails.DeleteAllOnSubmit(context.tblSaleInvoiceDetails.Where(c => c.ID == id));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static void DeleteSalefromLedger(int accountID, int referenceNo, string referenceType)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblGeneralLedgers.DeleteAllOnSubmit(context.tblGeneralLedgers.Where(c => c.Account_ID == accountID && c.Reference_No == referenceNo && c.Reference_Type == referenceType));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static void DeleteEntryfromProductLedger(int pumpID, int productID, int referenceID, string referenceType)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblProductLedgers.DeleteAllOnSubmit(context.tblProductLedgers.Where(c => c.PumpID == pumpID && c.Product_ID == productID && c.Reference_Type == referenceType && c.Reference_ID == referenceID));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static void DeleteEntryfromProductLedger(int pumpID, int referenceID, string referenceType)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblProductLedgers.DeleteAllOnSubmit(context.tblProductLedgers.Where(c => c.PumpID == pumpID && c.Reference_Type == referenceType && c.Reference_ID == referenceID));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailBEs(int accountid, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceDetails
                          
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Account_ID == accountid && saleHead.Dated >= startdate && saleHead.Dated <= enddate
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              }
                          }).ToList<SaleInvoiceDetailBE>();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }
            if (accountid != 1)
            {
                result = result.Where(p => p.Is_Cash == false).ToList();
            }
            return result;
        }

        public static SaleInvoiceDetailBE GetSIDetailByProductID(int product_id)
        {
            // Declare variables
            SaleInvoiceDetailBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblSaleInvoiceDetails
                          
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Product_ID == product_id && accounts.Account_Type_ID == 1 && objEntity.Is_Deleted== false && objEntity.Is_Active == true 
                          orderby objEntity.ID descending
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              }
                          }).ToList<SaleInvoiceDetailBE>().Take(1).SingleOrDefault();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }

            return result;
        }

        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailbyProductID(int productid, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceDetails
                         
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Product_ID == productid && saleHead.Dated >= startdate && saleHead.Dated <= enddate
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Sale_Invoice_Head_BE = new SaleInvoiceHeadBE()
                              {
                                  Dated = saleHead.Dated,
                              },
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              }
                          }).ToList<SaleInvoiceDetailBE>();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }
            
            return result;
        }

        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailByPumpID(int pumpID, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceDetails
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where saleHead.Dated >= startdate && saleHead.Dated < enddate.AddDays(1) && saleHead.PumpID == pumpID
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_ID =accounts.Account_Type_ID,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accounts.Account_Type_ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  ProductCode = products.ProductCode,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              },
                              Sale_Invoice_Head_BE = new SaleInvoiceHeadBE()
                              {
                                  Dated = saleHead.Dated,
                                  InvoiceNo = saleHead.InvoiceNo,
                              }
                          }).ToList<SaleInvoiceDetailBE>();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }
            
            return result;
        }

        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailBEs(int pumpID, DateTime saleDate, Boolean isCash)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                result = (from objEntity in context.tblSaleInvoiceDetails
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where saleHead.Dated == saleDate && objEntity.Is_Cash == isCash && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              Product_ID = objEntity.Product_ID,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accountType.ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              },
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                          }).ToList<SaleInvoiceDetailBE>();

            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }

            return result;
        }

        public static List<SaleInvoiceDetailBE> GetSaleInvoiceDetailBEs(int pumpID, DateTime saleDate)
        {
            // Declare variables
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceDetails
                          join saleHead in context.tblSaleInvoiceHeads
                          on objEntity.Invoice_ID equals saleHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID

                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID

                          where saleHead.Dated == saleDate  && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new SaleInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              Product_ID = objEntity.Product_ID,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Purchase_Price = objEntity.Purchase_Price,
                              Account_BE = new AccountBE()
                              {
                                  ID = objEntity.Account_ID,
                                  Name = accounts.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = accountType.ID,
                                      Name = accountType.Name
                                  },
                              },
                              Product_BE = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = products.Name,
                                  Measure_Unit_BE = new MeasureUnitBE()
                                  {
                                      ID = products.MeasureUnitID,
                                      Name = measureUnit.Name
                                  }
                              },
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                          }).ToList<SaleInvoiceDetailBE>();
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }
            finally
            {
                // Clean up
                context.Dispose();
            }

            return result;
        }

        public static List<SaleInvoiceDetailBE> GetProductSaleByPumpID(int pumpID, DateTime selectedDate)
        {
            List<SaleInvoiceDetailBE> result = new List<SaleInvoiceDetailBE>();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetProductSaleByPumpID", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters.Add("@Dated", SqlDbType.Date);
            cmd.Parameters[1].Value = selectedDate.Date;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    SaleInvoiceDetailBE n = new SaleInvoiceDetailBE();
                    n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Quantity = Convert.ToDecimal(dr["Quantity"]);
                    n.Price = Convert.ToDecimal(dr["Price"]);
                    n.Sale_Invoice_Head_BE = new SaleInvoiceHeadBE();
                    n.Sale_Invoice_Head_BE.Dated = Convert.ToDateTime(dr["Dated"]);
                    n.Product_BE = new ProductBE();
                    n.Product_BE.ProductCode = Convert.ToString(dr["ProductCode"]);
                    n.Product_BE.Name = Convert.ToString(dr["Name"]);
                    result.Add(n);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                sqlCon.Close();
            }
            return result;
        }
    }
}
