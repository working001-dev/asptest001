using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace api_insEXchange
{
    static class APIGET
    {
        public static int img_err = 0;
        static APIGET()
        {

        }

        public static void API_TEST()
        {
            Console.WriteLine("Dew Test!!!!");
        }



        public static JArray API_GETJARRAY(String url)
        {
            //string reqUrl =;

            Uri address = new Uri(url);

            // Create the web request
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST
            request.Method = "GET";
            request.ContentType = "text/json";
            string strOutputXml;
            try
            {
                // using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                //  {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                // Get the response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output
                strOutputXml = reader.ReadToEnd();
                //Console.WriteLine(strOutputXml);

                JArray json = JArray.Parse(strOutputXml);
                //JObject json = JObject.Parse(strOutputXml);
                // Console.WriteLine(json.GetType().FullName);
                // Console.WriteLine( textArray[0]["USER_CD"] + "  " + textArray[0]["USER_TNAME"]);

                //label1.Text = json["rates"]["JPY"].ToString();
                request.ServicePoint.CloseConnectionGroup(request.ConnectionGroupName);
                request = null;
                return json;
                //  }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message + " => " + ex.HResult);
                return JArray.Parse("[]");
            }

        }
        public static JObject API_GETJOBJECT(String url)
        {
            //string reqUrl =;

            Uri address = new Uri(url);

            // Create the web request
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST
            request.Method = "GET";
            request.ContentType = "text/json";
            string strOutputXml;
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    // Console application output
                    strOutputXml = reader.ReadToEnd();
                    //Console.WriteLine(strOutputXml);

                    //JArray json = JArray.Parse(strOutputXml);
                    JObject json = JObject.Parse(strOutputXml);
                    // Console.WriteLine(json.GetType().FullName);
                    // Console.WriteLine( textArray[0]["USER_CD"] + "  " + textArray[0]["USER_TNAME"]);

                    //label1.Text = json["rates"]["JPY"].ToString();
                    request.ServicePoint.CloseConnectionGroup(request.ConnectionGroupName);
                    request = null;
                    return json;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message + " => " + ex.HResult);
                return JObject.Parse("{}");
            }

        }

        public static string API_REQUEST(String url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var postData = "thing=" + Uri.EscapeDataString("hello");
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "text/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


            //Console.WriteLine(responseString);
            return responseString;
        }
    }
}
