using HAccounts.APIs.Utils;
using HAccounts.BE;
using HAccounts.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;

namespace HAccounts.APIs.Controllers
{
    public class ReportController : ApiController
    {

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetReceiveableReport([FromBody] GeneralRequestBE inParams)
        {

            if (inParams != null && !String.IsNullOrEmpty(inParams.UserID.ToString()) && !String.IsNullOrEmpty(inParams.AccessKey))
            {
                if (AccessKeyDAL.CheckValidAccessKey(Convert.ToInt32(inParams.UserID), inParams.AccessKey) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Invalid Access Key."
                    });
                }

                try
                {
                    UserBE currentUser = UserDAL.GetUserBEByID(Convert.ToInt32(inParams.UserID));
                    DateTime startDate = Convert.ToDateTime("2022-01-01");
                    DateTime endDate = DateTime.Now;
                    int pageNo = 0;
                    if (inParams.StartDate != null)
                    {
                        startDate = Convert.ToDateTime(inParams.StartDate);
                    }
                    if (inParams.EndDate != null)
                    {
                        endDate = Convert.ToDateTime(inParams.EndDate);
                    }
                    if (inParams.PageNo > 0)
                    {
                        pageNo = Convert.ToInt32(inParams.PageNo);
                    }
                  
                    List<AccountBE> ListofAllAccounts = new List<AccountBE>();

                    ListofAllAccounts = AccountDAL.GetAccountByPumpIDOnDate(currentUser.PumpID, endDate);

                    if (ListofAllAccounts != null && ListofAllAccounts.Count > 0)
                    {
                        ListofAllAccounts = ListofAllAccounts.Where(p => p.Account_Type_BE.ID == 6 || p.Account_Type_BE.ID == 7 || p.Account_Type_BE.ID == 14 || p.Account_Type_BE.ID == 15).ToList();
                    }

                    if (ListofAllAccounts != null && ListofAllAccounts.Count > 0)
                    {
                        ListofAllAccounts = ListofAllAccounts.Where(p => p.Balance > 0 && p.BalanceType.ToUpper() == "DEBIT").ToList();
                    }
                    if (inParams.OrderBy == "1")
                    {
                        ListofAllAccounts = ListofAllAccounts.OrderBy(p => p.Name).ToList();
                    }
                    else if (inParams.OrderBy == "2")
                    {
                        ListofAllAccounts = ListofAllAccounts.OrderByDescending(p => p.Name).ToList();
                    }
                    else if (inParams.OrderBy == "3")
                    {
                        ListofAllAccounts = ListofAllAccounts.OrderBy(p => p.Balance).ToList();
                    }
                    else
                    {
                        ListofAllAccounts = ListofAllAccounts.OrderByDescending(p => p.Balance).ToList();
                    }

                    var reducedList = ListofAllAccounts.Select(e => new { e.ID, e.Name, e.Description, e.Email_Address, e.Mobile_No, e.Account_Type_BE, e.Balance, e.BalanceType }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 1,
                        status_message = "Successfully returning receiveable report",
                        listofLedger = reducedList,
                    });


                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        status_code = 0,
                        status_message = "Sorry, unable to reply."
                    });
                }



            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status_code = 0,
                    status_message = "Invalid Request Parameters"
                });
            }
        }

    }
}
