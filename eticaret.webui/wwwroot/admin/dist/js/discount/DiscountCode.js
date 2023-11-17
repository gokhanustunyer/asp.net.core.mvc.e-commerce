class DiscountCodeManager
{
    constructor(modalLinkId, outInputId, categoryInputClassName, resultDivId)
    {
        this.categoryInputs = document.getElementsByClassName(categoryInputClassName);
        this.outInput = document.getElementById(outInputId);
        this.modalLink = document.getElementById(modalLinkId);
        this.searchResultsDiv = document.getElementById(resultDivId);
        this.selectedCategories = [];
        this.selectedProductIds = [];
        this.setEvents();
    }

    setEvents()
    {
        this.modalLink.addEventListener("click", () => { this.setProducts() });
    }

    setSelectedCategories()
    {
        this.checkedCategories = [];
        for (let i = 0; i < this.categoryInputs.length; i++)
        {
            if (this.categoryInputs[i].checked)
            {
                this.selectedCategories.push(this.categoryInputs[i].value);
            }
        }
    }

    setProducts()
    {
        this.setSelectedCategories();
        const formData = new FormData();
        formData.append("CategoryIds", this.selectedCategories);
        console.log(this.selectedCategories);
        this.postItem("/admin/getProductsByCategory", formData, (response) => { this.setProductsToDOM(response) });
    }

    setProductsToDOM(response) {
        this.searchResultsDiv.innerHTML = "";
        if (response.length == 0) {
            this.searchResultsDiv.innerHTML += `
                <div style="width:100%;height:100%;display:flex;align-items:center;flex-direction:column;justify-content:center" class="mt-4">
                    <i class="mdi mdi-magnify" style="font-size:4rem;color:#9a9a9a;"></i>
                    <span style="color:#9a9a9a;max-width:300px;text-align:center;margin-top:1rem">
                        Maalesef aradığınız kelimeler ile eşleşen bir ürün bulamadık
                    </span>
                 </div>`;
        }
        for (let i = 0; i < response.length; i++) {
            const div_content = document.createElement("div");
            const span = document.createElement("span");
            const i_mark = document.createElement("i");
            const img = document.createElement("img");
            const a_tag = document.createElement("a");

            a_tag.setAttribute("title", response[i].name);
            a_tag.setAttribute("class", "search-title");
            a_tag.innerHTML = response[i].name;
            img.setAttribute("class", "search-img");
            img.setAttribute("src", response[i].mainImageUrl);
            i_mark.setAttribute("class", "mdi mdi-bookmark-check");
            span.setAttribute("class", "selectRelated");
            div_content.onclick = () => { this.changeProducts(response[i].id, div_content) };
            div_content.setAttribute("class", "search-result relateds-s-r");
            div_content.setAttribute("style", "cursor:pointer");

            if (this.selectedProductIds.includes(response[i].id)) {
                span.setAttribute("style", "display:block");
            }

            span.appendChild(i_mark);
            div_content.appendChild(span);
            div_content.appendChild(img);
            div_content.appendChild(a_tag);
            this.searchResultsDiv.appendChild(div_content);
        }
    }

    changeProducts(productId, div_content) {
        var index = this.selectedProductIds.indexOf(productId);
        var span = div_content.querySelector("span");
        if (index != -1) {
            this.selectedProductIds.splice(index, 1);
            span.setAttribute("style", "display:none");
        }
        else {
            this.selectedProductIds.push(productId);
            span.setAttribute("style", "display:block");
        }
        this.changeOutIntput();
    }

    changeOutIntput() {
        this.outInput.value = "";
        this.selectedProductIds.forEach(id => {
            this.outInput.value += `${id},`;
        });
    }

    postItem(url, data, successCallback, errorCallback) {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            contentType: false,
            processData: false,
            success: successCallback,
            eror: errorCallback
        });
    }
}