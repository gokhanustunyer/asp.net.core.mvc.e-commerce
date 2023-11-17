class Cart
{
    constructor(codeInputId, useCodeButtonId, removeDiscountButtonId, priceLabelId, defaultPrice)
    {
        this.codeInput = document.getElementById(codeInputId);
        this.useCodeButton = document.getElementById(useCodeButtonId);
        this.priceLabel = document.getElementById(priceLabelId);
        this.removeDiscountButton = document.getElementById(removeDiscountButtonId);
        this.defaultPrice = Number(defaultPrice.replace(",", "."));
        this.setEvents();
    }

    setEvents()
    {
        if (this.useCodeButton != null)
        {
            this.useCodeButton.addEventListener("click", () => { this.useCode() });
        }
        if (this.removeDiscountButton != null)
        {
            this.removeDiscountButton.addEventListener("click", () => { this.removeCode() });
        }
    }

    useCode()
    {
        const formData = new FormData();
        formData.append("code", this.codeInput.value);
        $.ajax({
            method: "POST",
            url: "/user/CheckDiscountCode",
            data: formData,
            processData: false,
            contentType: false,
            success: () => { location.reload(); },
            error: () => { location.reload(); }    // TEKRAR BAKILACAK
        });
    }

    removeCode()
    {
        const formData = new FormData();
        this.sendPost("/user/removeDiscountCode", formData, () => {
            location.reload();
        });
    }

    sendPost(url, data, successCallbackFunc, errorCallbackFunc)
    {
        $.ajax({
            url: url,
            data: data,
            method: "POST",
            processData: false,
            contentType: false,
            success: successCallbackFunc,
            errorCallbackFunc: errorCallbackFunc
        });
    }
}

var removeFromBasket = (productId, sizeId, id) => {
    var formData = new FormData();
    formData.append("productId", productId);
    formData.append("sizeId", sizeId);
    $.ajax({
        type: "POST",
        url: "/User/Deletefrombasket",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            const quantity = document.querySelector(`#quantity${id}`);
            quantity.parentElement.remove();
            openDialog();
            location.reload();

        }
    })
}

var minusBasket = (productId, sizeId, id) => {
    var formData = new FormData();
    formData.append("productId", productId);
    formData.append("sizeId", sizeId);
    $.ajax({
        type: "POST",
        url: "/User/MinusFromBasket",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            const quantity = document.querySelector(`#quantity${id}`);
            let content = parseInt(quantity.innerHTML) - 1;
            if (content < 1) { quantity.parentElement.remove(); }
            else { quantity.innerHTML = content }
            location.reload();

        }
    })
}

var increaseBasket = (productId, sizeId, id) => {
    var formData = new FormData();
    formData.append("productId", productId);
    formData.append("sizeId", sizeId);
    $.ajax({
        type: "POST",
        url: "/User/IncreaseFromBasket",
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            const quantity = document.querySelector(`#quantity${id}`);
            let content = parseInt(quantity.innerHTML) + 1;
            quantity.innerHTML = content
            location.reload();
        }
    })
}

var ConfirmDialog = (title, func) => {
    openDialog();
    var dialog = document.querySelector(".dialog-window");
    dialog.setAttribute("style", "width:25%;height:180px;position:relative;");
    dialog.innerHTML = `<i onclick="openDialog()" style="padding: .5rem;cursor:pointer;position: absolute;right: 5px;/* width: 100%; */margin-top: -1rem;box-shadow: rgba(0, 0, 0, 0.02) 0px 1px 3px 0px, rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;border-radius: .15rem;background-color:#fff;text-align:right;top: 20px;"
        class="fa-solid fa-xmark block close-btn"></i>`;

    dialog.innerHTML += `
        <div class="mt-4">
            <hr>
            <p>${title}</p>
            <div style="display:block;text-align:right">
                <button class="btn btn-danger" onclick="${func}">Devam Et</button>
                <button class="btn btn-light text-dark" onclick="openDialog()">İptal</button>
            </div>
        </div>`
}


