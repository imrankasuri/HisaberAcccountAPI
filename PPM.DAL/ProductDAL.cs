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
    public static class ProductDAL
    {
        private static string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString; 

        public static int Save(ProductBE productBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblProduct    clinq = null;
                
                clinq  = ConvertToLinqObject(productBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (productBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblProducts.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblProducts.Attach(clinq, true);
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

        public static ProductBE GetProductByID(int id)
        {
            // Declare variables
            ProductBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProducts
                          join measureUnit in context.tblMeasureUnits
                          on objEntity.MeasureUnitID equals measureUnit.ID
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted== false
                          select new ProductBE
                          {
                              ID = objEntity.ID,
                              ProductCode = objEntity.ProductCode,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              MeasureUnitID = objEntity.MeasureUnitID,
                              Description = objEntity.Description,
                              Sale_Price = objEntity.Sale_Price,
                              Last_Purchase_Price = objEntity.Last_Purchase_Price,
                              Is_Default = objEntity.Is_Default,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Measure_Unit_BE = new MeasureUnitBE()
                              {
                                  ID = objEntity.MeasureUnitID,
                                  Name = measureUnit.Name,
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

        public static ProductBE GetProductByID(int id, int pumpID)
        {
            // Declare variables
            ProductBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProducts
                          join measureUnit in context.tblMeasureUnits
                          on objEntity.MeasureUnitID equals measureUnit.ID
                          where objEntity.ID == id && objEntity.Is_Active == true && objEntity.Is_Deleted == false && objEntity.PumpID == pumpID
                          select new ProductBE
                          {
                              ID = objEntity.ID,
                              ProductCode = objEntity.ProductCode,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              MeasureUnitID = objEntity.MeasureUnitID,
                              Description = objEntity.Description,
                              Sale_Price = objEntity.Sale_Price,
                              Last_Purchase_Price = objEntity.Last_Purchase_Price,
                              Is_Default = objEntity.Is_Default,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Measure_Unit_BE = new MeasureUnitBE()
                              {
                                  ID = objEntity.MeasureUnitID,
                                  Name = measureUnit.Name,
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

        public static ProductBE GetProductByPumpIDProductCode(int PumpId, string productCode)
        {
            // Declare variables
            ProductBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProducts
                          join measureUnit in context.tblMeasureUnits
                          on objEntity.MeasureUnitID equals measureUnit.ID
                          where objEntity.ProductCode.ToLower() == productCode.ToLower() && objEntity.PumpID == PumpId && objEntity.Is_Active == true && objEntity.Is_Deleted == false
                          select new ProductBE
                          {
                              ID = objEntity.ID,
                              ProductCode = objEntity.ProductCode,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              MeasureUnitID = objEntity.MeasureUnitID,
                              Description = objEntity.Description,
                              Sale_Price = objEntity.Sale_Price,
                              Last_Purchase_Price = objEntity.Last_Purchase_Price,
                              Is_Default = objEntity.Is_Default,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Measure_Unit_BE = new MeasureUnitBE()
                              {
                                  ID = objEntity.MeasureUnitID,
                                  Name = measureUnit.Name,
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

        public static ProductBE GetProductByPumpIDProductName(int PumpId, string productName)
        {
            // Declare variables
            ProductBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProducts
                          join measureUnit in context.tblMeasureUnits
                          on objEntity.MeasureUnitID equals measureUnit.ID
                          where objEntity.Name.ToLower() == productName.ToLower() && objEntity.PumpID == PumpId && objEntity.Is_Active == true && objEntity.Is_Deleted == false
                          select new ProductBE
                          {
                              ID = objEntity.ID,
                              ProductCode = objEntity.ProductCode,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              MeasureUnitID = objEntity.MeasureUnitID,
                              Description = objEntity.Description,
                              Sale_Price = objEntity.Sale_Price,
                              Last_Purchase_Price = objEntity.Last_Purchase_Price,
                              Is_Default = objEntity.Is_Default,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Measure_Unit_BE = new MeasureUnitBE()
                              {
                                  ID = objEntity.MeasureUnitID,
                                  Name = measureUnit.Name,
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

        public static List<ProductBE> GetProductBEs(int pumpID)
        {
            // Declare variables
            List<ProductBE> result = new List<ProductBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProducts
                          join measureUnit in context.tblMeasureUnits
                          on objEntity.MeasureUnitID equals measureUnit.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new ProductBE
                          {
                              ID = objEntity.ID,
                              ProductCode = objEntity.ProductCode,
                              PumpID = objEntity.PumpID,
                              Name = objEntity.Name,
                              MeasureUnitID = objEntity.MeasureUnitID,
                              Description = objEntity.Description,
                              Sale_Price = objEntity.Sale_Price,
                              Last_Purchase_Price = objEntity.Last_Purchase_Price,
                              Is_Default = objEntity.Is_Default,
                              Is_Deleted = objEntity.Is_Deleted,
                              Is_Active = objEntity.Is_Active,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Measure_Unit_BE = new MeasureUnitBE()
                              {
                                  ID = objEntity.MeasureUnitID,
                                  Name = measureUnit.Name,
                              },
                          }).ToList<ProductBE>();
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

        public static tblProduct ConvertToLinqObject(ProductBE objEntity)
        {
            // Declare variables
            tblProduct  result = new tblProduct();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.ProductCode = objEntity.ProductCode;
                result.PumpID = objEntity.PumpID;
                result.Name = objEntity.Name;
                result.MeasureUnitID = objEntity.MeasureUnitID;
                result.Description = objEntity.Description;
                result.Sale_Price = objEntity.Sale_Price;
                result.Last_Purchase_Price = objEntity.Last_Purchase_Price;
                result.Is_Default = objEntity.Is_Default;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.Is_Active = objEntity.Is_Active;
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

        public static List<ProductBE> GetProductsByPumpID(int pumpID)
        {
            List<ProductBE> result = new List<ProductBE>();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetProductsByPumpID", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters[0].Value = pumpID;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    ProductBE n = new ProductBE();
                    n.ID = Convert.ToInt32(dr["ID"]);
                    n.ProductCode = Convert.ToString(dr["ProductCode"]);
                    n.MeasureUnitID = Convert.ToInt32(dr["MeasureUnitID"]);
                    n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Name = Convert.ToString(dr["Name"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.Sale_Price = Convert.ToDecimal(dr["Sale_Price"]);
                    n.Last_Purchase_Price = Convert.ToDecimal(dr["Last_Purchase_Price"]);
                    n.Is_Default = Convert.ToBoolean(dr["Is_Default"]);
                    n.Is_Active = Convert.ToBoolean(dr["Is_Active"]);
                    n.Is_Deleted = Convert.ToBoolean(dr["Is_Deleted"]);
                    
                    n.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    n.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]);
                    n.TimeStamp = Convert.ToString(dr["TimeStamp"]);
                    n.Measure_Unit_BE = new MeasureUnitBE();
                    n.Measure_Unit_BE.Name = Convert.ToString(dr["MeasureUnitName"]);
                    n.Measure_Unit_BE.ID = Convert.ToInt32(dr["MeasureUnitID"]);
                    if (dr["Balance"] != System.DBNull.Value)
                    {
                        n.Balance = Convert.ToDecimal(dr["Balance"]);
                        n.BalanceType = Convert.ToString(dr["BalanceType"]);
                    }
                    else
                    {
                        n.Balance = 0;
                        n.BalanceType = "Debit";
                    }
                    result.Add(n);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                sqlCon.Close();
            }
            return result;
        }

        public static List<ProductBE> GetProductsByPumpIDonDate(int pumpID, DateTime endDate)
        {
            List<ProductBE> result = new List<ProductBE>();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetProductBalanceOnDate", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Pump_ID", SqlDbType.Int);
            cmd.Parameters.Add("@Transaction_Date", SqlDbType.Date);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = endDate;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    ProductBE n = new ProductBE();
                    n.ID = Convert.ToInt32(dr["ID"]);
                    n.ProductCode = Convert.ToString(dr["ProductCode"]);
                    n.MeasureUnitID = Convert.ToInt32(dr["MeasureUnitID"]);
                    n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Name = Convert.ToString(dr["Name"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.Sale_Price = Convert.ToDecimal(dr["Sale_Price"]);
                    n.Last_Purchase_Price = Convert.ToDecimal(dr["Last_Purchase_Price"]);
                    n.Is_Default = Convert.ToBoolean(dr["Is_Default"]);
                    n.Is_Active = Convert.ToBoolean(dr["Is_Active"]);
                    n.Is_Deleted = Convert.ToBoolean(dr["Is_Deleted"]);

                    n.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    n.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]);
                    n.TimeStamp = Convert.ToString(dr["TimeStamp"]);
                    n.Measure_Unit_BE = new MeasureUnitBE();
                    n.Measure_Unit_BE.Name = Convert.ToString(dr["MeasureUnitName"]);
                    n.Measure_Unit_BE.ID = Convert.ToInt32(dr["MeasureUnitID"]);
                    if (dr["Balance"] != System.DBNull.Value)
                    {
                        n.Balance = Convert.ToDecimal(dr["Balance"]);
                        n.BalanceType = Convert.ToString(dr["BalanceType"]);
                    }
                    else
                    {
                        n.Balance = 0;
                        n.BalanceType = "Debit";
                    }
                    result.Add(n);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                sqlCon.Close();
            }
            return result;
        }

        public static int updateProductLedger(int productID)
        {
            int result = 1;
            //SqlConnection sqlCon = new SqlConnection(ConString);
            //SqlCommand cmd = new SqlCommand("SP_AdjustProductLedger", sqlCon);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@Product_ID", SqlDbType.Int);
            //cmd.Parameters[0].Value = productID;
            //try
            //{
            //    sqlCon.Open();
            //    result = cmd.ExecuteNonQuery();

            //}
            //catch (Exception exp)
            //{
            //    throw exp;
            //}
            //finally
            //{
            //    sqlCon.Close();
            //}
            return result;
        }

        public static void DeleteDIPRecord(int pumpID, DateTime selectedDate)
        {
            int result = 0;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("DeleteDIPRecord", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters.Add("@Dated", SqlDbType.Date);
            cmd.Parameters[0].Value = pumpID;
            cmd.Parameters[1].Value = selectedDate.Date;
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
            List<ProductBE> ListofProducts = ProductDAL.GetProductsByPumpID(pumpID);
            if (ListofProducts != null && ListofProducts.Count > 0)
            {
                foreach (ProductBE ac in ListofProducts)
                {
                    ProductDAL.updateProductLedger(ac.ID);
                }
            }
        }

        public static List<ProductBE> GetProductSoldBetweenDates(int pumpID, DateTime startDate, DateTime endDate)
        {
            List<ProductBE> result = new List<ProductBE>();
            string sqlQuery = @"SELECT distinct dbo.tblSaleInvoiceDetails.Product_ID FROM     dbo.tblSaleInvoiceDetails INNER JOIN
                                dbo.tblSaleInvoiceHead ON dbo.tblSaleInvoiceDetails.Invoice_ID = dbo.tblSaleInvoiceHead.ID
                                where dbo.tblSaleInvoiceHead.PumpID =" + pumpID + @"and dbo.tblSaleInvoiceHead.Dated between '" + startDate.Date.ToString("yyyy-MM-dd") + "' and '" + endDate.ToString("yyyy-MM-dd") + "';";
            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand(sqlQuery, sqlCon);
            cmd.CommandType = CommandType.Text;
            
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    ProductBE n = new ProductBE();
                    n  = ProductDAL.GetProductByID(Convert.ToInt32(dr["Product_ID"]));
                    result.Add(n);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                sqlCon.Close();
            }
            return result;
        }

    }
}
