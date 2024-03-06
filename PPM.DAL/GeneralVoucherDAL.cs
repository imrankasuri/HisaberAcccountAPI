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
    public static class GeneralVoucherDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save( GeneralVoucherBE generalVoucherBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblGeneralVoucher      clinq = null;
                
                clinq  = ConvertToLinqObject(generalVoucherBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (generalVoucherBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now;
                        clinq.Updated_Date = DateTime.Now;
                        context.tblGeneralVouchers.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       // clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblGeneralVouchers.Attach(clinq, true);
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

        public static GeneralVoucherBE GetVoucherByID(int id)
        {
            // Declare variables
            GeneralVoucherBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblGeneralVouchers
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted == false
                          select new GeneralVoucherBE 
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Description = objEntity.Description,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
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

        public static List<GeneralVoucherBE> GetAllVoucherBEs()
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          where  objEntity.Is_Active == true && objEntity.Is_Deleted == false
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Description = objEntity.Description,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static List<GeneralVoucherBE> GetAllVoucherBEs(int pumpID)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          where objEntity.Is_Active == true && objEntity.Is_Deleted == false && objEntity.PumpID == pumpID
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Description = objEntity.Description,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = creditAccount.Account_Type_ID,
                                  }
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = debitAccount.Account_Type_ID,
                                  }
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              
                              
                          }).ToList<GeneralVoucherBE>();
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

        public static List<GeneralVoucherBE> GetAllVoucherBEs(int accountid, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && (objEntity.Debit_Account_ID == accountid || objEntity.Credit_Account_ID == accountid) && objEntity.Dated >= startdate && objEntity.Dated <= enddate
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Description = objEntity.Description,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static tblGeneralVoucher ConvertToLinqObject(GeneralVoucherBE objEntity)
        {
            // Declare variables
            tblGeneralVoucher  result = new tblGeneralVoucher();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.VoucherNo = objEntity.VoucherNo;
                result.PumpID = objEntity.PumpID;
                result.Dated = objEntity.Dated;
                result.Description = objEntity.Description;
                result.Debit_Account_ID = objEntity.Debit_Account_ID;
                result.Credit_Account_ID = objEntity.Credit_Account_ID;
                result.Amount = objEntity.Amount;
                result.AddedByUserID = objEntity.AddedByUserID;
                result.AddedByUser = objEntity.AddedByUser;
                result.UpdatedByUser = objEntity.UpdatedByUser;
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

        //public static void DeleteVoucherByID(int id)
        //{
        //    GeneralVoucherBE gv = GeneralVoucherDAL.GetVoucherByID(id);
        //    GeneralLedgerBE glDebit = GeneralLedgerDAL.GetLedgerEntrybyAccountRefType(gv.Debit_Account_ID, id, "Voucher");
        //    GeneralLedgerBE glCredit = GeneralLedgerDAL.GetLedgerEntrybyAccountRefType(gv.Credit_Account_ID, id, "Voucher");
            
        //    PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            context.tblGeneralLedgers.DeleteAllOnSubmit(context.tblGeneralLedgers.Where(c => c.ID == glDebit.ID));
        //            context.tblGeneralLedgers.DeleteAllOnSubmit(context.tblGeneralLedgers.Where(c => c.ID == glCredit.ID));
        //            context.tblGeneralVouchers.DeleteAllOnSubmit(context.tblGeneralVouchers.Where(c => c.ID == id));
        //            context.SubmitChanges();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //        scope.Complete();
        //    }
        //    int debitAcDeleted = AccountDAL.updateAccountLedger(glDebit.Account_ID);
        //    int creditAcDeleted = AccountDAL.updateAccountLedger(glCredit.Account_ID);
        //}

        public static void DeleteCompleteVoucher(int VoucherID, int pumpID)
        {
            int result = 0;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("DeleteVoucher", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters.Add("@VoucherID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = VoucherID;
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

        public static List<GeneralVoucherBE> GetAllDebitVoucherBEs(int accountid, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Debit_Account_ID == accountid && objEntity.Dated.Date >= startdate && objEntity.Dated.Date < enddate.AddDays(1)
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Description = objEntity.Description,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static List<GeneralVoucherBE> GetAllCreditVoucherBEs(int accountid, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Credit_Account_ID == accountid && objEntity.Dated >= startdate && objEntity.Dated <= enddate
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              Description = objEntity.Description,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static List<GeneralVoucherBE> GetAllVoucherByPumpID(int pumpID, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Dated >= startdate && objEntity.Dated <= enddate && objEntity.PumpID == pumpID
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              Description = objEntity.Description,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                                  Account_Type_ID = creditAccount.Account_Type_ID,
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                                  Account_Type_ID = debitAccount.Account_Type_ID
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static List<GeneralVoucherBE> GetAllDebitVoucherBEsByType(int accountid, DateTime startdate, DateTime enddate)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          
                          join debitAccount in context.tblAccounts
                          on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          join debitAccountType in context.tblAccountTypes
                          on debitAccount.Account_Type_ID equals debitAccountType.ID
                          join creditAccountType in context.tblAccountTypes
                          on creditAccount.Account_Type_ID equals creditAccountType.ID

                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && debitAccountType.ID == accountid && objEntity.Dated >= startdate && objEntity.Dated <= enddate
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              Description = objEntity.Description,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static List<GeneralVoucherBE> GetAllDebitVoucherByAccountTypeIDBEs(int pumpID, int accountTypeID, DateTime dated)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers
                          
                          join debitAccount in context.tblAccounts
                         on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          join debitAccountType in context.tblAccountTypes
                          on debitAccount.Account_Type_ID equals debitAccountType.ID
                          join creditAccountType in context.tblAccountTypes
                          on creditAccount.Account_Type_ID equals creditAccountType.ID

                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && debitAccountType.ID == accountTypeID && objEntity.Dated.Date == dated.Date && objEntity.PumpID == pumpID
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              Description = objEntity.Description,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = creditAccount.Account_Type_ID,
                                      Name = creditAccountType.Name,
                                  }
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = debitAccount.Account_Type_ID,
                                      Name = debitAccountType.Name,
                                  }
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static List<GeneralVoucherBE> GetAllCreditVoucherByAccountTypeIDBEs(int pumpID, int accountTypeID, DateTime dated)
        {
            // Declare variables
            List<GeneralVoucherBE> result = new List<GeneralVoucherBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblGeneralVouchers

                          join debitAccount in context.tblAccounts
                         on objEntity.Debit_Account_ID equals debitAccount.ID
                          join creditAccount in context.tblAccounts
                          on objEntity.Credit_Account_ID equals creditAccount.ID
                          join debitAccountType in context.tblAccountTypes
                          on debitAccount.Account_Type_ID equals debitAccountType.ID
                          join creditAccountType in context.tblAccountTypes
                          on creditAccount.Account_Type_ID equals creditAccountType.ID

                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && creditAccountType.ID == accountTypeID && objEntity.Dated.Date == dated.Date && objEntity.PumpID == pumpID
                          select new GeneralVoucherBE
                          {
                              ID = objEntity.ID,
                              VoucherNo = objEntity.VoucherNo,
                              PumpID = objEntity.PumpID,
                              Amount = objEntity.Amount,
                              Credit_Account_ID = objEntity.Credit_Account_ID,
                              Debit_Account_ID = objEntity.Debit_Account_ID,
                              Dated = objEntity.Dated,
                              Description = objEntity.Description,
                              AddedByUserID = objEntity.AddedByUserID,
                              AddedByUser = objEntity.AddedByUser,
                              UpdatedByUser = objEntity.UpdatedByUser,
                              Credit_Account = new AccountBE()
                              {
                                  ID = objEntity.Credit_Account_ID,
                                  Name = creditAccount.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = creditAccount.Account_Type_ID,
                                      Name = creditAccountType.Name,
                                  }
                              },
                              Debit_Account = new AccountBE()
                              {
                                  ID = objEntity.Debit_Account_ID,
                                  Name = debitAccount.Name,
                                  Account_Type_BE = new AccountTypeBE()
                                  {
                                      ID = debitAccount.Account_Type_ID,
                                      Name = debitAccountType.Name,
                                  }
                              },
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<GeneralVoucherBE>();
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

        public static int GetNextVoucherNo(int pumpID)
        {
            // Declare variables
            GeneralVoucherBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblGeneralVouchers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          select new GeneralVoucherBE
                          {
                              VoucherNo = objEntity.VoucherNo,
                          }).OrderByDescending(p => p.VoucherNo).FirstOrDefault();
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
                return result.VoucherNo + 1;
            }
        }

    }
}
