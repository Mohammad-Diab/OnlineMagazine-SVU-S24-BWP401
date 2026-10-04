
let articleId;
let imageLoadingCount = 1;
$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        Islogin = islogin;
        if (islogin) {
            $('#articlesManagerTab').show();
            $('#messagesManagerTab').show();
            $('#logoutBtn').show();
        } else {
            $('#loginBtn').show();
        }
        const params = new URLSearchParams(window.location.search);
        articleId = params.get('id');
        if (!articleId) {
            showMessage('لم يتم تحديد مقال');
            return;
        }
        requestToServer(`Magazine/GetArticle?articleId=${articleId}`, 'get', '', (data) => {
            fillObjectData(data);
        });
    })

});

function fillObjectData(el) {
    if (el) {
        document.title = "المقالات - " + el.title;

        $('#article-title').text(el.title);
        $('#read-count').text(el.readCount);
        $('#publish-date').text(char14ToDateTime(el.createdAt));
        $('#article-image').attr('alt', el.title);
        GetImage(el.image, 'article-image');
        const paragraphes = $('#paragraphes');
        paragraphes.empty();
        let pCount = 0;
        el.paragraphes.forEach((p, ind) => {
            $('#article-pargraphs').append(`<p>${p}</p>`);
            let pHtml = `<div id="paragraph-${ind}">`
            if (p.title) {
                pHtml += `<h3 class="my-3">${p.title}</h3>`
            }
            pHtml += '<div class="row align-items-center">'

            const contentHtml = p.content ?
                `<div class="col col-12 ${p.image ? 'col-md-6' : ''}"><p class="text-justify">${p.content}</p></div>` : '';
            const imageHtml = p.image ?
                `<div class="col col-12 ${p.content ? 'col-md-6' : ''}"><img style="width: 100%" id="paragraph-${ind}-image" src="../assets/imgs/loading.gif" alt="" class="img-fluid"></div>` : '';

            pHtml += pCount % 2 !== 0 ? contentHtml + imageHtml : imageHtml + contentHtml;

            pHtml += '</div>'
            pHtml += '<hr>'
            pHtml += '</div>\n';
            paragraphes.append(pHtml);
            if (p.image) {
                pCount += 1;
            }
        });

        if (el.paragraphes?.length) {
            for (let i = 0; i < el.paragraphes?.length; i++) {
                if (el.paragraphes[i].image) {
                    GetImage(el.paragraphes[i].image, `paragraph-${i}-image`, i + 1);
                }
            }
        }

    }
}
