using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();
            //Get All REgions From Web API
            var client = _httpClientFactory.CreateClient();

            var httpresponsemessage = await client.GetAsync("https://localhost:7164/api/Regions");

            httpresponsemessage.EnsureSuccessStatusCode();

            response.AddRange(await httpresponsemessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel regionmodel)
        {
            var client = _httpClientFactory.CreateClient();

            var httprequestmessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7164/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(regionmodel), System.Text.Encoding.UTF8, "application/json")
            };
            var httpresponsemessage = await client.SendAsync(httprequestmessage);
            httpresponsemessage.EnsureSuccessStatusCode();

            var response = await httpresponsemessage.Content.ReadFromJsonAsync<RegionDTO>();

            if(response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7164/api/Regions/{id.ToString()}");
            
            if(response is not null)
            {
                return View(response);
            }

            return View(null);
        }

        [HttpPut]
        public async Task<IActionResult> Edit (RegionDTO region)
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7164/api/Regions/{region.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(region), System.Text.Encoding.UTF8, "application/json")
            };

            var httpresponse = await client.SendAsync(request);
            httpresponse.EnsureSuccessStatusCode();

            var response = await httpresponse.Content.ReadFromJsonAsync<RegionDTO>();

            if (response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }

            return View("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpresponse = await client.DeleteAsync($"https://localhost:7164/api/Regions/{request.Id}");
                httpresponse.EnsureSuccessStatusCode();

                return RedirectToAction("Edit", "Regions");
            }
            catch (Exception ex)
            {

                throw;
            }

            return View("Edit") ;

        }
    
    
    }
}
