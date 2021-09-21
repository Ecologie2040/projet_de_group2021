$('.delete_article').on('click', function (e) {
    e.preventDefault();
    let id = $(this).data('id');
    let element = $(this).closest('tr');
    Swal.fire({
        title: 'Êtes-vous sûr ?',
        text: "Cet article sera supprimé définitivement !",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Supprimer',
        cancelButtonText: 'Annuler'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "SupprimerArticle/" + id,
                method: "GET",
                success: function (data) {
                    if (data.type == "success") {
                        Swal.fire(
                            'Article supprimé !',
                            'L\'article a été correctement supprimé',
                            'success'
                        )
                        element.remove();
                    } else if (data.type == "error") {
                        Swal.fire(
                            'Erreur',
                            data.message,
                            'error'
                        )
                    }
                }
            });
        }
    })
});

$('.delete_category').on('click', function (e) {
    e.preventDefault();
    let id = $(this).data('id');
    let element = $(this).closest('tr');
    Swal.fire({
        title: 'Êtes-vous sûr ?',
        text: "Cette catégorie sera supprimée définitivement !",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Supprimer',
        cancelButtonText: 'Annuler'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "SupprimerCategorie/" + id,
                method: "GET",
                success: function (data) {
                    if (data.type == "success") {
                        Swal.fire(
                            'Catégorie supprimée !',
                            'La catégorie a été correctement supprimée',
                            'success'
                        )
                        element.remove();
                    } else if (data.type == "error") {
                        Swal.fire(
                            'Erreur',
                            data.message,
                            'error'
                        )
                    }
                }
            });
        }
    })
});

$('.delete_admin').on('click', function (e) {
    e.preventDefault();
    let id = $(this).data('id');
    let element = $(this).closest('tr');
    Swal.fire({
        title: 'Êtes-vous sûr ?',
        text: "Vous êtes sur le point de supprimer un compte administrateur",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Supprimer',
        cancelButtonText: 'Annuler'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "SupprimerAdmin/" + id,
                method: "GET",
                success: function (data) {
                    if (data.type == "success") {
                        Swal.fire(
                            'Compte supprimé !',
                            data.message,
                            'success'
                        )
                        element.remove();
                    } else if (data.type == "error") {
                        Swal.fire(
                            'Erreur',
                            data.message,
                            'error'
                        )
                    }
                }
            });
        }
    })
});

$('.delete_comment').on('click', function (e) {
    e.preventDefault();
    let id = $(this).data('id');
    let element = $(this).closest('tr');
    Swal.fire({
        title: 'Êtes-vous sûr ?',
        text: "Ce commentaire sera supprimé définitivement !",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Supprimer',
        cancelButtonText: 'Annuler'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "../SupprimerCommentaire/" + id,
                method: "GET",
                success: function (data) {
                    if (data.type == "success") {
                        Swal.fire(
                            'Commentaire supprimé !',
                            'Le commentaire a été correctement supprimé',
                            'success'
                        )
                        element.remove();
                    } else if (data.type == "error") {
                        Swal.fire(
                            'Erreur',
                            data.message,
                            'error'
                        )
                    }
                }
            });
        }
    })
});

$('#comment-form').submit((e) => {
    let firstname = $('#comment-form').find('#firstname').val();
    let lastname = $('#comment-form').find('#lastname').val();
    let comment = $('#comment-form').find('#comment').val();
    let article_id = $('#comment-form').find('#submit-comment').data('id_article');
    let compteur = $('.comments-title .count').text();

    if (firstname.length > 0 && lastname.length > 0 && comment.length > 0) {

        $.ajax({
            url: "",
            method: "POST",
            dataType: "json",
            data: { "Commentaire.Nom": firstname, "Commentaire.Prenom": lastname, "Commentaire.Reaction": comment, "Article.Id": article_id },
            success: function (data) {
                if (data.type == "success") {
                    Swal.fire({
                        icon: 'success',
                        text: data.message
                    });
                    compteur++
                    $('.comments-title .count').text(compteur);
                    if (compteur > 1) {
                        $('.comments-title .count-text').text("Réactions");
                    }
                    $('.article-view-comments .comments-title').after(`<div class="user-comment-box">
                        <div class="user-picture" ><img src="pictures/icons/user-reaction.svg" alt=""></div>
                            <div class="user-reaction">
                                <div class="comment-username">${firstname} ${lastname} a écrit :</div>
                                <div class="user-comment">${comment}</div>
                            </div>
                        </div >`);
                    $(".no-comment").remove();
                    $('input[type="text"], textarea').val('');
                } else if (data.type == "error") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Erreur',
                        text: data.message
                    })
                }
            }
        });
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Erreur',
            text: 'Tous les champs ne sont pas remplis !'
        })
    }

    return false;
});

$('.article-like').on('click', '#like-button-primary', (e) => {
    e.preventDefault();
    let id = $('.article-like').data('id');
    let compteur = $('.badge-success .likes-count').text();
    $.ajax({
        url: "/Groupe_2/Accueil/AimerArticle/" + id,
        method: "GET",
        success: function (data) {
            if (data.type == "success") {
                $('#like-button-primary').remove();
                $('.article-like').append('<button type="button" id="like-button-success" class="btn bg-success"><i class="fa fa-thumbs-up"></i> Je n\'aime plus</button >');
                compteur++;
                $('.badge-success .likes-count').text(compteur);
                if (compteur > 1) {
                    $('.badge-success .likes-text').text("J'aimes");
                }
                Swal.fire({
                    icon: 'success',
                    text: data.message
                });
            } else if (data.type == "error") {
                Swal.fire(
                    'Erreur',
                    data.message,
                    'error'
                )
            }
        }
    });
    
})

$('.article-like').on('click', '#like-button-success', (e) => {
    let id = $('.article-like').data('id');
    let compteur = $('.badge-success .likes-count').text();
    $.ajax({
        url: "/Groupe_2/Accueil/NePlusAimerArticle/" + id,
        method: "GET",
        success: function (data) {
            if (data.type == "success") {
                $('#like-button-success').remove();
                $('.article-like').append('<button type="button" id="like-button-primary" class="btn bg-primary"><i class="fa fa-thumbs-up"></i> J\'aime</button >');
                compteur--;
                $('.badge-success .likes-count').text(compteur);
                if (compteur < 2) {
                    $('.badge-success .likes-text').text("J'aime");
                }
                Swal.fire({
                    icon: 'success',
                    text: data.message
                });
            } else if (data.type == "error") {
                Swal.fire(
                    'Erreur',
                    data.message,
                    'error'
                )
            }
        }
    });
});

$('.user-comment-box').each(function () {
    let usercomment = $(this).find('.user-comment');
    if (usercomment.text().length > 200) {
        let viewmore = "<a class=\"viewall\" href=\"#\"> Tout voir </a>";
        let textlength = usercomment.html().length;
        let maintext = usercomment.html().substring(0, 400);
        let secondarytext = usercomment.html().substring(400, textlength);
        usercomment.empty();
        usercomment.html(maintext + viewmore + "<span class=\"hiddentext\">"+ secondarytext +"</span>");

    }
});

$('.table').each(function () {
    let usercomment = $(this).find('.td-comment');
    if (usercomment.text().length > 200) {
        let viewmore = "<a class=\"viewall\" href=\"#\"> Tout voir </a>";
        let textlength = usercomment.html().length;
        let maintext = usercomment.html().substring(0, 400);
        let secondarytext = usercomment.html().substring(400, textlength);
        usercomment.empty();
        usercomment.html(maintext + viewmore + "<span class=\"hiddentext\">" + secondarytext + "</span>");

    }
});

$('.viewall').click(function (e) {
    e.preventDefault();
    $(this).parent().find('.hiddentext').css('display', 'inline');
    $(this).remove();
});


if ($('.articles-suggestion').hasClass('articles-suggestion') == false) {
    $('.container').css('max-width', '1000px');
    $('.article-container').css('display', 'block');
}

$('#rechercheArticle').submit(function (e) {
    let recherche = $('#rechercheTexte').val();
    if (recherche.length <= 1) {
        $('#rechercheTexte').addClass('is-invalid');
        $('.invalid-feedback').css('display', 'block');
        $('.resultats').empty();
    }
    if (recherche.length > 1) {
        $('#rechercheTexte').removeClass('is-invalid');
        $('.invalid-feedback').css('display', 'none');
        $('')
        $('.resultats').empty();
        $.ajax({
            url: "Recherche",
            method: "POST",
            dataType: "json",
            data: { "recherche": recherche},
            success: function (data) {
                if (data.type != 0) {
                    if (data.length == 0) {
                        $('.resultats').append(`<div class="alert alert-warning" role="alert">
                                            Aucun article n'a été trouvé
                                            </div>`);
                    }
                    $.each(data, function (e, obj) {
                        $('.resultats').append(`<a class="article-viewer-link" href="/groupe_2/Accueil/Article/${obj.id}">
                <div class="article-viewer">
                    <div class="article-image">
                        <img src="/groupe2_uploads/420-${obj.image}" alt="">
                    </div>
                    <div class="article-body">
                        <div class="article-category">
                            <div class="category belgium link" data-link="${obj.categorieID}">${obj.categorie}</div>
                        </div>
                        <div class="article-title">${obj.titre}</div>
                        <div class="article-date">Publié le ${obj.date_creation}</div>
                    </div>
                </div>
            </a>`);
                    })
                }
                if (data.type == 0) {
                    $('.resultats').append(`<div class="alert alert-warning" role="alert">
                                           ${data.message}
                                            </div>`);
                }
            }
        });
    }
    return false;
});

$('.resultats').on('click', '.link', function (e) {
    e.preventDefault();
    let id = $('.link').data('link');
    window.location.href = `/groupe_2/Categories/Afficher/${id}`;
})