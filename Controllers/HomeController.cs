using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RestSharp;
using Tapsell.Models;
using System.Text.RegularExpressions;

namespace Tapsell.Controllers
{
    public class HomeController(IConfiguration configuration) : Controller
    {


        [Route("/rdata/{*rest}")]
        [Route("/")]
        public async Task<IActionResult> Index(string? rData = null, string? rest = null)
        {
            var setting1 = configuration["Site"];

            var html = await LoadMusicFa(rData, rest);
            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = 200
            };
        }

        private async Task<string> LoadMusicFa(string? rData = null, string? rest = null)
        {
            try
            {

                var localParameter = "rdata";
                var newsUrl = $"https://music-fa.com";
                string url = Request.Scheme + "://" + Request.Host + Request.Path;
                if (!string.IsNullOrEmpty(rest))
                {
                    url = $"{newsUrl}/{rest}";
                }
                else
                    url = newsUrl;

                using HttpClient client = new HttpClient();

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();

                // New base URL to replace with
                string newBaseUrl = "https://localhost:44303/?rData=";

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
