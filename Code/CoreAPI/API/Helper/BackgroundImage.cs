using System;
using System.Net;
using System.Globalization;

using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BackgroundImage
{
    class SpotlightImage
    {
        private const string MetaHeader = "[SpotlightImage]";

        public string Uri { get; set; }
        public string Sha256 { get; set; }
        public int FileSize { get; set; }
        public string Title { get; set; }
        public string Copyright { get; set; }
    }


    /// <summary>
    /// Wrapper around the Spotlight JSON API
    /// </summary>
    static class Spotlight
    {
        /// <summary>
        /// Request new images from the Spotlight API and return raw JSON response
        /// </summary>
        /// <param name="maxres">Force maximum image resolution. Otherwise, current image resolution is used</param>
        /// <param name="locale">Null = Auto detect from current system, or specify xx-XX value format such as en-US</param>
        /// <returns>Raw JSON response</returns>
        /// <exception cref="System.Net.WebException">An exception is thrown if the request fails</exception>
        private static string PerformApiRequest(bool maxres, string locale = null)
        {
            WebClient webClient = new WebClient();
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            RegionInfo currentRegion = new RegionInfo(currentCulture.LCID);
            string region = currentRegion.TwoLetterISORegionName.ToLower();

            if (locale == null)
                locale = currentCulture.Name;
            else if (locale.Length > 2 && locale.Contains("-"))
                region = locale.Split('-')[1].ToLower();

            int screenWidth = 99999;
            int screenHeight = 99999;

            string request = String.Format(
                "https://arc.msn.com/v3/Delivery/Cache?pid=209567&fmt=json&rafb=0&ua=WindowsShellClient"
                    + "%2F0&disphorzres={0}&dispvertres={1}&lo=80217&pl={2}&lc={3}&ctry={4}&time={5}",
                    screenWidth,
                    screenHeight,
                locale,
                locale,
                region,
                DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            );

            byte[] stringRaw = webClient.DownloadData(request);
            return Encoding.UTF8.GetString(stringRaw);
        }

        /// <summary>
        /// Request new images from the Spotlight API and return image urls
        /// </summary>
        /// <param name="maxres">Force maximum image resolution. Otherwise, current main monitor screen resolution is used</param>
        /// <param name="portrait">Null = Auto detect from current main monitor resolution, True = portrait, False = landscape.</param>
        /// <param name="locale">Null = Auto detect from current system, or specify xx-XX value format such as en-US</param>
        /// <param name="attempts">Amount of API call attempts before raising an exception if an error occurs</param>
        /// <returns>List of images</returns>
        /// <exception cref="System.Net.WebException">An exception is thrown if the request fails</exception>
        /// <exception cref="System.IO.InvalidDataException">An exception is thrown if the JSON data is invalid</exception>
        public static /*SpotlightImage[]*/string GetImageUrls(bool maxres = false, bool? portrait = null, string locale = null, int attempts = 1)
        {
            while (true)
            {
                try
                {
                    attempts--;
                    return GetImageUrlsSingleAttempt(maxres, portrait, locale);
                }
                catch (Exception e)
                {
                    if (attempts > 0)
                    {
                        Console.Error.WriteLine("SpotlightAPI: " + e.GetType() + ": " + e.Message + " - Waiting 10 seconds before retrying...");
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                    else throw;
                }
            }
        }

        /// <summary>
        /// Request new images from the Spotlight API and return image urls (Single Attempt)
        /// </summary>
        /// <param name="maxres">Force maximum image resolution. Otherwise, current main monitor screen resolution is used</param>
        /// <param name="portrait">Null = Auto detect from current main monitor resolution, True = portrait, False = landscape.</param>
        /// <param name="locale">Null = Auto detect from current system, or specify xx-XX value format such as en-US</param>
        /// <returns>List of images</returns>
        /// <exception cref="System.Net.WebException">An exception is thrown if the request fails</exception>
        /// <exception cref="System.IO.InvalidDataException">An exception is thrown if the JSON data is invalid</exception>
        private static /*SpotlightImage[]*/ string GetImageUrlsSingleAttempt(bool maxres = false, bool? portrait = null, string locale = null)
        {
            List<SpotlightImage> images = new List<SpotlightImage>();
            Json.JSONData imageData = Json.ParseJson(PerformApiRequest(maxres, locale));



            if (imageData.Type == Json.JSONData.DataType.Object && imageData.Properties.ContainsKey("batchrsp"))
            {
                imageData = imageData.Properties["batchrsp"];
                if (imageData.Type == Json.JSONData.DataType.Object && imageData.Properties.ContainsKey("items"))
                {
                    if (!imageData.Properties.ContainsKey("ver") || imageData.Properties["ver"].StringValue != "1.0")
                        Console.Error.WriteLine("SpotlightAPI: Unknown or missing API response version. Errors may occur.");
                    imageData = imageData.Properties["items"];
                    if (imageData.Type == Json.JSONData.DataType.Array)
                    {
                        foreach (Json.JSONData element in imageData.DataArray)
                        {
                            if (element.Type == Json.JSONData.DataType.Object && element.Properties.ContainsKey("item"))
                            {
                                Json.JSONData item = element.Properties["item"];
                                if (item.Type == Json.JSONData.DataType.String)
                                    item = Json.ParseJson(item.StringValue);
                                if (item.Type == Json.JSONData.DataType.Object && item.Properties.ContainsKey("ad") && item.Properties["ad"].Type == Json.JSONData.DataType.Object)
                                {
                                    item = item.Properties["ad"];

                                    foreach (var aditem in item.Properties)
                                    {
                                        string title;
                                        Json.JSONData titleObj = aditem.Value.Properties.ContainsKey("title_text") ? aditem.Value.Properties["title_text"] : null;
                                        if (titleObj != null && titleObj.Type == Json.JSONData.DataType.Object && titleObj.Properties.ContainsKey("tx"))
                                            title = titleObj.Properties["tx"].StringValue;
                                        else title = null;

                                        string copyright;
                                        Json.JSONData copyrightObj = aditem.Value.Properties.ContainsKey("copyright_text") ? aditem.Value.Properties["copyright_text"] : null;
                                        if (copyrightObj != null && copyrightObj.Type == Json.JSONData.DataType.Object && copyrightObj.Properties.ContainsKey("tx"))
                                            copyright = copyrightObj.Properties["tx"].StringValue;
                                        else copyright = null;



                                        string urlField = "image_fullscreen_001_landscape";//portrait.Value ? "image_fullscreen_001_portrait" : "image_fullscreen_001_landscape";
                                        if (aditem.Key==urlField /*&& aditem.Value.Properties[urlField].Type == Json.JSONData.DataType.Object*/)
                                        {
                                            //item = item.Properties[urlField];
                                            if (aditem.Value.Properties.ContainsKey("u") && aditem.Value.Properties.ContainsKey("sha256") && aditem.Value.Properties.ContainsKey("fileSize")
                                                && aditem.Value.Properties["u"].Type == Json.JSONData.DataType.String
                                                && aditem.Value.Properties["sha256"].Type == Json.JSONData.DataType.String
                                                && aditem.Value.Properties["fileSize"].Type == Json.JSONData.DataType.String)
                                            {
                                                int fileSizeParsed = 0;
                                                if (int.TryParse(aditem.Value.Properties["fileSize"].StringValue, out fileSizeParsed))
                                                {
                                                    SpotlightImage image = new SpotlightImage()
                                                    {
                                                        Uri = aditem.Value.Properties["u"].StringValue,
                                                        Sha256 = aditem.Value.Properties["sha256"].StringValue,
                                                        FileSize = fileSizeParsed,
                                                        Title = title,
                                                        Copyright = copyright
                                                    };
                                                    //try
                                                    //{
                                                    //    System.Convert.FromBase64String(image.Sha256);
                                                    //}
                                                    //catch
                                                    //{
                                                    //    image.Sha256 = null;
                                                    //}
                                                    //if (!String.IsNullOrEmpty(image.Uri)
                                                    //    && !String.IsNullOrEmpty(image.Sha256)
                                                    //    && image.FileSize > 0)
                                                    //{
                                                    images.Add(image);
                                                }
                                                //else Console.Error.WriteLine("SpotlightAPI: Ignoring image with empty uri, hash and/or file size less or equal to 0.");
                                            }
                                           // else Console.Error.WriteLine("SpotlightAPI: Ignoring image with invalid, non-number file size.");
                                        }
                                       // else Console.Error.WriteLine("SpotlightAPI: Ignoring item image uri with missing 'u', 'sha256' and/or 'fileSize' field(s).");
                                        //}
                                        //else Console.Error.WriteLine("SpotlightAPI: Ignoring item image with missing uri.");
                                    }


                                }
                               // else Console.Error.WriteLine("SpotlightAPI: Ignoring item with missing 'ad' object.");
                            }
                            //else Console.Error.WriteLine("SpotlightAPI: Ignoring non-object item while parsing 'batchrsp/items' field in JSON API response.");
                        }
                    }
                    //else throw new InvalidDataException("SpotlightAPI: 'batchrsp/items' field in JSON API response is not an array.");
                }
                //else throw new InvalidDataException("SpotlightAPI: Missing 'batchrsp/items' field in JSON API response." + (locale != null ? " Locale '" + locale + "' may be invalid." : ""));
            }
            //else throw new InvalidDataException("SpotlightAPI: API did not return a 'batchrsp' JSON object.");

           return images[0].Uri;
           // return images.ToArray();
        }
    }

    static class Json
    {
        /// <summary>
        /// Parse some JSON and return the corresponding JSON object
        /// </summary>

        public static JSONData ParseJson(string json)
        {
            int cursorpos = 0;
            return String2Data(json, ref cursorpos);
        }

        /// <summary>
        /// The class storing unserialized JSON data
        /// The data can be an object, an array or a string
        /// </summary>

        public class JSONData
        {
            public enum DataType { Object, Array, String };
            private DataType type;
            public DataType Type { get { return type; } }
            public Dictionary<string, JSONData> Properties;
            public List<JSONData> DataArray;
            public string StringValue;
            public JSONData(DataType datatype)
            {
                type = datatype;
                Properties = new Dictionary<string, JSONData>();
                DataArray = new List<JSONData>();
                StringValue = String.Empty;
            }
        }

        /// <summary>
        /// Parse a JSON string to build a JSON object
        /// </summary>
        /// <param name="toparse">String to parse</param>
        /// <param name="cursorpos">Cursor start (set to 0 for function init)</param>

        private static JSONData String2Data(string toparse, ref int cursorpos)
        {
            try
            {
                JSONData data;
                switch (toparse[cursorpos])
                {
                    //Object
                    case '{':
                        data = new JSONData(JSONData.DataType.Object);
                        cursorpos++;
                        while (toparse[cursorpos] != '}')
                        {
                            if (toparse[cursorpos] == '"')
                            {
                                JSONData propertyname = String2Data(toparse, ref cursorpos);
                                if (toparse[cursorpos] == ':') { cursorpos++; } else { /* parse error ? */ }
                                JSONData propertyData = String2Data(toparse, ref cursorpos);
                                data.Properties[propertyname.StringValue] = propertyData;
                            }
                            else cursorpos++;
                        }
                        cursorpos++;
                        break;

                    //Array
                    case '[':
                        data = new JSONData(JSONData.DataType.Array);
                        cursorpos++;
                        while (toparse[cursorpos] != ']')
                        {
                            if (toparse[cursorpos] == ',') { cursorpos++; }
                            JSONData arrayItem = String2Data(toparse, ref cursorpos);
                            data.DataArray.Add(arrayItem);
                        }
                        cursorpos++;
                        break;

                    //String
                    case '"':
                        data = new JSONData(JSONData.DataType.String);
                        cursorpos++;
                        while (toparse[cursorpos] != '"')
                        {
                            if (toparse[cursorpos] == '\\')
                            {
                                try //Unicode character \u0123
                                {
                                    if (toparse[cursorpos + 1] == 'u'
                                        && isHex(toparse[cursorpos + 2])
                                        && isHex(toparse[cursorpos + 3])
                                        && isHex(toparse[cursorpos + 4])
                                        && isHex(toparse[cursorpos + 5]))
                                    {
                                        //"abc\u0123abc" => "0123" => 0123 => Unicode char n°0123 => Add char to string
                                        data.StringValue += char.ConvertFromUtf32(int.Parse(toparse.Substring(cursorpos + 2, 4), System.Globalization.NumberStyles.HexNumber));
                                        cursorpos += 6; continue;
                                    }
                                    else if (toparse[cursorpos + 1] == 'n')
                                    {
                                        data.StringValue += '\n';
                                        cursorpos += 2;
                                        continue;
                                    }
                                    else if (toparse[cursorpos + 1] == 'r')
                                    {
                                        data.StringValue += '\r';
                                        cursorpos += 2;
                                        continue;
                                    }
                                    else if (toparse[cursorpos + 1] == 't')
                                    {
                                        data.StringValue += '\t';
                                        cursorpos += 2;
                                        continue;
                                    }
                                    else cursorpos++; //Normal character escapement \"
                                }
                                catch (IndexOutOfRangeException) { cursorpos++; } // \u01<end of string>
                                catch (ArgumentOutOfRangeException) { cursorpos++; } // Unicode index 0123 was invalid
                            }
                            data.StringValue += toparse[cursorpos];
                            cursorpos++;
                        }
                        cursorpos++;
                        break;

                    //Number
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                        data = new JSONData(JSONData.DataType.String);
                        StringBuilder sb = new StringBuilder();
                        while ((toparse[cursorpos] >= '0' && toparse[cursorpos] <= '9') || toparse[cursorpos] == '.')
                        {
                            sb.Append(toparse[cursorpos]);
                            cursorpos++;
                        }
                        data.StringValue = sb.ToString();
                        break;

                    //Boolean : true
                    case 't':
                        data = new JSONData(JSONData.DataType.String);
                        cursorpos++;
                        if (toparse[cursorpos] == 'r') { cursorpos++; }
                        if (toparse[cursorpos] == 'u') { cursorpos++; }
                        if (toparse[cursorpos] == 'e') { cursorpos++; data.StringValue = "true"; }
                        break;

                    //Boolean : false
                    case 'f':
                        data = new JSONData(JSONData.DataType.String);
                        cursorpos++;
                        if (toparse[cursorpos] == 'a') { cursorpos++; }
                        if (toparse[cursorpos] == 'l') { cursorpos++; }
                        if (toparse[cursorpos] == 's') { cursorpos++; }
                        if (toparse[cursorpos] == 'e') { cursorpos++; data.StringValue = "false"; }
                        break;

                    //Unknown data
                    default:
                        cursorpos++;
                        return String2Data(toparse, ref cursorpos);
                }
                while (cursorpos < toparse.Length
                    && (char.IsWhiteSpace(toparse[cursorpos])
                    || toparse[cursorpos] == '\r'
                    || toparse[cursorpos] == '\n'))
                    cursorpos++;
                return data;
            }
            catch (IndexOutOfRangeException)
            {
                return new JSONData(JSONData.DataType.String);
            }
        }

        /// <summary>
        /// Small function for checking if a char is an hexadecimal char (0-9 A-F a-f)
        /// </summary>
        /// <param name="c">Char to test</param>
        /// <returns>True if hexadecimal</returns>

        private static bool isHex(char c) { return ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')); }
    }


    static class BingImageOfTheDay
    {
        public static async Task<string> ImageOfTheDay()
        {
            Random rnd = new Random();
            string[] mkt = new string[] {  "es-AR", "en-AU", "de-AT", "nl-BE", "fr-BE", "pt-BR", "en-CA", "fr-CA", "es-CL", "da-DK", "fi-FI",
                                                "fr-FR", "de-DE", "zh-HK", "en-IN", "en-ID", "it-IT", "ja-JP", "ko-KR", "en-MY", "es-MX", "nl-NL", "en-NZ", "zh-CN",
                                                "pl-PL", "pt-PT", "en-PH", "ru-RU", "ar-SA", "en-ZA", "es-ES", "sv-SE", "fr-CH", "de-CH", "tr-TR", "en-GB", "en-US", "es-US" };

            int mktindex = rnd.Next(38);     // creates a number between 0 and 5

            string strBingImageURL = string.Format("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=7&mkt=" + mkt[mktindex].ToString() + "&safeSearch=Strict");
            string strJSONString = "";

            HttpClient client = new HttpClient();
          

            // Using an Async call makes sure the app is responsive during the time the response is fetched.
            // GetAsync sends an Async GET request to the Specified URI.
            HttpResponseMessage response = await client.GetAsync(new Uri(strBingImageURL));

            // Content property get or sets the content of a HTTP response message. 
            // ReadAsStringAsync is a method of the HttpContent which asynchronously 
            // reads the content of the HTTP Response and returns as a string.
            strJSONString = await response.Content.ReadAsStringAsync();

            JObject jobj = JObject.Parse(strJSONString);

            rnd = new Random();
            int index = rnd.Next(6);     // creates a number between 0 and 5

            string imageurl = "https://www.bing.com/" + jobj["images"][index]["url"].Value<string>();

            return imageurl;
        }
    }


}
