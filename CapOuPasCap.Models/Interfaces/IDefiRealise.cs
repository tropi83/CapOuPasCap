using System;
using System.Collections.Generic;
using System.Text;
using CapOuPasCap.Models.Classes;

namespace CapOuPasCap.Models.Interfaces
{
    public interface IDefiRealise
    {
        int Id { get; }

        IUtilisateur Utilisateur { get; set; }

        IDefi Defi { get; set; }


    }
}
