var updateAddresses = () => {
    const addresses = document.querySelector("#addresses");
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
                          <div class="row pt-2">
                            <div class="col-sm-2">
                              <h6 class="mb-0">${response[i]['title']}</h6>
                            </div>
                            <div class="col-sm-8 text-secondary">
                                <p>${response[i]['detailedAddress']}</p>
                            </div>
                            <div class="col-sm-2 text-secondary text-right">
                                <a onclick='editAddress("${response[i]['city']}",
                                "${response[i]['district']}","${response[i]['neighborhood']}",
                                "${response[i]['title']}", "${response[i]['detailedAddress']}",
                                "${response[i]['id']}")'>
                                    <i style="cursor:pointer;padding:0 .2rem"
                                    class="text-primary fa-solid fa-marker"></i>
                                </a>
                                <a onclick="deleteAddressDialog('${response[i]['id']}',
                                '${response[i]['title']}')">
                                    <i style="cursor:pointer;padding:0 .2rem"
                                    class="text-danger fa-solid fa-trash"></i>
                                </a>
                            </div>
                          </div>
                `;
            }
        }
    })
}

window.addEventListener('load', () => { updateAddresses(); })