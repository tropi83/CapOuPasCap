using System;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class DefiRealisePostDto
    {
        [Required(ErrorMessage = "UtilisateurId est requis")]
        public Guid UtilisateurId { get; set; }

        [Required(ErrorMessage = "DefiId est requis")]
        public Guid DefiId { get; set; }

    }
}
