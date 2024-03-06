using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

using System.Data.SqlClient;
using System.Data;

namespace HAccounts.DAL
{
    public static class DefaultProductDAL
    {
        private static string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString; 

        public static int Save(DefaultProductBE defaultProductBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblDefaultProduct    clinq = null;
                
                clinq  = ConvertToLinqObject(defaultProductBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (defaultProductBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblDefaultProducts.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblDefaultProducts.Attach(clinq, true);
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

        public static DefaultProductBE GetDefaultProductByID(int id)
        {
            // Declare variables
            DefaultProductBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblDefaultProducts
                          join measureUnit in context.tblMeasureUnits
                          on objEntity.MeasureUnitID equals measureUnit.ID
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted== false
                          select new DefaultProductBE
                          {
                              ID = objEntity.ID,
                              ProductCode = objEntity.ProductCode,
                              Name = objEntity.Name,
                              MeasureUnitID = objEntity.MeasureUnitID,
                              Description = objEntity.Description,
                              Sale_Price = objEntity.Sale_Price,
                              Last_Purchase_Price = objEntity.Last_Purchase_Price,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Measure_Unit_BE = new MeasureUnitBE()
                              {
                                  ID = objEntity.MeasureUnitID,
                                  Name = objEntity.Name,
                              },
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

        public static List<DefaultProductBE> GetDefaultProducts()
        {
            // Declare variables
            List<DefaultProductBE> result = new List<DefaultProductBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblDefaultProducts
                          join measureUnit in context.tblMeasureUnits
                          on objEntity.MeasureUnitID equals measureUnit.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true
                          orderby objEntity.ID 
                          select new DefaultProductBE
                          {
                              ID = objEntity.ID,
                              ProductCode = objEntity.ProductCode,
                              Name = objEntity.Name,
                              MeasureUnitID = objEntity.MeasureUnitID,
                              Description = objEntity.Description,
                              Sale_Price = objEntity.Sale_Price,
                              Last_Purchase_Price = objEntity.Last_Purchase_Price,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Measure_Unit_BE = new MeasureUnitBE()
                              {
                                  ID = objEntity.MeasureUnitID,
                                  Name = objEntity.Name,
                              },
                          }).ToList<DefaultProductBE>();
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

        public static tblDefaultProduct ConvertToLinqObject(DefaultProductBE objEntity)
        {
            // Declare variables
            tblDefaultProduct  result = new tblDefaultProduct();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.ProductCode = objEntity.ProductCode;
                result.Name = objEntity.Name;
                result.MeasureUnitID = objEntity.MeasureUnitID;
                result.Description = objEntity.Description;
                result.Sale_Price = objEntity.Sale_Price;
                result.Last_Purchase_Price = objEntity.Last_Purchase_Price;
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
