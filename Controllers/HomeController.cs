using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RestSharp;
using Tapsell.Models;
using System.Text.RegularExpressions;

namespace Tapsell.Controllers
{
    public class HomeController(IConfiguration configuration) : Controller
    {


        // [Route("/rData")]
        [Route("/rdata/{*rest}")]

        [Route("/")]
        //[HttpGet("{*catchall}")]

        public async Task<IActionResult> Index(string? rData = null)
        {
            if (!Request.Host.HasValue)
                return BadRequest("Error Loading Website.....!!!");

            var domainUrl = Request.Host.Value;


#if DEBUG
            var testSite = await LoadRozMusic(rData);
            return new ContentResult
            {
                Content = testSite,
                ContentType = "text/html",
                StatusCode = 200
            };
#endif


            if (domainUrl.Contains("partobime"))
            {
                var html = await LoadMusicFa(rData);
                return new ContentResult
                {
                    Content = html,
                    ContentType = "text/html",
                    StatusCode = 200
                };

            }
            else if (domainUrl.Contains("inring"))
            {
                var html = await LoadUpMusics(rData);
                return new ContentResult
                {
                    Content = html,
                    ContentType = "text/html",
                    StatusCode = 200
                };

            }
            else
            {
                return BadRequest("Error Loading Website.....!!!");
            }


            return BadRequest("Error Loading Website.....!!!");
            // var html = await LoadMusicFa(rData);
            // return new ContentResult
            // {
            //     Content = html,
            //     ContentType = "text/html",
            //     StatusCode = 200
            // };

        }


        public async Task<string> LoadRozMusic(string? rData = null)
        {
            try
            {

                var fakeDomain = "https://mhdbest.ir";
                var localParameter = "rdata";
                var newsUrl = $"https://rozmusic.com";
                string url = Request.Scheme + "://" + Request.Host + Request.Path;
                if (!string.IsNullOrEmpty(rData))
                {
                    url = $"{newsUrl}{rData}";
                }
                else
                    url = newsUrl;

                using HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();


                var html = await response.Content.ReadAsStringAsync();

#if DEBUG
                string newBaseUrl = "https://localhost:7294/?rData=";
#else
      string newBaseUrl = $"{fakeDomain}/?rData=";
#endif



                // Regular expression to find href attributes in a tags with the specific domain and capture the path part
                string pattern = @"(<a[^>]*\shref=['""]https:\/\/rozmusic\.com)([^'""]*['""][^>]*>)";

                // Method to replace the matched URL with the new base URL and keep the path intact
                string result = Regex.Replace(html, pattern, m =>
                {
                    string path = m.Groups[2].Value;
                    return m.Groups[1].Value.Replace("https://rozmusic.com", newBaseUrl) + path;
                }, RegexOptions.IgnoreCase);


                string pattern1 = @"<title>(.*?)<\/title>";
                string replacement = "<title>مهد موزیک 👌 لول متفاوتی از شادی با مهد موزیک</title>";
                result = Regex.Replace(result, pattern1, replacement);



                #region تگ های مدباادز

                result = result.Replace("<footer>", "<div id=\"mediaad-YwWm0\"></div><footer>");
                // result = result.Replace("<div class=\"post_content\">", "<div id=\"mediaad-zv4yB\" ></div><div class=\"post_content\">");
                result = result.Replace("<div class=\"post_content\">", "<div id=\"mediaad-G7X5p\" ></div><div class=\"post_content\">");
                result = result.Replace("<aside class=\"tab\">", "<div id=\"mediaad-1d4mG\" ></div><aside class=\"tab\">");


                int count = 0;
                result = Regex.Replace(result, @"<article class=""box_right post"">", match =>
                {
                    count++;
                    return count == 2 ? "<div id=\"mediaad-qnM1Z\" ></div><article class=\"box_right post\">" : match.Value;
                });

                count = 0;
                result = Regex.Replace(result, @"<article class=""box_right post"">", match =>
                {
                    count++;
                    return count == 3 ? "<div id=\"mediaad-dY7jy\" ></div><article class=\"box_right post\">" : match.Value;
                });

                count = 0;
                result = Regex.Replace(result, @"<article class=""box_right post"">", match =>
                {
                    count++;
                    return count == 5 ? "<div id=\"mediaad-EgXPk\" ></div><article class=\"box_right post\">" : match.Value;
                });




                #region صفحات داخلی

                if (!string.IsNullOrEmpty(rData))
                {
                    result = result.Replace("<div class=\"matnmusic\">", "<div id=\"mediaad-zv4yB\" ></div><div class=\"matnmusic\">");
                    result = result.Replace("<aside class=\"tab\">", "<div id=\"mediaad-45PZ3\" ></div><aside class=\"tab\">");

                }

                #endregion



                #endregion

                #region لود فایل مدیاادز

                result = result.Replace(@"</head>",
                    """
                    <script type="text/javascript">
                        const head = document.getElementsByTagName("head")[0];
                        const script = document.createElement("script");
                        script.type = "text/javascript";
                        script.async = true;
                        script.src = "https://s1.mediaad.org/serve/mhdbest.ir/loader.js";
                        head.appendChild(script);
                    </script>
                    """);

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }




        private async Task<string> LoadMusicFa(string? rData = null)
        {
            try
            {

                var localParameter = "rdata";
                var newsUrl = $"https://music-fa.com";
                string url = Request.Scheme + "://" + Request.Host + Request.Path;
                if (!string.IsNullOrEmpty(rData))
                {
                    url = $"{newsUrl}{rData}";
                }
                else
                    url = newsUrl;

                using HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();

#if DEBUG
                string newBaseUrl = "https://localhost:7294/?rData=";
#else
      string newBaseUrl = "https://partobime.ir/?rData=";
#endif



                // Regular expression to find href attributes in a tags with the specific domain and capture the path part
                string pattern = @"(<a[^>]*\shref=['""]https:\/\/music-fa\.com)([^'""]*['""][^>]*>)";

                // Method to replace the matched URL with the new base URL and keep the path intact
                string result = Regex.Replace(html, pattern, m =>
                {
                    string path = m.Groups[2].Value;
                    return m.Groups[1].Value.Replace("https://music-fa.com", newBaseUrl) + path;
                }, RegexOptions.IgnoreCase);

                // Replace all domain parts in href attributes with the new domain
                //  string result = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);


                string pattern1 = @"<title>(.*?)<\/title>";
                string replacement = "<title>موزیک بادز• دانلود جدیدترین آهنگ ها</title>";
                result = Regex.Replace(result, pattern1, replacement);

                result = result.Replace(@"<main class=""mf_home mf_fx"">",
                    @"<div id=""mediaad-0e3zA""></div><main class=""mf_home mf_fx"">");

                result = result.Replace(@"<div class=""bklnk"">",
                    @"<div id=""mediaad-g3QWB"" ></div><div class=""bklnk"">");

                if (!string.IsNullOrEmpty(rData))
                {
                    if (rData.Contains("download-song"))
                    {
                        result = result.Replace(@"<article class=""mf_pst""",
                            @"<div id=""mediaad-MGaM2"" ></div><article class=""mf_pst""");
                    }

                }



                result = result.Replace(@"</head>",
                    """
                    <script type="text/javascript">
                        const head = document.getElementsByTagName("head")[0];
                        const script = document.createElement("script");
                        script.type = "text/javascript";
                        script.async = true;
                        script.src = "https://s1.mediaad.org/serve/partobime.ir/loader.js";
                        head.appendChild(script);
                    </script>
                    """);

                return result;

            }
            catch (Exception ex)
            {
                return "";
            }
        }


        private async Task<string> LoadUpMusics(string? rData = null)
        {
            try
            {
                var fakeDomain = "https://inring.ir";
                var localParameter = "rdata";
                var newsUrl = $"https://upmusics.com";
                string url = Request.Scheme + "://" + Request.Host + Request.Path;
                if (!string.IsNullOrEmpty(rData))
                {
                    url = $"{newsUrl}{rData}";
                }
                else
                    url = newsUrl;

                using HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();

#if DEBUG
                string newBaseUrl = "https://localhost:7294/?rData=";
#else
      string newBaseUrl = $"{fakeDomain}/?rData=";
#endif



                // Regular expression to find href attributes in a tags with the specific domain and capture the path part
                string pattern = @"(<a[^>]*\shref=['""]https:\/\/upmusics\.com)([^'""]*['""][^>]*>)";

                // Method to replace the matched URL with the new base URL and keep the path intact
                string result = Regex.Replace(html, pattern, m =>
                {
                    string path = m.Groups[2].Value;
                    return m.Groups[1].Value.Replace("https://upmusics.com", newBaseUrl) + path;
                }, RegexOptions.IgnoreCase);

                // Replace all domain parts in href attributes with the new domain
                //  string result = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);


                string pattern1 = @"<title>(.*?)<\/title>";
                string replacement = "<title>رینگ موزیک😍 دانلود جدیدترین آهنگ ها در رینگ موزیک</title>";
                result = Regex.Replace(result, pattern1, replacement);

                #region حذف تبلیغات اصلی سایت

                // Define the regex pattern to match the div with id starting with 'mediaad-'
                string mediaAdspattern = @"<div id='mediaad-[^']*'></div>";

                // Replace matched patterns with an empty string
                result = Regex.Replace(result, pattern, string.Empty);


                result = result.Replace("https://static.pushe.co/pusheweb.js", "").Replace("static.pushe.co/pusheweb.js", "").Replace("static.pushe.co", "");



                #endregion


                result = result.Replace("<div class=\"upctr\">", "<br><div id=\"mediaad-3e7Mx\"></div><div class=\"upctr\">");
                result = result.Replace("<div class=\"upctr\">", "<div class=\"upctr\">");
                result = result.Replace("<h2>جدیدترین آهنگ ها</h2>", "<h2></h2><br/><div id=\"mediaad-X2n8O\"></div>");
                result = result.Replace("<h2>آهنگ های ویژه</h2>", "<h2></h2><br/><div id=\"mediaad-g3wrr\"></div>");
                result = result.Replace("<div class=\"upbu\">", "<div id=\"mediaad-kXqyA\"></div><div id=\"mediaad-rmlRJ\"></div><div id=\"mediaad-g3qkm\"></div><div class=\"upbu\">");
                result = result.Replace("<div class=\"uph1 uph12\">", "<div id=\"mediaad-exOxY\"></div><div class=\"uph1 uph12\">");
                result = result.Replace("<footer", "<div id=\"mediaad-exR76\" ></div><footer");

                if (!string.IsNullOrEmpty(rData))
                {
                    result = result.Replace("<ul class=\"upslnk upf\">", "<div id=\"mediaad-rmD4P\" ></div><ul class=\"upslnk upf\">");
                    result = result.Replace("<div class=\"updmp3 upf\"><", "<div id=\"mediaad-jg7zp\" ><div class=\"updmp3 upf\"><");

                }

                #region لود فایل مدیاادز

                result = result.Replace(@"</head>",
                    """
                    <script type="text/javascript">
                        const head = document.getElementsByTagName("head")[0];
                        const script = document.createElement("script");
                        script.type = "text/javascript";
                        script.async = true;
                        script.src = "https://s1.mediaad.org/serve/inring.ir/loader.js";
                        head.appendChild(script);
                    </script>
                    """);

                #endregion



                return result;

            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
