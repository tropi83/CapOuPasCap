using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CapOuPasCap.Models.Interfaces;

namespace CapOuPasCap.Models.Classes
{
    public class Commentaire
    {
        public Guid Id { get; init; }

        [Required]
        [MaxLength(140)]
        public string Texte { get; set; } = String.Empty;

        public DateTime DateDeCreation { get; set; } = DateTime.Now;

        [Required]
        public Guid DefiId { get; set; }

        [Required]
        public Utilisateur Createur { get; set; } = new();
    }
}
