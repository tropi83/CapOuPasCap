using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class LikeDto
    {
        public Guid Id { get; init; }

        public Guid UtilisateurId { get; set; }

        public Guid DefiId { get; set; }
    }
}
