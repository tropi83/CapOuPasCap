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
    public class LikesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public LikesController(DataBaseContext context)
        {
            _context = context;
        }


        // GET: api/Likes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikes()
        {
            return await _context.Like.Select(x => x.ToDto()).ToListAsync();
        }


        // GET: api/Likes/2
        [HttpGet("{id}")]
        public async Task<ActionResult<LikeDto>> GetLike(Guid id)
        {
            var like = await _context.Like.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (like == null)
            {
                return NotFound();
            }

            return like.ToDto();
        }



        // POST: api/Likes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LikeDto>> PostLike(LikePostDto likePostDto)
        {
            var utilisateurTrouve = await _context.Utilisateur.FindAsync(likePostDto.UtilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var defiTrouve = await _context.Defi.FindAsync(likePostDto.DefiId);
            if (defiTrouve == null)
            {
                return NotFound("Defi non trouvé");
            }

            var likeParUtilisateurTrouve = await _context.Like.Where(p => p.UtilisateurId == utilisateurTrouve.Id).Where(p => p.DefiId == defiTrouve.Id).FirstOrDefaultAsync();
            if (likeParUtilisateurTrouve != null)
            {
                return BadRequest("Défi déjà like");
            }

            Like like = new()
            {
                UtilisateurId = likePostDto.UtilisateurId,
                DefiId = likePostDto.DefiId,
            };

            _context.Like.Add(like);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", new { id = like.Id }, like.ToDto());
        }

        // DELETE: api/Likes/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            var like = await _context.Like.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (like == null)
            {
                return NotFound();
            }

            _context.Like.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
