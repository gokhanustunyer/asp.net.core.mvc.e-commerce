function updateAddresses() {
    const add_address_cosntant = `
            <div class="add-address mb-1" onclick="addAddress()"                 
                style="cursor:pointer">             
                <div style="display:flex;width:100%;height:100%;
                     justify-content:center;align-items:center">
                    <h4 style="font-size:1.25rem">+ Yeni Adres</h4>
                </div>
            </div>`;
    const addresses = document.querySelector(".addresses").querySelector(".f-wrap");
    var formData = new FormData();
    $.ajax({
        type: "POST",
        url: "/user/getAddresses",
        data: formData,
        contentType: false,
        processData: false,
        success: (response) => {
            addresses.innerHTML = "";
            for (let i = 0; i < response.length; i++) {
                addresses.innerHTML += `
                    <div class="address mb-1" onclick="selectAddress(this)" style="border:1px solid #000">
                        <label class="" style="margin-bottom:.5rem" id="addressLabel">
                            <input type="radio" name="AddressId" value="${response[i]["id"]}">
                        </label>
                        <div class="address-info">
                            <h4 class="address-title">${response[i]["title"]}</h4>
                            <p class="address-p">${response[i]['detailedAddress']}</p>
                            <p class="address-p">${response[i]['district']}/${response[i]["city"]}</p>
                        </div>
                        <a onclick='editAddress("${response[i]['city']}",
                                "${response[i]['district']}","${response[i]['neighborhood']}",
                                "${response[i]['title']}", "${response[i]['detailedAddress']}",
                                "${response[i]['id']}")' class="address-edit address-p" id="editAddress">Düzenle</a>
                    </div>
                `;
            }
            addresses.innerHTML += add_address_cosntant;
            selectAddress(document.querySelector(".address"))
        }
    })
}
window.addEventListener('load', () => { updateAddresses(); })