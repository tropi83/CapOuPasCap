using System;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class UtilisateurPostDto
    {
        [Required]
        [MaxLength(240)]
        public string Pseudo { get; set; } = String.Empty;

        [Required]
        [MaxLength(240)]
        public string MotDePasse { get; set; } = String.Empty;


    }
}
