using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SBDB_CAD.Models;

namespace SBDB_CAD.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Method creates a HTTP client pool for API connections
        /// </summary>
        /// <param name="httpClientFactory">Interface for HTTP clients</param>
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        /// <summary>
        /// Main index method
        /// </summary>
        /// <param name="CadInput">User selection from the front end</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(CadInputModel CadInput)
        {
            string baseUrl = "https://ssd-api.jpl.nasa.gov/cad.api";
            string url = $"{baseUrl}?date-min={CadInput.StartDate.ToString("yyyy-MM-dd")}&date-max={CadInput.EndDate.ToString("yyyy-MM-dd")}&dist-min={CadInput.MinDistance}&dist-max={CadInput.MaxDistance}&fullname=true";
            if (!string.IsNullOrEmpty(CadInput.Body) && CadInput.Body != "All")
            {
                url += $"&body={CadInput.Body}";
            }
            if (!string.IsNullOrEmpty(CadInput.ObjectType) && CadInput.ObjectType != "all")
            {
                url += $"&{CadInput.ObjectType}=true";
            }
            ViewData["ApiUrl"] = url;
            List<CadModel> CadData = new List<CadModel>();
            try
            {
                CadData = await GetApiDataAsync(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CadData", CadData);
            }
            var viewModel = new CadViewModel
            {
                CadInput = CadInput,
                CadData = CadData
            };
            return View(viewModel);
        }
        /// <summary>
        /// Get response from SBDB Close-Approach Data API
        /// </summary>
        /// <param name="url">API request URL</param>
        /// <returns>List of CadModel objects containing the needed data</returns>
        private async Task<List<CadModel>> GetApiDataAsync(string url)
        {
            List<CadModel> Data = new List<CadModel>();
            try
            {
                var response = (await _httpClient.GetStringAsync(url)).ToString();
                var apiResponse = JsonConvert.DeserializeObject<CadApiModel>(response);
                if (apiResponse.Data != null)
                {
                    foreach (var item in apiResponse.Data)
                    {
                        try
                        {
                            var cadModel = new CadModel
                            {
                                des = item[0],
                                orbit_id = int.Parse(item[1]),
                                jd = ParseNullableDouble(item[2]),
                                cd = DateTime.ParseExact(item[3], "yyyy-MMM-dd HH:mm", CultureInfo.InvariantCulture),
                                dist = ParseNullableDouble(item[4]),
                                dist_min = ParseNullableDouble(item[5]),
                                dist_max = ParseNullableDouble(item[6]),
                                v_rel = ParseNullableDouble(item[7]),
                                v_inf = item[8],
                                t_sigma_f = item[9],
                                h = ParseNullableDouble(item[10]),
                                fullname = item[11]
                            };

                            Data.Add(cadModel);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing data: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return Data;
        }
        /// <summary>
        /// Prases information from the API.
        /// </summary>
        /// <param name="value">Value is the intended value</param>
        /// <returns>Returns either NaN or the double value</returns>
        private double ParseNullableDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return double.NaN;
            }
            else if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }
            return double.NaN;
        }
        /// <summary>
        /// About page
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            ViewData["Message"] = "SBDB Close-Approach Data API visualizer";

            return View();
        }
        /// <summary>
        /// Contact page
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Below is the contact info.";

            return View();
        }
        /// <summary>
        /// Privacy policy
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Error handling
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
