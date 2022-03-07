using System;
using System.ComponentModel.DataAnnotations;

namespace CapOuPasCap.Dtos
{
    public class DefiPutDto
    {
        [Required]
        public Boolean Cache { get; set; } = false;

    }
}
