using PortailIEPSM.Areas.Groupe_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortailIEPSM.Areas.Groupe_2.ViewModels
{
    public class AccueilViewModel
    {
        public List<Article> TousLesArticles { get; set; }
        public Article PremierArticle { get; set; }

}
}
