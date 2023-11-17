var ConfirmDialog = (title, func) => {
    openDialog();
    var dialog = document.querySelector(".dialog-window");
    dialog.setAttribute("style", "width:25%;height:180px;position:relative;");
    dialog.innerHTML = `<i onclick="openDialog()" style="padding: .5rem;cursor:pointer;position: absolute;right: 5px;/* width: 100%; */margin-top: -1rem;box-shadow: rgba(0, 0, 0, 0.02) 0px 1px 3px 0px, rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;border-radius: .15rem;background-color:#fff;text-align:right;top: 20px;"
        class="fa-solid fa-xmark block close-btn"></i>`;

    dialog.innerHTML += `
        <div class="mt-4">
            <hr>
            <p>${title}</p>
            <div style="display:block;text-align:right">
                <button class="btn btn-danger" onclick="${func}">Devam Et</button>
                <button class="btn btn-light text-dark" onclick="openDialog()">İptal</button>
            </div>
        </div>`
}

var resetPassword = () => {
    openDialog();
    var formData = new FormData();
    $.ajax({
        type: "POST",
        url: "/user/updatePasswordRequest",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            let notyf = new Notyf({
                duration: 3000,
                dismissible: true,
                position: { x: 'right', y: 'bottom' }
            });
            notyf.success("Şifre yenileme işlemi için tarafınıza mail gönderilmiştir");
        }
    })
}