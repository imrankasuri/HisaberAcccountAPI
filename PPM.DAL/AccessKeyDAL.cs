using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class AccessKeyDAL
    {
        public static int Save(AccessKeyBE    accessKeyBE)
        {

            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                tblAccessKey     clinq = null;
                clinq = ConvertToLinqObject(accessKeyBE);

                try
                {
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (accessKeyBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblAccessKeys.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblAccessKeys.Attach(clinq, true);
                    }
                    context.SubmitChanges();
                    result = clinq.ID;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    context.Dispose();
                }

                scope.Complete();
            }


            return result;
        }

        public static AccessKeyBE GetAccessKeyByID(int id)
        {
            // Declare variables
            AccessKeyBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblAccessKeys
                          where objEntity.ID == id && objEntity.Is_Deleted == false
                          select new AccessKeyBE
                          {
                              ID = objEntity.ID,
                              UserID = objEntity.UserID,
                              AccessKey = objEntity.AccessKey,
                              ExpiryDate = objEntity.ExpiryDate,
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

        public static AccessKeyBE GetAccessKeyByUserID(int userID)
        {
            AccessKeyBE result = null;
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                result = (from objEntity in context.tblAccessKeys
                          where objEntity.UserID == userID && objEntity.Is_Deleted == false
                          select new AccessKeyBE
                          {
                              ID = objEntity.ID,
                              UserID = objEntity.UserID,
                              AccessKey = objEntity.AccessKey,
                              ExpiryDate = objEntity.ExpiryDate,
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

        public static List<AccessKeyBE> GetAllUserBEs()
        {
            // Declare variables
            List<AccessKeyBE> result = new List<AccessKeyBE>();
            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblAccessKeys
                          where objEntity.Is_Deleted == false
                          orderby objEntity.ID
                          select new AccessKeyBE
                          {
                              ID = objEntity.ID,
                              UserID = objEntity.UserID,
                              AccessKey = objEntity.AccessKey,
                              ExpiryDate = objEntity.ExpiryDate,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<AccessKeyBE>();
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

        public static tblAccessKey ConvertToLinqObject(AccessKeyBE objEntity)
        {
            // Declare variables
            tblAccessKey result = new tblAccessKey();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.UserID = objEntity.UserID;
                result.AccessKey = objEntity.AccessKey;
                result.ExpiryDate = objEntity.ExpiryDate;
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

        public static void DeleteAccessKey(int UserID)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblAccessKeys.DeleteAllOnSubmit(context.tblAccessKeys.Where(c => c.UserID == UserID));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static Boolean CheckValidAccessKey(int userID, string apiAccessKey)
        {
            AccessKeyBE result = null;
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                result = (from objEntity in context.tblAccessKeys 
                          where objEntity.UserID == userID && objEntity.AccessKey == apiAccessKey && objEntity.ExpiryDate > DateTime.Now
                          select new AccessKeyBE
                          {
                              ID = objEntity.ID,
                              UserID = objEntity.UserID,
                              AccessKey = objEntity.AccessKey,
                              ExpiryDate = objEntity.ExpiryDate,
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

            if(result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

    }
}
