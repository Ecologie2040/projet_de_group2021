using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PortailIEPSM.Areas.Groupe_2.Models;

namespace PortailIEPSM.Areas.Groupe_2.Models
{
    public class Categorie
    {
        public Categorie()
        {
            Confirmation = new Erreur();
        }
       public int Id { get; set; }
       [Required(ErrorMessage = "Le champ nom est obligatoire")]
       public string Nom { get; set; }
       [Required(ErrorMessage = "Le champ description est obligatoire")]
       public string Description { get; set; }
       public string Image { get; set; }
        [Display(Name = "Importer une image (SVG)")]
       public IFormFile UploadImage { get; set; }
       public int Visible { get; set; }
       public Erreur Confirmation { get; set; }
        [Range(0, 1, ErrorMessage = "Le champ visibilité n'est pas valide")]
       public Visible VisibleEnum { get; set; }
    }
}
