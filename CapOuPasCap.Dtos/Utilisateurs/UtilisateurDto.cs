using System;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class UtilisateurDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(240)]
        public string Pseudo { get; set; } = String.Empty;

        public string ?MotDePasse { get; set; } = null;
    }
}
