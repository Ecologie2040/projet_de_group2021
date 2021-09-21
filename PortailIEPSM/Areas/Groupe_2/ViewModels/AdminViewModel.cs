using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PortailIEPSM.Areas.Groupe_2.Models;

namespace PortailIEPSM.Areas.Groupe_2.ViewModels
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            Confirmation = new Erreur();
        }
        public Article Article { get; set; }
        public List<Categorie> Categories { get; set; }
        public Erreur Confirmation { get; set; }
        
    }
}
