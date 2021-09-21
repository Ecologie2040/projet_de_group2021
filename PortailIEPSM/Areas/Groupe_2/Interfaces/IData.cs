using PortailIEPSM.Areas.Groupe_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortailIEPSM.Areas.Groupe_2.Interfaces
{
    public interface IData : IDisposable
    {
        List<Article> ObtenirTousLesArticles(bool toutafficher);
        Article ObtenirArticle(int id);
        List<Categorie> ObtenirCategories();
        Categorie ObtenirCategorie(int id);
        bool CategorieExiste(Article article);
        void CreerArticle(Article article);
        void ModifierArticle(Article article);
        int SupprimerArticle(int id);
        bool NomCategorieValide(Categorie categorie);
        void CreerCategorie(Categorie categorie);
        void ModifierCategorie(Categorie categorie);
        int SupprimerCategorie(int id);
        List<Commentaire> AfficherTousLesCommentaires();
        int PublierCommentaire(Commentaire commentaire);
        int SupprimerCommentaire(int id);
        int AimerArticle(int id);
        int NePlusAimerArticle(int id);
        void CreerAdmin(Admin admin);
        List<Admin> ObtenirAdministrateurs();
        int ModifierAdmin(Admin admin);
        int SupprimerAdmin(int id);
    }
}
