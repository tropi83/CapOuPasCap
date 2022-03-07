using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class DefiDto
    {
        public Guid Id { get; init; }

        [Required]
        [MaxLength(64)]
        public string Nom { get; set; } = String.Empty;

        [Required]
        [MaxLength(140)]
        public string Description { get; set; }

        public List<CommentaireDto> Commentaires { get; set; }

        [Required]
        public UtilisateurDto Createur { get; set; }

        public DateTime DateDeCreation { get; set; }

        public Boolean ?Realise { get; set; } = null;

        public Guid? RealiseId { get; set; } = null;

        public int NbRealise { get; set; }

        public Boolean ?Like { get; set; } = null;

        public Guid ?LikeId { get; set; } = null;

        public int NbLike { get; set; }

        public Boolean? Cache { get; set; } = null;

    }
}
