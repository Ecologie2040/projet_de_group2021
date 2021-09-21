using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortailIEPSM.Areas.Groupe_2.Models;

namespace PortailIEPSM.Areas.Groupe_2.ViewModels
{
    public class ArticleViewModel
    {
        public Article Article { get; set; }
        public List<Article> SuggestionArticles { get; set; }
        public List<Commentaire> Commentaires { get; set; }
        public Commentaire Commentaire { get; set; }
        public Admin Auteur { get; set; }
    }
}
