using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapOuPasCap.Dtos;
using CapOuPasCap.Models.Classes;
using CapOuPasCap.Models.DataAccess;

namespace CapOuPasCap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefisRealisesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public DefisRealisesController(DataBaseContext context)
        {
            _context = context;
        }


        // GET: api/DefisRealises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DefiRealiseDto>>> GetDefiRealises()
        {
            return await _context.DefiRealise.Select(x => x.ToDto()).ToListAsync();
        }


        // GET: api/DefisRealises/2
        [HttpGet("{id}")]
        public async Task<ActionResult<DefiRealiseDto>> GetDefiRealise(Guid id)
        {
            var defiRealise = await _context.DefiRealise.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (defiRealise == null)
            {
                return NotFound();
            }

            return defiRealise.ToDto();
        }



        // POST: api/DefisRealises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DefiRealiseDto>> PostDefiRealise(DefiRealisePostDto defiRealisePostDto)
        {
            var utilisateurTrouve = await _context.Utilisateur.FindAsync(defiRealisePostDto.UtilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var defiTrouve = await _context.Defi.FindAsync(defiRealisePostDto.DefiId);
            if (defiTrouve == null)
            {
                return NotFound("Defi non trouvé");
            }

            var defisRealisesParUtilisateurTrouve = await _context.DefiRealise.Where(p => p.UtilisateurId == utilisateurTrouve.Id).Where(p => p.DefiId == defiTrouve.Id).FirstOrDefaultAsync();
            if (defisRealisesParUtilisateurTrouve != null)
            {
                return BadRequest("Défi déjà réalisé");
            }

            DefiRealise defiRealise = new()
            {
                UtilisateurId = defiRealisePostDto.UtilisateurId,
                DefiId = defiRealisePostDto.DefiId,
            };

            _context.DefiRealise.Add(defiRealise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDefiRealise", new { id = defiRealise.Id }, defiRealise.ToDto());
        }

        // DELETE: api/DefisRealises/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDefiRealise(Guid id)
        {
            var defiRealise = await _context.DefiRealise.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (defiRealise == null)
            {
                return NotFound();
            }

            _context.DefiRealise.Remove(defiRealise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DefiExists(Guid id)
        {
            return _context.Defi.Any(e => e.Id == id);
        }
    }
}
