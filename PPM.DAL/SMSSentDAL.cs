using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class SMSSentDAL
    {

        public static long Save(SMSSentBE sMSSentBE)
        {
            // Declare variables
            long result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

                tblSMSSent clinq = null;
                // Update company values with values from incoming company object
                clinq = ConvertToLinqObject(sMSSentBE);

                try
                {
                    // Update Updated Date

                    if (sMSSentBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        clinq.Updated_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblSMSSents.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        // Existing company - we are updating the database
                        // Add company details to datacontext
                        clinq.Updated_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblSMSSents.Attach(clinq, true);
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

        public static SMSSentBE GetSMSSentBEByID(long id)
        {
            // Declare variables
            SMSSentBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblSMSSents
                          where objEntity.ID == id
                          select new SMSSentBE
                          {
                              ID = objEntity.ID,
                              SMS_Mask = objEntity.SMS_Mask,
                              SMS_Text = objEntity.SMS_Text,
                              SMS_TO = objEntity.SMS_TO,
                              Sender_Department = objEntity.Sender_Department,
                              Sender_Name = objEntity.Sender_Name,
                              Sender_ID = objEntity.Sender_ID,
                              Is_Approved = objEntity.Is_Approved,
                              Is_Delivered = objEntity.Is_Delivered,
                              Error_Code = objEntity.Error_Code,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Service_Provider = objEntity.Service_Provider,
                              Response_Details= objEntity.Response_Details,
                              Reference_No = objEntity.Reference_No,
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

        public static List<SMSSentBE> GetAllSMSSentBE()
        {
            // Declare variables
            List<SMSSentBE> result = new List<SMSSentBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSMSSents
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true
                          orderby objEntity.ID descending 
                          select new SMSSentBE
                          {
                              ID = objEntity.ID,
                              SMS_Mask = objEntity.SMS_Mask,
                              SMS_Text = objEntity.SMS_Text,
                              SMS_TO = objEntity.SMS_TO,
                              Sender_Department = objEntity.Sender_Department,
                              Sender_Name = objEntity.Sender_Name,
                              Sender_ID = objEntity.Sender_ID,
                              Is_Approved = objEntity.Is_Approved,
                              Is_Delivered = objEntity.Is_Delivered,
                              Error_Code = objEntity.Error_Code,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Reference_No = objEntity.Reference_No,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Service_Provider = objEntity.Service_Provider,
                              Response_Details = objEntity.Response_Details,
                          }).ToList<SMSSentBE>();
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

        public static tblSMSSent ConvertToLinqObject(SMSSentBE objEntity)
        {
            // Declare variables
            tblSMSSent result = new tblSMSSent();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.SMS_Mask = objEntity.SMS_Mask;
                result.SMS_Text = objEntity.SMS_Text;
                result.SMS_TO = objEntity.SMS_TO;
                result.Reference_No = objEntity.Reference_No;
                result.Sender_Department = objEntity.Sender_Department;
                result.Sender_Name = objEntity.Sender_Name;
                result.Sender_ID = objEntity.Sender_ID;
                result.Is_Approved = objEntity.Is_Approved;
                result.Is_Delivered = objEntity.Is_Delivered;
                result.Error_Code = objEntity.Error_Code;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;
                result.Response_Details = objEntity.Response_Details;
                result.Service_Provider = objEntity.Service_Provider;
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
