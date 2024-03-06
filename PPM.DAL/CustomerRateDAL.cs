using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class CustomerRateDAL
    {

        public static int Save(CustomerRateBE   customerRateBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
               tblCustomerRate   clinq = null;
                
                clinq  = ConvertToLinqObject(customerRateBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (customerRateBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblCustomerRates.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblCustomerRates.Attach(clinq, true);
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

        public static CustomerRateBE  GetCustomerRateByID(int id)
        {
            // Declare variables
            CustomerRateBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblCustomerRates
                          where objEntity.ID == id 
                          select new CustomerRateBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Customer_ID = objEntity.Customer_ID,
                              Product_ID = objEntity.Product_ID,
                              Selling_Rate = objEntity.Selling_Rate,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())
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

        public static List<CustomerRateBE> GetAllCustomerRateBEs(int pumpID)
        {
            // Declare variables
            List<CustomerRateBE> result = new List<CustomerRateBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblCustomerRates 
                          where objEntity.PumpID == pumpID
                          orderby objEntity.ID 
                          select new CustomerRateBE
                          {

                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Customer_ID = objEntity.Customer_ID,
                              Product_ID = objEntity.Product_ID,
                              Selling_Rate = objEntity.Selling_Rate,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())
                              
                          }).ToList<CustomerRateBE>();
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

        public static List<CustomerRateBE> GetCustomerRateBEByCustomerID(int customerID, int pumpID)
        {
            // Declare variables
            List<CustomerRateBE> result = new List<CustomerRateBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblCustomerRates
                          join product in context.tblProducts
                          on objEntity.Product_ID equals product.ID
                          where objEntity.Customer_ID == customerID && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new CustomerRateBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Customer_ID = objEntity.Customer_ID,
                              Product_ID = objEntity.Product_ID,
                              Selling_Rate = objEntity.Selling_Rate,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Selected_Product = new ProductBE()
                              {
                                  ID = objEntity.Product_ID ?? 0,
                                  Name = product.Name,
                                  Sale_Price = product.Sale_Price,
                              },
                              Selected_Account = new AccountBE()
                              {
                                  ID = objEntity.Customer_ID,
                              }
                          }).ToList<CustomerRateBE>();
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

        public static List<CustomerRateBE> GetCustomerRateBEByProductID(int productID, int pumpID)
        {
            // Declare variables
            List<CustomerRateBE> result = new List<CustomerRateBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblCustomerRates
                          join product in context.tblProducts
                          on objEntity.Product_ID equals product.ID
                          where objEntity.Product_ID == productID && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new CustomerRateBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Customer_ID = objEntity.Customer_ID,
                              Product_ID = objEntity.Product_ID,
                              Selling_Rate = objEntity.Selling_Rate,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Selected_Product = new ProductBE()
                              {
                                  ID = objEntity.Product_ID ?? 0,
                                  Name = product.Name,
                                  Sale_Price = product.Sale_Price,
                              },
                              Selected_Account = new AccountBE()
                              {
                                  ID = objEntity.Customer_ID,
                              }
                          }).ToList<CustomerRateBE>();
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

        public static tblCustomerRate ConvertToLinqObject(CustomerRateBE objEntity)
        {
            // Declare variables
            tblCustomerRate  result = new tblCustomerRate();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.PumpCode = objEntity.PumpCode;
                result.Customer_ID = objEntity.Customer_ID;
                result.Product_ID = objEntity.Product_ID;
                result.Selling_Rate = objEntity.Selling_Rate;
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

        public static void DeleteCustomerRateByCustomerID(int id)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblCustomerRates.DeleteAllOnSubmit(context.tblCustomerRates.Where(c => c.Customer_ID == id));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }

        public static CustomerRateBE GetCustomerRateByCustomerIDProductID(int customerID, int ProductID, int pumpID)
        {
            // Declare variables
            CustomerRateBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblCustomerRates
                          where objEntity.Customer_ID == customerID && objEntity.Product_ID == ProductID  && objEntity.PumpID == pumpID
                          select new CustomerRateBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Customer_ID = objEntity.Customer_ID,
                              Product_ID = objEntity.Product_ID,
                              Selling_Rate = objEntity.Selling_Rate,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())
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

        public static void DeleteCustomerRate(int id)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblCustomerRates.DeleteAllOnSubmit(context.tblCustomerRates.Where(c => c.Customer_ID == id));
                    context.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                scope.Complete();
            }
        }
    }
}
