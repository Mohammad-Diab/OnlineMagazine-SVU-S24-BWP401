$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../home');
            return;
        }
        getAllArticles();
    })
});

function getAllArticles() {
    requestToServer('Magazine/GetArticles', 'get', '', (data) => {
        let table = $('#articles-table');
        table.empty();
        let htmlVal = '';
        if (data?.length) {
            for (let i = 0; i < data.length; i++) {
                const el = data[i];
                htmlVal += '<tr>'
                htmlVal += `<td>${i + 1}</td>`
                htmlVal += `<td>${el.title}</td>`
                htmlVal += `<td>${char14ToDateTime(el.createdAt)}</td>`
                htmlVal += `<td>${el.readCount} مرة</td>`

                htmlVal += '<td>';
                htmlVal += `<button class="btn btn-sm btn-danger" onclick="deleteArticle('${el.id}')">حذف</button>`
                htmlVal += '</td>';

                htmlVal += '</tr>'
            }
        } else {
            htmlVal += '<tr><td colspan="5" class="text-center">'
            htmlVal += 'لم تقم باضافة اي عنصر بعد'
            htmlVal += '</td></tr>'
        }
        table.html(htmlVal);
    })
}

function deleteArticle(id) {
    if (confirmMessage("هل تريد حذف المقال المختار؟", () => {
        requestToServer(`Magazine/DeleteArticle?articleId=${id}`, 'get', '', (data) => {
            if (data) {
                getAllArticles();
            } else {
                showMessage('فشلت العملية');
            }
        });
    }, () => { })) { }
}


function addParagraph() {
    let paragraphesCount = $('#paragraphes').children().length;
    let newParagraph = `<div id="paragraph-${paragraphesCount}">`
    newParagraph += `<div class="mb-3">`
    newParagraph += `<label for="p${paragraphesCount}-title" class="form-label">عنوان الفقرة</label>`
    newParagraph += `<input type="text" class="form-control" id="p${paragraphesCount}-title" name="p${paragraphesCount}-title">`
    newParagraph += `</div>`
    newParagraph += `<div class="mb-3">`
    newParagraph += `<label for="p${paragraphesCount}-content" class="form-label">النص</label>`
    newParagraph += `<textarea class="form-control" id="p${paragraphesCount}-content" name="p${paragraphesCount}-content"></textarea>`
    newParagraph += `</div>`
    newParagraph += `<div class="mb-3">`
    newParagraph += `<label for="p${paragraphesCount}-image" class="form-label">الصورة</label>`
    newParagraph += `<input type="file" class="form-control" id="p${paragraphesCount}-image" name="p${paragraphesCount}-image" accept="image/*">`
    newParagraph += `</div> `
    newParagraph += `<button type="button" class="btn btn-danger d-block" style="margin-right: auto;"`
    newParagraph += `onclick="removeParagraph(${paragraphesCount})">حذف فقرة</button>`
    newParagraph += `<hr />`
    newParagraph += `</div>\n`;
    $('#paragraphes').append(newParagraph);
}

function removeParagraph(index) {
    let paragraphesCount = $('#paragraphes').children().length;
    if (paragraphesCount === 1) {
        showMessage("يجب ان يحتوي المقال على فقرة واحدة على الاقل");
        return;
    }

    if (confirmMessage("هل تريد حذف هذه الفقرة؟", () => {
        $(`#paragraph-${index}`).remove();
    }, () => { })) { }
}

(() => {
    'use strict'
    const form = document.querySelector('form')
    form.addEventListener('submit', event => {
        event.preventDefault()
        event.stopPropagation()
        form.classList.add('was-validated');

        if (!form.checkValidity()) {
            return;
        }
        const title = $('#title').val();
        const image = $("#image").get(0).files[0];

        if (title) {
            $('#title')[0].setCustomValidity('');
        } else {
            $('#title')[0].setCustomValidity('error');
        }

        if (image) {
            $('#image')[0].setCustomValidity('');
        } else {
            $('#image')[0].setCustomValidity('error');
        }

        var formData = new FormData();
        formData.append("title", title);
        formData.append("image", image);


        let paragraphes = $('#paragraphes').children();
        let pAdded = 0;
        for (let i = 0; i < paragraphes.length; i++) {
            const pTitle = $(`#p${i}-title`).val();
            const pContent = $(`#p${i}-content`).val();
            const pImage = $(`#p${i}-image`).get(0).files[0];
            let validParagraph = false;
            if (pImage) {
                $(`#p${i}-image`)[0].setCustomValidity('');
                $(`#p${i}-content`)[0].setCustomValidity('');
                $(`#p${i}-title`)[0].setCustomValidity('');
                validParagraph = true;
            } else if (pContent && pTitle) {
                $(`#p${i}-image`)[0].setCustomValidity('');
                $(`#p${i}-content`)[0].setCustomValidity('');
                $(`#p${i}-title`)[0].setCustomValidity('');
                validParagraph = true
            } else {
                $(`#p${i}-image`)[0].setCustomValidity('error');
                $(`#p${i}-content`)[0].setCustomValidity('error');
                $(`#p${i}-title`)[0].setCustomValidity('error');
            }

            if (validParagraph) {
                pAdded += 1;
                formData.append(`paragraphes[${i}][title]`, pTitle);
                formData.append(`paragraphes[${i}][content]`, pContent);
                formData.append(`paragraphes[${i}][image]`, pImage);
                formData.append(`paragraphes[${i}][position]`, i);
            }
        }

        if (pAdded === 0) {
            showMessage("يجب ان يحتوي المقال على فقرة واحدة على الاقل");
            return;
        }

        requestToServer('Magazine/CreateArticle', 'post', formData, (result) => {
            if (result) {
                getAllArticles();
                $('#addArticleModal').modal('hide');
                form.reset();
                form.classList.remove('was-validated');
            } else {
                showMessage("فشل حفظ المقال");
            }
        }, false);
    }, false)
})()