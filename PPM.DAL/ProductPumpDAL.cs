using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class ProductPumpDAL
    {

        public static int Save(ProductPumpBE product_PumpBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblProductPump      clinq = null;
                
                clinq  = ConvertToLinqObject(product_PumpBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (product_PumpBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblProductPumps.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblProductPumps.Attach(clinq, true);
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

        public static ProductPumpBE GetProductPumpBEByID(int id)
        {
            // Declare variables
            ProductPumpBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProductPumps 
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          where objEntity.ID == id 
                          select new ProductPumpBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Pump_No = objEntity.Pump_No,
                              Product_ID = objEntity.Product_ID,
                              Selected_Product = new ProductBE()
                              {
                                  Name = products.Name,
                                  ProductCode = products.ProductCode,
                                  ID = objEntity.Product_ID,
                              },
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

        public static ProductPumpBE GetProductPumpByPumpNumber(int pump_No, int pumpID)
        {
            // Declare variables
            ProductPumpBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProductPumps
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          where objEntity.Pump_No == pump_No && objEntity.PumpID == pumpID
                          select new ProductPumpBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Pump_No = objEntity.Pump_No,
                              Product_ID = objEntity.Product_ID,
                              Selected_Product = new ProductBE()
                              {
                                  Name = products.Name,
                                  ID = objEntity.Product_ID,
                              },
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

        public static List<ProductPumpBE> GetProductPumpBEs()
        {
            // Declare variables
            List<ProductPumpBE> result = new List<ProductPumpBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProductPumps
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true
                          orderby objEntity.ID 
                          select new ProductPumpBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Pump_No = objEntity.Pump_No,
                              Product_ID = objEntity.Product_ID,
                              Selected_Product = new ProductBE()
                              {
                                  Name = products.Name,
                                  ID = objEntity.Product_ID,
                              },
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())
                              
                          }).ToList<ProductPumpBE>();
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

        public static List<ProductPumpBE> GetProductPumpByPumpID(int pumpID)
        {
            // Declare variables
            List<ProductPumpBE> result = new List<ProductPumpBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProductPumps
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new ProductPumpBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Pump_No = objEntity.Pump_No,
                              Product_ID = objEntity.Product_ID,
                              Selected_Product = new ProductBE()
                              {
                                  Name = products.Name,
                                  ID = objEntity.Product_ID,
                              },
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<ProductPumpBE>();
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

        public static List<ProductPumpBE> GetProductPumpBEs(int Product_ID)
        {
            // Declare variables
            List<ProductPumpBE> result = new List<ProductPumpBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProductPumps
                          join products in context.tblProducts
                          on objEntity.Product_ID equals products.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Product_ID == Product_ID 
                          orderby objEntity.ID
                          select new ProductPumpBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Pump_No = objEntity.Pump_No,
                              Product_ID = objEntity.Product_ID,
                              Selected_Product = new ProductBE()
                              {
                                  Name = products.Name,
                                  ID = objEntity.Product_ID,
                              },
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())

                          }).ToList<ProductPumpBE>();
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

        public static tblProductPump ConvertToLinqObject(ProductPumpBE objEntity)
        {
            // Declare variables
            tblProductPump  result = new tblProductPump();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.Pump_No = objEntity.Pump_No;
                result.Product_ID = objEntity.Product_ID;
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


        public static void DeleteNozzle(int pumpID, int nozzleID)
        {
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    context.tblProductPumps.DeleteAllOnSubmit(context.tblProductPumps.Where(c => c.PumpID == pumpID && c.ID == nozzleID));
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
