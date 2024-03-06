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
    public static class AccountDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(AccountBE  accountBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblAccount  clinq = null;
                
                clinq  = ConvertToLinqObject(accountBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (accountBE.ID == 0)
                    {
                        accountBE.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblAccounts.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblAccounts.Attach(clinq, true);
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

        public static AccountBE GetAccountByID(int id, int pumpID)
        {
            // Declare variables
            AccountBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblAccounts
                          join accountType in context.tblAccountTypes
                          on objEntity.Account_Type_ID equals accountType.ID
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted== false && objEntity.PumpID == pumpID
                          select new AccountBE 
                          {
                              ID = objEntity.ID,
                              Account_Type_ID = objEntity.Account_Type_ID,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              Description = objEntity.Description,
                              Mobile_No = objEntity.Mobile_No,
                              Email_Address = objEntity.Email_Address,
                              Phone_No = objEntity.Phone_No,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Is_Default = objEntity.Is_Default,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Account_Type_BE = new AccountTypeBE()
                              {
                                  ID = objEntity.Account_Type_ID,
                                  Name = accountType.Name,
                              },
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

        public static AccountBE GetAccountByPumpIDTypeID(int pumpID, int accountTypeID)
        {
            // Declare variables
            AccountBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblAccounts
                          join accountType in context.tblAccountTypes
                          on objEntity.Account_Type_ID equals accountType.ID
                          where objEntity.Account_Type_ID == accountTypeID && objEntity.PumpID == pumpID && objEntity.Is_Default == true && objEntity.Is_Active == true && objEntity.Is_Deleted == false
                          select new AccountBE
                          {
                              ID = objEntity.ID,
                              Account_Type_ID = objEntity.Account_Type_ID,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              Description = objEntity.Description,
                              Mobile_No = objEntity.Mobile_No,
                              Email_Address = objEntity.Email_Address,
                              Phone_No = objEntity.Phone_No,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Is_Default = objEntity.Is_Default,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Account_Type_BE = new AccountTypeBE()
                              {
                                  ID = objEntity.Account_Type_ID,
                                  Name = accountType.Name,
                              },
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

        public static AccountBE GetAccountByName(string accountName, int pumpID)
        {
            // Declare variables
            AccountBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblAccounts
                          join accountType in context.tblAccountTypes
                          on objEntity.Account_Type_ID equals accountType.ID
                          where objEntity.Name.ToLower()  == accountName.ToLower() && objEntity.Is_Active == true && objEntity.Is_Deleted== false && objEntity.PumpID == pumpID

                          select new AccountBE
                          {
                              ID = objEntity.ID,
                              Account_Type_ID = objEntity.Account_Type_ID,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              Description = objEntity.Description,
                              Mobile_No = objEntity.Mobile_No,
                              Email_Address = objEntity.Email_Address,
                              Phone_No = objEntity.Phone_No,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Is_Default = objEntity.Is_Default,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Account_Type_BE = new AccountTypeBE()
                              {
                                  ID = objEntity.Account_Type_ID,
                                  Name = accountType.Name,
                              },
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

        public static List<AccountBE> GetAccountBEs(int pumpID)
        {
            // Declare variables
            List<AccountBE> result = new List<AccountBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblAccounts
                          join accountType in context.tblAccountTypes
                          on objEntity.Account_Type_ID equals accountType.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID 
                          orderby objEntity.ID
                          select new AccountBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Account_Type_ID = objEntity.Account_Type_ID,
                              Is_Default= objEntity.Is_Default,
                              Name = objEntity.Name,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Description = objEntity.Description,
                              Mobile_No = objEntity.Mobile_No,
                              Email_Address = objEntity.Email_Address,
                              Phone_No = objEntity.Phone_No,
                              Account_Type_BE = new AccountTypeBE()
                              {
                                  ID = objEntity.Account_Type_ID,
                                  Name = accountType.Name
                              },
                          }).ToList<AccountBE>();
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

        public static tblAccount ConvertToLinqObject(AccountBE objEntity)
        {
            // Declare variables
            tblAccount  result = new tblAccount();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.Name = objEntity.Name;
                result.Description = objEntity.Description;
                result.Email_Address = objEntity.Email_Address;
                result.Phone_No = objEntity.Phone_No;
                result.Mobile_No = objEntity.Mobile_No;
                result.Account_Type_ID = objEntity.Account_Type_ID;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.Is_Default = objEntity.Is_Default;
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

        public static List<AccountBE> GetAccountByPumpID(int pumpID)
        {
            List<AccountBE> result = new List<AccountBE>();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetAccountsByPumpID", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    AccountBE n = new AccountBE();
                    n.ID = Convert.ToInt32(dr["ID"]);
                    n.Account_Type_ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Name = Convert.ToString(dr["Name"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.Mobile_No = Convert.ToString(dr["Mobile_No"]);
                    n.Email_Address = Convert.ToString(dr["Email_Address"]);
                    if (dr["Phone_No"] != null)
                    {
                        n.Phone_No = Convert.ToString(dr["Phone_No"]);
                    }
                    n.Is_Active = Convert.ToBoolean(dr["Is_Active"]);
                    n.Is_Default = Convert.ToBoolean(dr["Is_Default"]);
                    n.Is_Deleted = Convert.ToBoolean(dr["Is_Deleted"]);
                    n.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    n.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]);
                    n.TimeStamp = Convert.ToString(dr["TimeStamp"]);
                    n.Account_Type_BE = new AccountTypeBE();
                    n.Account_Type_BE.Name = Convert.ToString(dr["AccountTypeName"]);
                    n.Account_Type_BE.ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    n.Account_Type_ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    if (dr["Balance"] != System.DBNull.Value)
                    {
                        n.Balance = Convert.ToDecimal(dr["Balance"]);
                        n.BalanceType = Convert.ToString(dr["BalanceType"]);
                    }
                   else
                    {
                        n.Balance = 0;
                        n.BalanceType = "Debit";
                    }
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

        public static AccountBE GetAccountByIDPumpID(int pumpID, int AccountID)
        {
            List<AccountBE> result = new List<AccountBE>();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetAccountByIDPumpID", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters.Add("@AccountID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = AccountID;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    AccountBE n = new AccountBE();
                    n.ID = Convert.ToInt32(dr["ID"]);
                    n.Account_Type_ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Name = Convert.ToString(dr["Name"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.Mobile_No = Convert.ToString(dr["Mobile_No"]);
                    n.Email_Address = Convert.ToString(dr["Email_Address"]);
                    if (dr["Phone_No"] != null)
                    {
                        n.Phone_No = Convert.ToString(dr["Phone_No"]);
                    }
                    n.Is_Active = Convert.ToBoolean(dr["Is_Active"]);
                    n.Is_Default = Convert.ToBoolean(dr["Is_Default"]);
                    n.Is_Deleted = Convert.ToBoolean(dr["Is_Deleted"]);
                    n.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    n.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]);
                    n.TimeStamp = Convert.ToString(dr["TimeStamp"]);
                    n.Account_Type_BE = new AccountTypeBE();
                    n.Account_Type_BE.Name = Convert.ToString(dr["AccountTypeName"]);
                    n.Account_Type_BE.ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    n.Account_Type_ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    if (dr["Balance"] != System.DBNull.Value)
                    {
                        n.Balance = Convert.ToDecimal(dr["Balance"]);
                        n.BalanceType = Convert.ToString(dr["BalanceType"]);
                    }
                    else
                    {
                        n.Balance = 0;
                        n.BalanceType = "Debit";
                    }
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
            return result.FirstOrDefault();
        }
        public static List<AccountBE> GetAccountByPumpIDOnDate(int pumpID, DateTime endDate)
        {
            List<AccountBE> result = new List<AccountBE>();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetAcountBalanceOnDate", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Pump_ID", SqlDbType.Int);
            cmd.Parameters.Add("@Transaction_Date", SqlDbType.Date);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = endDate;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    AccountBE n = new AccountBE();
                    n.ID = Convert.ToInt32(dr["ID"]);
                    n.Account_Type_ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Name = Convert.ToString(dr["Name"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.Mobile_No = Convert.ToString(dr["Mobile_No"]);
                    n.Email_Address = Convert.ToString(dr["Email_Address"]);
                    if (dr["Phone_No"] != null)
                    {
                        n.Phone_No = Convert.ToString(dr["Phone_No"]);
                    }
                    n.Is_Active = Convert.ToBoolean(dr["Is_Active"]);
                    n.Is_Default = Convert.ToBoolean(dr["Is_Default"]);
                    n.Is_Deleted = Convert.ToBoolean(dr["Is_Deleted"]);
                    n.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    n.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]);
                    n.TimeStamp = Convert.ToString(dr["TimeStamp"]);
                    n.Account_Type_BE = new AccountTypeBE();
                    n.Account_Type_BE.Name = Convert.ToString(dr["AccountTypeName"]);
                    n.Account_Type_BE.ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    n.Account_Type_ID = Convert.ToInt32(dr["Account_Type_ID"]);
                    if (dr["Balance"] != System.DBNull.Value)
                    {
                        n.Balance = Convert.ToDecimal(dr["Balance"]);
                        n.BalanceType = Convert.ToString(dr["BalanceType"]);
                    }
                    else
                    {
                        n.Balance = 0;
                        n.BalanceType = "Debit";
                    }
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
        public static int updateAccountLedger(int accountID)
        {
            int result = 1;
            //SqlConnection sqlCon = new SqlConnection(ConString);
            //SqlCommand cmd = new SqlCommand("SP_Member_Balance", sqlCon);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@p_Account_ID", SqlDbType.Int);
            //cmd.Parameters[0].Value = accountID;
            //cmd.CommandTimeout = 0;
            //try
            //{
            //    sqlCon.Open();
            //    result = cmd.ExecuteNonQuery();
                
            //}
            //catch (Exception exp)
            //{
            //    throw exp;
            //}
            //finally
            //{
            //    sqlCon.Close();
            //}
            return result;
        }

        public static TableCountBE GetTableCounts(int pumpID)
        {
            TableCountBE result = new TableCountBE();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetPumpRecordCount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    result.SalesHeadCount = Convert.ToInt32(dr["SalesHeadCount"]);
                    result.SaleDetailCount = Convert.ToInt32(dr["SaleDetailCount"]);
                    result.PurchaseHeadCount = Convert.ToInt32(dr["PurchaseHeadCount"]);
                    result.PurchaseDetailCount = Convert.ToInt32(dr["PurchaseDetailCount"]);
                    result.VoucherCount = Convert.ToInt32(dr["VoucherCount"]);
                    result.GeneralLedgerCount = Convert.ToInt32(dr["GeneralLedgerCount"]);
                    result.ProductLedgerCount = Convert.ToInt32(dr["ProductLedgerCount"]);
                    result.ProductsCount = Convert.ToInt32(dr["ProductsCount"]);
                    result.AccountsCount = Convert.ToInt32(dr["AccountsCount"]);
                    result.LoginLogsCount = Convert.ToInt32(dr["LoginLogsCount"]);
                    result.DipReadingCount = Convert.ToInt32(dr["DipReadingCount"]);
                    result.DipReadingAdjCount = Convert.ToInt32(dr["DipReadingAdjCount"]);
                    result.PumpReadingCount = Convert.ToInt32(dr["PumpReadingCount"]);
                    result.PumpMachineCount = Convert.ToInt32(dr["PumpMachineCount"]);
                    result.CustomerRateCount = Convert.ToInt32(dr["CustomerRateCount"]);
                    result.InvestmentSummaryCount = Convert.ToInt32(dr["InvestmentSummaryCount"]);
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
