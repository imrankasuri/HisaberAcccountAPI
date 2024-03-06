using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class SaleInvoiceHeadDAL
    {

        public static int Save(SaleInvoiceHeadBE saleInvoiceHeadBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
               tblSaleInvoiceHead    clinq = null;

               clinq = ConvertToLinqObject(saleInvoiceHeadBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (saleInvoiceHeadBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblSaleInvoiceHeads.InsertOnSubmit(clinq);
                    }
                    else
                    {

                        context.tblSaleInvoiceHeads.Attach(clinq, true);
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

        public static SaleInvoiceHeadBE GetSIHeadByID(int pumpID, int id)
        {
            // Declare variables
            SaleInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblSaleInvoiceHeads 
                          where objEntity.ID == id && objEntity.PumpID == pumpID
                          select new SaleInvoiceHeadBE 
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              AddedBy = objEntity.AddedBy,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              TotalSales = objEntity.Cash_Total + objEntity.Credit_Total
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

        public static SaleInvoiceHeadBE GetSIHeadByInvoiceNo(int pumpID, int invoiceNo)
        {
            // Declare variables
            SaleInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblSaleInvoiceHeads
                          where objEntity.InvoiceNo == invoiceNo && objEntity.PumpID == pumpID
                          select new SaleInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              AddedBy = objEntity.AddedBy,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              TotalSales = objEntity.Cash_Total + objEntity.Credit_Total
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

        public static SaleInvoiceHeadBE GetSIHeadByDate(int pumpID, DateTime InvoiceDate)
        {
            // Declare variables
            SaleInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblSaleInvoiceHeads
                          where objEntity.Dated.Date  == InvoiceDate.Date && objEntity.PumpID == pumpID
                          select new SaleInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              AddedBy = objEntity.AddedBy,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              TotalSales = objEntity.Cash_Total + objEntity.Credit_Total
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

        public static List<SaleInvoiceHeadBE> GetSaleInvoiceHeadBEs(int pumpID)
        {
            // Declare variables
            List<SaleInvoiceHeadBE> result = new List<SaleInvoiceHeadBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceHeads 
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID descending 
                          select new SaleInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              AddedBy = objEntity.AddedBy,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TotalSales = objEntity.Cash_Total + objEntity.Credit_Total,

                          }).ToList<SaleInvoiceHeadBE>();
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

        public static List<SaleInvoiceHeadBE> GetSaleInvoiceHeadBEs(int pumpID, DateTime selectedDate)
        {
            // Declare variables
            List<SaleInvoiceHeadBE> result = new List<SaleInvoiceHeadBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceHeads
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Dated == selectedDate
                          orderby objEntity.ID descending
                          select new SaleInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              AddedBy = objEntity.AddedBy,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TotalSales = objEntity.Cash_Total + objEntity.Credit_Total,

                          }).ToList<SaleInvoiceHeadBE>();
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

        public static tblSaleInvoiceHead ConvertToLinqObject(SaleInvoiceHeadBE objEntity)
        {
            // Declare variables
            tblSaleInvoiceHead result = new tblSaleInvoiceHead();

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
                result.AddedBy = objEntity.AddedBy;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
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

        public static int GetNextSaleInvoiceNumber(int pumpID)
        {
            // Declare variables
            SaleInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblSaleInvoiceHeads 
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          select new SaleInvoiceHeadBE
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

        public static int GetPreviousInvoiceNumber(int pumpID, int invoiceid)
        {
            // Declare variables
            SaleInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblSaleInvoiceHeads
                          where objEntity.Is_Deleted == false && objEntity.InvoiceNo < invoiceid && objEntity.PumpID == pumpID
                          select new SaleInvoiceHeadBE
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
                return 0;
            }
            else
            {
                return result.InvoiceNo;
            }
        }

        public static int GetLastInvoiceID(int pumpID)
        {
            // Declare variables
            SaleInvoiceHeadBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblSaleInvoiceHeads
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID descending
                          select new SaleInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              AddedBy = objEntity.AddedBy,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              TotalSales = objEntity.Cash_Total + objEntity.Credit_Total
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
            if (result == null)
            {
                return 0;
            }
            else
            {
                return result.ID;
            }
        }

        public static void DeleteSaleInvoiceByInvoice(int id)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblSaleInvoiceHeads.DeleteAllOnSubmit(context.tblSaleInvoiceHeads.Where(c => c.ID == id));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static List<SaleInvoiceHeadBE> GetSaleInvoiceHeadBEs(DateTime startDate, DateTime endDate, int pumpID)
        {
            // Declare variables
            List<SaleInvoiceHeadBE> result = new List<SaleInvoiceHeadBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSaleInvoiceHeads
                          where objEntity.Is_Deleted == false && objEntity.PumpID ==pumpID && objEntity.Is_Active == true && objEntity.Is_Deleted == false && objEntity.Dated >= startDate && objEntity.Dated <= endDate
                          orderby objEntity.ID descending
                          select new SaleInvoiceHeadBE
                          {
                              ID = objEntity.ID,
                              InvoiceNo = objEntity.InvoiceNo,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Cash_Total = objEntity.Cash_Total,
                              Credit_Total = objEntity.Credit_Total,
                              Description = objEntity.Description,
                              AddedBy = objEntity.AddedBy,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              TotalSales = objEntity.Cash_Total + objEntity.Credit_Total

                          }).ToList<SaleInvoiceHeadBE>();
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
