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
    public static class PurchaseInvoiceDetailDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(PurchaseInvoiceDetailBE  purchaseInvoiceDetailBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
               tblPurchaseInvoiceDetail clinq = null;
                
                clinq  = ConvertToLinqObject(purchaseInvoiceDetailBE);

                try
                {
                    // Update Updated Date
                    
                    if (purchaseInvoiceDetailBE.ID == 0)
                    {
                       
                        context.tblPurchaseInvoiceDetails.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblPurchaseInvoiceDetails.Attach(clinq, true);
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

        public static PurchaseInvoiceDetailBE GetPIDetailByID(int id)
        {
            // Declare variables
            PurchaseInvoiceDetailBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblPurchaseInvoiceDetails

                          join purchaseHead in context.tblPurchaseInvoiceHeads
                          on objEntity.Invoice_ID equals purchaseHead.ID
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
                          select new PurchaseInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updatedby = objEntity.Updatedby,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
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

        public static List<PurchaseInvoiceDetailBE> GetPurchaseInvoiceDetailBEsbyPumpID(int pumpID)
        {
            // Declare variables
            List<PurchaseInvoiceDetailBE> result = new List<PurchaseInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceDetails

                          join purchaseHead in context.tblPurchaseInvoiceHeads
                         on objEntity.Invoice_ID equals purchaseHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new PurchaseInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updatedby = objEntity.Updatedby,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
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
                              Purchase_InvoiceHead_BE = new PurchaseInvoiceHeadBE()
                              {
                                  Dated = purchaseHead.Dated,
                              }

                          }).ToList<PurchaseInvoiceDetailBE>();
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

        public static List<PurchaseInvoiceDetailBE> GetPurchaseInvoiceDetailBEs(int Invoice_Id)
        {
            // Declare variables
            List<PurchaseInvoiceDetailBE> result = new List<PurchaseInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceDetails 
                         
                          join purchaseHead in context.tblPurchaseInvoiceHeads
                         on objEntity.Invoice_ID equals purchaseHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Invoice_ID == Invoice_Id
                          orderby objEntity.ID
                          select new PurchaseInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updatedby = objEntity.Updatedby,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
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

                          }).ToList<PurchaseInvoiceDetailBE>();
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

        public static tblPurchaseInvoiceDetail ConvertToLinqObject(PurchaseInvoiceDetailBE objEntity)
        {
            // Declare variables
            tblPurchaseInvoiceDetail  result = new tblPurchaseInvoiceDetail();

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
                result.Updatedby = objEntity.Updatedby;
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

        public static void DeletePurchaseInvoiceDetailByInvoice(int id)
        {
            List<PurchaseInvoiceDetailBE> listofPurchaseDetail = PurchaseInvoiceDetailDAL.GetPurchaseInvoiceDetailBEs(id);
            if (listofPurchaseDetail != null && listofPurchaseDetail.Count > 0)
            {
                foreach (PurchaseInvoiceDetailBE sbe in listofPurchaseDetail)
                {
                    int accountID = sbe.Account_ID;

                    DeletePurchasefromLedger(sbe.Account_ID, sbe.Invoice_ID, "Purchase");
                    AccountDAL.updateAccountLedger(accountID);
                }
            }


            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblPurchaseInvoiceDetails.DeleteAllOnSubmit(context.tblPurchaseInvoiceDetails.Where(c => c.Invoice_ID == id));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static void DeletePurchasefromLedger(int accountID, int referenceNo, string referenceType)
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
                    context.tblProductLedgers.DeleteAllOnSubmit(context.tblProductLedgers.Where(c => c.PumpID == pumpID  && c.Reference_Type == referenceType && c.Reference_ID == referenceID));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static List<PurchaseInvoiceDetailBE> GetPurchaseInvoiceDetailBEs(int account_id, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<PurchaseInvoiceDetailBE> result = new List<PurchaseInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceDetails
                          join purchaseHead in context.tblPurchaseInvoiceHeads
                          on objEntity.Invoice_ID equals purchaseHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Account_ID == account_id && purchaseHead.Dated >= startdate && purchaseHead.Dated <= enddate
                          orderby objEntity.ID
                          select new PurchaseInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updatedby = objEntity.Updatedby,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
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

                          }).ToList<PurchaseInvoiceDetailBE>();
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
            if (account_id != 1)
            {
                result = result.Where(p=> p.Is_Cash == false).ToList();
            }
            return result;
        }

        public static List<PurchaseInvoiceDetailBE> GetPurchaseInvoiceDetailByProduct(int product_id, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<PurchaseInvoiceDetailBE> result = new List<PurchaseInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceDetails
                          join purchaseHead in context.tblPurchaseInvoiceHeads
                          on objEntity.Invoice_ID equals purchaseHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.Product_ID == product_id && purchaseHead.Dated >= startdate && purchaseHead.Dated <= enddate
                          orderby objEntity.ID
                          select new PurchaseInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date= objEntity.Created_Date,
                              Is_Active= objEntity.Is_Active,
                              Is_Deleted= objEntity.Is_Deleted,
                              Updatedby= objEntity.Updatedby,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
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

                          }).ToList<PurchaseInvoiceDetailBE>();
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

        public static List<PurchaseInvoiceDetailBE> GetPurchaseInvoiceDetailByPump(int pumpID, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<PurchaseInvoiceDetailBE> result = new List<PurchaseInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceDetails
                          join purchaseHead in context.tblPurchaseInvoiceHeads
                          on objEntity.Invoice_ID equals purchaseHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where objEntity.PumpID == pumpID && purchaseHead.Dated >= startdate && purchaseHead.Dated <= enddate
                          orderby objEntity.ID
                          select new PurchaseInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updatedby = objEntity.Updatedby,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
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
                              },
                              Purchase_InvoiceHead_BE = new PurchaseInvoiceHeadBE()
                              {
                                  Dated = purchaseHead.Dated,
                              }


                          }).ToList<PurchaseInvoiceDetailBE>();
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

        public static List<PurchaseInvoiceDetailBE> GetPurchaseInvoiceDetailBEs(DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<PurchaseInvoiceDetailBE> result = new List<PurchaseInvoiceDetailBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceDetails
                          join purchaseHead in context.tblPurchaseInvoiceHeads
                          on objEntity.Invoice_ID equals purchaseHead.ID
                          join accounts in context.tblAccounts
                          on objEntity.Account_ID equals accounts.ID
                          join accountType in context.tblAccountTypes
                          on accounts.Account_Type_ID equals accountType.ID
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          join measureUnit in context.tblMeasureUnits
                          on products.MeasureUnitID equals measureUnit.ID
                          where purchaseHead.Dated >= startdate && purchaseHead.Dated <= enddate 
                          orderby objEntity.ID
                          select new PurchaseInvoiceDetailBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Product_ID = objEntity.Product_ID,
                              Account_ID = objEntity.Account_ID,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Updatedby = objEntity.Updatedby,
                              Updated_Date = objEntity.Updated_Date,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Cash = objEntity.Is_Cash,
                              Price = objEntity.Price,
                              Quantity = objEntity.Quantity,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Vehicle_No = objEntity.Vehicle_No,
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

                          }).ToList<PurchaseInvoiceDetailBE>();
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

        public static void DeleteCompletePurchaseInvoice(int invoiceID, int pumpID)
        {
            int result = 0;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("DeletePurchaseInvoice", sqlCon);
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

    }
}
