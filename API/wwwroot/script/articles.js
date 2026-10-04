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
        requestToServer('Magazine/GetArticles', 'get', '', (data) => {
            if (data) {
                fillGrid(data, 'articles');
            }
        });

    })
});

let imageLoadingCount = 0