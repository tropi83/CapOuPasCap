using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CapOuPasCap.Models.Interfaces;

namespace CapOuPasCap.Models.Classes
{
    public class Utilisateur
    {
        public Guid Id { get; init; }
        [Required]
        [MaxLength(100)]
        public string Pseudo { get; set; } = String.Empty;
        [Required]
        [MaxLength(100)]
        public string MotDePasse { get; set; } = String.Empty;
    }
}
