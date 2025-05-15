using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KontaktverwaltungLibrary.Models;

namespace KontaktverwaltungService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KontaktverwaltungController : ControllerBase
    {
        public readonly KontaktverwaltungContext _context;
        public readonly List<Kontakt> kontaktliste;

        
        public KontaktverwaltungController (KontaktverwaltungContext context)
        {
            _context = context;
            kontaktliste = _context.Kontakt.ToList();

        }

        [HttpGet("Kontaktliste")]
        public List<Kontakt> AlleKontakte()
        {
            return kontaktliste;
        }

  

        [HttpGet("Kontakt/{id}")]
        public IActionResult KontaktById(int id)
        {
            var kontakt = _context.Kontakt.Find(id);
            if (kontakt == null)
                return NotFound();

            return Ok(kontakt);
        }

        [HttpPost("create")]
        public IActionResult CreateKontakt([FromBody] Kontakt newKontakt)
        {
            _context.Kontakt.Add(newKontakt);
            _context.SaveChanges();
            return Created($"api/kontaktverwaltung/{newKontakt.Id}", newKontakt);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteKontakt(int id)
        {
            var kontakt = _context.Kontakt.Find(id);
            if (kontakt == null)
                return NotFound();

            _context.Kontakt.Remove(kontakt);
            _context.SaveChanges();

            return NoContent(); // Status 204
        }

        [HttpPut("{id}")]
        public IActionResult UpdateKontakt(int id, [FromBody] Kontakt updatedKontakt)
        {
            var existing = _context.Kontakt.Find(id);
            if (existing == null)
                return NotFound();

            existing.Vorname = updatedKontakt.Vorname;
            existing.Nachname = updatedKontakt.Nachname;
            existing.Adresse = updatedKontakt.Adresse;
            existing.Postleitzahl = updatedKontakt.Postleitzahl;
            existing.Ort = updatedKontakt.Ort;
            existing.Geschlecht = updatedKontakt.Geschlecht;
            existing.Telefon = updatedKontakt.Telefon;
            existing.Mail = updatedKontakt.Mail;
            existing.BildURL = updatedKontakt.BildURL;

            _context.SaveChanges();
            return NoContent();
        }



    }
}
