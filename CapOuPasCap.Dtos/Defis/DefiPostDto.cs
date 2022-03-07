using System;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class DefiPostDto
    {
        [Required]
        [MaxLength(64)]
        public string Nom { get; set; } = String.Empty;

        [Required]
        [MaxLength(140)]
        public string Description { get; set; }

        [Required]
        public Guid? UtilisateurId { get; set; }
    }
}
