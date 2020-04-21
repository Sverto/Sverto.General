using HtmlAgilityPack;
using Sverto.General.Coding;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Sverto.General.Extensions
{
    public static class WebExtensions
    {
        /// <summary>
        /// Convert a String of html code to a html object
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlDocument ToHtml(this string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }

        /// <summary>
        /// Convert a Char Array of html code to a html object
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlDocument ToHtml(this byte[] html)
        {
            return ToHtml(ToHtmlString(html));
        }
        public static string ToHtmlString(this byte[] html)
        {
            return Encoding.UTF8.GetString(html);
        }

        /// <summary>
        /// Convert a name-value collection to a String query ex. 'value1=a&value2=b'
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection nvc)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                string[] values = nvc.GetValues(key);
                if (values == null) continue;

                foreach (string value in values)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Send a POST using client to the provided url with given parameters and their values
        /// </summary>
        /// <param name="client">Instance of WebClient</param>
        /// <param name="url">The URL to POST to</param>
        /// <param name="reqParams">The parameters and their values to POST</param>
        /// <returns></returns>
        public static ResultS Post(this WebClient client, string url, NameValueCollection reqParams)
        {
            //Console.WriteLine("POST: " + url);
            try
            {
                return new ResultS(client.UploadValues(url, "POST", reqParams).ToHtmlString());
            }
            catch (WebException ex)
            {
                return new ResultS(ex);
            }
        }

        /// <summary>
        /// Send a POST using client to the provided url with given parameters and their values
        /// </summary>
        /// <param name="client">Instance of WebClient</param>
        /// <param name="url">The URL to POST to</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="paramValue">Value for parameter one</param>
        /// <param name="paramName2">Second parameter name</param>
        /// <param name="paramValue2">Value for parameter two</param>
        /// <returns></returns>
        public static ResultS Post(this WebClient client, string url, string paramName = null, string paramValue = null, string paramName2 = null, string paramValue2 = null)
        {
            var reqParams = new NameValueCollection();
            if (paramName != null)
                reqParams.Add(paramName, paramValue);
            if (paramName2 != null)
                reqParams.Add(paramName2, paramValue2);

            return Post(client, url, reqParams);
        }

        /// <summary>
        /// Send a GET using client to the provided url with given parameters and their values
        /// </summary>
        /// <param name="client">Instance of WebClient</param>
        /// <param name="url">The URL to GET</param>
        /// <param name="reqParams">The parameters and their values to append</param>
        /// <returns></returns>
        public static ResultS Get(this WebClient client, string url, NameValueCollection reqParams)
        {
            if (reqParams.Count > 0)
            {
                var builder = new StringBuilder(url);
                var values = ToQueryString(reqParams);
                if (url.Contains('?'))
                    builder.Append('&');
                else
                    builder.Append('?');
                builder.Append(values);
                url = builder.ToString();
            }
            try
            {
                return new ResultS(client.DownloadString(url));
            }
            catch (WebException ex)
            {
                return new ResultS(ex);
            }
        }

        /// <summary>
        /// Send a GET using client to the provided url with given parameters and their values
        /// </summary>
        /// <param name="client">Instance of WebClient</param>
        /// <param name="url">The URL to GET</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="paramValue">Value for parameter one</param>
        /// <param name="paramName2">Second parameter name</param>
        /// <param name="paramValue2">Value for parameter two</param>
        /// <returns></returns>
        public static ResultS Get(this WebClient client, string url, string paramName = null, string paramValue = null, string paramName2 = null, string paramValue2 = null)
        {
            var reqParams = new NameValueCollection();
            if (paramName != null)
                reqParams.Add(paramName, paramValue);
            if (paramName2 != null)
                reqParams.Add(paramName2, paramValue2);

            return Get(client, url, reqParams);
        }
    }
}
