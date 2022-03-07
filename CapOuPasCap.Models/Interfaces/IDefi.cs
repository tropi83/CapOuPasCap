using System;
using System.Collections.Generic;
using System.Text;
using CapOuPasCap.Models.Classes;

namespace CapOuPasCap.Models.Interfaces
{
    public interface IDefi
    {
        int Id { get; }

        string Nom { get; set; }

        string Texte { get; set; }

        List<ICommentaire> Commentaires { get; set; }

        IUtilisateur Createur { get; set; }

        DateTime DateDeCreation { get; set; }
    }
}
