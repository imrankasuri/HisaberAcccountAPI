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
    public static class GeneralLedgerDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(GeneralLedgerBE generalLedgerBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
              tblGeneralLedger clinq = null;
                
                clinq  = ConvertToLinqObject(generalLedgerBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (generalLedgerBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblGeneralLedgers.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblGeneralLedgers.Attach(clinq, true);
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
           // int recordEffected = AccountDAL.updateAccountLedger(generalLedgerBE.Account_ID);
            return result;
        }

        public static GeneralLedgerBE GeLedgerByID(int id)
        {
            // Declare variables
            GeneralLedgerBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.ID == id 
                          select new GeneralLedgerBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).SingleOrDefault();
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

        public static GeneralLedgerBE GetLastLedgerEntry(int iaccountId)
        {
            // Declare variables
            GeneralLedgerBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Account_ID == iaccountId
                          orderby objEntity.ID descending
                          select new GeneralLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
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

        public static List<GeneralLedgerBE> GetLedgerByAccountIDOld(int pumpID, int accountID)
        {
            // Declare variables
            List<GeneralLedgerBE> result = new List<GeneralLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Account_ID == accountID && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new GeneralLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<GeneralLedgerBE>();
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

        public static List<GeneralLedgerBE> GetLedgerByAccountID(int pumpID, int accountID)
        {
            List<GeneralLedgerBE> result = new List<GeneralLedgerBE>();
            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetGeneralLedgerByAccountID", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Account_ID", SqlDbType.Int);
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters[0].Value = accountID;
            cmd.Parameters[1].Value = pumpID;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    GeneralLedgerBE n = new GeneralLedgerBE();
                    n.ID = Convert.ToInt32(dr["ID"]);
                    n.Account_ID = Convert.ToInt32(dr["Account_ID"]);
                    n.Transaction_Date = Convert.ToDateTime(dr["Transaction_Date"]);
                    //n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.Vehicle_No = Convert.ToString(dr["Vehicle_No"]);
                    n.Reference_No = Convert.ToInt32(dr["Reference_No"]);
                    n.Reference_Type = Convert.ToString(dr["Reference_Type"]);
                    n.Debit = Convert.ToDecimal(dr["Debit"]);
                    n.Credit = Convert.ToDecimal(dr["Credit"]);
                    n.Balance = Convert.ToDecimal(dr["Balance"]);
                    n.BalanceType = Convert.ToString(dr["BalanceType"]);
                    n.Is_Active = Convert.ToBoolean(dr["Is_Active"]);
                    n.Is_Deleted = Convert.ToBoolean(dr["Is_Deleted"]);
                    n.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    n.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]);
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

        public static List<GeneralLedgerBE> GetLedgerByPumpID(int pumpID)
        {
            // Declare variables
            List<GeneralLedgerBE> result = new List<GeneralLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true  && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new GeneralLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<GeneralLedgerBE>();
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

        public static List<GeneralLedgerBE> GetLedgerGreaterThanStartDate(int accountID, DateTime startDate)
        {
            // Declare variables
            List<GeneralLedgerBE> result = new List<GeneralLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Account_ID == accountID && objEntity.Transaction_Date >= startDate
                          orderby objEntity.ID
                          select new GeneralLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<GeneralLedgerBE>();
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

        public static List<GeneralLedgerBE> GetLedgerLesserThanStartDate(int accountID, DateTime startDate)
        {
            // Declare variables
            List<GeneralLedgerBE> result = new List<GeneralLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Account_ID == accountID && objEntity.Transaction_Date < startDate.AddDays(1)
                          orderby objEntity.ID
                          select new GeneralLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<GeneralLedgerBE>();
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


        public static List<GeneralLedgerBE> GetLedgerbyDate(int accountID, DateTime startDate, DateTime endDate)
        {
            // Declare variables
            List<GeneralLedgerBE> result = new List<GeneralLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Account_ID == accountID && objEntity.Transaction_Date.Date >= startDate && objEntity.Transaction_Date < endDate.AddDays(1)
                          orderby objEntity.ID
                          select new GeneralLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<GeneralLedgerBE>();
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

        public static tblGeneralLedger ConvertToLinqObject(GeneralLedgerBE objEntity)
        {
            // Declare variables
            tblGeneralLedger  result = new tblGeneralLedger();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.Account_ID = objEntity.Account_ID;
                result.Transaction_Date = objEntity.Transaction_Date;
                result.Description = objEntity.Description;
                result.Reference_No = objEntity.Reference_No;
                result.Reference_Type = objEntity.Reference_Type;
                result.Vehicle_No = objEntity.Vehicle_No;
                result.Receipt_No = objEntity.Receipt_No;
                result.Debit = objEntity.Debit;
                result.Credit = objEntity.Credit;
                result.Balance = objEntity.Balance;
                result.BalanceType = objEntity.BalanceType;
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

        public static DateTime? GetPreviousDate(int pumpID, int accountID, DateTime transactionDate)
        {
            // Declare variables
            GeneralLedgerBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Account_ID == accountID && objEntity.Transaction_Date < transactionDate
                          select new GeneralLedgerBE
                          {
                              Transaction_Date = objEntity.Transaction_Date,
                          }).OrderByDescending(p => p.Transaction_Date).FirstOrDefault();
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
                return null;
            }
            else
            {
                return result.Transaction_Date;
            }
        }

        public static GeneralLedgerBE GetLedgerEntrybyAccountRefType(int accountId, int refNo, string refType)
        {
            // Declare variables
            GeneralLedgerBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblGeneralLedgers
                          where objEntity.Account_ID == accountId && objEntity.Reference_No == refNo && objEntity.Reference_Type== refType
                          orderby objEntity.ID descending
                          select new GeneralLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_ID = objEntity.Account_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Description = objEntity.Description,
                              Reference_No = objEntity.Reference_No,
                              Reference_Type = objEntity.Reference_Type,
                              Receipt_No = objEntity.Receipt_No,
                              Vehicle_No = objEntity.Vehicle_No,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
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
    }
}
