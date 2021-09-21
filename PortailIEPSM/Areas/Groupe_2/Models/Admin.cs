using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortailIEPSM.Areas.Groupe_2.Models
{
    public class Admin
    {

        public Admin()
        {
            Confirmation = new Erreur();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Vous devez saisir un nom")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Vous devez saisir un prénom")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "Vous devez saisir une adresse e-mail")]
        [Display(Name = "Adresse e-mail")]
        public string Email { get; set; }
        [Display(Name = "Mot de passe")]
        public string Pass { get; set; }
        public Erreur Confirmation { get; set; }
    }
}
