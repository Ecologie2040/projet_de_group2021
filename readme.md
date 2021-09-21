# Projet gestion de news (groupe 2 - Hugo, Bruno, Oleko)

## Détails importants

### Base de données

La base de données __au format BACPAC__ se trouve dans le [dossier database](https://github.com/IepsmPOO/IEPSM/tree/Hugo-Bruno-Oleko/PortailIEPSM/Areas/Groupe_2/Database).  
La chaîne de connexion peut être modifiée dans la classe [Data.cs](https://github.com/IepsmPOO/IEPSM/blob/Hugo-Bruno-Oleko/PortailIEPSM/Areas/Groupe_2/Models/Data.cs) qui se trouve dans les modèles.  
````
private string Connection = "Server=(localdb)\\MSSQLLocalDB;Database=groupe2_database;Trusted_Connection=True";
````

### Ajout de dossiers dans wwwroot

Étant donné que nous utilisons notre propre CSS et JS sur certaines pages, nous stockons nos [styles](https://github.com/IepsmPOO/IEPSM/tree/Hugo-Bruno-Oleko/PortailIEPSM/wwwroot/css/css_groupe2) et [scripts](https://github.com/IepsmPOO/IEPSM/tree/Hugo-Bruno-Oleko/PortailIEPSM/wwwroot/js/groupe2_js) dans nos propores dossiers afin d'éviter les conflits avec les autres groupes.  
Le dossier [groupe2_uploads](https://github.com/IepsmPOO/IEPSM/tree/Hugo-Bruno-Oleko/PortailIEPSM/wwwroot/groupe2_uploads) permet de stocker les images des articles et des catégories.

### Utilisation des sections dans le layout principal

Afin de greffer notre CSS et JS à AdminLTE, nous avons ajouté deux sections (CSS et JS) dans le [layout principal](https://github.com/IepsmPOO/IEPSM/blob/Hugo-Bruno-Oleko/PortailIEPSM/Views/Shared/_Layout.cshtml).  
````
@RenderSection("Styles", required: false)
@RenderSection("Scripts", false)
 ````
 ````
 @section Styles {
    <link href="~/css/css_groupe2/groupe2.css" rel="stylesheet" />
}

 @section Scripts {
    <script src="~/themes/AdminLTE-3.0.5/plugins/sweetalert2/sweetalert2.all.js"></script>
    <script src="~/js/groupe2_js/scripts.js"></script>
}
````
### Ajout de l'identification dans Startup.Cs

Afin d'intégrer l'identification à l'administration de notre projet, nous avons ajouté du code dans [Startup.Cs](https://github.com/IepsmPOO/IEPSM/blob/Hugo-Bruno-Oleko/PortailIEPSM/Startup.cs). Le code ci-dessous n'impactera pas les autres groupes car nous utilisons notre propre AuthenticationSchemes.
````
[Authorize(AuthenticationSchemes = "groupe2_admin")]
````
````
public void ConfigureServices(IServiceCollection services)
   {
       services.AddControllersWithViews();
       services.AddAuthentication()
       .AddCookie("groupe2_admin", option =>
       {
           option.LoginPath = "/Groupe_2/Connexion";
           option.Cookie.Name = "article_session";
       });
   }
````
````
app.UseAuthentication();
 ````
