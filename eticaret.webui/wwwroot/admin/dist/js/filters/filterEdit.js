var numberOfOption = 0;
function saveOption(filterId, index) {
    const filterName = document.getElementById(`optionName${index}`).value;
    const prId = document.getElementById("prId").value;
    const formData = new FormData();
    formData.append("filterId", filterId);
    formData.append("filterName", filterName);
    $.ajax({
        type: "POST",
        url: "/admin/updateFilter",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            getOptions(prId);
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

function deleteOption(filterId, filterBoxId) {
    const formData = new FormData();
    formData.append("filterBoxId", filterBoxId);
    formData.append("filterId", filterId);
    const prId = document.getElementById("prId").value;

    $.ajax({
        type: "POST",
        url: "/admin/deleteFilterFromFilterBox",
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
                "Seçenek Bilgisi Başarıyla Silindi"
            );
        }
    });
}

function newOption() {
    const htmlContent = document.getElementById("optionCount");
    htmlContent.innerHTML += `
                    <div class="mb-2" style="display:flex;justify-content:center;align-items:center">
                        <div style="display:flex;flex-direction: column;margin-right:.5rem">
                            <label class="admin-label">Başlık</label>
                            <input id="optionName${numberOfOption}" name="optionNames" type="text" class="form-control"
                            placeholder="Kalıp" style="padding: .75rem .5rem;border-radius: .25rem;">
                        </div>
                        <div class="mt-4 d-flex">
                            <a style="width:146.11px;height:47px;display:flex;align-items:center;justify-content: center;" onclick="addNewOption(${numberOfOption})" class="btn btn-sm btn-success text-white">Kaydet</a>
                        </div>
                    </div>`;
}

function addNewOption(index) {
    const prId = document.getElementById("prId").value;
    const sizeName = document.getElementById(`optionName${index}`).value;
    const formData = new FormData();
    formData.append("filterBoxId", prId);
    formData.append("filterName", sizeName);

    $.ajax({
        type: "POST",
        url: "/admin/addNewFilterToFilterBox",
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

function getOptions(filterBoxId) {
    const formData = new FormData();
    formData.append("filterBoxId", filterBoxId);
    $.ajax({
        type: "POST",
        data: formData,
        url: "/admin/getFiltersByFilterBoxId",
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
    htmlContent.innerHTML = ``;
    for (let i = 0; i < data.length; i++) {
        htmlContent.innerHTML += `
                        <div class="mb-2" style="display:flex;justify-content:center;align-items:center">
                            <div style="display:flex;flex-direction: column;margin-right:.5rem">
                                <label class="admin-label">Başlık</label>
                                <input id="optionName${i}" name="optionNames" type="text" class="form-control"
                                placeholder="Kalıp" value="${data[i]["filterTitle"]}" style="padding: .75rem .5rem;border-radius: .25rem;">
                            </div>
                            <div class="mt-4 d-flex">
                                <a onclick="saveOption('${data[i]["id"]}', ${i})" class="btn btn-sm btn-success text-white" style="height:47px;display:flex;align-items:center;justify-content: center;">Kaydet</a>
                                <a href="/admin/filterProducts/${data[i]["id"]}" class="btn btn-sm btn-primary text-white" style="height:47px;display:flex;align-items:center;justify-content: center;">Ürünler</a>
                                <a onclick="deleteOption('${data[i]["id"]}', '${prId}')" class="text-white btn btn-sm btn-danger" style="height:47px;display:flex;align-items:center;justify-content: center;"><i class="mdi mdi-delete"></i></a>
                            </div>
                        </div>`;
    }
}