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
    public static class TankDefinationDAL
    {
       
        public static int Save(TankDefinationBE tankDefinationBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblTankDefination   clinq = null;
                
                clinq  = ConvertToLinqObject(tankDefinationBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (tankDefinationBE.ID == 0)
                    {
                        tankDefinationBE.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblTankDefinations.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblTankDefinations.Attach(clinq, true);
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

        public static TankDefinationBE GetTankDefinationByID(int id, int pumpID)
        {
            // Declare variables
            TankDefinationBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblTankDefinations
                         
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted== false && objEntity.PumpID == pumpID
                          select new TankDefinationBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              TankNo = objEntity.TankNo,
                              ProductID = objEntity.ProductID,
                              TankFullCapacity = objEntity.TankFullCapacity,
                              UseableCapacity = objEntity.UseableCapacity,
                              TankSizeDetails = objEntity.TankSizeDeails,
                              TankShape = objEntity.TankShape,
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

        public static List<TankDefinationBE> GetTankDefinationsBEs(int pumpID)
        {
            // Declare variables
            List<TankDefinationBE> result = new List<TankDefinationBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblTankDefinations
                          join objProducts in context.tblProducts
                          on objEntity.ProductID equals objProducts.ID 
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new TankDefinationBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              TankNo = objEntity.TankNo,
                              ProductID = objEntity.ProductID,
                              TankFullCapacity = objEntity.TankFullCapacity,
                              UseableCapacity = objEntity.UseableCapacity,
                              TankSizeDetails = objEntity.TankSizeDeails,
                              TankShape = objEntity.TankShape,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              SelectedProduct = new ProductBE()
                              {
                                  ID= objEntity.ProductID,
                                  Name = objProducts.Name,
                                  ProductCode = objProducts.ProductCode,
                              },
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<TankDefinationBE>();
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

        public static tblTankDefination ConvertToLinqObject(TankDefinationBE objEntity)
        {
            // Declare variables
            tblTankDefination  result = new tblTankDefination();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.TankNo = objEntity.TankNo;
                result.ProductID = objEntity.ProductID;
                result.TankFullCapacity = objEntity.TankFullCapacity;
                result.UseableCapacity = objEntity.UseableCapacity;
                result.TankSizeDeails = objEntity.TankSizeDetails;
                result.TankShape = objEntity.TankShape;
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

        public static TankDefinationBE GetTankByTankNumber(int tankNumber, int pumpID)
        {
            // Declare variables
            TankDefinationBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblTankDefinations

                          where objEntity.TankNo == tankNumber && objEntity.Is_Active == true && objEntity.Is_Deleted == false && objEntity.PumpID == pumpID
                          select new TankDefinationBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              TankNo = objEntity.TankNo,
                              ProductID = objEntity.ProductID,
                              TankFullCapacity = objEntity.TankFullCapacity,
                              UseableCapacity = objEntity.UseableCapacity,
                              TankSizeDetails = objEntity.TankSizeDeails,
                              TankShape = objEntity.TankShape,
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


    }
}
