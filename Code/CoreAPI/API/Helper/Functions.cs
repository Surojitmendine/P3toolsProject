using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Helper
{
    public class Functions
    {
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        #region  Helper Functions

        public dynamic SetFKValueNullIfZero(dynamic d)
        {
            foreach (PropertyInfo propertyInfo in d.GetType().GetProperties())
            {
                string propname = propertyInfo.Name;
                if (propname.Contains("FK_") && (propertyInfo.PropertyType == typeof(Int32) || propertyInfo.PropertyType == typeof(Nullable<Int32>)))
                {
                    var v = propertyInfo.GetValue(d, null);
                    if (v == 0)
                    {
                        propertyInfo.SetValue(d, null);
                    }
                }

                //if(propname.ToLower().Contains("date"))
                //{
                //    var v = propertyInfo.GetValue(d, null);


                //        DateTime? dt =v==null?null: Convert.ToDateTime(DateTime.ParseExact(v, "dd/MM/yyyy", CultureInfo.InvariantCulture),enGBcultureFormat);
                //        propertyInfo.SetValue(d, dt);

                //}


            }

            return d;
        }

        /// <summary>
        /// dd/MM/YYYY Format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime ConvertStringToDate(string date)
        {

            string[] arrdate =string.IsNullOrEmpty(date)==false? date.Split('/'):new string[] { };
            string formateddate = string.Empty;
            if (arrdate.Length == 3)
            {
                string tmpdate = string.Empty;
                string tmpmonth = string.Empty;
                //tmpdate = arrdate[0].Length == 1 ? "0" + arrdate[0] : arrdate[0];
                tmpdate = arrdate[0];
                tmpmonth = arrdate[1].Length == 1 ? "0" + arrdate[1] : arrdate[1];
                formateddate = tmpmonth + "/" + tmpdate + "/" + arrdate[2];
            }
            if (!string.IsNullOrEmpty(formateddate))
            {
                // DateTime odateTime = DateTime.ParseExact(formateddate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
                DateTime odateTime = Convert.ToDateTime(formateddate,  CultureInfo.InvariantCulture);
                return odateTime;
            }
            else
            {
                return DateTime.Now;
            }

            
        }

        public string ConvertDateToString(DateTime? date)
        {

            return date.HasValue?date.Value.Date.ToString("dd/MM/yyyy"):string.Empty;


        }

        public TimeSpan ConvertStringToTime(string time)
        {
            string text = time;
            string[] formats = { "hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt" };

            var success = DateTime.TryParseExact(text, formats, CultureInfo.CurrentCulture,
                DateTimeStyles.None, out var value);

            return value.TimeOfDay;
        }

        public string ConvertTimeToString(TimeSpan? time)
        {
            //string text = time;
            //string[] formats = { "hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt" };
            string strtime =time.HasValue? DateTime.Today.Add((TimeSpan)time).ToString("hh:mm tt"):string.Empty;
            //var success = DateTime.TryParseExact(text, formats, CultureInfo.CurrentCulture,
            //    DateTimeStyles.None, out var value);

            //return value.TimeOfDay;
            return strtime;
        }

        public TimeSpan ConvertSystemTimeToCustomTime(TimeSpan time)
        {
            string text = time.ToString(@"h\:mm\:ss");          

            var success = TimeSpan.TryParse(text, out TimeSpan value);

            return value;
        }

        /// <summary>
        /// 0 based index (January is at 0 index)
        /// </summary>
        /// <param name="MonthNumber"></param>
        /// <returns></returns>
        public  string MonthName(int MonthNumber)
        {
            List<string> names = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();//.RemoveAll(x=>string.IsNullOrEmpty(x));

            names.RemoveAll(x => string.IsNullOrEmpty(x));

            return  names[MonthNumber];
        }

        #endregion
    }
}
