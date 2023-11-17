class ImageManager
{
    constructor(dialogId, productId)
    {
        this.imageIndexs = {};
        this.response = {};
        this.dialog_content = document.getElementById(dialogId);
        this.productId = productId;
    }

    uploadImage()
    {
        var files = $('#postedFiles')[0].files;
        var id = $('#productId').val();
        var formData = new FormData();
        for (var i = 0; i < files.length; i++)
        {
            formData.append("postedFiles", files[i]);
        }
        formData.append('id', id);
        formData.append('index', this.response.length);
        $.ajax({
            url: '/Admin/AddImage',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.getImages(id);
                let notyf = new Notyf({
                    duration: 3000,
                    dismissible: true,
                    position: { x: 'right', y: 'bottom' }
                });
                notyf.success(
                    "Resim Başarıyla Eklendi"
                );
            }
        });
    }

    openImagesDialog(id)
    {
        openDialog();
        this.getImages(id);
    }

    getImages()
    {
        var formData = new FormData();
        formData.append("id", this.productId);
        $.ajax({
            url: "/Admin/GetProductImages",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.response = response;
                imageManager.setImages();
            }
        });
    }

    setImages()
    {
        this.dialog_content.innerHTML = "";
        for (var i = 0; i < this.response.length; i++) {
            this.imageIndexs[this.response[i].id] = this.response[i].index;
            this.dialog_content.innerHTML += `
                <div class="product-image-cart" style="width: 18rem;">
                    <img src="${this.response[i].path}"
                    class="product-image-cart-img mb-1">
                    <div class="" style="width: 100%;margin-bottom: .25rem;padding:0">
                        <button onclick="imageManager.deleteImage('${this.response[i].id}')" 
                            class="btn btn-sm btn-danger text-white" style="width: 100%;">
                                Sil
                        </button>
                    </div>
                    <div class="card-body" style="width: 100%;margin-bottom: .25rem;padding:0">
                        <button class="btn btn-sm btn-success text-white"
                            style="width: 49%;" onclick="imageManager.increaseIndex('${this.response[i].id}')">    <
                        </button>
                        <button class="btn btn-sm btn-success text-white" onclick="imageManager.minusIndex('${this.response[i].id}')"
                            style="width: 49%;">    >
                        </button>
                    </div>
                </div>`
        }
    }

    deleteImage(imageId)
    {
        var formData = new FormData();
        formData.append("ImageId", imageId);
        formData.append("ProductId", this.productId);
        $.ajax({
            url: "/Admin/DeleteImage",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.getImages();
                if (response) {
                    let notyf = new Notyf({
                        duration: 3000,
                        dismissible: true,
                        position: { x: 'right', y: 'bottom' }
                    });
                    notyf.success(
                        "Resim Silme İşlemi Başarılı"
                    );
                }
            }
        });
    }

    setAsMainImage(imageId)
    {
        var formData = new FormData();
        formData.append("ImageId", imageId);
        formData.append("ProductId", this.productId);
        $.ajax({
            url: "/Admin/SetAsMainImage",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.getImages();
                if (response) {
                    let notyf = new Notyf({
                        duration: 3000,
                        dismissible: true,
                        position: { x: 'right', y: 'bottom' }
                    });
                    notyf.success(
                        "Ana Resim Güncellendi"
                    );
                }
            }
        });
    }

    increaseIndex(imageId)
    {
        let index = this.response.find(object => object["id"] == imageId)["index"];
        if (index != this.response.length - 1)
        {
            this.response[index]["index"] += 1;
            this.response[index + 1]["index"] -= 1;
            this.response = this.response.sort((a, b) => b.index - a.index);
            this.response.reverse();
            this.setImages();
        }
    }

    minusIndex(imageId)
    {
        let index = this.response.find(object => object["id"] == imageId)["index"];
        if (index != 0) {
            this.response[index]["index"] -= 1;
            this.response[index - 1]["index"] += 1;
            this.response = this.response.sort((a, b) => b.index - a.index);
            this.response.reverse();
            this.setImages();
        }
    }

    saveAlignment()
    {
        const data = [];
        const formData = new FormData();
        this.response.forEach(response => {
            data.push({"imageId": response["id"], "index": response["index"]});
        });
        var alignmentJson = JSON.stringify(data);
        formData.append("alignmentJson", alignmentJson);
        formData.append("productId", this.productId)
        $.ajax({
            type: "POST",
            url: "/admin/saveAlignment",
            data: formData,
            contentType: false,
            processData: false,
            success: () => {
                let notyf = new Notyf({
                    duration: 3000,
                    dismissible: true,
                    position: { x: 'right', y: 'bottom' }
                });
                notyf.success(
                    "Sıralama Başarıyla Güncellendi"
                );
            }
        });
    }
}

//var uploadImage = () => {
//    var files = $('#postedFiles')[0].files;
//    var id = $('#productId').val();
//    var formData = new FormData();
//    for (var i = 0; i < files.length; i++) {
//        formData.append("postedFiles", files[i]);
//    }
//    formData.append('id', id);
//    $.ajax({
//        url: '/Admin/AddImage',
//        type: 'POST',
//        data: formData,
//        contentType: false,
//        processData: false,
//        success: function (response) {
//            getImages(id);
//            let notyf = new Notyf({
//                duration: 3000,
//                dismissible: true,
//                position: { x: 'right', y: 'bottom' }
//            });
//            notyf.success(
//                "Resim Başarıyla Eklendi"
//            );
//        }
//    });
//};

//var openImagesDialog = (id) => {
//    openDialog();
//    getImages(id);
//}

//var getImages = (productId) => {
//    const dialog_content = document.querySelector("#images");
//    var formData = new FormData();
//    formData.append("id", productId);
//    $.ajax({
//        url: "/Admin/GetProductImages",
//        type: "POST",
//        data: formData,
//        contentType: false,
//        processData: false,
//        success: function (response) {
//            setImages(dialog_content, response, productId);
//        }
//    })

//}

//var setImages = (HtmlObject, response, productId) => {
//    HtmlObject.innerHTML = "";
//    for (var i = 0; i < response.length; i++) {
//        HtmlObject.innerHTML += `
//                <div class="product-image-cart" style="width: 18rem;">
//                    <img src="${response[i].path}"
//                    class="product-image-cart-img mb-1">
//                    <div class="card-body" style="width: 100%;margin-bottom: .25rem;">
//                        <button onclick="deleteImage('${productId}', '${response[i].id}')" 
//                            class="admin-button admin-btn-sm" style="width: 100%;">
//                                Sil
//                        </button>
//                    </div>
//                    <div class="card-body" style="width: 100%;margin-bottom: .25rem;">
//                        <button class="admin-button admin-btn-sm"
//                            style="width: 49%;">    <
//                        </button>
//                        <button class="admin-button admin-btn-sm"
//                            style="width: 49%;">    >
//                        </button>
//                    </div>
//                </div>`
//    }
//}

//var deleteImage = (productId, imageId) => {
//    var formData = new FormData();
//    formData.append("ImageId", imageId);
//    formData.append("ProductId", productId);
//    $.ajax({
//        url: "/Admin/DeleteImage",
//        type: "POST",
//        data: formData,
//        contentType: false,
//        processData: false,
//        success: function (response) {
//            getImages(productId);
//            if (response) {
//                let notyf = new Notyf({
//                    duration: 3000,
//                    dismissible: true,
//                    position: { x: 'right', y: 'bottom' }
//                });
//                notyf.success(
//                    "Resim Silme İşlemi Başarılı"
//                );
//            }
//        }
//    })
//}

//var setAsMainImage = (productId, imageId) => {
//    var formData = new FormData();
//    formData.append("ImageId", imageId);
//    formData.append("ProductId", productId);
//    $.ajax({
//        url: "/Admin/SetAsMainImage",
//        type: "POST",
//        data: formData,
//        contentType: false,
//        processData: false,
//        success: function (response) {
//            getImages(productId);
//            if (response) {
//                let notyf = new Notyf({
//                    duration: 3000,
//                    dismissible: true,
//                    position: { x: 'right', y: 'bottom' }
//                });
//                notyf.success(
//                    "Ana Resim Güncellendi"
//                );
//            }
//        }
//    });
//}