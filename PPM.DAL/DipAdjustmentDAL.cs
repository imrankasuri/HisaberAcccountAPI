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
    public static class DipAdjustmentDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(DIPAdjustmentBE    dIPAdjustmentBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblDIPAdjustment   clinq = null;
                
                clinq  = ConvertToLinqObject(dIPAdjustmentBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (dIPAdjustmentBE.ID == 0)
                    {
                        dIPAdjustmentBE.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblDIPAdjustments.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        context.tblDIPAdjustments.Attach(clinq, true);
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

        public static DIPAdjustmentBE GetDipAdjustmentByID(int id, int pumpID)
        {
            // Declare variables
            DIPAdjustmentBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblDIPAdjustments
                         
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted== false && objEntity.PumpID == pumpID
                          select new DIPAdjustmentBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              ProductID = objEntity.ProductID,
                              TotalPhysicalStock = objEntity.TotalPhysicalStock,
                              TotalSystemStock = objEntity.TotalSystemStock,
                              DifferenceQuantity = objEntity.DifferenceQuantity,
                              AdjustmentRate = objEntity.AdjustmentRate,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
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

        public static List<DIPAdjustmentBE> GetDipAdjustmentsBEs(int pumpID)
        {
            // Declare variables
            List<DIPAdjustmentBE> result = new List<DIPAdjustmentBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblDIPAdjustments
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new DIPAdjustmentBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              ProductID = objEntity.ProductID,
                              TotalPhysicalStock = objEntity.TotalPhysicalStock,
                              TotalSystemStock = objEntity.TotalSystemStock,
                              DifferenceQuantity = objEntity.DifferenceQuantity,
                              AdjustmentRate = objEntity.AdjustmentRate,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<DIPAdjustmentBE>();
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

        public static List<DIPAdjustmentBE> GetDipAdjustmentBEs(int pumpID, DateTime selectedDate)
        {
            // Declare variables
            List<DIPAdjustmentBE> result = new List<DIPAdjustmentBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblDIPAdjustments
                          join product in context.tblProducts
                          on objEntity.ProductID equals product.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Dated == selectedDate.Date
                          orderby objEntity.ID
                          select new DIPAdjustmentBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              ProductID = objEntity.ProductID,
                              TotalPhysicalStock = objEntity.TotalPhysicalStock,
                              TotalSystemStock = objEntity.TotalSystemStock,
                              DifferenceQuantity = objEntity.DifferenceQuantity,
                              AdjustmentRate = objEntity.AdjustmentRate,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              SelectedProduct = new ProductBE()
                              {
                                  ID = product.ID,
                                  ProductCode = product.ProductCode,
                                  Name = product.Name,
                              },
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<DIPAdjustmentBE>();
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

        public static List<DIPAdjustmentBE> GetDipAdjustmentBEs(int pumpID, DateTime startDate, DateTime endDate)
        {
            // Declare variables
            List<DIPAdjustmentBE> result = new List<DIPAdjustmentBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblDIPAdjustments
                          join product in context.tblProducts
                          on objEntity.ProductID equals product.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID && objEntity.Dated >= startDate.Date && objEntity.Dated <= endDate.Date
                          orderby objEntity.ID
                          select new DIPAdjustmentBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              ProductID = objEntity.ProductID,
                              TotalPhysicalStock = objEntity.TotalPhysicalStock,
                              TotalSystemStock = objEntity.TotalSystemStock,
                              DifferenceQuantity = objEntity.DifferenceQuantity,
                              AdjustmentRate = objEntity.AdjustmentRate,
                              Dated = objEntity.Dated,
                              IsPosted = objEntity.IsPosted,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              AdjustmentAmount = objEntity.AdjustmentRate * objEntity.DifferenceQuantity,
                              SelectedProduct = new ProductBE()
                              {
                                  ID = product.ID,
                                  ProductCode = product.ProductCode,
                                  Name = product.Name,
                              },
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<DIPAdjustmentBE>();
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

        public static tblDIPAdjustment ConvertToLinqObject(DIPAdjustmentBE objEntity)
        {
            // Declare variables
            tblDIPAdjustment  result = new tblDIPAdjustment();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.ProductID = objEntity.ProductID;
                result.TotalPhysicalStock = objEntity.TotalPhysicalStock;
                result.TotalSystemStock = objEntity.TotalSystemStock;
                result.DifferenceQuantity = objEntity.DifferenceQuantity;
                result.AdjustmentRate = objEntity.AdjustmentRate;
                result.Dated = objEntity.Dated;
                result.IsPosted = objEntity.IsPosted;
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

        public static void DeleteAdjustment(int adjustmentId, int pumpID)
        {
            int result = 0;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("DeleteDipAdjustment", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters.Add("@Adjustment_ID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = adjustmentId;
            try
            {
                sqlCon.Open();
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}
