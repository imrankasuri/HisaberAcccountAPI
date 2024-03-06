using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class EmailLogDAL
    {

        public static long Save(EmailLogBE  emailLogBE)
        {
            // Declare variables
            long result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
               PPSLinqToSqlDataContext    context = new PPSLinqToSqlDataContext();
                
               tblEmailLog  clinq = null;
                clinq  = ConvertToLinqObject(emailLogBE);
                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);

                    if (emailLogBE.ID == 0)
                    {
                        // New company - we are adding to the database
                        // Update Created Date
                        clinq.Created_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                        // Add company record to datacontext
                        context.tblEmailLogs.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblEmailLogs.Attach(clinq, true);
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

        public static EmailLogBE GetEmailLogBEByID(int id)
        {
            // Declare variables
            EmailLogBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblEmailLogs
                          where objEntity.ID == id && objEntity.Is_Deleted == false 
                          select new EmailLogBE 
                          {
                              ID = objEntity.ID,
                              Email_To = objEntity.Email_To,
                              Email_From = objEntity.Email_From,
                              Subject = objEntity.Subject,
                              Reference = objEntity.Reference,
                              Event_Type = objEntity.Event_Type,
                              Delivery_Status = objEntity.Delivery_Status,
                              Dated = objEntity.Dated,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
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

        public static List<EmailLogBE> GetAllEmailLogBEs()
        {
            // Declare variables
            List<EmailLogBE> result = new List<EmailLogBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblEmailLogs
                          where objEntity.Is_Deleted == false
                          select new EmailLogBE
                          {
                              ID = objEntity.ID,
                              Email_To = objEntity.Email_To,
                              Email_From = objEntity.Email_From,
                              Subject = objEntity.Subject,
                              Reference = objEntity.Reference,
                              Event_Type = objEntity.Event_Type,
                              Delivery_Status = objEntity.Delivery_Status,
                              Dated = objEntity.Dated,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<EmailLogBE>();
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

        public static tblEmailLog ConvertToLinqObject(EmailLogBE objEntity)
        {
            // Declare variables
            tblEmailLog  result = new tblEmailLog();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.Email_To = objEntity.Email_To;
                result.Email_From = objEntity.Email_From;
                result.Subject = objEntity.Subject;
                result.Reference = objEntity.Reference;
                result.Event_Type = objEntity.Event_Type;
                result.Delivery_Status = objEntity.Delivery_Status;
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
