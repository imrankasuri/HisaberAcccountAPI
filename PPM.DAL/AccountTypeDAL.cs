using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class AccountTypeDAL
    {

        public static int Save(AccountTypeBE  accountTypeBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblAccountType    clinq = null;
                
                clinq  = ConvertToLinqObject(accountTypeBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (accountTypeBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblAccountTypes.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblAccountTypes.Attach(clinq, true);
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

        public static AccountTypeBE GetAccountTypeByID(int id)
        {
            // Declare variables
            AccountTypeBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from tblmunits in context.tblAccountTypes
                          where tblmunits.ID == id 
                          select new AccountTypeBE 
                          {
                              ID = tblmunits.ID,
                              Name = tblmunits.Name,
                              Created_Date = tblmunits.Created_Date,
                              Updated_Date = tblmunits.Updated_Date,
                              Is_Active = tblmunits.Is_Active,
                              Is_Deleted = tblmunits.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(tblmunits.TimeStamp.ToArray())
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

        public static List<AccountTypeBE> GetAccountTypeBEs()
        {
            // Declare variables
            List<AccountTypeBE> result = new List<AccountTypeBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from tblmunits in context.tblAccountTypes 
                          where tblmunits.Is_Deleted==false && tblmunits.Is_Active==true
                          orderby tblmunits.ID 
                          select new AccountTypeBE
                          {
                              ID = tblmunits.ID,
                              Name = tblmunits.Name,
                              Created_Date = tblmunits.Created_Date,
                              Updated_Date = tblmunits.Updated_Date,
                              Is_Active = tblmunits.Is_Active,
                              Is_Deleted = tblmunits.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(tblmunits.TimeStamp.ToArray())
                              
                          }).ToList<AccountTypeBE>();
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

        public static tblAccountType ConvertToLinqObject(AccountTypeBE accountTypeBE)
        {
            // Declare variables
            tblAccountType  result = new tblAccountType();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = accountTypeBE.ID;
                result.Name = accountTypeBE.Name;
                result.Created_Date = accountTypeBE.Created_Date;
                result.Updated_Date = accountTypeBE.Updated_Date;
                result.Is_Active = accountTypeBE.Is_Active;
                result.Is_Deleted = accountTypeBE.Is_Deleted;
                if (accountTypeBE.TimeStamp != null)
                {
                    result.TimeStamp = new System.Data.Linq.Binary(Convert.FromBase64String(accountTypeBE.TimeStamp.ToString()));
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
