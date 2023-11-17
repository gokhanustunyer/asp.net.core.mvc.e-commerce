const DEFAULT_FAVS = JSON.parse(localStorage.getItem("arite_favorites"));


var setFav = (obj, id) => {
    if (obj.classList.contains("fa-solid")) {
        let remove_parent = obj.parentElement.parentElement.parentElement;
        let favs = JSON.parse(localStorage.getItem("arite_favorites"));
        favs.splice(id, 1);
        localStorage.setItem("arite_favorites", JSON.stringify(favs));

        obj.classList.remove("fa-solid");
        obj.classList.add("fa-regular");
        obj.style.color = "black";
        let notyf = new Notyf({
            duration: 3000,
            dismissible: true,
            position: { x: 'right', y: 'bottom' }
        });
        notyf.success(
            "Ürün Favorilerden Çıkartıldı"
        );
    }

    else {
        obj.style.color = "#444453";
        obj.classList.remove("fa-regular");
        obj.classList.add("fa-solid");
        let favs = JSON.parse(localStorage.getItem("arite_favorites"));
        if (favs == "null" || favs == null) {
            favs = [];
        }
        if (!favs.includes(id)) {
            favs.push(id);
            localStorage.setItem("arite_favorites", JSON.stringify(favs));
        }
        let notyf = new Notyf({
            duration: 3000,
            dismissible: true,
            position: { x: 'right', y: 'bottom' }
        });
        notyf.success(
            "Ürün Favorilere Eklendi"
        );
    }
}

window.addEventListener('load', () => {
    var prods = document.querySelectorAll(".set-fav");
    for (let i = 0; i < prods.length; i++) {
        prods[i].setAttribute("style", "color: red;");
        let parent = "<div class='products_product'>" + prods[i].parentElement.parentElement.innerHTML + "</div>";
        if (DEFAULT_FAVS.includes(parent)) {
            prods[i].style.color = "red";
        }
        else {
            prods[i].style.color = null;
        };
    }
})


