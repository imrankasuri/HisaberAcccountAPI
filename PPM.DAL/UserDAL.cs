using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;


namespace HAccounts.DAL
{
    public static class UserDAL
    {
        public static int Save(UserBE   userBE)
        {

            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                tblUser     clinq = null;
                clinq = ConvertToLinqObject(userBE);

                try
                {
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (userBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblUsers.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblUsers.Attach(clinq, true);
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

        public static UserBE GetUserBEByID(int id)
        {
            // Declare variables
            UserBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblUsers
                          where objEntity.ID == id && objEntity.Is_Deleted == false
                          select new UserBE
                          {
                              ID = objEntity.ID,
                              FullName = objEntity.FullName,
                              LastPasswordChange = objEntity.LastPasswordChange,
                              User_Mobile = objEntity.User_Mobile,
                              User_Email = objEntity.User_Email,
                              Password = objEntity.Password,
                              User_Type = objEntity.User_Type,
                              Android_Token = objEntity.Android_Token,
                              Email_Verified = objEntity.Email_Verified,
                              Verification_Code = objEntity.Verification_Code,
                              Verification_Expiry= objEntity.Verification_Expiry,
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

        public static UserBE GetUserBEByIDWithoutStatus(int id)
        {
            // Declare variables
            UserBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblUsers
                          where objEntity.ID == id 
                          select new UserBE
                          {
                              ID = objEntity.ID,
                              FullName = objEntity.FullName,
                              LastPasswordChange = objEntity.LastPasswordChange,
                              User_Mobile = objEntity.User_Mobile,
                              User_Email = objEntity.User_Email,
                              Password = objEntity.Password,
                              User_Type = objEntity.User_Type,
                              Android_Token = objEntity.Android_Token,
                              Email_Verified = objEntity.Email_Verified,
                              Verification_Code = objEntity.Verification_Code,
                              Verification_Expiry = objEntity.Verification_Expiry,
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

        public static UserBE GetUserByEmailAddress(string emailaddress)
        {
            // Declare variables
            UserBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblUsers
                          where  objEntity.User_Email == emailaddress
                          select new UserBE
                          {
                              ID = objEntity.ID,
                    
                              FullName = objEntity.FullName,
                              LastPasswordChange = objEntity.LastPasswordChange,
                              User_Mobile = objEntity.User_Mobile,
                              User_Email = objEntity.User_Email,
                              Password = objEntity.Password,
                              User_Type = objEntity.User_Type,
                              Android_Token = objEntity.Android_Token,
                              Email_Verified = objEntity.Email_Verified,
                              Verification_Code = objEntity.Verification_Code,
                              Verification_Expiry = objEntity.Verification_Expiry,
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

        public static UserBE GetUserBEByMobileNo(string mobileNo)
        {
            UserBE result = null;
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                result = (from objEntity in context.tblUsers
                          where objEntity.User_Mobile == mobileNo && objEntity.Is_Deleted == false
                          select new UserBE
                          {
                              ID = objEntity.ID,
                           
                              User_Mobile = objEntity.User_Mobile,
                              User_Email = objEntity.User_Email,
                              FullName = objEntity.FullName,
                              LastPasswordChange = objEntity.LastPasswordChange,
                              Password = objEntity.Password,
                              User_Type = objEntity.User_Type,
                              Android_Token = objEntity.Android_Token,
                              Email_Verified = objEntity.Email_Verified,
                              Verification_Code = objEntity.Verification_Code,
                              Verification_Expiry = objEntity.Verification_Expiry,
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

        public static UserBE GetUserBEByEmailPassword(string user_name, string password )
        {
            UserBE result = null;
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                result = (from objEntity in context.tblUsers
                          where objEntity.User_Email == user_name && objEntity.Is_Deleted == false && objEntity.Password == password 
                          select new UserBE
                          {
                              ID = objEntity.ID,
                              User_Mobile = objEntity.User_Mobile,
                              User_Email = objEntity.User_Email,
                              Password = objEntity.Password,
                              User_Type = objEntity.User_Type,
                              FullName = objEntity.FullName,
                              LastPasswordChange = objEntity.LastPasswordChange,
                              Android_Token = objEntity.Android_Token,
                              Email_Verified = objEntity.Email_Verified,
                              Verification_Code = objEntity.Verification_Code,
                              Verification_Expiry = objEntity.Verification_Expiry,
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

        public static List<UserBE> GetAllUserBEs()
        {
            // Declare variables
            List<UserBE> result = new List<UserBE>();
            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblUsers
                          where objEntity.Is_Deleted == false
                          orderby objEntity.ID
                          select new UserBE
                          {
                              ID = objEntity.ID,
                              User_Mobile = objEntity.User_Mobile,
                              User_Email = objEntity.User_Email,
                              FullName = objEntity.FullName,
                              LastPasswordChange = objEntity.LastPasswordChange,
                              Password = objEntity.Password,
                              User_Type = objEntity.User_Type,
                              Android_Token = objEntity.Android_Token,
                              Email_Verified = objEntity.Email_Verified,
                              Verification_Code = objEntity.Verification_Code,
                              Verification_Expiry = objEntity.Verification_Expiry,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<UserBE>();
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

        public static tblUser ConvertToLinqObject(UserBE objEntity)
        {
            // Declare variables
            tblUser result = new tblUser();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.FullName = objEntity.FullName;
                result.LastPasswordChange = objEntity.LastPasswordChange;
                result.User_Mobile = objEntity.User_Mobile;
                result.User_Email = objEntity.User_Email;
                result.Password = objEntity.Password;
                result.User_Type = objEntity.User_Type;
                result.Android_Token = objEntity.Android_Token;
                result.Email_Verified = objEntity.Email_Verified;
                result.Verification_Code = objEntity.Verification_Code;
                result.Verification_Expiry = objEntity.Verification_Expiry;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;

                if (objEntity.TimeStamp != null)
                {
                    result.TimeStamp = new System.Data.Linq.Binary(Convert.FromBase64String(objEntity.TimeStamp.ToString()));
                }
                result.Updated_Date = objEntity.Updated_Date;

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
