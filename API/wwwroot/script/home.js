$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        Islogin = islogin;
        if(islogin){
            $('#articlesManagerTab').show();
            $('#messagesManagerTab').show();
            $('#logoutBtn').show();
        } else {
            $('#loginBtn').show();
        }
        requestToServer('Magazine/GetNewArticles', 'get', '', (data) => {
            requestToServer('Magazine/GetTopArticles', 'get', '', (data2) => {
                if (data2) {
                    fillGrid(data2, 'mostRead');
                }
            });
            if (data) {
                fillGrid(data, 'newestArticles');
            }
        });
    })
});

let imageLoadingCount = 0
document.documentElement.style.setProperty('--animate-duration', '1s');

