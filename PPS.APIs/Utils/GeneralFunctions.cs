using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using HAccounts.BE;
using HAccounts.DAL;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HAccounts.APIs.Utils
{
    public static class GeneralFunctions
    {

       
        public static string GetRandomNumber(int start, int end)
        {
            string s = "";

            Random rnd = new Random();
            rnd.Next();
            int randomNo = rnd.Next(start, end); // creates a number between 1 and 12
            s = randomNo.ToString("000000");
            return s;
        }

        public static string Encrypt(string clearText)
        {
            //string EncryptionKey = "MAKV2SPBNI99212";
            string EncryptionKey = "WeLoveHisaaber77";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            //string EncryptionKey = "MAKV2SPBNI99212";
            string EncryptionKey = "WeLoveHisaaber77";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string SendNotification(string deviceID, string messageText)
        {
            string serverKey = "AAAAYaL98BM:APA91bEKdmQLvAjyVleva1tJhB9rPo0I55d_ksCGarVYRbU2Jg2pGw2UXlxKqvIWdb9GBa_RdUm0BXw9NO943tVLFU22uFB77icxCt9JcV4TCivPwWtVF1fO6t36Qoobvqpd3_osBwPX";

            try
            {
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                //httpWebRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"registration_ids\": [\"" + deviceID + "\"],\"data\": {\"message\": \"" + messageText + "\",}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static Random rng = new Random();
        public static void Shuffle<CurrencyBE>(this IList<CurrencyBE> list)
        {

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CurrencyBE value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string GetMessageString(string messageText, string messageType)
        {
            string finalstring = "";
            if (messageType.ToLower() == "success")
            {
                finalstring = @"<div class='alert alert-success alert-dismissible fade show' role='alert'>
                                        <strong>Success!</strong> " + messageText + @"<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
            }
            else if (messageType.ToLower() == "info")
            {
                finalstring = @"<div class='alert alert-info alert-dismissible fade show' role='alert'>
                                        <strong>Information!</strong> " + messageText + @"<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
            }
            else
            {
                finalstring = @"<div class='alert alert-danger alert-dismissible fade show' role='alert'>
                                        <strong>Error!</strong> " + messageText + @"<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
            }
            return finalstring;
        }

        public static string GetAdminMessageString(string messageText, string messageType)
        {
            string finalstring = "";
            if (messageType.ToLower() == "success")
            {
                finalstring = @"<div class='alert alert-success alert-dismissible' role='alert'>
                                         <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
                                        <strong>Success!</strong> " + messageText + @"</div>";
            }
            else
            {
                finalstring = @"<div class='alert alert-danger alert-dismissible' role='alert'>
                                         <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
                                        <strong>Information!</strong> " + messageText + @"</div>";
            }
            return finalstring;
        }

        public static Boolean IsNumericValue(string s)
        {
            int i = 0;
            bool result = int.TryParse(s, out i); //i now = 108  
            return result;
        }

        public static Boolean IsDecimalValue(string s)
        {
            decimal i = 0;
            bool result = decimal.TryParse(s, out i); //i now = 108  
            return result;
        }


        public static string ToKMB(this decimal num)
        {
            if (num > 999999999 || num < -999999999)
            {
                return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
            }
            else
            if (num > 999999 || num < -999999)
            {
                return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
            }
            else
            if (num > 999 || num < -999)
            {
                return num.ToString("0,.#K", CultureInfo.InvariantCulture);
            }
            else
            {
                return num.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (c == ' ')
                {
                    sb.Append('-');
                }
                else if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static DateTime GetDateTime()
        {
            DateTime d = DateTime.Now.AddHours(Constants.timeDifference);
            return d;
        }

        public static bool IsValidEmail(string email)
        {
            var r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
            return !string.IsNullOrEmpty(email) && r.IsMatch(email);
        }

       
       

    }
}