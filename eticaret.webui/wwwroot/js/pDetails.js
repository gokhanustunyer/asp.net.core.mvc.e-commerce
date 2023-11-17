var setPrice = (discountRate) => {
    const select_box = document.querySelector("#optionSb");
    let id = select_box.options[select_box.selectedIndex].id;
    let value = select_box.options[select_box.selectedIndex].value;
    let stock = parseInt(select_box.options[select_box.selectedIndex].getAttribute("stock"));
    document.querySelector(".current-price").innerHTML = `${id} TL`;
    document.querySelector("#sizeId").value = value;

    const discounted_price = document.querySelector(".discounted-price");
    if (discounted_price != null)
    {
        id = parseFloat(id.replace(",", "."));
        discounted_price.innerHTML = `${(Math.round(100 * (id * (100 - discountRate) / 100)) / 100).toString().replace('.',',')} TL`
    }
    let selects = document.querySelector(".info-piece select");
    if (stock < 10) {
        for (let i = stock; i < 10; i++) {
            selects.options[i].setAttribute("disabled", "disabled");
        }
    }
    else {
        for (let i = 0; i < 10; i++) {
            selects.options[i].removeAttribute("disabled");
        }
    }
}


function addToLogs(id) {
    let ids = JSON.parse(localStorage.getItem("arite_logs_ids"));
    ids = (ids == null) ? [] : ids;
    if (!ids.includes(id)) {
        ids.push(id);
    }
    var index = ids.indexOf(id);
    if (index !== -1) {
        ids.splice(index, 1);
    }
    ids.push(id);
    localStorage.setItem("arite_logs_ids", JSON.stringify(ids));
    if (ids.length < 2) {
        document.getElementById("last-iters").innerHTML = "";
    }
}


const prIds = JSON.parse(localStorage.getItem("arite_logs_ids"));
const formData = new FormData();
formData.append("prIds", prIds);
$.ajax({
    url: "/home/getProductsById",
    type: "POST",
    data: formData,
    contentType: false,
    processData: false,
    success: (response) => {
        setLastVisitedProducts(response, activePrId);
    }
})

function setLastVisitedProducts(products, activeProductId) {
    const lastVisitedsDiv = document.getElementById("lastVisitedProducts");
    var innerHTML = "";
    products.forEach(item => {
        if (item.id != activeProductId) {
            innerHTML += `
                <a class="col-lg-3 col-md-4 col-sm-6 mb-3 mr-3 p-0" href="/urunler/${item.url}">
                    <div class="product-cart">
                        <div class="product-cart-img">
                            <img src="${item.mainImageUrl}"/>
                        </div>
                        <div class="product-cart-info">
                            <div class="d-flex quicksand">
                                <div class="ellipsis" style="width:100%">
                                    <p class="ellipsis">${item.name}</p>
                                </div>
                                <div class="text-right" style="width: 40%;">
                                    <p class="font-weight-bold">${item.price} TL</p>
                                </div>
                            </div>
                        </div>
                        <div class="absolute-icons">
                            <span class="cart-icon-link"><i class="cart-icon fa-regular fa-heart"></i></span>
                            <span class="cart-icon-link mt-2"><i class="cart-icon fa-solid fa-basket-shopping"></i></span>
                            <span class="cart-icon-link mt-2"><i class="fa-solid fa-magnifying-glass"></i></span>
                        </div>
                    </div>
                </a>`;
        }
    });
    lastVisitedsDiv.innerHTML = innerHTML;

    if (products.length < 1) {
        document.getElementById("lastVisitedsDiv").style.display = 'none';
    }
    else {
        if (products.length < 4) {
            const elements = document.getElementById("lastVisitedsDiv").getElementsByClassName("div-scroller");
            for (let i = 0; i < elements.length; i++) {
                elements[i].style.display = "None";
            }
        }
    }
}