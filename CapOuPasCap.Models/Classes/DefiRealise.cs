using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CapOuPasCap.Models.Interfaces;

namespace CapOuPasCap.Models.Classes
{
    public class DefiRealise
    {
        public Guid Id { get; init; }

        [Required]
        public Guid? UtilisateurId { get; set; }

        [Required]
        public Guid? DefiId { get; set; }
    }
}
