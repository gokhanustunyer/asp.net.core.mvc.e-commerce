var addAddress = () => {
    var dialog = document.querySelector(".dialog-window");
    dialog.setAttribute("style","width:75%;height:540px;overflow-y:scroll");
    dialog.innerHTML = `<i onclick="openDialog()" style="padding: 1rem;cursor:pointer;position: fixed;width: 72%;margin-top: -1rem;box-shadow: rgba(0, 0, 0, 0.02) 0px 1px 3px 0px, rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;border-radius:.35rem;background-color:#fff;text-align:right"
        class="fa-solid fa-xmark block close-btn"></i>`;

    dialog.innerHTML += `
        <div class="mt-5 quicksand">
          <div class="form-group">
            <small style="color:#b0b0b0">Başlık</small>
            <input class="user-input w-100 d-block" id="title" placeholder="Ev, İş yeri...">
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">Şehir</small>
            <select name="city" class="custom-select rounded-0" onchange="changeCity(this)" id="cities"></select>
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">İlçe</small>
            <select name="district" class="custom-select rounded-0" disabled 
                onchange="changeDistrict(this)" id="districts"></select>
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">Mahalle</small>
            <select disabled name="neighborhood" class="custom-select rounded-0" id="neighborhood"></select>
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">Açık Adres</small>
            <textarea class="user-input w-100 d-block" id="detailedAddress" rows="5" height="150"></textarea>
          </div>
          <hr>
          <button onclick="saveAddress()" class="user-button-dark mb-2">Adresi Kaydet</button>
        </div>`
    getCities("GetAllCities");
    openDialog();
}

var editAddress = (c, d, n, t, desc, id) => {
    var dialog = document.querySelector(".dialog-window");
    dialog.setAttribute("style","width:75%;height:540px;overflow-y:scroll");
    dialog.innerHTML = `<i onclick="openDialog()" style="padding: 1rem;cursor:pointer;position: fixed;width: 72%;margin-top: -1rem;box-shadow: rgba(0, 0, 0, 0.02) 0px 1px 3px 0px, rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;border-radius:.35rem;background-color:#fff;text-align:right"
        class="fa-solid fa-xmark block close-btn"></i>`;
    dialog.innerHTML += `
        <div class="mt-5 quicksand">
          <div class="form-group">
            <small style="color:#b0b0b0">Başlık</small>
            <input class="user-input w-100 d-block" id="title" value="${t}">
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">Şehir</small>
            <select name="city" class="custom-select rounded-0" onchange="changeCity(this)" id="cities"></select>
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">İlçe</small>
            <select name="district" class="custom-select rounded-0" disabled 
                onchange="changeDistrict(this)" id="districts"></select>
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">Mahalle</small>
            <select disabled name="neighborhood" class="custom-select rounded-0" id="neighborhood"></select>
          </div>
          <div class="form-group">
            <small style="color:#b0b0b0">Açık Adres</small>
            <textarea class="user-input w-100 d-block" id="detailedAddress" style="height:150px!important">${desc}</textarea>
          </div>
          <hr>
          <input hidden id="addressEditID" value="${id}">
          <button onclick="saveAddress()" class="user-button-dark mb-2">Adresi Güncelle</button>
        </div>`
    getCities("GetAllCities", c, d, n);
    openDialog()
}

var saveAddress = () => {
    const city_select = document.querySelector("#cities");
    const district_select = document.querySelector("#districts");
    const neighborhood_select = document.querySelector("#neighborhood");

    const title = document.querySelector("#title").value;
    const city = city_select.options[city_select.selectedIndex].id;
    const district = district_select.options[district_select.selectedIndex].id;
    const neighborhood = neighborhood_select.options[neighborhood_select.selectedIndex].id;
    const detailed_address = document.querySelector("#detailedAddress").value;
    var address_id = document.querySelector("#addressEditID");
    address_id = (address_id != null) ? address_id.value : null;

    var formData = new FormData();
    formData.append("title", title);
    formData.append("cityid", city);
    formData.append("districtid", district);
    formData.append("neighborhoodid", neighborhood);
    formData.append("detailedAddress", detailed_address);
    formData.append("addressId", address_id);

    $.ajax({
        url: "/user/addAddress",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            updateAddresses();
            openDialog();
            let notyf = new Notyf({
                duration: 3000,
                dismissible: true,
                position: { x: 'right', y: 'bottom' }
            });
            if (address_id != null) {
                notyf.success("Adres Bilgisi Başarıyla Güncellendi");
            }
            else {
                notyf.success("Adres Bilgisi Başarıyla Eklendi");
            }
        }
    })
}


var deleteAddressDialog = (addressId, title) => {
    openDialog();
    var dialog = document.querySelector(".dialog-window");
    dialog.setAttribute("style","width:25%;height:180px;position:relative;");
    dialog.innerHTML = `<i onclick="openDialog()" style="padding: .5rem;cursor:pointer;position: absolute;right: 5px;/* width: 100%; */margin-top: -1rem;box-shadow: rgba(0, 0, 0, 0.02) 0px 1px 3px 0px, rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;border-radius: .15rem;background-color:#fff;text-align:right;top: 20px;"
        class="fa-solid fa-xmark block close-btn"></i>`;

    dialog.innerHTML += `
        <div class="mt-4">
            <hr>
            <p>${title} İsimli Adres Kaydı Silinecek Emin misiniz?</p>
            <div style="display:block;text-align:right">
                <button class="btn btn-danger" onclick="deleteAddress('${title}','${addressId}')">Sil</button>
                <button class="btn btn-light text-dark" onclick="openDialog()">İptal</button>
            </div>
        </div>`
}

var deleteAddress = (title, addressId) => {
    var formData = new FormData();
    formData.append("addressId", addressId);
    $.ajax({
        type: "POST",
        url: "/user/deleteAddress",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            let notyf = new Notyf({
                duration: 3000,
                dismissible: true,
                position: { x: 'right', y: 'bottom' }
            });
            notyf.success(`${title} İsimli Adres Kaydı Başarıyla Silindi`);
            updateAddresses();
            openDialog();
        }
    })
}
