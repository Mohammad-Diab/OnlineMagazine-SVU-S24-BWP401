$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../home');
            return;
        }
        getAllMessages();
    })
});

function getAllMessages() {
    requestToServer('Magazine/GetMessages', 'get', '', (data) => {
        let table = $('#messages-table');
        table.empty();
        let htmlVal = '';
        if (data?.length) {
            for (let i = 0; i < data.length; i++) {
                const el = data[i];
                htmlVal += `<tr style="background-color: ${el.isRead ? '' : 'lightgoldenrodyellow'}">`
                htmlVal += `<td>${i + 1}${el.isRead ? '' : '<span class="badge mx-2 bg-danger">جديد</span>'}</td>`
                htmlVal += `<td>${el.name}</td>`
                htmlVal += `<td>${el.email}</td>`
                htmlVal += `<td>${char14ToDateTime(el.sentAt)}</td>`

                htmlVal += '<td>';
                htmlVal += `<button class="btn btn-sm btn-primary" onclick="viewMessage('${el.id}')">عرض</button>`
                htmlVal += `<button class="btn btn-sm btn-danger mx-2" onclick="deleteMessage('${el.id}')">حذف</button>`
                htmlVal += '</td>';

                htmlVal += '</tr>'
            }
        } else {
            htmlVal += '<tr><td colspan="5" class="text-center">'
            htmlVal += 'لا يوجد رسائل'
            htmlVal += '</td></tr>'
        }
        table.html(htmlVal);
    })
}

function deleteMessage(id) {
    if (confirmMessage("هل تريد حذف الرسالة المحددة؟", () => {
        requestToServer(`Magazine/DeleteMessage?messageId=${id}`, 'get', '', (result) => {
            if (result) {
                getAllMessages();
            }
        })
    }, () => { })) { }
}

function viewMessage(id) {
    requestToServer(`Magazine/GetMessage?messageId=${id}`, 'get', '', (message) => {
        getAllMessages();
        $('#messageContent').val(message);
        $('#viewMessageModal').modal('show');
    })
}