using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CapOuPasCap.Models.Interfaces;

namespace CapOuPasCap.Models.Classes
{
    public class Defi
    {
        public Guid Id { get; init; }

        [Required]
        [MaxLength(64)]
        public string Nom { get; set; } = String.Empty;

        [Required]
        [MaxLength(140)]
        public string Description { get; set; } = String.Empty;

        public DateTime DateDeCreation { get; set; } = DateTime.Now;

        public List<Commentaire> Commentaires { get; set; } = new();

        [Required]
        public Utilisateur Createur { get; set; } = new();

        public Boolean ?Cache { get; set; } = false;


    }
}
