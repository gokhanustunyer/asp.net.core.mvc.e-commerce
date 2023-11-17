class AddProductDOMManager
{
    constructor(relatedsDialogId, filterDialogId)
    {
        this.relatedsDialog = document.getElementById(relatedsDialogId);
        this.filterDialog = document.getElementById(filterDialogId);
    }

    addToFilterDialog(filterData)
    {

    }
}


function getNotices() {
    $.ajax({
        type: "POST",
        url: "/Admin/GetNotices",
        data: new FormData(),
        contentType: false,
        processData: false,
        success: (response) => {
            setNotices(response);
        }
    })
}


function setNotices(response) {
    const notices = document.getElementById("notices");
    notices.innerHTML = `<li class="table-header">
                            <div class="col col-3 text-white">Duyuru</div>
                            <div class="col col-4 text-white">Sil</div>
                         </li>`
    for (let item of response) {
        notices.innerHTML += `
            <li class="table-row d-flex mt-3">
                <input hidden value='${item.item2}' class='notice_id'>
                <input type="text" class="admin-input notice_value" style="width:100%" value="${item.item1}">
                <button onclick="deleteNotice('${item.item2}')" class="btn btn-primary"><i class="mdi mdi-close"></i></button>
            </li>`
    }
}

function saveNotices() {
    const notices = document.getElementById("notices");
    const rows = notices.querySelectorAll(".table-row");

    var sucesses = 0;
    for (let row of rows) {
        let inputs = row.querySelectorAll("input");
        $.ajax({
            type: "POST",
            url: "/Admin/SetNotice",
            data: { Id: inputs[0].value, Value: inputs[1].value },
            success: () => {
                sucesses++;
                if (sucesses == rows.length) {
                    let notyf = new Notyf({
                        duration: 3000,
                        dismissible: true,
                        position: { x: 'right', y: 'bottom' }
                    });
                    notyf.success(
                        "Duyurular Başarıyla Güncellendi"
                    );
                }
            }
        });
    }
}

function newNotice() {
    const notices = document.getElementById("notices");
    notices.innerHTML += `
            <li class="table-row d-flex mt-3">
                <input hidden value='a3677ead-4249-492f-ba33-94959c1dcd5d' class='notice_id'>
                <input type="text" class="admin-input notice_value" style="width:100%" value=''>
                <button class="btn btn-primary"><i class="mdi mdi-close"></i></button>
            </li>`
}
function deleteNotice(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteNotice",
        data: { id: id },
        success: () => {
            let notyf = new Notyf({
                duration: 3000,
                dismissible: true,
                position: { x: 'right', y: 'bottom' }
            });
            notyf.success(
                "Duyuru Başarıyla Silindi"
            );

            extractNotice(id);
        }
    });
}
function extractNotice(id) {
    const notices = document.getElementById("notices");
    var idInputs = notices.querySelectorAll(".notice_id");
    var valueInputs = notices.querySelectorAll(".notice_value");

    notices.innerHTML = `<li class="table-header">
                            <div class="col col-3 text-white">Duyuru</div>
                            <div class="col col-4 text-white">Sil</div>
                         </li>`
    var counter = 0;
    for (let i = 0; i < idInputs.length; i++) {
        if (idInputs[i].value != id) {
            notices.innerHTML += `
                <li class="table-row d-flex">
                    <input hidden value='${idInputs[i].value}' class='notice_id'>
                    <input type="text" class="admin-input notice_value" style="width:100%" value='${valueInputs[i].value}'>
                    <button onclick="deleteNotice('${idInputs[i].value}')" class="btn btn-primary"><i class="mdi mdi-close"></i></button>
                </li>`
        }
    }
}

function editRelateds() {
    openDialog();
}
function adminSearch(){
    var formData = new FormData();
    var search = document.querySelector("#searchInput").value;
    formData.append("search", search);
    $.ajax({
        type: "POST",
        url: "/admin/search",
        data: formData,
        contentType: false,
        processData: false,
        success: (response) => {
            setSearchResults(response);
        }
    })
}
function setSearchResults(response) {
    var dialog = document.querySelector(".relateds-search-results");
    dialog.innerHTML = "";
    if (response.length == 0) {
        dialog.innerHTML += `<div style="width:100%;height:100%;display:flex;align-items:center;flex-direction:column;justify-content:center" class="mt-4"><i class="fa-solid fa-magnifying-glass" style="font-size:4rem;color:#9a9a9a;"></i><span style="color:#9a9a9a;max-width:300px;text-align:center;margin-top:1rem">Maalesef aradığınız kelimeler ile eşleşen bir ürün bulamadık</span></div>`;
    }
    for (let i = 0; i < response.length; i++) {
        dialog.innerHTML += `
            <div class="search-result relateds-s-r" onclick="addToRelateds('${response[i].id}', this)" style="cursor:pointer">
                <span class="selectRelated"><i class="mdi mdi-bookmark-check"></i></span>
                <img class="search-img" src="${response[i].mainImageUrl}">
                <a title="${response[i].name}" class="search-title">${response[i].name}</a>
            </div>`;
    }
}

function addToRelateds(productId, div) {
    console.log(productId, div);
    const input = document.querySelector("#relatedProductIds");
    input.value += `${productId},`;
    div.querySelector("span").setAttribute("style","display:block");
}

//document.querySelector("#searchInput")
//    .addEventListener("keyup", () => {
//        if (event.code === 'Space') { adminSearch(); }
//    });

function addToFilters(filterId, filterName) {
    const form = document.querySelector("#form");
    const dialog = document.querySelector("#filterModal .dialog-content");
    const label = `<span style="border-radius:6px;background-color:#dfdfdf;padding:0 1rem;">${filterName} <i class="fa-solid fa-x" style="margin-left:.5rem;cursor:pointer;" onclick="AddProductFilter.removeFromFilterList('${filterId}')"></i></span>`;
    if (!dialog.innerHTML.includes(label)) {
        dialog_content = label + dialog.innerHTML;
        dialog.innerHTML = dialog_content;
    }
}

function removeFilter() {

}