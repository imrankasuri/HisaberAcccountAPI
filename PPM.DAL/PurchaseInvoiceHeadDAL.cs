using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class PurchaseInvoiceHeadDAL
    {

        public static int Save(PurchaseInvoiceHeadBE   purchaseInvoiceHeadBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
               tblPurchaseInvoiceHead    clinq = null;
                
                clinq  = ConvertToLinqObject(purchaseInvoiceHeadBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (purchaseInvoiceHeadBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblPurchaseInvoiceHeads.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblPurchaseInvoiceHeads.Attach(clinq, true);
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

        public static PurchaseInvoiceHeadBE GetPIHeadByID(int id)
        {
            // Declare variables
            PurchaseInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblPurchaseInvoiceHeads
                          where objEntity.ID == id 
                          select new PurchaseInvoiceHeadBE 
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              AddedBy = objEntity.AddedBy,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())
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

        public static List<PurchaseInvoiceHeadBE> GetPurchaseInvoiceHeadBEs(int pumpID)
        {
            // Declare variables
            List<PurchaseInvoiceHeadBE> result = new List<PurchaseInvoiceHeadBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceHeads
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID descending 
                          select new PurchaseInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              AddedBy = objEntity.AddedBy,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              TotalPurchase = objEntity.Cash_Total + objEntity.Credit_Total,

                          }).ToList<PurchaseInvoiceHeadBE>();
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

        public static List<PurchaseInvoiceHeadBE> GetPurchaseInvoiceHeadBEs(int pumpID, DateTime selectedDate)
        {
            // Declare variables
            List<PurchaseInvoiceHeadBE> result = new List<PurchaseInvoiceHeadBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceHeads
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Dated == selectedDate
                          orderby objEntity.ID descending
                          select new PurchaseInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              AddedBy = objEntity.AddedBy,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<PurchaseInvoiceHeadBE>();
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

        //public static List<PurchaseInvoiceHeadBE> GetPurchaseInvoiceHeadBEs(int pumpID)
        //{
        //    // Declare variables
        //    List<PurchaseInvoiceHeadBE> result = new List<PurchaseInvoiceHeadBE>();

        //    //// Set data context objects
        //    PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

        //    try
        //    {
        //        // Read in list of Image Set Accounts from the database
        //        result = (from objEntity in context.tblPurchaseInvoiceHeads
        //                  where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
        //                  orderby objEntity.ID descending
        //                  select new PurchaseInvoiceHeadBE
        //                  {
        //                      ID = objEntity.ID,
        //                      InvoiceNo = objEntity.InvoiceNo,
        //                      PumpID = objEntity.PumpID,
        //                      PumpCode = objEntity.PumpCode,
        //                      Dated = objEntity.Dated,
        //                      Cash_Total = objEntity.Cash_Total,
        //                      Credit_Total = objEntity.Credit_Total,
        //                      Description = objEntity.Description,
        //                      Reference_No = objEntity.Reference_No,
        //                      AddedBy = objEntity.AddedBy,
        //                      Is_Active = objEntity.Is_Active,
        //                      Is_Deleted = objEntity.Is_Deleted,
        //                      Created_Date = objEntity.Created_Date,
        //                      Updated_Date = objEntity.Updated_Date,
        //                      TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

        //                  }).ToList<PurchaseInvoiceHeadBE>();
        //    }
        //    catch (Exception ex)
        //    {
        //        // pass error back to calling method
        //        throw ex;
        //    }
        //    finally
        //    {
        //        // Clean up
        //        context.Dispose();
        //    }

        //    return result;
        //}

        public static tblPurchaseInvoiceHead ConvertToLinqObject(PurchaseInvoiceHeadBE objEntity)
        {
            // Declare variables
            tblPurchaseInvoiceHead  result = new tblPurchaseInvoiceHead();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.InvoiceNo = objEntity.InvoiceNo;
                result.PumpID = objEntity.PumpID;
                result.PumpCode = objEntity.PumpCode;
                result.Dated = objEntity.Dated;
                result.Cash_Total = objEntity.Cash_Total;
                result.Credit_Total = objEntity.Credit_Total;
                result.Description = objEntity.Description;
                result.Reference_No = objEntity.Reference_No;
                result.AddedBy = objEntity.AddedBy;
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

        public static int GetNextPurchaseInvoiceNumber(int pumpID)
        {
            // Declare variables
            PurchaseInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblPurchaseInvoiceHeads 
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          select new PurchaseInvoiceHeadBE
                          {
                              InvoiceNo = objEntity.InvoiceNo,
                          }).OrderByDescending(p => p.InvoiceNo).FirstOrDefault();
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
            if (result == null)
            {
                return 1;
            }
            else
            {
                return result.InvoiceNo + 1;
            }
        }

        public static void DeletePurchaseInvoiceByInvoice(int id)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblPurchaseInvoiceHeads.DeleteAllOnSubmit(context.tblPurchaseInvoiceHeads.Where(c => c.ID == id));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static List<PurchaseInvoiceHeadBE> GetPurchaseInvoiceHeadBEs(DateTime startdate, DateTime enddate,int pumpID)
        {
            // Declare variables
            List<PurchaseInvoiceHeadBE> result = new List<PurchaseInvoiceHeadBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPurchaseInvoiceHeads
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Is_Deleted == false && objEntity.Dated >= startdate && objEntity.Dated <= enddate 
                          orderby objEntity.ID descending
                          select new PurchaseInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              AddedBy = objEntity.AddedBy,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<PurchaseInvoiceHeadBE>();
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
    }
}
