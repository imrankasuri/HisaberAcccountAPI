using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HAccounts.BE;
using System.Transactions;
using System.Linq;

namespace HAccounts.DAL
{
    public static class SettingDAL
    {
        static string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["PPSCon"].ConnectionString;
        public static int Save(SettingBE  settingBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
               PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

               tblSetting clinq = null;
                clinq = ConvertToLinqObject(settingBE);

                try
                {
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (settingBE.ID == 0)
                    {
                        clinq.Created_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblSettings.InsertOnSubmit(clinq);
                    }
                    else
                    {
                        // Existing Image Set Account - we are updating the database
                        // Add Image Set Account to datacontext
                        context.tblSettings.Attach(clinq, true);
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

        public static SettingBE GetSettingBEById(int id)
        {
            // Declare variables
            SettingBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblSettings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.ID == id
                          select new SettingBE
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Value = objEntity.Value,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Updated_By = objEntity.Updated_By,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
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

        public static List<SettingBE> GetAllSettingBEs()
        {
            // Declare variables
            List<SettingBE> result = new List<SettingBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblSettings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true
                          orderby objEntity.ID 
                          select new SettingBE
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Value = objEntity.Value,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Updated_By = objEntity.Updated_By,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                          }).ToList<SettingBE>();
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

        public static tblSetting ConvertToLinqObject(SettingBE objEntity)
        {
            // Declare variables
            tblSetting result = new tblSetting();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.Name = objEntity.Name;
                result.Value = objEntity.Value;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;
                result.Updated_By = objEntity.Updated_By;
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

        public static SettingBE GetSettingBEByName(string setting_Name)
        {
            // Declare variables
            SettingBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblSettings
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true && objEntity.Name == setting_Name
                          select new SettingBE
                          {
                              ID = objEntity.ID,
                              Name = objEntity.Name,
                              Value = objEntity.Value,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Updated_By = objEntity.Updated_By,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
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

        public static void CloseAccount()
        {
            SqlConnection sqlcon = new SqlConnection(ConString);
            SqlCommand cmd = new SqlCommand("CloseAccount", sqlcon);
            cmd.CommandType = CommandType.StoredProcedure;
            sqlcon.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {

                throw exp;
            }

            finally
            {
                sqlcon.Close();
            }
        }

    }
}
