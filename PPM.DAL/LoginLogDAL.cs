using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class LoginLogDAL
    {

        public static long Save(LoginLogBE loginLogBE)
        {
            // Declare variables
            long result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
               PPSLinqToSqlDataContext    context = new PPSLinqToSqlDataContext();
                
                tblLoginLog  clinq = null;
                clinq  = ConvertToLinqObject(loginLogBE);
                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);

                    if (loginLogBE.ID == 0)
                    {
                        // New company - we are adding to the database
                        // Update Created Date
                        clinq.Created_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                        // Add company record to datacontext
                        context.tblLoginLogs.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblLoginLogs.Attach(clinq, true);
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

        public static LoginLogBE GetLoginLogBEByID(int id)
        {
            // Declare variables
            LoginLogBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblLoginLogs
                          where objEntity.ID == id && objEntity.Is_Deleted == false 
                          select new LoginLogBE 
                          {
                              ID = objEntity.ID,
                              User_Name = objEntity.User_Name,
                              Password = objEntity.Password,
                              Is_Success = objEntity.Is_Success,
                              IP_Address = objEntity.IP_Address,
                              User_Type = objEntity.User_Type,
                              Login_Source = objEntity.Login_Source,
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

        public static LoginLogBE GetLastLoginLogBE(string userType, string userName)
        {
            // Declare variables
            LoginLogBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblLoginLogs
                          where objEntity.User_Type == userType && objEntity.Is_Deleted == false && objEntity.User_Name == userName && objEntity.Is_Success == true 
                          orderby objEntity.ID descending 
                          select new LoginLogBE
                          {
                              ID = objEntity.ID,
                              User_Name = objEntity.User_Name,
                              Password = objEntity.Password,
                              Is_Success = objEntity.Is_Success,
                              IP_Address = objEntity.IP_Address,
                              User_Type = objEntity.User_Type,
                              Login_Source = objEntity.Login_Source,
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

        public static List<LoginLogBE> GetAllLoginLogBEs()
        {
            // Declare variables
            List<LoginLogBE> result = new List<LoginLogBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblLoginLogs
                          where objEntity.Is_Deleted == false
                          select new LoginLogBE
                          {
                              ID = objEntity.ID,
                              User_Name = objEntity.User_Name,
                              Password = objEntity.Password,
                              Is_Success = objEntity.Is_Success,
                              IP_Address = objEntity.IP_Address,
                              User_Type = objEntity.User_Type,
                              Login_Source = objEntity.Login_Source,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<LoginLogBE>();
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

        public static List<LoginLogBE> GetAllLoginLogBEs(string User_Type)
        {
            // Declare variables
            List<LoginLogBE> result = new List<LoginLogBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblLoginLogs
                          where objEntity.Is_Deleted == false && objEntity.User_Type== User_Type 
                          select new LoginLogBE
                          {
                              ID = objEntity.ID,
                              User_Name = objEntity.User_Name,
                              Password = objEntity.Password,
                              Is_Success = objEntity.Is_Success,
                              IP_Address = objEntity.IP_Address,
                              User_Type = objEntity.User_Type,
                              Login_Source = objEntity.Login_Source,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<LoginLogBE>();
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

        public static List<LoginLogBE> GetAllLoginLogBEs(string User_Type, string UserName)
        {
            // Declare variables
            List<LoginLogBE> result = new List<LoginLogBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblLoginLogs
                          where objEntity.Is_Deleted == false && objEntity.User_Type == User_Type && objEntity.User_Name == UserName 
                          orderby objEntity.ID descending
                          select new LoginLogBE
                          {
                              ID = objEntity.ID,
                              User_Name = objEntity.User_Name,
                              Password = objEntity.Password,
                              Is_Success = objEntity.Is_Success,
                              IP_Address = objEntity.IP_Address,
                              User_Type = objEntity.User_Type,
                              Login_Source = objEntity.Login_Source,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<LoginLogBE>();
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

        public static List<LoginLogBE> GetTopLoginLogBEs(string User_Type, string UserName)
        {
            // Declare variables
            List<LoginLogBE> result = new List<LoginLogBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblLoginLogs
                          where objEntity.Is_Deleted == false && objEntity.User_Type == User_Type && objEntity.User_Name == UserName
                          orderby objEntity.ID descending
                          select new LoginLogBE
                          {
                              ID = objEntity.ID,
                              User_Name = objEntity.User_Name,
                              Password = objEntity.Password,
                              Is_Success = objEntity.Is_Success,
                              IP_Address = objEntity.IP_Address,
                              User_Type = objEntity.User_Type,
                              Login_Source = objEntity.Login_Source,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).Take(2).ToList<LoginLogBE>();
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

        public static tblLoginLog ConvertToLinqObject(LoginLogBE objEntity)
        {
            // Declare variables
            tblLoginLog  result = new tblLoginLog();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.User_Name = objEntity.User_Name;
                result.Password = objEntity.Password;
                result.Is_Success = objEntity.Is_Success;
                result.IP_Address = objEntity.IP_Address;
                result.User_Type = objEntity.User_Type;
                result.Login_Source = objEntity.Login_Source;
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
