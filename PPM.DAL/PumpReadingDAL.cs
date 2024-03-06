using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class PumpReadingDAL
    {

        public static int Save(PumpReadingBE  pumpReadingBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblPumpReading  clinq = null;
                
                clinq  = ConvertToLinqObject(pumpReadingBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (pumpReadingBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblPumpReadings.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblPumpReadings.Attach(clinq, true);
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

        public static PumpReadingBE GetPumpReadingByID(int id)
        {
            // Declare variables
            PumpReadingBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblPumpReadings
                          where objEntity.ID == id 
                          select new PumpReadingBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpMachineID = objEntity.PumpMachineID,
                              Pump_No = objEntity.Pump_No,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              Last_Reading = objEntity.Last_Reading,
                              Current_Reading = objEntity.Current_Reading,
                              Returned = objEntity.Returned,
                              Dated = objEntity.Dated,
                              Invoice_ID = objEntity.Invoice_ID,
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

        public static List<PumpReadingBE> GetPumpReadingBEs(int pumpID, int machineID)
        {
            // Declare variables
            List<PumpReadingBE> result = new List<PumpReadingBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPumpReadings 
                          where objEntity.Is_Deleted==false && objEntity.Is_Active==true && objEntity.PumpID == pumpID && objEntity.PumpMachineID == machineID
                          orderby objEntity.ID 
                          select new PumpReadingBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpMachineID = objEntity.PumpMachineID,
                              Pump_No = objEntity.Pump_No,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              Last_Reading = objEntity.Last_Reading,
                              Current_Reading = objEntity.Current_Reading,
                              Returned = objEntity.Returned,
                              Dated = objEntity.Dated,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())
                              
                          }).ToList<PumpReadingBE>();
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

        public static List<PumpReadingBE> GetPumpReadingBEs(int pumpID, int machineID, DateTime startDate, DateTime endDate)
        {
            // Declare variables
            List<PumpReadingBE> result = new List<PumpReadingBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPumpReadings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.PumpMachineID == machineID && objEntity.Dated >= startDate && objEntity.Dated < endDate.AddDays(1)
                          orderby objEntity.ID
                          select new PumpReadingBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpMachineID = objEntity.PumpMachineID,
                              Pump_No = objEntity.Pump_No,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              Last_Reading = objEntity.Last_Reading,
                              Current_Reading = objEntity.Current_Reading,
                              Returned = objEntity.Returned,
                              UsedQuantity = (objEntity.Current_Reading - objEntity.Last_Reading ) - objEntity.Returned,
                              Dated = objEntity.Dated,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<PumpReadingBE>();
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


        public static List<PumpReadingBE> GetPumpReadingByInvoiceID(int id)
        {
            // Declare variables
            List<PumpReadingBE> result = new List<PumpReadingBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPumpReadings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Invoice_ID == id
                          orderby objEntity.ID
                          select new PumpReadingBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpMachineID = objEntity.PumpMachineID,
                              Pump_No = objEntity.Pump_No,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              Last_Reading = objEntity.Last_Reading,
                              Current_Reading = objEntity.Current_Reading,
                              Returned = objEntity.Returned,
                              Dated = objEntity.Dated,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<PumpReadingBE>();
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

        public static List<PumpReadingBE> GetPumpReadingByPumpIDDate(int pumpID, DateTime invoiceDate)
        {
            // Declare variables
            List<PumpReadingBE> result = new List<PumpReadingBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblPumpReadings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Dated == invoiceDate.Date
                          orderby objEntity.ID
                          select new PumpReadingBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpMachineID = objEntity.PumpMachineID,
                              Pump_No = objEntity.Pump_No,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              Last_Reading = objEntity.Last_Reading,
                              Current_Reading = objEntity.Current_Reading,
                              Returned = objEntity.Returned,
                              Dated = objEntity.Dated,
                              Invoice_ID = objEntity.Invoice_ID,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<PumpReadingBE>();
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

        public static tblPumpReading ConvertToLinqObject(PumpReadingBE objEntity)
        {
            // Declare variables
            tblPumpReading  result = new tblPumpReading();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.PumpMachineID = objEntity.PumpMachineID;
                result.Pump_No = objEntity.Pump_No;
                result.ProductCode = objEntity.ProductCode;
                result.ProductName = objEntity.ProductName;
                result.Last_Reading = objEntity.Last_Reading;
                result.Current_Reading = objEntity.Current_Reading;
                result.Returned = objEntity.Returned;
                result.Dated = objEntity.Dated;
                result.Invoice_ID = objEntity.Invoice_ID;
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

        public static PumpReadingBE GetPreviousPumpReading(int pumpno)
        {
            // Declare variables
            PumpReadingBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblPumpReadings
                          where objEntity.Pump_No == pumpno 
                          orderby objEntity.ID descending
                          select new PumpReadingBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpMachineID = objEntity.PumpMachineID,
                              Pump_No = objEntity.Pump_No,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              Last_Reading = objEntity.Last_Reading,
                              Current_Reading = objEntity.Current_Reading,
                              Returned = objEntity.Returned,
                              Dated = objEntity.Dated,
                              Invoice_ID = objEntity.Invoice_ID,
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

        public static void DeletePumpReadingInvoice(int invoiceID, int pumpID)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblPumpReadings.DeleteAllOnSubmit(context.tblPumpReadings.Where(c => c.Invoice_ID== invoiceID && c.PumpID == pumpID));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }
    }
}
