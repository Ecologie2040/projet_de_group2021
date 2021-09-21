using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PortailIEPSM.Areas.Groupe_2.Models;
using PortailIEPSM.Areas.Groupe_2.ViewModels;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PortailIEPSM.Areas.Groupe_2.Controllers
{
    [Area("Groupe_2")]
    [Authorize(AuthenticationSchemes = "groupe2_admin")]
    public class AdminController : Controller
    {
        private Data Db;

        private readonly IWebHostEnvironment Env;

        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            Db = new Data();
            Env = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreerArticle()
        {
           AdminViewModel model = new AdminViewModel();

            model.Categories = Db.ObtenirCategories();
           
            return View(model);
        }

        [HttpPost]
        public IActionResult CreerArticle(AdminViewModel model)
        {
            model.Categories = Db.ObtenirCategories();

            if (ModelState.IsValid)
            {
                DateTime date_creation = DateTime.Now;
                date_creation.ToString("dd/MM/YYYY");
                model.Article.Date_creation = date_creation;
                Upload upload = new Upload(Env);
                string imagepath = upload.UploadFile(model.Article.UploadImage);
                model.Article.Image = imagepath;
                if (model.Article.Image != null)
                {
                    if (Db.CategorieExiste(model.Article) == true || model.Article.CategorieID == 0)
                    {
                        model.Article.Auteur = Convert.ToInt32(User.FindFirst(claim => claim.Type == ClaimTypes.Authentication).Value);
                        Db.CreerArticle(model.Article);
                        model.Confirmation = new Erreur { Code = 2, Message = "L'article a été publié" };
                    }
                    else
                    {
                        model.Confirmation = new Erreur { Code = 1, Message = "La catégorie n'est pas valide" };
                    }
                } else
                {
                    model.Confirmation = new Erreur { Code = 1, Message = "L'image de l'article est manquante ou n'est pas valide (format, taille)" };
                }
            }
            return View(model);
        }

        public IActionResult ModifierArticle(AdminViewModel model, int id)
        {
            Article article = Db.ObtenirArticle(id);
            if(article == null)
            {
                return RedirectToAction("GestionArticles", "Admin");
            }
            article.VisibleEnum = (Visible)article.Visible;
            model.Article = article;
            model.Categories = Db.ObtenirCategories();
            return View(model);
        }

        [HttpPost]
        public IActionResult ModifierArticle(AdminViewModel model)
        {
            model.Categories = Db.ObtenirCategories();
            if (ModelState.IsValid)
            {
                DateTime date_modification = DateTime.Now;
                date_modification.ToString("dd/MM/YYYY");
                model.Article.Date_modification = date_modification;
                if(model.Article.UploadImage == null)
                {
                    model.Article.Image = Db.ObtenirArticle(model.Article.Id).Image;
                } else
                {
                    Upload upload = new Upload(Env);
                    model.Article.Image = upload.UploadFile(model.Article.UploadImage);
                }
                if (model.Article.Image != null)
                {
                    if (Db.CategorieExiste(model.Article) == true)
                    {
                        Db.ModifierArticle(model.Article);
                        model.Confirmation = new Erreur { Code = 2, Message = "L'article a été modifié" };
                    }
                    else
                    {
                        model.Confirmation = new Erreur { Code = 1, Message = "La catégorie n'est pas valide" };
                    }
                } else
                {
                        model.Confirmation = new Erreur { Code = 1, Message = "L'image de l'article est manquante ou n'est pas valide (format, taille)" };
                }
            }
            return View(model);
        }

        public IActionResult GestionArticles()
        {
            List<Article> articles = Db.ObtenirTousLesArticles(true);
            return View(articles);
        }

        [HttpGet]
        public JsonResult SupprimerArticle(int id)
        {
            Article article = Db.ObtenirArticle(id);
            int result = Db.SupprimerArticle(id);
            if(result > 0)
            {
                string filePath = article.Image;
                Upload upload = new Upload(Env);
                return Json(new { type = "success", message = "L'article a été supprimé" });
            } else
            {
                return Json(new { type = "error", message = "Une erreur est survenue" });
            }
           
        }

        public IActionResult GestionCategories()
        {
            List<Categorie> categories = Db.ObtenirCategories();
            return View(categories);
        }

        public IActionResult CreerCategorie()
        {
            Categorie model = new Categorie();
            
            model.VisibleEnum = (Visible)1;
            return View(model);
        }

        [HttpPost]
        public IActionResult CreerCategorie(Categorie model)
        {
            if (ModelState.IsValid)
            {
                if (Db.NomCategorieValide(model) == false)
                {
                    Upload uploadsvg = new Upload(Env);
                    model.Image = uploadsvg.UploadSVG(model.UploadImage);
                    if (model.Image != null)
                    {
                        Db.CreerCategorie(model);
                        model.Confirmation = new Erreur { Code = 2, Message = "La catégorie a été créée !" };
                    } else
                    {
                        model.Confirmation = new Erreur { Code = 1, Message = "L'image de l'article est manquante ou n'est pas valide (format, taille)" };
                    }
                } else
                {
                    model.Confirmation = new Erreur { Code = 1, Message = $"Une catégorie possède déjà le nom : {model.Nom}" };
                }
            }
            return View(model);
        }

        public IActionResult ModifierCategorie(int id)
        {
            Categorie categorie = Db.ObtenirCategorie(id);
            if (categorie == null)
            {
                return RedirectToAction("GestionCategories", "Admin");
            }
            categorie.VisibleEnum = (Visible)categorie.Visible;
            return View(categorie);
        }

        [HttpPost]
        public IActionResult ModifierCategorie(Categorie model)
        {
            if(ModelState.IsValid)
            {
                Categorie categorie = Db.ObtenirCategorie(model.Id);
                if (categorie.Nom == model.Nom || Db.NomCategorieValide(model) == false)
                {
                    if (model.UploadImage == null)
                    {
                        model.Image = Db.ObtenirCategorie(model.Id).Image;
                    }
                    else
                    {
                        Upload uploadsvg = new Upload(Env);
                        model.Image = uploadsvg.UploadSVG(model.UploadImage);
                    }
                        if (model.Image != null)
                        {
                            Db.ModifierCategorie(model);
                            model.Confirmation = new Erreur { Code = 2, Message = "La catégorie a été modifée !" };
                        }
                        else
                        {
                            model.Confirmation = new Erreur { Code = 1, Message = "L'image de l'article est manquante ou n'est pas valide (format, taille)" };
                        }
                    
                }
                else
                {
                    model.Confirmation = new Erreur { Code = 1, Message = $"Une catégorie possède déjà le nom : {model.Nom}" };
                }
                }
            return View(model);
        }

        [HttpGet]
        public JsonResult SupprimerCategorie(int id)
        {
            int result = Db.SupprimerCategorie(id);
            if(result > 0)
            {
                return Json(new { type = "success", message = "La catégorie a été supprimée" });
            } else
            {
                return Json(new { type = "error", message = "Une erreur est survenue" });
            }
           
        }

        public IActionResult GestionCommentaires(int id)
        {
            List<Commentaire> commentaires = Db.AfficherTousLesCommentaires().FindAll(comm => comm.Id_article == id);
            return View(commentaires);
        }

        [HttpGet]
        public JsonResult SupprimerCommentaire(int id)
        {
            int result = Db.SupprimerCommentaire(id);
            if(result > 0)
            {
                return Json(new { type = "success", message = "Le commentaire a été supprimé" });
            } else
            {
                return Json(new { type = "error", message = "Une erreur est survenue" });
            }
            
        }

        public IActionResult CreerAdmin()
        {
            Admin model = new Admin();
            return View(model);
        }

        [HttpPost]
        public IActionResult CreerAdmin(Admin model, string motdepasse)
        {
            if(ModelState.IsValid)
            {
                if (motdepasse != null)
                {
                    Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z]).{8,15}$");
                    Match verif = regex.Match(motdepasse);
                    if (verif.Success)
                    {
                        model.Pass = BCrypt.Net.BCrypt.HashPassword(motdepasse);
                        Db.CreerAdmin(model);
                        model.Confirmation = new Erreur { Code = 2, Message = "Le compte administrateur a été créé" };
                    }
                    else
                    {
                        model.Confirmation = new Erreur { Code = 1, Message = "Votre mot de passe doit contenir minimum 8 caractères avec des majuscules et minuscules" };
                    }
                } else
                {
                    model.Confirmation = new Erreur { Code = 1, Message = "Vous devez saisir un mot de passe" };
                }
            }

            return View(model);
        }

        public IActionResult GestionAdmin()
        {

            List<Admin> administrateurs = Db.ObtenirAdministrateurs();
  
            return View(administrateurs);
        }

        public IActionResult ModifierAdmin(int id)
        {
            Admin administrateur = Db.ObtenirAdministrateurs().Find(a => a.Id == id);
            if(administrateur == null)
            {
                return RedirectToAction("GestionAdmin", "Admin");
            }
            return View(administrateur);
        }

        [HttpPost]
        public IActionResult ModifierAdmin(Admin model, string newpassword, string repeat)
        {
            if (ModelState.IsValid)
            {
                bool mdpOK = true;
                if (newpassword == null && repeat == null)
                {
                    model.Pass = Db.ObtenirAdministrateurs().Find(a => a.Id == model.Id).Pass;

                } else
                {
                    if (newpassword != repeat)
                    {
                        model.Confirmation = new Erreur { Code = 1, Message = "Les deux mots de passe ne correspondent pas" };
                        mdpOK = false;
                    } else
                    {
                        Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z]).{8,15}$");
                        Match verif = regex.Match(newpassword);
                        if(verif.Success)
                        {
                            model.Pass = BCrypt.Net.BCrypt.HashPassword(newpassword);
                        } else
                        {
                            mdpOK = false;
                            model.Confirmation = new Erreur { Code = 1, Message = "Votre mot de passe doit contenir minimum 8 caractères avec des majuscules et minuscules" };
                        }
                       
                    }
                }

                if(mdpOK == true)
                {
                    if(Db.ModifierAdmin(model) > 0)
                    {
                        model.Confirmation = new Erreur { Code = 2, Message = "Le compte administrateur a été modifié" };
                    } else {

                        model.Confirmation = new Erreur { Code = 1, Message = "Une erreur est survenue" };
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult SupprimerAdmin(int id)
        {
            int result = Db.SupprimerAdmin(id);
            if (result > 0)
            {
                return Json(new { type = "success", message = "Le compte administrateur a été supprimé" });
            }
            else
            {
                return Json(new { type = "error", message = "Une erreur est survenue" });
            }

        }
    }

    
}
