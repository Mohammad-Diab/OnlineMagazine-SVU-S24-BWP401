
$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (userFullName) => {
        if (userFullName) {
            redirectToPage('../home');
        }
    })
});

(() => {
    'use strict'
    const form = document.querySelector('form')
    form.addEventListener('submit', event => {
        event.preventDefault()
        event.stopPropagation()

        if (!form.checkValidity()) {
            return;
        }
        let username = $('#username').val();
        let password = $('#password').val();
        requestToServer('Users/Login?username=' + username + '&password=' + password, 'get', '', (result) => {
            if (result) {
                setCookie('myToken', result);
                document.location.reload();
            } else {
                showMessage('Bad username or password.'); return;
            }
        })
    }, false)
})()