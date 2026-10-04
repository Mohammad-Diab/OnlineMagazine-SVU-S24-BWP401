
var ServerUrl = "http://localhost/API/";
var ClientUrl = "http://localhost/WebStore";
var Islogin = false;


function requestToServer(url, type, data, onSuccess, processData = true) {
    data = type.toLowerCase() == 'get' ? '' : (processData ? JSON.stringify(data) : data);
    let token = getCookie('myToken')
    $.ajax({
        url: ServerUrl + url,
        method: type,
        data: data,
        crossDomain: true,
        processData,
        contentType: processData ? "application/json" : false,
        headers: { myToken: token ?? '' },
        ...(processData ? {} : { enctype: "multipart/form-data" }),
        error: function (result) {
            showMessage(result.Message || "لا يمكن الاتصال بالمخدم");
        },
        success: function (result) {
            if (result.code === 200) {
                if (typeof onSuccess == 'function') {
                    onSuccess(result.data)
                }
            } else {
                showMessage(`خطأ ${result.code}: ${result.errorMessage || "لا يمكن الاتصال بالمخدم"}`);
                if (result.code === 500) {
                    redirectToPage('../login');
                }
            }
        }
    });
}


function setCookie(name, value, minutes) {
    var expires = "";
    if (minutes) {
        var date = new Date();
        date.setTime(date.getTime() + (minutes * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}


function showMessage(message) {
    alert(message);
}

function confirmMessage(message, onOk, onCancel) {
    if (confirm(message)) {
        if (typeof onOk != 'undefined') {
            onOk();
        }
    } else {
        if (typeof onCancel != 'undefined') {
            onCancel();
        }
    }
}

function readInput(message, onVal) {
    let value = prompt(message)
    if (value) {
        if (typeof onVal != 'undefined') {
            onVal(value);
        }
    }
}

function logout() {
    requestToServer('Users/Logout', 'get', '', () => {
        setCookie('myToken', null);
        redirectToPage('../');
    })
}

function fillGrid(data, gridId) {
    let grid = $(`#${gridId}`);
    grid.empty();
    let htmlVal = '';
    if (data?.length) {
        for (let i = 0; i < data.length; i++) {
            const el = data[i];
            htmlVal += getObject(gridId, el, i);
        }
    } else {
        htmlVal += '<p style="flex: 1" class="text-center fs-4">';
        htmlVal += 'لا يوجد عناصر';
        htmlVal += '</p>';
    }
    grid.html(htmlVal);
    if (data?.length) {
        for (let i = 0; i < data.length; i++) {
            GetImage(data[i].image, `image-${gridId}-${data[i].id}`, i);
        }
    }
}

let getObject = (gridId, el, index) => {
    let htmlVal = `<a href="../article?id=${el.id}" class="card card-width">`
    htmlVal += `<div class="image-container">`
    htmlVal += `<img src="../assets/imgs/loading.gif" id="image-${gridId}-${el.id}" class="card-img-top" alt="${el.title}">`
    htmlVal += `</div>`
    htmlVal += `<div title="${el.title}" class="card-footer d-flex justify-content-between align-items-center">`
    htmlVal += `<h6 class="card-title card-title-text mb-0">${el.title}</h6>`
    htmlVal += `<p class="card-text text-end">${char14ToDateTime(el.createdAt)}</p>`
    htmlVal += `</div>`
    htmlVal += '</a>\n';

    return htmlVal
}

function GetImage(path, id, index = 1) {
    if (typeof imageLoadingCount != 'undefined') {
        imageLoadingCount += 1;
    }

    setTimeout(() => {
        requestToServer('Magazine/GetImage?path=' + path, 'get', '', (data) => {
            $(`#${id}`).attr('src', data);
            $(`#${id}`).removeClass('is-loading');
        })
    }, index * 300);
}

function redirectToPage(pageLink) {
    const currentUrl = new URL(window.location.href);
    currentUrl.search = '';
    currentUrl.hash = '';
    const newUrl = `${currentUrl.origin}${currentUrl.pathname.replace(/\/$/, '')}/${pageLink}`;
    window.location.href = newUrl;
}

function char14ToDateTime(dateStr) {
    if (!dateStr || dateStr.length !== 14) {
        return ""
    }
    var year = dateStr.substring(0, 4);
    var month = dateStr.substring(4, 6);
    var day = dateStr.substring(6, 8);
    var formattedDate = year + '-' + month + '-' + day;
    return formattedDate;
}
