using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using HAccounts.DAL;

namespace HAccounts.DAL
{
    public static class ExceptionDAL
    {
        public static int Save(ExceptionBE  exceptionBE)
        {

            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                tblException   clinq = null;

                clinq = ConvertToLinqObject(exceptionBE);

                try
                {
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (exceptionBE.ID == 0)
                    {
                        clinq.Created_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblExceptions.InsertOnSubmit(clinq);


                    }
                    else
                    {
                        // Existing Image Set Account - we are updating the database
                        // Add Image Set Account to datacontext
                        context.tblExceptions.Attach(clinq, true);
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

        public static ExceptionBE GetExceptionBEByID(int id)
        {
            // Declare variables
            ExceptionBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblExceptions
                          where objEntity.ID == id && objEntity.Is_Deleted == false
                          select new ExceptionBE
                          {
                              ID = objEntity.ID,
                              Dated = objEntity.Dated,
                              Event_Name = objEntity.Event_Name,
                              Exception_Details = objEntity.Exception_Details,
                              Message = objEntity.Message,
                              Module_Name= objEntity.Module_Name,
                              Page_URL = objEntity.Page_URL,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
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

        public static List<ExceptionBE> GetExceptionBEs()
        {
            // Declare variables
            List<ExceptionBE> result = new List<ExceptionBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblExceptions
                          where objEntity.Is_Deleted == false
                          orderby objEntity.ID
                          select new ExceptionBE
                          {
                              ID = objEntity.ID,
                              Dated = objEntity.Dated,
                              Event_Name = objEntity.Event_Name,
                              Exception_Details = objEntity.Exception_Details,
                              Message = objEntity.Message,
                              Module_Name = objEntity.Module_Name,
                              Page_URL = objEntity.Page_URL,
                              Created_Date = objEntity.Created_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Updated_Date = objEntity.Updated_Date,
                          }).ToList<ExceptionBE>();
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

        public static tblException ConvertToLinqObject(ExceptionBE objEntity)
        {
            // Declare variables
            tblException result = new tblException();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.Module_Name = objEntity.Module_Name;
                result.Page_URL = objEntity.Page_URL;
                result.Event_Name = objEntity.Event_Name;
                result.Message = objEntity.Message;
                result.Exception_Details = objEntity.Exception_Details;
                result.Dated = objEntity.Dated;
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

    }
}
