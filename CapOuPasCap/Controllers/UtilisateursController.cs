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
    public class UtilisateursController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public UtilisateursController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilisateurDto>>> GetUtilisateur()
        {
            return await _context.Utilisateur.Select(x => x.ToDto()).ToListAsync();
        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilisateurDto>> GetUtilisateur(Guid id)
        {
            var utilisateur = await _context.Utilisateur.FindAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur.ToDto();
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateur(Guid id, Utilisateur utilisateur)
        {
            if (id != utilisateur.Id)
            {
                return BadRequest();
            }

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Utilisateur/Inscription
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /**/
        [HttpPost("Inscription")]
        public async Task<ActionResult<UtilisateurDto>> PostUtilisateur(UtilisateurPostDto utilisateurPostDto)
        {

            var utilisateurPseudoExiste = await _context.Utilisateur.Where(x => x.Pseudo == utilisateurPostDto.Pseudo).FirstOrDefaultAsync();

            if (utilisateurPseudoExiste != null)
            {
                return BadRequest("Pseudo déjà utilisé");
            }

            Utilisateur user = new()
            {
                Pseudo = utilisateurPostDto.Pseudo,
                MotDePasse = utilisateurPostDto.MotDePasse,
            };

            _context.Utilisateur.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateur", new { id = user.Id }, user.ToDto());
        }

        // POST: api/Utilisateur/Connexion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /**/
        [HttpPost("Connexion")]
        public async Task<ActionResult<UtilisateurDto>> ConnectUtilisateur(UtilisateurAuthDto utilisateurAuthDto)
        {
            var utilisateur = await _context.Utilisateur.Where(x => x.Pseudo == utilisateurAuthDto.Pseudo).FirstOrDefaultAsync();

            if (utilisateur == null)
            {
                return BadRequest("Email ou mot de passe incorrect");
            }

            if(utilisateur.MotDePasse != utilisateurAuthDto.MotDePasse)
            {
                return BadRequest("Email ou mot de passe incorrect");
            }

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.Id }, utilisateur.ToDto());
          
        }

    }
}
