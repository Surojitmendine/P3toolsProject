using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Common.Utility
{
    public class clsHelper
    {
        public static string ConvertAmountToINR(double dblAmount, bool boolUseShortFormat = false) //string strAmount
        {
            string strFormattedAmount = "";
            if (boolUseShortFormat == false)
            {
                strFormattedAmount = dblAmount.ToString("#,0.00", System.Globalization.CultureInfo.CreateSpecificCulture("hi-IN"));
            }
            else
            {
                string strAmt = "", strAmtPart1 = "", strAmtPart2 = "";
                double dblAmtPart1 = 0, dblAmtPart2 = 0;

                // Displays 123,45,67,890   
                if (dblAmount < 1000)
                    strFormattedAmount = dblAmount.ToString("#,0.00", System.Globalization.CultureInfo.CreateSpecificCulture("hi-IN"));

                // Displays 123,45,68K
                else if (dblAmount >= 1000 && dblAmount < 100000)
                    strFormattedAmount = dblAmount.ToString("#,#,K", System.Globalization.CultureInfo.CreateSpecificCulture("hi-IN"));//InvariantCulture

                // Displays 123,5L
                else if (dblAmount >= 100000 && dblAmount < 10000000)
                {
                    strAmt = dblAmount.ToString();
                    strAmtPart1 = strAmt.Substring(0, (strAmt.Length - 5));
                    strAmtPart2 = strAmt.Substring((strAmt.Length - 5), 5);

                    dblAmtPart1 = Convert.ToDouble(strAmtPart1);
                    dblAmtPart2 = Convert.ToDouble(strAmtPart2);

                    if (dblAmtPart2 > 55999)
                    {
                        dblAmtPart1 = dblAmtPart1 + 1;
                    }

                    strAmtPart1 = dblAmtPart1.ToString("#,#", System.Globalization.CultureInfo.CreateSpecificCulture("hi-IN"));

                    strFormattedAmount = strAmtPart1 + "L";
                }
                // Displays 123C
                else if (dblAmount >= 10000000)
                {
                    strAmt = dblAmount.ToString();
                    strAmtPart1 = strAmt.Substring(0, (strAmt.Length - 7));
                    strAmtPart2 = strAmt.Substring((strAmt.Length - 7), 7);

                    dblAmtPart1 = Convert.ToDouble(strAmtPart1);
                    dblAmtPart2 = Convert.ToDouble(strAmtPart2);

                    if (dblAmtPart2 > 5599999)
                    {
                        dblAmtPart1 = dblAmtPart1 + 1;
                    }

                    strAmtPart1 = dblAmtPart1.ToString("#,#", System.Globalization.CultureInfo.CreateSpecificCulture("hi-IN"));

                    strFormattedAmount = strAmtPart1 + "C";
                }
            }

            return strFormattedAmount;
        }
        public static long fnConvert2Long(object pText)
        {
            try
            {
                if (pText == null || pText == String.Empty || pText == "{}")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(pText);
                }
            }
            catch
            {
                return 0;
            }
        }
        public static int fnConvert2Int(object pText)
        {
            try
            {
                if (pText == null || pText == string.Empty || pText == "{}")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt16(pText);
                }
            }
            catch
            {
                return 0;
            }
        }
        public static decimal fnConvert3Decimal(object pText)
        {
            try
            {
                if (pText == null || pText == "" || pText == "{}")
                {
                    return 0;
                }
                else
                {
                    //return Convert.ToDecimal(pText).ToString("0.00");
                    return Convert.ToDecimal(Convert.ToDecimal(pText).ToString("0.000"));
                }
            }
            catch
            {
                return 0;
            }
        }
        public static decimal fnConvert2Decimal(object pText)
        {
            try
            {
                if (pText == null || pText == "" || pText == "{}")
                {
                    return 0;
                }
                else
                {
                    //return Convert.ToDecimal(pText).ToString("0.00");
                    return Convert.ToDecimal(Convert.ToDecimal(pText).ToString("0.00"));
                }
            }
            catch
            {
                return 0;
            }
        }
        public static double fnConvert2Double(object pText)
        {
            try
            {
                if (pText == null || pText.ToString() == "" || pText.ToString() == "{}")
                {
                    return 0;
                }
                else
                {
                    //return Convert.ToDecimal(pText).ToString("0.00");
                    return Convert.ToDouble(pText);
                }
            }
            catch
            {
                return 0;
            }
        }
        public static string fnConvert2String(String pText)
        {
            try
            {
                if (String.IsNullOrEmpty(pText))
                {
                    return "";
                }
                else
                {
                    return Convert.ToString(pText);
                }
            }
            catch
            {
                return "";
            }
        }
        public static String fnConvert2PascalWithSpace(String pText)
        {
            String mText = pText;
            if (mText != "")
            {
                mText = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(mText.ToLower());
            }
            return mText;
        }
        public static String fnConvert2StartCapital(String pText)
        {
            String mText = pText;
            if (mText != "")
            {
                return char.ToUpper(mText[0]) + pText.Substring(1).ToLower();
            }
            return mText;
        }
        public static bool fnIsNumeric(string pText)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(pText);
        }
        public static string fnAutoNumber()
        {
            String mAutoNo = System.DateTime.Now.ToString("ddmmyy");
            return mAutoNo;
        }
        public static String fnAutoNumber(int pdigit)
        {
            Random generator = new Random();
            String mAutoNo = Convert.ToString(generator.Next(0, 1000000).ToString("D6"));
            return mAutoNo;
        }
        public static String fnAutoNumber8()
        {
            Random generator = new Random();
            String mAutoNo = Convert.ToString(generator.Next(0, 100000000).ToString("D8"));
            return mAutoNo;
        }
        public static string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$%";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 , 0x20, 0x4d, 0x49, 0x76
             });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
        public static string SplitQueryString(string QueryString)
        {

            string strReq = "";
            try
            {

                strReq = QueryString;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                return strReq;
            }
            catch (Exception ex)
            {
                strReq = ex.Message.ToString();
                return strReq;
            }
            finally { }
        }
        public static string SplitQueryString(string QueryString, char SplitType)
        {

            string strReq = "";
            try
            {

                strReq = QueryString;
                strReq = strReq.Substring(strReq.IndexOf(SplitType) + 1);

                return strReq;
            }
            catch (Exception ex)
            {
                strReq = ex.Message.ToString();
                return strReq;
            }
            finally { }
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$%";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76, 0x20, 0x4d, 0x49, 0x76
            });
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
        public static Boolean fnConvert2Boolean(String pText)
        {
            try
            {
                if (String.IsNullOrEmpty(pText))
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean(pText);
                }
            }
            catch
            {
                return false;
            }
        }
        public Boolean fnValidEmail(String pEmail)
        {
            String mPattern = "^[a-zA-Z0-9][-\\._a-zA-Z0-9]*@[a-zA-Z0-9][-\\.a-zA-Z0-9]*\\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
            System.Text.RegularExpressions.Regex mCheck = new System.Text.RegularExpressions.Regex(mPattern, RegexOptions.IgnorePatternWhitespace);
            Boolean mValid = false;
            if (String.IsNullOrEmpty(pEmail) == true)
            {
                mValid = true;
            }
            else
            {
                mValid = mCheck.IsMatch(pEmail);
            }
            return mValid;
        }
        public static decimal[] fnRoundoff(decimal netamt)
        {
            decimal[] myarray = new decimal[2];
            decimal roundoff = 0;
            decimal double_result = (decimal)((netamt - (decimal)((long)netamt)));
            if (double_result != 0)
            {
                if (double_result >= (decimal)(0.5))
                {
                    roundoff = (1 - double_result);
                }
                else
                {
                    roundoff = -(double_result);
                }
            }
            else
            {
                roundoff = 0;
            }
            myarray[0] = roundoff;
            myarray[1] = (netamt + roundoff);
            return myarray;
        }
        public static decimal fnCeilingFloor(Decimal decvalue)
        {
            decimal trnvalue = 0;
            int mint = (int)(decvalue);
            float floatpart = ((float)decvalue - mint);
            if (floatpart >= 0.5)
            {
                trnvalue = Math.Ceiling(decvalue);
            }
            else
            {
                trnvalue = Math.Floor(decvalue);
            }

            return trnvalue;
        }
        public static String fnAutoNumber4()
        {
            Random generator = new Random();
            String mAutoNo = Convert.ToString(generator.Next(0, 1000000).ToString("D6"));
            return mAutoNo;
        }
        public static string fnSendEmail(string SMTPServer, Boolean EnableSSL_TrueFalse, string Username, string Password, int SMTP_Port, string mfromEmail, string mToEmail, string mSubject, string mBody)
        {
            string EmailStatus = "";
            try
            {
                using (MailMessage mm = new MailMessage())
                {
                    mm.From = new MailAddress(mfromEmail);
                    mm.To.Add(mToEmail);
                    mm.Subject = mSubject;
                    mm.Body = mBody;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = SMTPServer;
                    smtp.EnableSsl = EnableSSL_TrueFalse;
                    NetworkCredential NetworkCred = new NetworkCredential(Username, Password);

                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = SMTP_Port;
                    smtp.Send(mm);
                    EmailStatus = "";
                    return EmailStatus;
                }
            }
            catch (Exception ex)
            {
                EmailStatus = ex.Message.ToString();
                return EmailStatus;
            }
            finally { }
        }
        //public static string EmailHelper(String calledFrom, String smtpServer, int smtpPort, bool SSL, String password, String fromEmail, String toEmail, String emailSubject, String emailBody, String attachmentPath, String attachmentName)
        //{
        //    string error = "";
        //    try
        //    {
        //        NetworkCredential login = new NetworkCredential(fromEmail, password);
        //        SmtpClient smtp = new SmtpClient(smtpServer);
        //        smtp.Port = smtpPort;
        //        smtp.EnableSsl = SSL;
        //        smtp.Credentials = login;

        //        MailMessage msg = new MailMessage { From = new MailAddress(fromEmail) };
        //        msg.To.Add(new MailAddress(toEmail));
        //        msg.Subject = emailSubject;
        //        msg.Body = emailBody;

        //        System.Net.Mail.Attachment attachment;
        //        String Path = HttpContext.Current.Server.MapPath(attachmentPath);
        //        attachment = new System.Net.Mail.Attachment(Path);
        //        msg.Attachments.Add(attachment);

        //        msg.IsBodyHtml = false;
        //        msg.Priority = MailPriority.Normal;
        //        msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //        smtp.Send(msg);
        //        error = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        error = "Issue";
        //        clsDatabase.fnErrorLog(calledFrom, ex.Message.ToString());
        //    }
        //    return error;
        //}
        public static DataTable fnToDataTable<T>(List<T> items, string[] redundantColumns = null)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            if (redundantColumns != null)
            {
                foreach (string rc in redundantColumns)
                {
                    dataTable.Columns.Remove(rc);
                }
            }
            return dataTable;
        }
        public static List<T> fnDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItemFromDataTable<T>(row);
                data.Add(item);
            }
            return data;
        }

        #region Private Methods
        private static T GetItemFromDataTable<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion
    }
    public class clsDatabase
    {
        public static String fnDBOperation(String mConnection ,String pSPName, params object[] pParaValue)
        {
            SqlConnection mCon = new SqlConnection(mConnection);
            string mResult = "";
            try
            {
                mCon.Open();
                SqlCommand cmd = new SqlCommand(pSPName, mCon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(cmd);
                cmd.Parameters.RemoveAt(0);
                SqlParameter[] pParaName = new SqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(pParaName, 0);
                cmd.Parameters.Clear();
                mResult = fnAddParaValue(cmd, pParaName, pParaValue);
                if (mResult == "") // Parameter value passed
                {
                    return Convert.ToString(cmd.ExecuteScalar());
                }
                else
                {
                    return mResult;
                }
            }
            catch (Exception e)
            {
                mResult = fnError(e); // Error Message
                fnErrorLog(pSPName, mResult); // Error Logging 
                return mResult;
            }
            finally
            {
                mCon.Close();
            }
        }
        private static string fnAddParaValue(SqlCommand pCom, SqlParameter[] pParaName, object[] pParaValue)
        {
            if ((pCom == null))
            {
                return "SQL Command initialization issue...";
            }
            if (((pParaName == null) | (pParaValue == null)))
            {
                return "SQL Command Parameter initialization issue...";
            }
            if ((pParaName.Length != pParaValue.Length))
            {
                return "SQL Command Parameter length size issue...";
            }
            for (int index = 0; index <= pParaName.Length - 1; index++)
            {

                pCom.Parameters.AddWithValue(pParaName[index].ParameterName, pParaValue[index]);
            }
            return "";
        }
        public static DataSet fnDataSet(String mConnection, String pSPName, params object[] pParaValue)
        {
            SqlConnection mCon = new SqlConnection(mConnection);
            DataSet DS = new DataSet("Data");
            String Result = "";
            try
            {
                mCon.Open();
                SqlCommand mCom = new SqlCommand(pSPName, mCon);
                mCom.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(mCom);
                mCom.Parameters.RemoveAt(0);
                SqlParameter[] pParaName = new SqlParameter[mCom.Parameters.Count];
                mCom.Parameters.CopyTo(pParaName, 0);
                mCom.Parameters.Clear();
                if ((pParaName.Length > 0))
                {
                    for (int index = 0; index <= pParaName.Length - 1; index++)
                    {
                        mCom.Parameters.AddWithValue(pParaName[index].ParameterName, pParaValue[index]);
                    }
                }
                SqlDataAdapter DAP = new SqlDataAdapter(mCom);
                DAP.Fill(DS);
                return DS;
            }
            catch (Exception e)
            {
                Result = fnError(e);
                return DS;
            }
            finally
            {
                fnErrorLog(pSPName, Result);
                mCon.Close();
            }
        }

        public static DataTable fnDataTable(String mConnection ,String pSPName)
        {
            SqlConnection mCon = new SqlConnection(mConnection);
            DataTable DT = new DataTable("Data");
            String mResult = "";
            try
            {
                mCon.Open();
                SqlCommand mCom = new SqlCommand(pSPName, mCon);
                mCom.CommandType = CommandType.StoredProcedure;
                DT.Load(mCom.ExecuteReader());
                return DT;
            }
            catch (Exception e)
            {
                mResult = fnError(e);
                return DT;
            }
            finally
            {
                fnErrorLog(pSPName, mResult);
                mCon.Close();
            }
        }
        public static DataTable fnDataTable(String mConnection,String pSPName, params object[] pParaValue)
        {
            SqlConnection mCon = new SqlConnection(mConnection);
            DataTable DT = new DataTable("Data");
            String mResult = "";
            try
            {
                mCon.Open();
                SqlCommand mCom = new SqlCommand(pSPName, mCon);
                mCom.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(mCom);
                mCom.Parameters.RemoveAt(0);
                SqlParameter[] pParaName = new SqlParameter[mCom.Parameters.Count];
                mCom.Parameters.CopyTo(pParaName, 0);
                mCom.Parameters.Clear();
                if ((pParaName.Length > 0))
                {
                    for (int index = 0; index <= pParaName.Length - 1; index++)
                    {
                        mCom.Parameters.AddWithValue(pParaName[index].ParameterName, pParaValue[index]);
                    }
                }
                DT.Load(mCom.ExecuteReader());
                return DT;
            }
            catch (Exception e)
            {
                mResult = fnError(e);
                return DT;
            }
            finally
            {
                fnErrorLog(pSPName, mResult);
                mCon.Close();
            }
        }
        public static string fnError(Exception pError)
        {
            String mMessage = "";
            mMessage += "Error Message : " + pError.Message.ToString() + Environment.NewLine;
            return mMessage;
        }
        public static void fnErrorLog(String SPName, String Result)
        {
            String path = Path.Combine(Directory.GetCurrentDirectory().ToString(), "Errros");
            String mError = "Date & Time : " + DateTime.Now + Environment.NewLine +
                            "SP Name :  " + SPName + Environment.NewLine +
                            "Message :  " + Result + Environment.NewLine +
                            "----------END--------- " + Environment.NewLine;
            try
            {
                if (File.Exists(path) == false)
                {
                    File.Create(path);
                }
                File.WriteAllText(path, mError);
            }
            catch (Exception) { }
            finally { }
        }
    }
}
