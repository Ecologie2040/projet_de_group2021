using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortailIEPSM.Areas.Groupe_2.Models;
using PortailIEPSM.Areas.Groupe_2.ViewModels;

namespace PortailIEPSM.Areas.Groupe_2.Controllers
{
    [Area("Groupe_2")]
    public class AccueilController : Controller
    {
        private Data Db;
        public AccueilController()
        {
            Db = new Data();
        }
       
        public IActionResult Index(AccueilViewModel model)
        {
            ViewData["titre"] = "Articles à la une";
            List<Article> articles = Db.ObtenirTousLesArticles(false);
            if (articles != null)
            {
                articles.RemoveAt(0);
                model.TousLesArticles = articles;
                model.PremierArticle = Db.ObtenirTousLesArticles(false).First();
                return View(model);
            }
            return View("AucunArticle");
        }

        public IActionResult Article(ArticleViewModel model, int id)
        {
            model.Article = Db.ObtenirArticle(id);
            if(model.Article == null || model.Article.Categorie == String.Empty || model.Article.Visible == 0)
            {
                return RedirectToAction("Index");
            }
            ViewData["title"] = model.Article.Titre;
            model.SuggestionArticles = Db.ObtenirTousLesArticles(false).FindAll(article => (article.CategorieID == model.Article.CategorieID) && (article.Id != model.Article.Id));
            model.Commentaires = Db.AfficherTousLesCommentaires().FindAll(commentaire => commentaire.Id_article == model.Article.Id);
            model.Auteur = Db.ObtenirAdministrateurs().Find(a => a.Id == model.Article.Auteur);
            return View(model);
        }

        [HttpPost]
        public IActionResult Article(ArticleViewModel model)
        {
            model.Commentaire.Id_article = model.Article.Id;
            if(!String.IsNullOrEmpty(model.Commentaire.Nom) && !String.IsNullOrEmpty(model.Commentaire.Prenom)
                && !String.IsNullOrEmpty(model.Commentaire.Reaction)) 
            {
                int result = Db.PublierCommentaire(model.Commentaire);
                if(result > 0)
                {
                    return Json(new { type = "success", message = "Commentaire publié" });
                } else
                {
                    return Json(new { type = "error", message = "Une erreur est survenue" });
                }
               
            }

            return Json(new { type = "error", message = "Tous les champs ne sont pas remplis !" });

        }

        [HttpGet]
        public IActionResult AimerArticle(int id)
        {
            int result = Db.AimerArticle(id);
            if(result > 0)
            {
                return Json(new { type = "success", message = "Vous aimez cet article" });
            } else
            {
                return Json(new { type = "error", message = "Une erreur est survenue" });
            }
           
        }

        [HttpGet]
        public IActionResult NePlusAimerArticle(int id)
        {
            int result = Db.NePlusAimerArticle(id);
            if(result > 0)
            {
                return Json(new { type = "success", message = "Vous n'aimez plus cet article" });
            } else
            {
                return Json(new { type = "error", message = "Une erreur est survenue" });
            }
           
        }

        public IActionResult Recherche()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Recherche(string recherche)
        {
            List<Article> touslesarticles = Db.ObtenirTousLesArticles(false);
                if (touslesarticles != null) {
                var articles_recherche = touslesarticles.FindAll(article => article.Titre.ToLower().Contains(recherche.ToLower()));
                var articles = articles_recherche.Select(a => new { a.Id, a.Titre, a.Image, a.Categorie, a.CategorieID, date_creation = a.Date_creation.ToString("dd/MM/yyyy") });
                return Json(articles);
            }
            return Json(new { type = 0, message = "Aucun article n'a été trouvé" });
            }
    }
}
