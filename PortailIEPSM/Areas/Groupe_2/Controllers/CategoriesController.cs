using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortailIEPSM.Areas.Groupe_2.Models;
using PortailIEPSM.Areas.Groupe_2.ViewModels;

namespace PortailIEPSM.Areas.Groupe_2.Controllers
{
    [Area("Groupe_2")]
    public class CategoriesController : Controller
    {
        private Data Db;
        public CategoriesController()
        {
            Db = new Data();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Accueil");
        }
        public IActionResult Tout()
        {
            ViewData["title"] = "Les catégories";
            List<Categorie> categories = Db.ObtenirCategories().FindAll(c => c.Visible == 1);
            return View(categories);
        }

        public IActionResult Afficher(AccueilViewModel model, int id)
        {
            Categorie categorie = Db.ObtenirCategorie(id);
            if (categorie != null && categorie.Visible == 1) {
              ViewData["title"] = categorie.Nom;
              List<Article> articles = Db.ObtenirTousLesArticles(false);
            if (articles != null)
                {
                    model.TousLesArticles = articles.FindAll(article => article.CategorieID == id);
                    if (model.TousLesArticles.Count > 0)
                    {
                        model.PremierArticle = model.TousLesArticles.First();
                        model.TousLesArticles.RemoveAt(0);
                    }
        
                }
                ViewData["titre"] = $"Catégorie : {categorie.Nom}";
                return View("../Accueil/Index", model);
            } else
            {
                return RedirectToAction("Index", "Accueil");
            }
        }
    }
}
