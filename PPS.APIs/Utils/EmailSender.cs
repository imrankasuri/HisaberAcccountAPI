using HAccounts.BE;
using HAccounts.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace HAccounts.APIs.Utils
{
    public static class EmailSender
    {
        //public static Boolean SendForgotEmail(BinaryTree_NodeBE mem, string linkPath)
        //{
        //    Boolean status = false;
        //    string tempString = "";
        //    try
        //    {
        //        string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/");
        //        filepath = filepath + "Email_Template\\Reset-Your-Password.htm";

        //        tempString = filepath;
        //        System.IO.StreamReader myFile = new System.IO.StreamReader(filepath);
        //        string myString = myFile.ReadToEnd();
        //        myFile.Close();
        //        myString = myString.Replace("{#ResetLink#}", linkPath);
        //        myString = myString.Replace("{#EmailAddress#}", mem.User_Name);


        //        MailMessage message = new MailMessage();
        //        SmtpClient client = new SmtpClient();
        //        message.To.Add(new MailAddress(mem.Email_Address));
        //        message.Subject = "[MAG VENTURES] Forgot Password";
        //        message.IsBodyHtml = true;

        //        message.Body = myString;
        //        string hostname = ConfigurationManager.AppSettings.Get("MailServerName").ToString().Trim();
        //        client.Host = hostname;
        //        string username = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //        string password = ConfigurationManager.AppSettings.Get("Password").ToString().Trim();
        //        message.From = new MailAddress(username, "MAG");
        //        System.Net.NetworkCredential basicCredentials = new System.Net.NetworkCredential(username, password);
        //        client.Credentials = basicCredentials;
        //        client.Send(message);
        //        status = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionBE exp = new ExceptionBE();
        //        exp.Created_Date = DateTime.Now;
        //        exp.Dated = DateTime.Now;
        //        exp.Event_Name = "Send Forgot Email";
        //        exp.Exception_Details = tempString + " " + ex.Message;
        //        exp.Module_Name = "Forgot Email";
        //        exp.Page_URL = "Forgot Email";
        //        exp.Updated_Date = DateTime.Now;
        //        ExceptionDAL.Save(exp);
        //    }
        //    finally
        //    {

        //    }
        //    #region EmailLogEntry
        //    //add entry in email log
        //    EmailLogBE elBE = new EmailLogBE();
        //    elBE.Created_Date = DateTime.Now;
        //    elBE.Updated_Date = DateTime.Now;
        //    elBE.Dated = DateTime.Now.Date;
        //    if (status == true)
        //    {
        //        elBE.Delivery_Status = "Success";
        //    }
        //    else
        //    {
        //        elBE.Delivery_Status = "Failure";
        //    }

        //    elBE.Email_From = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //    elBE.Email_To = mem.Email_Address;
        //    elBE.Event_Type = "Forgot Password";
        //    elBE.Reference = "Personal";
        //    elBE.Subject = "Forgot Password Information at www.investus.world";
        //    elBE.Updated_Date = DateTime.Now;
        //    EmailLogDAL.Save(elBE);
        //    #endregion

        //    return status;

        //}

        public static Boolean sendSignUpEmail(UserBE mem)
        {
            Boolean status = false;
            try
            {

                //DateTime dateTimeVariable = DateTime.UtcNow; // Or you can use your specific DateTime value here
                //string formattedDateTime = GeneralFunctions.Encrypt(dateTimeVariable.ToString("yyyyMMddHHmmssUTC"));

                DateTime dateTimeVariable =  DateTime.Now.AddHours(24); // Or you can use your specific DateTime value here
                string formattedDateTime = GeneralFunctions.Encrypt(dateTimeVariable.ToString());


                string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/");
                filepath = filepath + "Email_Template\\verify-email.htm";
                System.IO.StreamReader myFile = new System.IO.StreamReader(filepath);
                string myString = myFile.ReadToEnd();
                myFile.Close();
                //http://localhost:3000/verify-email?SecurityCode=asdfasdf&UserName=asdfadsf&ExpiryDate=adsfadsfasdf

                //myString = myString.Replace("{#link#}", Constants.applicationPath + "verify-email?" + "ExpiryDate=" + formattedDateTime + "&?email=" + mem.Email_Address + "&SecurityCode=" + mem.Email_Verification_Code + "&UserName=" + mem.User_Name);
                myString = myString.Replace("{#link#}", mem.Verification_Code);
                myString = myString.Replace("{#EmailAddress#}", mem.User_Email);
               

                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();
                message.To.Add(new MailAddress(mem.User_Email));
                message.Subject = "[HISAABER] Email Address Verification";
                message.IsBodyHtml = true;

                message.Body = myString;
                string hostname = ConfigurationManager.AppSettings.Get("MailServerName").ToString().Trim();
                client.Host = hostname;
                string username = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
                string password = ConfigurationManager.AppSettings.Get("Password").ToString().Trim();
                message.From = new MailAddress(username,"HISAABER");
                System.Net.NetworkCredential basicCredentials = new System.Net.NetworkCredential(username, password);
                client.Credentials = basicCredentials;
                client.Send(message);
                status = true;

                //log exception
                ExceptionBE exc = new ExceptionBE();
                exc.Created_Date = DateTime.Now;
                exc.Dated = DateTime.Now;
                exc.Event_Name = "sendSignUpEmail";
                string parameterList = "SecurityCode:" + mem.Verification_Code + " UserName=" + mem.User_Email + " ExpiryDate=" + formattedDateTime + " dateTimeVariable=" + dateTimeVariable;
                exc.Exception_Details = "No Exception";
                exc.Is_Active = true;
                exc.Is_Deleted = false;
                exc.Message = parameterList;
                exc.Module_Name = "API";
                exc.Page_URL = "EmailSender";
                exc.Updated_Date = DateTime.Now;
                ExceptionDAL.Save(exc);


            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                //throw ex;
            }
            finally
            {

            }
            #region EmailLogEntry
            //add entry in email log
            EmailLogBE elBE = new EmailLogBE();
            elBE.Created_Date = DateTime.Now;
            elBE.Updated_Date = DateTime.Now;
            elBE.Dated = DateTime.Now.Date;
            if (status == true)
            {
                elBE.Delivery_Status = "Success";
            }
            else
            {
                elBE.Delivery_Status = "Failure";
            }

            elBE.Email_From = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
            elBE.Email_To = mem.User_Email;
            elBE.Event_Type = "Registration";
            elBE.Reference = "Personal";
            elBE.Subject = "[HISAABER] Email Address Verification";
            elBE.Updated_Date = DateTime.Now;
            EmailLogDAL.Save(elBE);
            #endregion

            return status;

        }


        //public static Boolean sendAuthenticationCode(BinaryTree_NodeBE mem, string securityCode, string transactionType)
        //{
        //    Boolean status = false;
        //    try
        //    {
        //        string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/");
        //        filepath = filepath + "Email_Template\\AuthenticationCode.htm";
        //        System.IO.StreamReader myFile = new System.IO.StreamReader(filepath);
        //        string myString = myFile.ReadToEnd();
        //        myFile.Close();

        //        DateTime dateTimeVariable = DateTime.Now.AddMinutes(30); // Or you can use your specific DateTime value here
        //        string formattedDateTime = dateTimeVariable.ToString("yyyy-MM-dd HH:mm:ss") + " (GMT)";


        //        //int a = myString.IndexOf("<body>");
        //        //myString = myString.Remove(0, a + 6);
        //        //int b = myString.IndexOf("</body>");
        //        //int length = myString.Length;
        //        //myString = myString.Remove(b, length - b);
        //        //http://localhost:3000/verify-email?SecurityCode=asdfasdf&UserName=asdfadsf&ExpiryDate=adsfadsfasdf
        //        //myString = myString.Replace("{#link#}", Constants.applicationPath + "verify-email?" + "email=" + mem.Email_Address + "&SecurityCode=" + mem.Email_Verification_Code + "&UserName=" + mem.User_Name + "&ExpiryDate=" + DateTime.Now.AddMinutes(30).ToString());

        //        myString = myString.Replace("{#EmailAddress#}", mem.User_Name);
        //        myString = myString.Replace("{#AuthenticationCode#}", securityCode);
        //        myString = myString.Replace("{#ExpiryDate#}", formattedDateTime);
        //        myString = myString.Replace("{#TransactionType#}", transactionType);


        //        MailMessage message = new MailMessage();
        //        SmtpClient client = new SmtpClient();
        //        message.To.Add(new MailAddress(mem.Email_Address));
        //        message.Subject = "[MAG VENTURES] Verification code";
        //        message.IsBodyHtml = true;

        //        message.Body = myString;
        //        string hostname = ConfigurationManager.AppSettings.Get("MailServerName").ToString().Trim();
        //        client.Host = hostname;
        //        string username = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //        string password = ConfigurationManager.AppSettings.Get("Password").ToString().Trim();
        //        message.From = new MailAddress(username,"MAG");
        //        System.Net.NetworkCredential basicCredentials = new System.Net.NetworkCredential(username, password);
        //        client.Credentials = basicCredentials;
        //        client.Send(message);
        //        status = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    #region EmailLogEntry
        //    //add entry in email log
        //    EmailLogBE elBE = new EmailLogBE();
        //    elBE.Created_Date = DateTime.Now;
        //    elBE.Updated_Date = DateTime.Now;
        //    elBE.Dated = DateTime.Now.Date;
        //    if (status == true)
        //    {
        //        elBE.Delivery_Status = "Success";
        //    }
        //    else
        //    {
        //        elBE.Delivery_Status = "Failure";
        //    }

        //    elBE.Email_From = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //    elBE.Email_To = mem.Email_Address;
        //    elBE.Event_Type = "Security";
        //    elBE.Reference = "Personal";
        //    elBE.Subject = "[MAG VENTURES] Verification code";
        //    elBE.Updated_Date = DateTime.Now;
        //    EmailLogDAL.Save(elBE);
        //    #endregion

        //    return status;

        //}

        //public static Boolean sendPasswordUpdatedEmail(BinaryTree_NodeBE mem)
        //{
        //    Boolean status = false;
        //    try
        //    {
        //        string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/");
        //        filepath = filepath + "Email_Template\\Password-Updated-Successfully.htm";
        //        System.IO.StreamReader myFile = new System.IO.StreamReader(filepath);
        //        string myString = myFile.ReadToEnd();
        //        myFile.Close();
               

        //        myString = myString.Replace("{#EmailAddress#}", mem.User_Name);


        //        MailMessage message = new MailMessage();
        //        SmtpClient client = new SmtpClient();
        //        message.To.Add(new MailAddress(mem.Email_Address));
        //        message.Subject = "[MAG VENTURES] Password Updated";
        //        message.IsBodyHtml = true;

        //        message.Body = myString;
        //        string hostname = ConfigurationManager.AppSettings.Get("MailServerName").ToString().Trim();
        //        client.Host = hostname;
        //        string username = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //        string password = ConfigurationManager.AppSettings.Get("Password").ToString().Trim();
        //        message.From = new MailAddress(username, "MAG");
        //        System.Net.NetworkCredential basicCredentials = new System.Net.NetworkCredential(username, password);
        //        client.Credentials = basicCredentials;
        //        client.Send(message);
        //        status = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    #region EmailLogEntry
        //    //add entry in email log
        //    EmailLogBE elBE = new EmailLogBE();
        //    elBE.Created_Date = DateTime.Now;
        //    elBE.Updated_Date = DateTime.Now;
        //    elBE.Dated = DateTime.Now.Date;
        //    if (status == true)
        //    {
        //        elBE.Delivery_Status = "Success";
        //    }
        //    else
        //    {
        //        elBE.Delivery_Status = "Failure";
        //    }

        //    elBE.Email_From = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //    elBE.Email_To = mem.Email_Address;
        //    elBE.Event_Type = "Verification";
        //    elBE.Reference = "Personal";
        //    elBE.Subject = " [MAG VENTURES] Identity Verification Failed";
        //    elBE.Updated_Date = DateTime.Now;
        //    EmailLogDAL.Save(elBE);
        //    #endregion

        //    return status;

        //}

        //public static Boolean sendLoginEmail(BinaryTree_NodeBE mem, string ipAddress, string deviceName, string locationName)
        //{
        //    Boolean status = false;
        //    try
        //    {
        //        string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/");
        //        filepath = filepath + "Email_Template\\Login.htm";
        //        System.IO.StreamReader myFile = new System.IO.StreamReader(filepath);
        //        string myString = myFile.ReadToEnd();
        //        myFile.Close();

        //        DateTime dateTimeVariable = DateTime.Now; // Or you can use your specific DateTime value here

        //        // Format the DateTime using a custom format string
        //        string formattedDateTime = dateTimeVariable.ToString("yyyy-MM-dd HH:mm:ss") + " (GMT)";

        //        myString = myString.Replace("{#EmailAddress#}", mem.User_Name);
        //        myString = myString.Replace("{#Location#}", locationName);
        //        myString = myString.Replace("{#dateTime#}", formattedDateTime);
        //        myString = myString.Replace("{#ipAddress#}", ipAddress);
        //        myString = myString.Replace("{#DeviceName#}", deviceName);

        //        MailMessage message = new MailMessage();
        //        SmtpClient client = new SmtpClient();
        //        message.To.Add(new MailAddress(mem.Email_Address));
        //        message.Subject = "[MAG VENTURES] Login Activity on Your MAG Account";
        //        message.IsBodyHtml = true;

        //        message.Body = myString;
        //        string hostname = ConfigurationManager.AppSettings.Get("MailServerName").ToString().Trim();
        //        client.Host = hostname;
        //        string username = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //        string password = ConfigurationManager.AppSettings.Get("Password").ToString().Trim();
        //        message.From = new MailAddress(username, "MAG");
        //        System.Net.NetworkCredential basicCredentials = new System.Net.NetworkCredential(username, password);
        //        client.Credentials = basicCredentials;
        //        client.Send(message);
        //        status = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write(ex.Message);
        //        //throw ex;
        //    }
        //    finally
        //    {

        //    }
        //    #region EmailLogEntry
        //    //add entry in email log
        //    EmailLogBE elBE = new EmailLogBE();
        //    elBE.Created_Date = DateTime.Now;
        //    elBE.Updated_Date = DateTime.Now;
        //    elBE.Dated = DateTime.Now.Date;
        //    if (status == true)
        //    {
        //        elBE.Delivery_Status = "Success";
        //    }
        //    else
        //    {
        //        elBE.Delivery_Status = "Failure";
        //    }

        //    elBE.Email_From = ConfigurationManager.AppSettings.Get("MailingAddress").ToString().Trim();
        //    elBE.Email_To = mem.Email_Address;
        //    elBE.Event_Type = "Login";
        //    elBE.Reference = "Personal";
        //    elBE.Subject = "Login Activity on Your MAG Account";
        //    elBE.Updated_Date = DateTime.Now;
        //    EmailLogDAL.Save(elBE);
        //    #endregion

        //    return status;

        //}
    }
}