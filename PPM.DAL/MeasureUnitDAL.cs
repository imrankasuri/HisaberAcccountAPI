using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;
namespace HAccounts.DAL
{
    public static class MeasureUnitDAL
    {

        public static int Save(MeasureUnitBE measureUnitBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
                tblMeasureUnit  clinq = null;
                
                clinq  = ConvertToLinqObject(measureUnitBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (measureUnitBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblMeasureUnits.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblMeasureUnits.Attach(clinq, true);
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

        public static MeasureUnitBE GetMeasureUnitByID(int id)
        {
            // Declare variables
            MeasureUnitBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblMeasureUnits
                          where objEntity.ID == id 
                          select new MeasureUnitBE 
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
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

        public static MeasureUnitBE GetMeasureUnitByName(string unitName)
        {
            // Declare variables
            MeasureUnitBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblMeasureUnits
                          where objEntity.Name.ToLower() == unitName.ToLower()
                          select new MeasureUnitBE
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
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

        public static List<MeasureUnitBE> GetMeasureUnitBEs()
        {
            // Declare variables
            List<MeasureUnitBE> result = new List<MeasureUnitBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblMeasureUnits 
                          where objEntity.Is_Deleted==false && objEntity.Is_Active==true
                          orderby objEntity.ID 
                          select new MeasureUnitBE
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray())
                              
                          }).ToList<MeasureUnitBE>();
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

        public static tblMeasureUnit ConvertToLinqObject(MeasureUnitBE objEntity)
        {
            // Declare variables
            tblMeasureUnit  result = new tblMeasureUnit();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.Name = objEntity.Name;
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

    }
}
