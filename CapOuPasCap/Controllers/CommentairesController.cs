using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapOuPasCap.Models.Classes;
using CapOuPasCap.Models.DataAccess;
using CapOuPasCap.Dtos;

namespace CapOuPasCap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentairesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public CommentairesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Commentaires
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentaireDto>>> GetCommentaire()
        {
            return await _context.Commentaire.Include(p => p.Createur).Select(x => x.ToDto()).ToListAsync();
        }

        // GET: api/Commentaires/Defi/2
        [HttpGet("Defi/{id}")]
        public async Task<ActionResult<IEnumerable<CommentaireDto>>> GetCommantaireByDefi(Guid id)
        {
            return await _context.Commentaire.Include(p => p.Createur).Where(x => x.DefiId == id).Select(x => x.ToDto()).ToListAsync();
        }

        // GET: api/Commentaires/2
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentaireDto>> GetCommentaire(Guid id)
        {
            var commentaire = await _context.Commentaire.Include(p => p.Createur).Where(e => e.Id == id).FirstOrDefaultAsync();

            if (commentaire == null)
            {
                return NotFound();
            }

            return commentaire.ToDto();
        }


        // POST: api/Commentaires
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommentaireDto>> PostCommentaire(CommentairePostDto commentairePostDto)
        {
            var utilisateurTrouve = await _context.Utilisateur.FindAsync(commentairePostDto.UtilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var defiFind = await _context.Defi.FindAsync(commentairePostDto.DefiId);
            if (defiFind == null)
            {
                return NotFound("Defi non trouvé");
            }

            Commentaire commentaire = new()
            {
                Texte = commentairePostDto.Texte,
                DefiId = defiFind.Id,
                Createur = utilisateurTrouve,
            };

            _context.Commentaire.Add(commentaire);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommentaire", new { id = commentaire.Id }, commentaire.ToDto());
        }
    }

}
