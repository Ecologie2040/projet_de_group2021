using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using PortailIEPSM.Areas.Groupe_2.Models;

namespace PortailIEPSM.Areas.Groupe_2.Controllers
{
    [Area("Groupe_2")]
    public class ConnexionController : Controller
    {
        private Data Db;

        public ConnexionController()
        {
            Db = new Data();
        }

        public IActionResult Index(Admin model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string email, string password, Admin model)
        {
           
                Admin admin = Db.ObtenirAdministrateurs().Find(a => a.Email == email);
                if ((admin != null) && (BCrypt.Net.BCrypt.Verify(password, admin.Pass)))
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Authentication, admin.Id.ToString()),
                    new Claim(ClaimTypes.Name, admin.Prenom),
                    new Claim(ClaimTypes.Email, admin.Email)
                };

                    var identity = new ClaimsIdentity(claims, "groupe2_admin");
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };
                    HttpContext.SignInAsync("groupe2_admin", principal, props).Wait();
                    return RedirectToAction("Index", "Admin");
                } else
                {
                model.Confirmation = new Erreur { Code = 1, Message = "Adresse e-mail ou mot de passe incorrect" };
                }
                
                 return View(model);
               
         
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("groupe2_admin");
            return RedirectToAction("Index", "Admin");
        }
    }
}
