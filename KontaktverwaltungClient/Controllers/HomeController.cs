using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using KontaktverwaltungClient.Models;
using KontaktverwaltungLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace KontaktverwaltungClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            using var client = new HttpClient();
            var url = "http://localhost:5027/api/Kontaktverwaltung/Kontaktliste";
            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var kontaktliste = JsonSerializer.Deserialize<List<Kontakt>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(kontaktliste);
        }

        public async Task<IActionResult> Kontaktansicht(int id)
        {
            using var client = new HttpClient();
            var url = $"http://localhost:5027/api/Kontaktverwaltung/Kontakt/{id}";
            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var kontakt = JsonSerializer.Deserialize<Kontakt>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(kontakt);
        }


        public IActionResult NeuerKontakt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NeuerKontakt(Kontakt kontakt)
        {
            var json = JsonSerializer.Serialize(kontakt);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var url = "http://localhost:5027/api/Kontaktverwaltung/create";

            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var fehlertext = await response.Content.ReadAsStringAsync();
                _logger.LogError($"POST fehlgeschlagen: {response.StatusCode} – {fehlertext}");
                return StatusCode((int)response.StatusCode, fehlertext);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> KontaktEntfernen(int id)
        {
            using var client = new HttpClient();
            var url = $"http://localhost:5027/api/Kontaktverwaltung/{id}";

            var response = await client.DeleteAsync(url);


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KontaktUpdate(int id)
        {
            using var client = new HttpClient();
            var url = $"http://localhost:5027/api/Kontaktverwaltung/Kontakt/{id}";
            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var kontakt = JsonSerializer.Deserialize<Kontakt>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(kontakt);
        }



        [HttpPost]
        public async Task<IActionResult> KontaktUpdate(Kontakt kontakt)
        {
            var json = JsonSerializer.Serialize(kontakt);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var url = $"http://localhost:5027/api/Kontaktverwaltung/{kontakt.Id}";
            await client.PutAsync(url, content);
  

            return RedirectToAction("Index");
        }

        //!Überflüssig!
        //public async Task<string?> Geschlecht(string vorname)
        //{
        //    using var client = new HttpClient();
        //    var url = $"https://api.genderize.io?name={vorname}";
        //    var response = await client.GetAsync(url);


        //    var result = await response.Content.ReadAsStringAsync();

        //    using var json = JsonDocument.Parse(result);
        //    var gender = json.RootElement.GetProperty("gender").GetString();

        //    return gender switch
        //    {
        //        "male" => "männlich",
        //        "female" => "weiblich",
        //        _ => "unbekannt"
        //    };
        //}



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
