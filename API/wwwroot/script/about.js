let cartProducts = [];
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
    })
});

function sendMessage(e) {
    e.preventDefault();
    e.stopPropagation();
    const form = e.target;
    $(form).removeClass('was-validated');

    const name = $('#name').val();
    const email = $('#email').val();
    const message = $('#message').val();


    if (name) {
        $('#name')[0].setCustomValidity('');
    } else {
        $('#name')[0].setCustomValidity('error');
    }

    if (email && /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email)) {
        $('#email')[0].setCustomValidity('');
    } else {
        $('#email')[0].setCustomValidity('error');
    }

    if (message) {
        $('#message')[0].setCustomValidity('');
    } else {
        $('#message')[0].setCustomValidity('error');
    }


    if (!form.checkValidity()) {
        $(form).addClass('was-validated');
        return;
    }

    $(form).addClass('was-validated');

    const deleveryOptions = {
        name,
        email,
        message,
    }

    requestToServer('Magazine/SendMessage', 'post', deleveryOptions, (data) => {
        if (data) {
            showMessage('تم ارسال الرسالة بنجاح');
            setTimeout(() => {
                window.location.reload();
            }, 200);
        }
    });
}