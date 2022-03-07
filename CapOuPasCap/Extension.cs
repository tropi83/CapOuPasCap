using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapOuPasCap.Dtos;
using CapOuPasCap.Models.Classes;

namespace CapOuPasCap
{
    public static class Extension
    {

        public static UtilisateurDto ToDto(this Utilisateur pUtilisateur)
        {
            return new UtilisateurDto
            {
                Id = pUtilisateur.Id,
                Pseudo = pUtilisateur.Pseudo
            };
        }

        public static DefiDto ToDto(this Defi pDefi)
        {
            return new DefiDto
            {
                Id = pDefi.Id,
                Nom = pDefi.Nom,
                Description = pDefi.Description,
                Createur = pDefi.Createur.ToDto(),
                DateDeCreation = (DateTime)pDefi.DateDeCreation,
                Commentaires = pDefi.Commentaires.Select(x => x.ToDto()).ToList(),
                Like = false,
                NbLike = 0,
                Realise = false,
                NbRealise = 0,
                Cache = pDefi.Cache,
            };
        }

        public static CommentaireDto ToDto(this Commentaire pCommentaire)
        {
            return new CommentaireDto
            {
                Id = pCommentaire.Id,
                Texte = pCommentaire.Texte,
                DateDeCreation = pCommentaire.DateDeCreation,
                DefiId = (Guid)pCommentaire.DefiId,
                Createur = pCommentaire.Createur.ToDto()
            };
        }

        public static DefiRealiseDto ToDto(this DefiRealise pDefiRealiseDto)
        {
            return new DefiRealiseDto
            {
                Id = pDefiRealiseDto.Id,
                UtilisateurId = (Guid)pDefiRealiseDto.UtilisateurId,
                DefiId = (Guid)pDefiRealiseDto.DefiId
            };
        }

        public static LikeDto ToDto(this Like pLikeDto)
        {
            return new LikeDto
            {
                Id = pLikeDto.Id,
                UtilisateurId = (Guid)pLikeDto.UtilisateurId,
                DefiId = (Guid)pLikeDto.DefiId
            };
        }



    }
}
