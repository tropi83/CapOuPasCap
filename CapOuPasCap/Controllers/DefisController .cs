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
    public class DefisController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public DefisController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Defis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DefiDto>>> GetDefi()
        {

            var defis = await _context.Defi.Include(p => p.Commentaires).Include(p => p.Createur).Select(x => x.ToDto()).ToListAsync();

            foreach (var defi in defis)
            {
                var defisLikes = await _context.Like.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();
                if (defisLikes != null)
                {
                    defi.NbLike = defisLikes.Count();
                }
            }

            return defis;

        }

        // GET: api/Defis/Utilisateur/{utilisateurId}
        [HttpGet("Utilisateur/{utilisateurId}")]
        public async Task<ActionResult<IEnumerable<DefiDto>>> GetDefiByUser(Guid utilisateurId)
        {
            var utilisateurTrouve = await _context.Utilisateur.FindAsync(utilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var defis =  await _context.Defi.Include(p => p.Commentaires).Include(p => p.Createur).Select(x => x.ToDto()).ToListAsync();

            foreach ( var defi in defis)
            {
                var defisRealiseParUtilisateur = await _context.DefiRealise.Where(p => p.UtilisateurId == utilisateurId).Where(p => p.DefiId == defi.Id).FirstOrDefaultAsync();
                var defisRealises = await _context.DefiRealise.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();

                var defisLikeParUtilisateur = await _context.Like.Where(p => p.UtilisateurId == utilisateurId).Where(p => p.DefiId == defi.Id).FirstOrDefaultAsync();
                var defisLikes = await _context.Like.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();

                if (defisRealises != null)
                {
                    if(defisRealiseParUtilisateur != null)
                    {
                        defi.Realise = true;
                        defi.RealiseId = defisRealiseParUtilisateur.Id;
                    }
                    else
                    {
                        defi.Realise = false;
                    }

                    defi.NbRealise = defisRealises.Count();
                }

                if (defisLikes != null)
                {
                    if (defisLikeParUtilisateur != null)
                    {
                        defi.Like = true;
                        defi.LikeId = defisLikeParUtilisateur.Id;
                    }
                    else
                    {
                        defi.Like = false;
                    }

                    defi.NbLike = defisLikes.Count();
                }

            }

            return defis;

        }

        // GET: api/Defis/Utilisateur/{utilisateurId}/Like/desc
        [HttpGet("Utilisateur/{utilisateurId}/Like/{ordre}")]
        public async Task<ActionResult<IEnumerable<DefiDto>>> GetDefiFiltreLike(Guid utilisateurId, string ordre)
        {

            var utilisateurTrouve = await _context.Utilisateur.FindAsync(utilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var defis = await _context.Defi.Include(p => p.Commentaires).Include(p => p.Createur).Select(x => x.ToDto()).ToListAsync();


            if(defis.Count() != 0)
            {
                foreach (var defi in defis)
                {
                    var defisRealiseParUtilisateur = await _context.DefiRealise.Where(p => p.UtilisateurId == utilisateurId).Where(p => p.DefiId == defi.Id).FirstOrDefaultAsync();
                    var defisRealises = await _context.DefiRealise.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();

                    var defisLikeParUtilisateur = await _context.Like.Where(p => p.UtilisateurId == utilisateurId).Where(p => p.DefiId == defi.Id).FirstOrDefaultAsync();
                    var defisLikes = await _context.Like.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();

                    if (defisRealises != null)
                    {
                        if (defisRealiseParUtilisateur != null)
                        {
                            defi.Realise = true;
                            defi.RealiseId = defisRealiseParUtilisateur.Id;
                        }
                        else
                        {
                            defi.Realise = false;
                        }

                        defi.NbRealise = defisRealises.Count();
                    }

                    if (defisLikes != null)
                    {
                        if (defisLikeParUtilisateur != null)
                        {
                            defi.Like = true;
                            defi.LikeId = defisLikeParUtilisateur.Id;
                        }
                        else
                        {
                            defi.Like = false;
                        }

                        defi.NbLike = defisLikes.Count();
                    }
                }

                if (ordre == "desc")
                {
                    defis.Sort((p1, p2) =>
                    {
                        return p2.NbLike - p1.NbLike;
                    });
                }
                else
                {
                    defis.Sort((p1, p2) =>
                    {
                        return p1.NbLike - p2.NbLike;
                    });
                }

            }

            return defis;

        }

        // GET: api/Defis/Utilisateur/{utilisateurId}/DateCreation/desc
        [HttpGet("Utilisateur/{utilisateurId}/DateCreation/{ordre}")]
        public async Task<ActionResult<IEnumerable<DefiDto>>> GetDefiFiltreCreation(Guid utilisateurId, string ordre)
        {

            var utilisateurTrouve = await _context.Utilisateur.FindAsync(utilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var defis = await _context.Defi.Include(p => p.Commentaires).Include(p => p.Createur).OrderBy(x => x.DateDeCreation).Select(x => x.ToDto()).ToListAsync();

            if (ordre == "desc")
            {
                defis = await _context.Defi.Include(p => p.Commentaires).Include(p => p.Createur).OrderByDescending(x => x.DateDeCreation).Select(x => x.ToDto()).ToListAsync();
            }

            if (defis.Count() != 0)
            {
                foreach (var defi in defis)
                {
                    var defisRealiseParUtilisateur = await _context.DefiRealise.Where(p => p.UtilisateurId == utilisateurId).Where(p => p.DefiId == defi.Id).FirstOrDefaultAsync();
                    var defisRealises = await _context.DefiRealise.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();

                    var defisLikeParUtilisateur = await _context.Like.Where(p => p.UtilisateurId == utilisateurId).Where(p => p.DefiId == defi.Id).FirstOrDefaultAsync();
                    var defisLikes = await _context.Like.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();

                    if (defisRealises != null)
                    {
                        if (defisRealiseParUtilisateur != null)
                        {
                            defi.Realise = true;
                            defi.RealiseId = defisRealiseParUtilisateur.Id;
                        }
                        else
                        {
                            defi.Realise = false;
                        }

                        defi.NbRealise = defisRealises.Count();
                    }

                    if (defisLikes != null)
                    {
                        if (defisLikeParUtilisateur != null)
                        {
                            defi.Like = true;
                            defi.LikeId = defisLikeParUtilisateur.Id;
                        }
                        else
                        {
                            defi.Like = false;
                        }

                        defi.NbLike = defisLikes.Count();
                    }
                }
            }

            return defis;

        }

        // GET: api/Defis/2
        [HttpGet("{id}")]
        public async Task<ActionResult<DefiDto>> GetDefi(Guid id)
        {
            var defi = await _context.Defi.Where(p => p.Id == id).Include(p => p.Commentaires).Include(p => p.Createur).FirstOrDefaultAsync();

            if (defi == null)
            {
                return NotFound();
            }

            return defi.ToDto();
        }


        // POST: api/Defis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DefiDto>> PostDefi(DefiPostDto defiPostDto)
        {

            var utilisateurTrouve = await _context.Utilisateur.FindAsync(defiPostDto.UtilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            Defi defi = new()
            {
                Nom = defiPostDto.Nom,
                Description = defiPostDto.Description,
                Createur = utilisateurTrouve
            };

            _context.Defi.Add(defi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDefi", new { id = defi.Id }, defi.ToDto());
        }

        // DELETE: api/Defis/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDefi(Guid id)
        {
            var defi = await _context.Defi.Where(p => p.Id == id).Include(p => p.Commentaires).FirstOrDefaultAsync();
            var defisLikes = await _context.Like.Where(p => p.DefiId == defi.Id).Select(x => x.ToDto()).ToListAsync();

            if (defi == null)
            {
                return NotFound();
            }

            if (defisLikes.Count() != 0)
            {
                return BadRequest("Le défi ne peut pas être supprimé car il contient des likes");
            }

            if(defi.Commentaires.Count() > 0)
            {
                return BadRequest("Le défi ne peut pas être supprimé car il contient des commentaires");
            }

            _context.Defi.Remove(defi);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // PUT: api/Defis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/Utilisateur/{utilisateurId}")]
        public async Task<IActionResult> PutDefi(Guid id, Guid utilisateurId, DefiPutDto defiPutDto)
        {

            var utilisateurTrouve = await _context.Utilisateur.FindAsync(utilisateurId);
            if (utilisateurTrouve == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var defi = await _context.Defi.Where(p => p.Id == id).Include(p => p.Commentaires).Include(p => p.Createur).FirstOrDefaultAsync();

            if (defi == null)
            {
                return NotFound();
            }

            if (defi.Createur.Id != utilisateurTrouve.Id)
            {
                return BadRequest("Utilisateur non autorisé");
            }

            _context.Entry(defi).State = EntityState.Modified;

            defi.Cache = defiPutDto.Cache;
            await _context.SaveChangesAsync();
      

            return NoContent();
        }


    }
}
