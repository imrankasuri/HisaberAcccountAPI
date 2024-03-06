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
    public static class ProductLedgerDAL
    {
        public static string ConString = ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(ProductLedgerBE productLedgerBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

                tblProductLedger clinq = null;
                
                clinq  = ConvertToLinqObject(productLedgerBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (productLedgerBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblProductLedgers.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblProductLedgers.Attach(clinq, true);
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
            //int noofRecord = ProductDAL.updateProductLedger(productLedgerBE.Product_ID);
            return result;
        }

        public static ProductLedgerBE GetProductLedgerByID(int id)
        {
            // Declare variables
            ProductLedgerBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProductLedgers
                          join product in context.tblProducts
                          on objEntity.Product_ID equals product.ID
                          where objEntity.ID == id 
                          select new ProductLedgerBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Product_ID = objEntity.Product_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Reference_ID = objEntity.Reference_ID,
                              Reference_Type = objEntity.Reference_Type,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Description = objEntity.Description,
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Selected_Product = new ProductBE()
                              {
                                   ID = objEntity.Product_ID,
                                   Name = product.Name 
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

        public static List<ProductLedgerBE> GetProductLedgerBEs()
        {
            // Declare variables
            List<ProductLedgerBE> result = new List<ProductLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProductLedgers 
                          join product in context.tblProducts
                          on objEntity.Product_ID equals product.ID
                          where objEntity.Is_Deleted==false && objEntity.Is_Active==true
                          orderby objEntity.ID 
                          select new ProductLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Product_ID = objEntity.Product_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Reference_ID = objEntity.Reference_ID,
                              Reference_Type = objEntity.Reference_Type,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Description = objEntity.Description,
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Selected_Product = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = product.Name
                              },
                          }).ToList<ProductLedgerBE>();
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

        public static List<ProductLedgerBE> GetLedger(int productID, DateTime dated)
        {
            // Declare variables
            List<ProductLedgerBE> result = new List<ProductLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProductLedgers
                          join product in context.tblProducts
                          on objEntity.Product_ID equals product.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Product_ID == productID && objEntity.Transaction_Date >= dated
                          orderby objEntity.ID
                          select new ProductLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Product_ID = objEntity.Product_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Reference_ID = objEntity.Reference_ID,
                              Reference_Type = objEntity.Reference_Type,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Description = objEntity.Description,
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Selected_Product = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = product.Name
                              },
                          }).ToList<ProductLedgerBE>();
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

        public static List<ProductLedgerBE> GetProductLedger(int productID, DateTime startDate, DateTime endDate)
        {
            // Declare variables
            List<ProductLedgerBE> result = new List<ProductLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProductLedgers
                          join product in context.tblProducts
                          on objEntity.Product_ID equals product.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Product_ID == productID && objEntity.Transaction_Date.Date >= startDate && objEntity.Transaction_Date < endDate.AddDays(1)
                          orderby objEntity.ID
                          select new ProductLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Product_ID = objEntity.Product_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Reference_ID = objEntity.Reference_ID,
                              Reference_Type = objEntity.Reference_Type,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Description = objEntity.Description,
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Selected_Product = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = product.Name
                              },
                          }).ToList<ProductLedgerBE>();
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
        public static List<ProductLedgerBE> GetProductLedgerBEsOld(int pumpID, int productID)
        {
            // Declare variables
            List<ProductLedgerBE> result = new List<ProductLedgerBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblProductLedgers
                          join product in context.tblProducts
                          on objEntity.Product_ID equals product.ID
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Product_ID == productID && objEntity.PumpID == pumpID
                          orderby objEntity.ID
                          select new ProductLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Product_ID = objEntity.Product_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Reference_ID = objEntity.Reference_ID,
                              Reference_Type = objEntity.Reference_Type,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Description = objEntity.Description,
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Selected_Product = new ProductBE()
                              {
                                  ID = objEntity.Product_ID,
                                  Name = product.Name
                              },
                          }).ToList<ProductLedgerBE>();
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

        public static List<ProductLedgerBE> GetProductLedgerBEs(int pumpID, int productID)
        {
            // Declare variables
            List<ProductLedgerBE> result = new List<ProductLedgerBE>();

            SqlDataReader dr;
            SqlConnection sqlCon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("GetProductLedgerByProductID", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Product_ID", SqlDbType.Int);
            cmd.Parameters.Add("@PumpID", SqlDbType.Int);
            cmd.Parameters[0].Value = productID;
            cmd.Parameters[1].Value = pumpID;
            try
            {
                sqlCon.Open();
                dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;
                while (dr.Read())
                {
                    ProductLedgerBE n = new ProductLedgerBE();
                    n.ID = Convert.ToInt32(dr["ID"]);
                    n.PumpID = Convert.ToInt32(dr["PumpID"]);
                    n.Product_ID = Convert.ToInt32(dr["Product_ID"]);
                    n.Transaction_Date = Convert.ToDateTime(dr["Transaction_Date"]);
                    n.Reference_ID = Convert.ToInt32(dr["Reference_ID"]);
                    n.Reference_Type = Convert.ToString(dr["Reference_Type"]);
                    n.Description = Convert.ToString(dr["Description"]);
                    n.Vehicle_No = Convert.ToString(dr["Vehicle_No"]);
                    n.Receipt_No = Convert.ToString(dr["Receipt_No"]);
                    n.Debit = Convert.ToDecimal(dr["Debit"]);
                    n.Credit = Convert.ToDecimal(dr["Credit"]);
                    n.Balance = Convert.ToDecimal(dr["Balance"]);
                    n.BalanceType = Convert.ToString(dr["BalanceType"]);
                    n.Is_Active = Convert.ToBoolean(dr["Is_Active"]);
                    n.Is_Deleted = Convert.ToBoolean(dr["Is_Deleted"]);
                    n.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    n.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]);
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

        public static ProductLedgerBE GetLastLedgerEntry(int productID)
        {
            // Declare variables
            ProductLedgerBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblProductLedgers
                          where objEntity.Product_ID == productID
                          orderby objEntity.ID descending
                          select new ProductLedgerBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              Product_ID = objEntity.Product_ID,
                              Transaction_Date = objEntity.Transaction_Date,
                              Reference_ID = objEntity.Reference_ID,
                              Reference_Type = objEntity.Reference_Type,
                              Debit = objEntity.Debit,
                              Credit = objEntity.Credit,
                              Balance = objEntity.Balance,
                              BalanceType = objEntity.BalanceType,
                              Description = objEntity.Description,
                              Vehicle_No = objEntity.Vehicle_No,
                              Receipt_No = objEntity.Receipt_No,
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

        public static tblProductLedger ConvertToLinqObject(ProductLedgerBE objEntity)
        {
            // Declare variables
            tblProductLedger  result = new tblProductLedger();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.Product_ID = objEntity.Product_ID;
                result.Transaction_Date = objEntity.Transaction_Date;
                result.Reference_ID = objEntity.Reference_ID;
                result.Reference_Type = objEntity.Reference_Type;
                result.Debit = objEntity.Debit;
                result.Credit = objEntity.Credit;
                result.Balance = objEntity.Balance;
                result.BalanceType = objEntity.BalanceType;
                result.Description = objEntity.Description;
                result.Vehicle_No = objEntity.Vehicle_No;
                result.Receipt_No = objEntity.Receipt_No;
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

        public static DateTime? GetPreviousDate(int productID, DateTime transactionDate)
        {
            // Declare variables
            GeneralLedgerBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblProductLedgers
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Product_ID == productID && objEntity.Transaction_Date < transactionDate
                          select new GeneralLedgerBE
                          {
                              Transaction_Date = objEntity.Transaction_Date,
                          }).OrderByDescending(p => p.Transaction_Date).FirstOrDefault();
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
            if (result == null)
            {
                return null;
            }
            else
            {
                return result.Transaction_Date;
            }
        }

    }
}
