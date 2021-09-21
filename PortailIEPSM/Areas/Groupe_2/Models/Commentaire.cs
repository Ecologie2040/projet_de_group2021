using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortailIEPSM.Areas.Groupe_2.Models
{
    public class Commentaire
    {
        public int Id { get; set; }
        public int Id_article { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Reaction { get; set; }
    }
}
