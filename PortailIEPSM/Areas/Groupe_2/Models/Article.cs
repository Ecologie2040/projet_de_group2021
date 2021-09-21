using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortailIEPSM.Areas.Groupe_2.Models
{
    public class Article
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vous devez saisir un titre")]
        [MinLength(5, ErrorMessage = "Le titre est trop court !")]
        public string Titre { get; set; }
        [Required(ErrorMessage = "Vous devez saisir une description")]
        public string Description { get; set; }
        [Display(Name = "Importer une photo")]
        public IFormFile UploadImage { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Vous devez écrire un article")]
        [Display(Name = "Corps de l'article")]
        public string Corps { get; set; }
        public DateTime Date_creation { get; set; }
        public int Jaimes { get; set; }
        public string Categorie { get; set; }
        [Display(Name = "Catégorie")]
        public int CategorieID { get; set; }
        public int Auteur { get; set; }
        public int Visible { get; set; }
        public Visible VisibleEnum { get; set; }
        public DateTime Date_modification { get; set; }
    }
}
