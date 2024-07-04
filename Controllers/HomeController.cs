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
            // var targetUrl = configuration["Site"];
            // if (targetUrl == "partobime.ir")
            // {
            //  
            // }

            var html = await LoadMusicFa(rData);
            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = 200
            };

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
