var numberOfOption = 0;
function saveOption(sizeId, productId, index)
{
    const stock = document.querySelector(`#optionCount${index}`).value;
    const price = document.querySelector(`#optionPrice${index}`).value;
    const formData = new FormData();
    formData.append("SizeId", sizeId);
    formData.append("ProductId", productId);
    formData.append("Stock", stock);
    formData.append("Price", price);
    $.ajax({
        type: "POST",
        url: "/admin/updateSize",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            getOptions(productId);
            let notyf = new Notyf({
                duration: 3000,
                dismissible: true,
                position: { x: 'right', y: 'bottom' }
            });
            notyf.success(
                "Seçenek Bilgisi Başarıyla Güncellendi"
            );
        }
    });
}

function deleteOption(sizeId, productId) {
    const formData = new FormData();
    formData.append("sizeId", sizeId);
    formData.append("productId", productId);

    $.ajax({
        type: "POST",
        url: "/admin/deleteOption",
        data: formData,
        processData: false,
        contentType: false,
        success: () => {
            getOptions(productId);
            let notyf = new Notyf({
                duration: 3000,
                dismissible: true,
                position: { x: 'right', y: 'bottom' }
            });
            notyf.success(
                "Seçenek Bilgisi Başarıyla Silindi"
            );
        }
    });
}

function newOption() {
    const htmlContent = document.getElementById("optionCount");
    htmlContent.innerHTML += `
                    <div class="mb-2" style="display:flex;justify-content:center;align-items:center">
                        <div style="display:flex;flex-direction: column;align-items: center">
                            <label class="admin-label">Başlık</label>
                            <input id="optionName${numberOfOption}" name="optionNames" type="text" class="form-control"
                            placeholder="M" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                        </div>
                        <div style="display:flex;flex-direction: column;align-items: center">
                            <label class="admin-label">Sayısı</label>
                            <input id="optionCount${numberOfOption}" name="optionCounts" type="text" class="form-control"
                            placeholder="30" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                        </div>
                        <div style="display:flex;flex-direction: column;align-items: center">
                            <label class="admin-label">Fiyatı</label>
                            <input id="optionPrice${numberOfOption}" name="optionPrices" type="text" class="form-control"
                            placeholder="359,99" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                        </div>
                        <div class="mt-4 d-flex">
                            <a onclick="addNewOption(${numberOfOption})" class="btn btn-sm btn-success text-white">Kaydet</a>
                            <a class="text-white btn btn-sm btn-danger"><i class="mdi mdi-delete"></i></a>
                        </div>
                    </div>`;
}

function addNewOption(index) {
    const prId = document.getElementById("prId").value;
    const sizeName = document.getElementById(`optionName${index}`).value;
    const stock = document.getElementById(`optionCount${index}`).value;
    const price = document.getElementById(`optionPrice${index}`).value;
    const formData = new FormData();
    formData.append("productId", prId);
    formData.append("sizeName", sizeName);
    formData.append("stock", stock);
    formData.append("price", price);

    $.ajax({
        type: "POST",
        url: "/admin/addNewOption",
        data: formData,
        processData: false,
        contentType: false,
        success: () => {
            getOptions(prId);
            let notyf = new Notyf({
                duration: 3000,
                dismissible: true,
                position: { x: 'right', y: 'bottom' }
            });
            notyf.success(
                "Seçenek Bilgisi Başarıyla Eklendi"
            );
        }
    })
}

function getOptions(productId) {
    const formData = new FormData();
    formData.append("productId", productId);
    $.ajax({
        type: "POST",
        data: formData,
        url: "/admin/getOptionsById",
        processData: false,
        contentType: false,
        success: (response) => {
            numberOfOption = response.length;
            setOptions(response);
        }
    });
}

function setOptions(data) {
    const htmlContent = document.getElementById("optionCount");
    const prId = document.getElementById("prId").value;
    htmlContent.innerHTML = `<p style="display:flex;justify-content:center"><i onclick="newOption()" style="cursor:pointer;display:flex;align-items:center;justify-content:center;height:40px;width:40px;background-color:#36bea6;border-radius:50%;color:#fff" class="mdi mdi-plus"></i></p>`;
    for (let i = 0; i < data.length; i++) {
        htmlContent.innerHTML += `
                        <div class="mb-2" style="display:flex;justify-content:center;align-items:center">
                            <div style="display:flex;flex-direction: column;align-items: center">
                                <label class="admin-label">Başlık</label>
                                <input disabled id="optionName${i}" name="optionNames" type="text" class="form-control"
                                placeholder="M" value="${data[i]["sizeName"]}" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                            </div>
                            <div style="display:flex;flex-direction: column;align-items: center">
                                <label class="admin-label">Sayısı</label>
                                <input id="optionCount${i}" name="optionCounts" type="text" class="form-control"
                                placeholder="30" value="${data[i]["stock"]}" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                            </div>
                            <div style="display:flex;flex-direction: column;align-items: center">
                                <label class="admin-label">Fiyatı</label>
                                <input id="optionPrice${i}" name="optionPrices" type="text" class="form-control"
                                placeholder="359,99" value="${data[i]["price"].toString().replace(".",",")}" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                            </div>
                            <div class="mt-4 d-flex">
                                <a onclick="saveOption('${data[i]["sizeId"]}', '${prId}', ${i})" class="btn btn-sm btn-success text-white">Kaydet</a>
                                <a onclick="deleteOption('${data[i]["sizeId"]}', '${data[i]["productId"]}')" class="text-white btn btn-sm btn-danger"><i class="mdi mdi-delete"></i></a>
                            </div>
                        </div>`;
    }
}