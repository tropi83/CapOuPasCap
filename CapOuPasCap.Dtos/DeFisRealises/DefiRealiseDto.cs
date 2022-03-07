using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class DefiRealiseDto
    {
        public Guid Id { get; init; }

        [Required(ErrorMessage = "UtilisateurId est requis")]
        public Guid UtilisateurId { get; set; }

        [Required(ErrorMessage = "DefiId est requis")]
        public Guid DefiId { get; set; }
    }
}
