using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class FuelProviderDAL
    {

        public static int Save(FuelProviderBE   fuelProviderBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
               tblFuleProvider        clinq = null;
                
                clinq  = ConvertToLinqObject(fuelProviderBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (fuelProviderBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblFuleProviders.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblFuleProviders.Attach(clinq, true);
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

        public static FuelProviderBE GetFuelProviderByID(int id)
        {
            // Declare variables
            FuelProviderBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblFuleProviders
                          where objEntity.ID == id 
                          select new FuelProviderBE 
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Title = objEntity.Logo_Title,
                              Logo_Reports = objEntity.Logo_Reports,
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

        public static FuelProviderBE GetFuelProviderByName(string companyName)
        {
            // Declare variables
            FuelProviderBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblFuleProviders
                          where objEntity.Name.ToLower() == companyName.ToLower()
                          select new FuelProviderBE
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Title = objEntity.Logo_Title,
                              Logo_Reports = objEntity.Logo_Reports,
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

        public static List<FuelProviderBE> GetFuelProviderss()
        {
            // Declare variables
            List<FuelProviderBE> result = new List<FuelProviderBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblFuleProviders
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true
                          orderby objEntity.ID 
                          select new FuelProviderBE
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Title = objEntity.Logo_Title,
                              Logo_Reports = objEntity.Logo_Reports,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<FuelProviderBE>();
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

        public static tblFuleProvider ConvertToLinqObject(FuelProviderBE objEntity)
        {
            // Declare variables
            tblFuleProvider  result = new tblFuleProvider();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.Name = objEntity.Name;
                result.Logo_Login = objEntity.Logo_Login;
                result.Logo_Title = objEntity.Logo_Title;
                result.Logo_Reports = objEntity.Logo_Reports;
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
