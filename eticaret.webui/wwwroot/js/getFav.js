var loadFavs = () => {
    const prIds = JSON.parse(localStorage.getItem("arite_favorites"));
    const formData = new FormData();
    formData.append("prIds", prIds);
    $.ajax({
        url: "/home/getProductsById",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: (response) => {
            setLastVisitedProducts(response);
        }
    })
};
function setLastVisitedProducts(products) {
    const lastVisitedsDiv = document.getElementById("fav-products");
    var innerHTML = "";
    products.forEach(item => {
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
    });
    lastVisitedsDiv.innerHTML = innerHTML;
    const favsTitle = document.getElementById("favs-title");
    favsTitle.innerText = `Favorilerim (${products.length})`;
}
window.addEventListener('load', loadFavs);


var setFav = (obj) => {
    let remove_parent = obj.parentElement.parentElement;
    remove_parent.setAttribute("style", "transition:all .5s;opacity: 0");

    setTimeout(() => {
        let parent = "<div class='products_product'>" + remove_parent.innerHTML + "</div>";
        let favs = JSON.parse(localStorage.getItem("arite_favorites"));
        favs.splice(favs.indexOf(parent), 1);
        localStorage.setItem("arite_favorites", JSON.stringify(favs));
        loadFavs();
        console.log(parent);
        let notyf = new Notyf({
            duration: 3000,
            dismissible: true,
            position: { x: 'right', y: 'bottom' }
        });
        notyf.success(
            "Ürün Favorilerden Çıkartıldı"
        );

    }, 500);

};



