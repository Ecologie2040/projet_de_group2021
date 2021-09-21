using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PortailIEPSM.Areas.Groupe_2.Interfaces;

namespace PortailIEPSM.Areas.Groupe_2.Models
{
    public class Data : IData
    {

        private string Connection = "Server=(localdb)\\MSSQLLocalDB;Database=groupe2_database;Trusted_Connection=True";
        private SqlConnection conn;

        public Data()
        {
            conn = new SqlConnection(Connection);
            conn.Open();
        }

        public void Dispose()
        {
            conn.Close();
        }

        public List<Article> ObtenirTousLesArticles(bool toutafficher)
        {
            List<Article> articles = new List<Article>();
            string query = String.Empty;
            string join = "LEFT";
            if (toutafficher == false)
            {
                query = "WHERE a.visible = 1 AND b.visible = 1";
                join = "INNER";
            }
            SqlCommand cmd = new SqlCommand($"SELECT a.id_article as id_article, a.titre, a.description, a.image as image_article, a.corps, a.date_creation, a.visible as article_visible, b.nom, b.id_categorie as id_categorie FROM dbo.articles as a {join} JOIN categories as b ON a.id_categorie = b.id_categorie {query} ORDER BY a.id_article DESC", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    articles.Add(new Article
                    {
                        Id = Convert.ToInt32(reader["id_article"]),
                        Titre = reader["titre"].ToString(),
                        Description = reader["description"].ToString(),
                        Image = reader["image_article"].ToString(),
                        Corps = reader["corps"].ToString(),
                        Date_creation = Convert.ToDateTime(reader["date_creation"]),
                        Categorie = reader["nom"].ToString(),
                        CategorieID = reader.IsDBNull(reader.GetOrdinal("id_categorie")) ? 0 : Convert.ToInt32(reader["id_categorie"]),
                        Visible = Convert.ToInt32(reader["article_visible"])
                    });
                }
                reader.Close();
                return articles;
            } else
            {
                return null;
            }

        }

        public Article ObtenirArticle(int id)
        {
            SqlCommand cmd = new SqlCommand($"SELECT a.id_article as id_article, a.titre, a.description, a.corps, a.date_creation, a.jaimes, a.id_auteur, a.image as image_article, a.visible as article_visible, a.date_modification, b.nom, b.id_categorie as id_categ FROM dbo.articles as a LEFT JOIN categories as b ON a.id_categorie = b.id_categorie " +
            $"WHERE a.id_article = {id}", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                Article article = new Article();
                while (reader.Read())
                {
                    article.Id = Convert.ToInt32(reader["id_article"]);
                    article.Titre = reader["titre"].ToString();
                    article.Description = reader["description"].ToString();
                    article.Image = reader["image_article"].ToString();
                    article.Corps = reader["corps"].ToString();
                    article.Date_creation = Convert.ToDateTime(reader["date_creation"]);
                    article.Jaimes = Convert.ToInt32(reader["jaimes"]);
                    article.Auteur = reader.IsDBNull(reader.GetOrdinal("id_auteur")) ? 0 : Convert.ToInt32(reader["id_auteur"]);
                    article.Categorie = reader["nom"].ToString();
                    article.CategorieID = reader.IsDBNull(reader.GetOrdinal("id_categ")) ? 0 : Convert.ToInt32(reader["id_categ"]);
                    article.Visible = Convert.ToInt32(reader["article_visible"]);

                }
                reader.Close();
                return article;
            } else
            {
                return null;
            }
        }

        public List<Categorie> ObtenirCategories()
        {
            List<Categorie> categories = new List<Categorie>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM categories", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            
                while (reader.Read())
                {
                    categories.Add(new Categorie
                    {
                        Id = Convert.ToInt32(reader["id_categorie"]),
                        Nom = reader["nom"].ToString(),
                        Description = reader["description"].ToString(),
                        Image = reader["image"].ToString(),
                        Visible = Convert.ToInt32(reader["visible"])
                    });
                }
                reader.Close();
                return categories;
        }


        public Categorie ObtenirCategorie(int id)
        {
            Categorie categorie = new Categorie();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM categories WHERE id_categorie = {id}", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    categorie.Id = Convert.ToInt32(reader["id_categorie"]);
                    categorie.Nom = reader["nom"].ToString();
                    categorie.Description = reader["description"].ToString();
                    categorie.Image = reader["image"].ToString();
                    categorie.Visible = Convert.ToInt32(reader["visible"]);

                }
            } else
            {
                return null;
            }
            reader.Close();
            return categorie;
        }



        public bool CategorieExiste(Article article)
        {
            var cmd = new SqlCommand($"SELECT id_categorie FROM categories WHERE id_categorie = {article.CategorieID}", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            bool valide = reader.HasRows;
            reader.Close();
            return valide;
        }

        public void CreerArticle(Article article)
        {
            var cmd = new SqlCommand($"INSERT INTO articles VALUES (@titre, @description, @corps, @date_creation, 0, @categorie, @auteur, @image, @visibilite, null)", conn);
            cmd.Parameters.AddWithValue("@titre", article.Titre);
            cmd.Parameters.AddWithValue("@description", article.Description);
            cmd.Parameters.AddWithValue("@image", article.Image);
            cmd.Parameters.AddWithValue("@corps", article.Corps);
            cmd.Parameters.AddWithValue("@date_creation", article.Date_creation);
            cmd.Parameters.AddWithValue("@categorie", article.CategorieID);
            cmd.Parameters.AddWithValue("@visibilite", article.VisibleEnum);
            cmd.Parameters.AddWithValue("@auteur", article.Auteur);
            cmd.ExecuteNonQuery();

        }

        public void ModifierArticle(Article article)
        {

            var cmd = new SqlCommand($"UPDATE articles SET titre = @titre, description = @description, corps = @corps, id_categorie = @categorie, image = @image, " +
            $"visible = @visibilite, date_modification = @date_modification WHERE id_article = {article.Id}", conn);
            cmd.Parameters.AddWithValue("@titre", article.Titre);
            cmd.Parameters.AddWithValue("@description", article.Description);
            cmd.Parameters.AddWithValue("@image", article.Image);
            cmd.Parameters.AddWithValue("@corps", article.Corps);
            cmd.Parameters.AddWithValue("@categorie", article.CategorieID);
            cmd.Parameters.AddWithValue("@visibilite", article.VisibleEnum);
            cmd.Parameters.AddWithValue("@date_modification", article.Date_modification);
            cmd.ExecuteNonQuery();
        }

        public int SupprimerArticle(int id)
        {
            var cmd = new SqlCommand($"DELETE FROM articles WHERE id_article = {id}", conn);
            return cmd.ExecuteNonQuery();
        }

        public bool NomCategorieValide(Categorie categorie)
        {
            var cmd = new SqlCommand($"SELECT nom FROM categories WHERE nom = '{categorie.Nom}'", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            bool valide = reader.HasRows;
            reader.Close();
            return valide;
        }

        public void CreerCategorie(Categorie categorie)
        {
            var cmd = new SqlCommand($"INSERT INTO categories VALUES (@nom, @description, @image, @visible)", conn);
            cmd.Parameters.AddWithValue("@nom", categorie.Nom);
            cmd.Parameters.AddWithValue("@description", categorie.Description);
            cmd.Parameters.AddWithValue("@image", categorie.Image);
            cmd.Parameters.AddWithValue("@visible", categorie.VisibleEnum);
            cmd.ExecuteNonQuery();
        }

        public void ModifierCategorie(Categorie categorie)
        {
            var cmd = new SqlCommand($"UPDATE categories SET nom = @nom, description = @description, image = @image, visible = @visible WHERE id_categorie = {categorie.Id}", conn);
            cmd.Parameters.AddWithValue("@nom", categorie.Nom);
            cmd.Parameters.AddWithValue("@description", categorie.Description);
            cmd.Parameters.AddWithValue("@image", categorie.Image);
            cmd.Parameters.AddWithValue("@visible", categorie.VisibleEnum);
            cmd.ExecuteNonQuery();
        }

        public int SupprimerCategorie(int id)
        {
            var cmd = new SqlCommand($"DELETE FROM categories WHERE id_categorie = {id}", conn);
            return cmd.ExecuteNonQuery();
        }

        public List<Commentaire> AfficherTousLesCommentaires()
        {
            List<Commentaire> commentaires = new List<Commentaire>();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM commentaires ORDER BY id_commentaire DESC", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                commentaires.Add(new Commentaire
                {
                    Id = Convert.ToInt32(reader["id_commentaire"]),
                    Id_article = Convert.ToInt32(reader["id_article"]),
                    Nom = reader["nom"].ToString(),
                    Prenom = reader["prenom"].ToString(),
                    Reaction = reader["commentaire"].ToString()
                });
            }
            reader.Close();
            return commentaires;
        }

        public int PublierCommentaire(Commentaire commentaire)
        {
            var cmd = new SqlCommand($"INSERT INTO commentaires VALUES (@id_article, @nom, @prenom, @commentaire)", conn);
            cmd.Parameters.AddWithValue("@id_article", commentaire.Id_article);
            cmd.Parameters.AddWithValue("@nom", commentaire.Nom);
            cmd.Parameters.AddWithValue("@prenom", commentaire.Prenom);
            cmd.Parameters.AddWithValue("@commentaire", commentaire.Reaction);
            try {
                return cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return 0;
            }

        }

        public int SupprimerCommentaire(int id)
        {
            var cmd = new SqlCommand($"DELETE FROM commentaires WHERE id_commentaire = {id}", conn);
            return cmd.ExecuteNonQuery();
        }

        public int AimerArticle(int id)
        {
            var cmd = new SqlCommand($"UPDATE articles SET jaimes += 1 WHERE id_article = {id}", conn);
            return cmd.ExecuteNonQuery();
        }

        public int NePlusAimerArticle(int id)
        {
            var cmd = new SqlCommand($"UPDATE articles SET jaimes -= 1 WHERE id_article = {id}", conn);
            return cmd.ExecuteNonQuery();
        }

        public void CreerAdmin(Admin admin)
        {
            var cmd = new SqlCommand($"INSERT INTO administrateurs VALUES (@nom, @prenom, @email, @pass)", conn);
            cmd.Parameters.AddWithValue("@nom", admin.Nom);
            cmd.Parameters.AddWithValue("@prenom", admin.Prenom);
            cmd.Parameters.AddWithValue("@email", admin.Email);
            cmd.Parameters.AddWithValue("@pass", admin.Pass);
            cmd.ExecuteNonQuery();
        }

        public List<Admin> ObtenirAdministrateurs()
        {
            List<Admin> administrateurs = new List<Admin>();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM administrateurs ORDER BY id_admin DESC", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                administrateurs.Add(new Admin
                {
                    Id = Convert.ToInt32(reader["id_admin"]),
                    Nom = reader["nom"].ToString(),
                    Prenom = reader["prenom"].ToString(),
                    Email = reader["email"].ToString(),
                    Pass = reader["pass"].ToString()
                });
            }
            reader.Close();
            return administrateurs;
        }

        public int ModifierAdmin(Admin admin)
        {
            var cmd = new SqlCommand($"UPDATE administrateurs SET nom = @nom, prenom = @prenom, email = @email, pass = @pass WHERE id_admin = {admin.Id}", conn);
            cmd.Parameters.AddWithValue("@nom", admin.Nom);
            cmd.Parameters.AddWithValue("@prenom", admin.Prenom);
            cmd.Parameters.AddWithValue("@email", admin.Email);
            cmd.Parameters.AddWithValue("@pass", admin.Pass);
            return cmd.ExecuteNonQuery();
        }

        public int SupprimerAdmin(int id)
        {
            var cmd = new SqlCommand($"DELETE FROM administrateurs WHERE id_admin = {id}", conn);
            return cmd.ExecuteNonQuery();
        }
    }
}
