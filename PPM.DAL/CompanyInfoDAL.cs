using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAccounts.BE;
using System.Transactions;

namespace HAccounts.DAL
{
    public static class CompanyInfoDAL
    {

        public static int Save(CompanyInfoBE  companyInfoBE)
        {
            // Declare variables
            int result = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                // Set data context objects
                PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();
                
               tblCompanyInfo       clinq = null;
                
                clinq  = ConvertToLinqObject(companyInfoBE);

                try
                {
                    // Update Updated Date
                    clinq.Updated_Date = System.DateTime.Now.AddHours(Constants.timeDifference);
                    if (companyInfoBE.ID == 0)
                    {
                        clinq.Created_Date = DateTime.Now.AddHours(Constants.timeDifference);
                        context.tblCompanyInfos.InsertOnSubmit(clinq);
                    }
                    else
                    {
                       
                        context.tblCompanyInfos.Attach(clinq, true);
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

        public static CompanyInfoBE GetCompanyInfoByID(int id)
        {
            // Declare variables
            CompanyInfoBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext  context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblCompanyInfos
                          where objEntity.ID == id 
                          select new CompanyInfoBE 
                          {
                              ID = objEntity.ID,
                              PumpCode = objEntity.PumpCode,
                              Name = objEntity.Name,
                              Address = objEntity.Address,
                              Phone = objEntity.Phone,
                              Fax = objEntity.Fax,
                              Email = objEntity.Email,
                              Website = objEntity.Website,
                              Mobile = objEntity.Mobile,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Reports = objEntity.Logo_Reports,
                              Logo_Title = objEntity.Logo_Title,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              FuelProviderID = objEntity.FuelProviderID,
                              PackageName = objEntity.PackageName,
                              PackageExpiry = objEntity.PackageExpiry,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Show_Pumps = objEntity.Show_Pumps
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

        public static CompanyInfoBE GetCompanyInfoByName(string companyName)
        {
            // Declare variables
            CompanyInfoBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblCompanyInfos
                          where objEntity.Name.ToLower() == companyName.ToLower()
                          select new CompanyInfoBE
                          {
                              ID = objEntity.ID,
                              PumpCode = objEntity.PumpCode,
                              Name = objEntity.Name,
                              Address = objEntity.Address,
                              Phone = objEntity.Phone,
                              Fax = objEntity.Fax,
                              Email = objEntity.Email,
                              Website = objEntity.Website,
                              Mobile = objEntity.Mobile,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Reports = objEntity.Logo_Reports,
                              Logo_Title = objEntity.Logo_Title,
                              FuelProviderID = objEntity.FuelProviderID,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              PackageName = objEntity.PackageName,
                              PackageExpiry = objEntity.PackageExpiry,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Show_Pumps = objEntity.Show_Pumps
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


        public static CompanyInfoBE GetCompanyInfoByMobileNo(string mobileNo)
        {
            // Declare variables
            CompanyInfoBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblCompanyInfos
                          where objEntity.Mobile.ToLower() == mobileNo.ToLower()
                          select new CompanyInfoBE
                          {
                              ID = objEntity.ID,
                              PumpCode = objEntity.PumpCode,
                              Name = objEntity.Name,
                              Address = objEntity.Address,
                              Phone = objEntity.Phone,
                              Fax = objEntity.Fax,
                              Email = objEntity.Email,
                              Website = objEntity.Website,
                              Mobile = objEntity.Mobile,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Reports = objEntity.Logo_Reports,
                              Logo_Title = objEntity.Logo_Title,
                              FuelProviderID = objEntity.FuelProviderID,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              PackageName = objEntity.PackageName,
                              PackageExpiry = objEntity.PackageExpiry,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Show_Pumps = objEntity.Show_Pumps
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

        public static CompanyInfoBE GetCompanyInfoByEmail(string emailAddress)
        {
            // Declare variables
            CompanyInfoBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {

                result = (from objEntity in context.tblCompanyInfos
                          where objEntity.Email.ToLower() == emailAddress.ToLower()
                          select new CompanyInfoBE
                          {
                              ID = objEntity.ID,
                              PumpCode = objEntity.PumpCode,
                              Name = objEntity.Name,
                              Address = objEntity.Address,
                              Phone = objEntity.Phone,
                              Fax = objEntity.Fax,
                              Email = objEntity.Email,
                              Website = objEntity.Website,
                              Mobile = objEntity.Mobile,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Reports = objEntity.Logo_Reports,
                              Logo_Title = objEntity.Logo_Title,
                              FuelProviderID = objEntity.FuelProviderID,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              PackageName = objEntity.PackageName,
                              PackageExpiry = objEntity.PackageExpiry,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Show_Pumps = objEntity.Show_Pumps
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
        public static List<CompanyInfoBE> GetCompanyINfoBEs()
        {
            // Declare variables
            List<CompanyInfoBE> result = new List<CompanyInfoBE>();

            //// Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in list of Image Set Accounts from the database
                result = (from objEntity in context.tblCompanyInfos
                          where objEntity.Is_Deleted == false && objEntity.Is_Active == true
                          orderby objEntity.ID 
                          select new CompanyInfoBE
                          {
                              ID = objEntity.ID,
                              PumpCode = objEntity.PumpCode,
                              Name = objEntity.Name,
                              Address = objEntity.Address,
                              Phone = objEntity.Phone,
                              Fax = objEntity.Fax,
                              Email = objEntity.Email,
                              Website = objEntity.Website,
                              Mobile = objEntity.Mobile,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Reports = objEntity.Logo_Reports,
                              Logo_Title = objEntity.Logo_Title,
                              FuelProviderID = objEntity.FuelProviderID,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              PackageName = objEntity.PackageName,
                              PackageExpiry = objEntity.PackageExpiry,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Show_Pumps = objEntity.Show_Pumps
                              
                          }).ToList<CompanyInfoBE>();
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

        public static tblCompanyInfo ConvertToLinqObject(CompanyInfoBE objEntity)
        {
            // Declare variables
            tblCompanyInfo  result = new tblCompanyInfo();

            try
            {
                // Convert entity values to Linq object equivalents
                result.ID = objEntity.ID;
                result.PumpCode = objEntity.PumpCode;
                result.Name = objEntity.Name;
                result.Address = objEntity.Address;
                result.Phone = objEntity.Phone;
                result.Fax = objEntity.Fax;
                result.Email = objEntity.Email;
                result.Mobile = objEntity.Mobile;
                result.Website = objEntity.Website;
                result.Logo_Title = objEntity.Logo_Title;
                result.Logo_Reports = objEntity.Logo_Reports;
                result.Logo_Login = objEntity.Logo_Login;
                result.FuelProviderID = objEntity.FuelProviderID;
                result.Created_Date = objEntity.Created_Date;
                result.Updated_Date = objEntity.Updated_Date;
                result.Is_Active = objEntity.Is_Active;
                result.Is_Deleted = objEntity.Is_Deleted;
                result.PackageExpiry = objEntity.PackageExpiry;
                result.PackageName = objEntity.PackageName;
                if (objEntity.TimeStamp != null)
                {
                    result.TimeStamp = new System.Data.Linq.Binary(Convert.FromBase64String(objEntity.TimeStamp.ToString()));
                }
                result.Show_Pumps = objEntity.Show_Pumps;
            }
            catch (Exception ex)
            {
                // pass error back to calling method
                throw ex;
            }

            return result;
        }

        public static CompanyInfoBE GetPreviousCompanyInfo(int companyID)
        {
            // Declare variables
            CompanyInfoBE result = null;
            // Set data context objects
            PPSLinqToSqlDataContext context = new PPSLinqToSqlDataContext();

            try
            {
                // Read in Image Set Account object from database
                result = (from objEntity in context.tblCompanyInfos
                          where objEntity.ID < companyID
                          orderby objEntity.ID descending
                          select new CompanyInfoBE
                          {
                              ID = objEntity.ID,
                              PumpCode = objEntity.PumpCode,
                              Name = objEntity.Name,
                              Address = objEntity.Address,
                              Phone = objEntity.Phone,
                              Fax = objEntity.Fax,
                              Email = objEntity.Email,
                              Website = objEntity.Website,
                              Mobile = objEntity.Mobile,
                              Logo_Login = objEntity.Logo_Login,
                              Logo_Reports = objEntity.Logo_Reports,
                              Logo_Title = objEntity.Logo_Title,
                              FuelProviderID = objEntity.FuelProviderID,
                              Created_Date = objEntity.Created_Date,
                              Updated_Date = objEntity.Updated_Date,
                              Is_Active = objEntity.Is_Active,
                              Is_Deleted = objEntity.Is_Deleted,
                              PackageName = objEntity.PackageName,
                              PackageExpiry = objEntity.PackageExpiry,
                              TimeStamp = Convert.ToBase64String(objEntity.TimeStamp.ToArray()),
                              Show_Pumps = objEntity.Show_Pumps
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
