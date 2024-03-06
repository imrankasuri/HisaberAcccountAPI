using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class InvestmentSummaryDAL
    {

        public static int Save(InvestmentSummaryBE investmentSummaryBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblInvestmentSummary  clinq = null;
                
                clinq  = ConvertToLinqObject(investmentSummaryBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (investmentSummaryBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblInvestmentSummaries.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        clinq.Updated_Date = DateTime.Now;
                        context.tblInvestmentSummaries.Attach(clinq, true);
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

        public static InvestmentSummaryBE GetSummaryByID(int pumpID, int id)
        {
            // Declare variables
            InvestmentSummaryBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblInvestmentSummaries
                          where objEntity.ID == id && objEntity.PumpID == pumpID
                          select new InvestmentSummaryBE 
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Stock_Value = objEntity.Stock_Value,
                              Credits = objEntity.Credits,
                              Cash = objEntity.Cash,
                              Income = objEntity.Income,
                              ExtraIncome = objEntity.ExtraIncome,
                              Adjustments = objEntity.Adjustments,
                              Gross_Investment = objEntity.Gross_Investment,
                              Amount_Payable = objEntity.Amount_Payable,
                              Net_Investment = objEntity.Net_Investment,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Expenses = objEntity.Expenses,
                              Net_Income = objEntity.Net_Income,
                              Investment_Difference = objEntity.Investment_Difference,
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

        public static InvestmentSummaryBE GetSummaryByDate(int pumpId, DateTime summaryDate)
        {
            // Declare variables
            InvestmentSummaryBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblInvestmentSummaries
                          where objEntity.Dated == summaryDate && objEntity.PumpID == pumpId
                          select new InvestmentSummaryBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Stock_Value = objEntity.Stock_Value,
                              Credits = objEntity.Credits,
                              Cash = objEntity.Cash,
                              Income = objEntity.Income,
                              ExtraIncome = objEntity.ExtraIncome,
                              Adjustments = objEntity.Adjustments,
                              Gross_Investment = objEntity.Gross_Investment,
                              Amount_Payable = objEntity.Amount_Payable,
                              Net_Investment = objEntity.Net_Investment,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Expenses = objEntity.Expenses,
                              Net_Income = objEntity.Net_Income,
                              Investment_Difference = objEntity.Investment_Difference,
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

        public static List<InvestmentSummaryBE> GetAllSummaries(int pumpID)
        {
            // Declare variables
            List<InvestmentSummaryBE> result = new List<InvestmentSummaryBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblInvestmentSummaries 
                          where objEntity.Is_Deleted==false && objEntity.Is_Active==true && objEntity.PumpID == pumpID
                          orderby objEntity.ID 
                          select new InvestmentSummaryBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Stock_Value = objEntity.Stock_Value,
                              Credits = objEntity.Credits,
                              Cash = objEntity.Cash,
                              Income = objEntity.Income,
                              ExtraIncome = objEntity.ExtraIncome,
                              Adjustments = objEntity.Adjustments,
                              Gross_Investment = objEntity.Gross_Investment,
                              Amount_Payable = objEntity.Amount_Payable,
                              Net_Investment = objEntity.Net_Investment,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Expenses = objEntity.Expenses,
                              Net_Income = objEntity.Net_Income,
                              Investment_Difference = objEntity.Investment_Difference,
                          }).ToList<InvestmentSummaryBE>();
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

        public static List<InvestmentSummaryBE> GetAllSummaries(int pumpID, DateTime startDate, DateTime endDate)
        {
            // Declare variables
            List<InvestmentSummaryBE> result = new List<InvestmentSummaryBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblInvestmentSummaries
                          where objEntity.PumpID == pumpID &&  objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Dated >= startDate && objEntity.Dated < endDate.AddDays(1)
                          orderby objEntity.ID
                          select new InvestmentSummaryBE
                          {
                              ID = objEntity.ID,
                              PumpID = objEntity.PumpID,
                              PumpCode = objEntity.PumpCode,
                              Dated = objEntity.Dated,
                              Stock_Value = objEntity.Stock_Value,
                              Credits = objEntity.Credits,
                              Cash = objEntity.Cash,
                              Income = objEntity.Income,
                              ExtraIncome = objEntity.ExtraIncome,
                              Adjustments = objEntity.Adjustments,
                              Gross_Investment = objEntity.Gross_Investment,
                              Amount_Payable = objEntity.Amount_Payable,
                              Net_Investment = objEntity.Net_Investment,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Expenses = objEntity.Expenses,
                              Net_Income = objEntity.Net_Income,
                              Investment_Difference = objEntity.Investment_Difference,
                          }).ToList<InvestmentSummaryBE>();
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

        public static tblInvestmentSummary ConvertToLinqObject(InvestmentSummaryBE objEntity)
        {
            // Declare variables
            tblInvestmentSummary  result = new tblInvestmentSummary();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpID = objEntity.PumpID;
                result.PumpCode = objEntity.PumpCode;
                result.Dated = objEntity.Dated;
                result.Stock_Value = objEntity.Stock_Value;
                result.Credits = objEntity.Credits;
                result.Cash = objEntity.Cash;
                result.Income = objEntity.Income;
                result.ExtraIncome = objEntity.ExtraIncome;
                result.Adjustments = objEntity.Adjustments;
                result.Gross_Investment = objEntity.Gross_Investment;
                result.Amount_Payable = objEntity.Amount_Payable;
                result.Net_Investment = objEntity.Net_Investment;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;
                result.Expenses = objEntity.Expenses;
                result.Net_Income = objEntity.Net_Income;
                result.Investment_Difference = objEntity.Investment_Difference;
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
