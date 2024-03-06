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
    public static class DipReadingDAL
    {
       
        public static int Save(DipReadingBE dipReadingBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblDipReading  clinq = null;
                
                clinq  = ConvertToLinqObject(dipReadingBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (dipReadingBE.ID == 0)
                    {
                        dipReadingBE.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblDipReadings.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblDipReadings.Attach(clinq, true);
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

        public static DipReadingBE GetDipReadingByID(int id, int pumpID)
        {
            // Declare variables
            DipReadingBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblDipReadings
                         
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted== false && objEntity.PumpID == pumpID
                          select new DipReadingBE 
                          {
                              ID = objEntity.ID,
                              AdjustmentID = objEntity.AdjustmentID,
                              TankID = objEntity.TankID,
                              PumpID = objEntity.PumpID,
                              TankNo = objEntity.TankNo,
                              ProductID = objEntity.ProductID,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              DIP = objEntity.DIP,
                              StockLtr = objEntity.StockLtr,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
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

        public static List<DipReadingBE> GetDipReadingBEs(int pumpID)
        {
            // Declare variables
            List<DipReadingBE> result = new List<DipReadingBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblDipReadings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new DipReadingBE
                          {
                              ID = objEntity.ID,
                              AdjustmentID = objEntity.AdjustmentID,
                              TankID = objEntity.TankID,
                              PumpID = objEntity.PumpID,
                              TankNo = objEntity.TankNo,
                              ProductID = objEntity.ProductID,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              DIP = objEntity.DIP,
                              StockLtr = objEntity.StockLtr,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                          }).ToList<DipReadingBE>();
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

        public static DipReadingBE GetLastFeededDipReadingBEs(int pumpID)
        {
            // Declare variables
            DipReadingBE result = new DipReadingBE();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblDipReadings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.Dated descending 
                          select new DipReadingBE
                          {
                              ID = objEntity.ID,
                              AdjustmentID = objEntity.AdjustmentID,
                              TankID = objEntity.TankID,
                              PumpID = objEntity.PumpID,
                              TankNo = objEntity.TankNo,
                              ProductID = objEntity.ProductID,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              DIP = objEntity.DIP,
                              StockLtr = objEntity.StockLtr,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
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

        public static List<DipReadingBE> GetDipReadingBEs(int pumpID, DateTime selectedDate)
        {
            // Declare variables
            List<DipReadingBE> result = new List<DipReadingBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblDipReadings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Dated == selectedDate.Date
                          orderby objEntity.ID
                          select new DipReadingBE
                          {
                              ID = objEntity.ID,
                              AdjustmentID = objEntity.AdjustmentID,
                              TankID = objEntity.TankID,
                              PumpID = objEntity.PumpID,
                              TankNo = objEntity.TankNo,
                              ProductID = objEntity.ProductID,
                              ProductCode = objEntity.ProductCode,
                              ProductName = objEntity.ProductName,
                              DIP = objEntity.DIP,
                              StockLtr = objEntity.StockLtr,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                          }).ToList<DipReadingBE>();
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

        public static tblDipReading ConvertToLinqObject(DipReadingBE objEntity)
        {
            // Declare variables
            tblDipReading  result = new tblDipReading();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.AdjustmentID = objEntity.AdjustmentID;
                result.TankID = objEntity.TankID;
                result.PumpID = objEntity.PumpID;
                result.TankNo = objEntity.TankNo;
                result.ProductID = objEntity.ProductID;
                result.ProductCode = objEntity.ProductCode;
                result.ProductName = objEntity.ProductName;
                result.DIP = objEntity.DIP;
                result.StockLtr = objEntity.StockLtr;
                result.Dated = objEntity.Dated;
                result.IsPosted = objEntity.IsPosted;
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


    }
}
