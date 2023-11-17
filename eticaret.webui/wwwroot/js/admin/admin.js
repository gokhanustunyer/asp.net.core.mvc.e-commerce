var notyf = new Notyf({
    duration: 3000,
    dismissible: true,
    position: { x: 'right', y: 'bottom' }
});

//window.addEventListener('load', () => {
//    const loader = document.querySelector('.loader-screen');
//    loader.style.opacity = "0";
//    setTimeout(() => { loader.style.display = "none"; }, 500);
//})

var submitForm = () => {
    const form = document.querySelector("#form");
    const decs_area = document.querySelector("#desc-area");
    const images = decs_area.querySelectorAll("img");

    images.forEach((image) => { image.setAttribute("id", "dFile") });

    var options = document.querySelector("#optionsAsString");
    const select_box = document.querySelector("#options");
    let value = select_box.options[select_box.selectedIndex].innerHTML;
    var optionsJson = [];
    for (let i = 0; i < value; i++) {
        let name = document.querySelector(`#optionName${i}`).value;
        let count = document.querySelector(`#optionCount${i}`).value;
        let price = document.querySelector(`#optionPrice${i}`).value;
        optionsJson.push({"name":name,"count":count,"price":price})
    }
    document.querySelector("#OptionsAsJsonString").value = JSON.stringify(optionsJson);
    form.submit();
}

function changeOptionCount(htmlObject) {
    const select_box = document.querySelector("#options");
    let parent = document.querySelector("#optionCount");
    let value = select_box.options[select_box.selectedIndex].innerHTML;
    parent.innerHTML = "";
    for (let i = 0; i < value; i++) {
        parent.innerHTML += `
            <div class="mb-2" style="display:flex;justify-content:center">
                <div style="display:flex;flex-direction: column;align-items: center">
                    <label class="admin-label">Başlık</label>
                    <input id="optionName${i}" name="optionNames" type="text" class="form-control"
                    placeholder="M" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                </div>
                <div style="display:flex;flex-direction: column;align-items: center">
                    <label class="admin-label">Sayısı</label>
                    <input id="optionCount${i}" name="optionCounts" type="text" class="form-control"
                    placeholder="30" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                </div>
                <div style="display:flex;flex-direction: column;align-items: center">
                    <label class="admin-label">Fiyatı</label>
                    <input id="optionPrice${i}" name="optionPrices" class="form-control"
                    placeholder="359,99" style="width:50%;padding: .75rem .5rem;border-radius: .25rem;">
                </div>
            </div>`;
    }
}

function changeFilterCount(htmlObject) {
    let parent = document.querySelector("#optionCount");
    let value = htmlObject.options[htmlObject.selectedIndex].innerHTML;
    parent.innerHTML = "";
    for (let i = 0; i < value; i++) {
        parent.innerHTML += `
            <div class="mb-2">
                <div style="display:flex;flex-direction: column;">
                    <label class="admin-label">Başlık</label>
                    <input id="optionName${i}" name="FilterNames" type="text" class="form-control" placeholder="Oversize">
                </div
            </div>`;
    }
    parent.innerHTML += "<hr>";
}

class ConfirmModal {
    static confirmModalDiv = document.getElementById("alertModal");

    static Warning(title, message, targetForm) {
        ConfirmModal.confirmModalDiv.classList.add("active");
        ConfirmModal.confirmModalDiv.innerHTML = `
            <div class="alertModalBox">
                <div class="alertModalTitleBox warning">
                    <i class="mdi mdi-alert"></i>
                    <p class="alertModalTitle mb-0">${title}</p>
                </div>
                <div class="alertModalMessageBox border-bottom">
                    <p class="alertModalMessage mb-0">${message}</p>
                </div>
                <div class="alertModalButtonBox mt-2">
                    <button class="btn btn-success text-white" onclick="ConfirmModal.confirm('${targetForm}')">Onayla</button>
                    <button class="btn btn-danger text-white" onclick="ConfirmModal.close()">İptal Et</button>
                </div>
            </div>`;
    }

    static close() {
        ConfirmModal.confirmModalDiv.classList.remove("active");
    }

    static confirm(targetForm) {
        const form = document.getElementById(targetForm);
        console.log(targetForm, form);
        form.submit();
    }
}

function OpenComfirmModal(title, message, targetForm, type) {
    if (type === "Warning") {
        ConfirmModal.Warning(title, message, targetForm);
    }
}


