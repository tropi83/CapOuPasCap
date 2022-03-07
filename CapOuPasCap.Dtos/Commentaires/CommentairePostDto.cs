using System;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class CommentairePostDto
    {
        [Required]
        [MaxLength(140)]
        public string Texte { get; set; } = String.Empty;

        [Required]
        public Guid DefiId { get; set; }

        [Required]
        public Guid? UtilisateurId { get; set; }
    }
}
