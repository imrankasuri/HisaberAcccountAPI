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
    public static class ProductMapDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(ProductMapBE   accountMapBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblProductMap   clinq = null;
                
                clinq  = ConvertToLinqObject(accountMapBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (accountMapBE.ID == 0)
                    {
                        accountMapBE.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblProductMaps.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblProductMaps.Attach(clinq, true);
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

        public static ProductMapBE GetProductMapByID(int id)
        {
            // Declare variables
            ProductMapBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProductMaps
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted== false
                          select new ProductMapBE 
                          {
                              ID = objEntity.ID,
                              OldProductID = objEntity.OldProductID,
                              NewProductID = objEntity.NewProductID,
                              Name = objEntity.Name,
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

        public static ProductMapBE GetProductByName(string accountName)
        {
            // Declare variables
            ProductMapBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProductMaps
                          where objEntity.Name.ToLower()  == accountName.ToLower() && objEntity.Is_Active == true && objEntity.Is_Deleted== false 

                          select new ProductMapBE
                          {
                              ID = objEntity.ID,
                              OldProductID = objEntity.OldProductID,
                              NewProductID = objEntity.NewProductID,
                              Name = objEntity.Name,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
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

        public static List<ProductMapBE> GetProductMapBEs()
        {
            // Declare variables
            List<ProductMapBE> result = new List<ProductMapBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Products from the database
                result = (from objEntity in context.tblProductMaps
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true 
                          orderby objEntity.ID
                          select new ProductMapBE
                          {
                              ID = objEntity.ID,
                              OldProductID = objEntity.OldProductID,
                              NewProductID = objEntity.NewProductID,
                              Name = objEntity.Name,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<ProductMapBE>();
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

        public static tblProductMap ConvertToLinqObject(ProductMapBE objEntity)
        {
            // Declare variables
            tblProductMap  result = new tblProductMap();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.OldProductID = objEntity.OldProductID;
                result.NewProductID = objEntity.NewProductID;
                result.Name = objEntity.Name;
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

        public static void DeleteProductMap()
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblProductMaps.DeleteAllOnSubmit(context.tblProductMaps.Where(c => c.ID > 0));
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
