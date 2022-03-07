using System;
using System.Collections.Generic;
using System.Text;

namespace CapOuPasCap.Models.Interfaces
{
    public interface ICommentaire
    {
        int Id { get; }

        string Texte { get; set; }

        IDefi Projet { get; set; }

        IUtilisateur Createur { get; set; }
    }
}
