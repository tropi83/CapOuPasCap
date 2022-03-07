using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class CommentaireDto
    {
        public Guid Id { get; init; }

        [MaxLength(140)]
        [Required]
        public string Texte { get; set; } = String.Empty;

        public DateTime DateDeCreation { get; set; }

        [Required]
        public Guid DefiId { get; set; }

        [Required]
        public UtilisateurDto Createur { get; set; }
    }
}
