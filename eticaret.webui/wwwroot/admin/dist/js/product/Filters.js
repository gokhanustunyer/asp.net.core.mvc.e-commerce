class Filter {
    constructor(eventButtonId, formId) {
        this.button = document.getElementById(eventButtonId);
        this.filterList = [];
        this.originalSearchResult = "";
        this.setEvents();
        this.form = document.getElementById(formId);
        this.input = document.createElement("input");
        this.input.hidden = "hidden";
        this.input.name = "filterIds";
        this.form.appendChild(this.input);
    }

    setEvents() {
        this.button.addEventListener("click", this.getFiltersBySelectedCategories);
    }

    addToFilterList(filterId, filterName) {
        let index = this.getIndexById(filterId);
        if (index == -1) {
            this.filterList.push({
                "filterId": filterId,
                "filterName": filterName
            });
            this.updateFilterInput();
            this.setFiltersToDOM();
        }
    }

    getIndexById(filterId) {
        for (let i = 0; i < this.filterList.length; i++) {
            if (this.filterList[i]["filterId"] == filterId) {
                return i;
            }
        }
        return -1;
    }

    removeFromFilterList(filterId) {
        let index = this.getIndexById(filterId);
        if (index != -1) {
            this.filterList.splice(index, 1);
            this.updateFilterInput();
        }
        this.setFiltersToDOM();
    }

    updateFilterInput() {
        this.input.value = "";
        this.filterList.forEach(filter => {
            this.input.value += `${filter["filterId"]},`;
        });
    }

    getFiltersBySelectedCategories() {
        const checkedCategories = [];
        const allCategories = document.querySelectorAll(".cat");
        allCategories.forEach(category =>
        {
            if (category.checked)
            {
                checkedCategories.push(category.value);
            }
        });
        const formData = new FormData();
        formData.append("CategoryIds", checkedCategories);

        $.ajax({
            type: "POST",
            url: "/admin/getfilterbycategory",
            data: formData,
            contentType: false,
            processData: false,
            success: (response) => { AddProductFilter.setFilterSearchSuccessResponse(response); },
            error: () => { AddProductFilter.setFilterSearchErrorResponse(); }
        });
    }

    setFilterSearchSuccessResponse(response) {
        const parentModal = document.getElementById("filterModal");
        const searchResults = parentModal.querySelector(".search-results");
        searchResults.innerHTML = "";
        var innerHtmlContent = "";
        response.forEach(filterBox => {
            innerHtmlContent += `<div class="dropdown">
                          <button style="min-width:125px" class="btn btn-success text-white dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            ${filterBox["filterBoxTitle"]}
                          </button><div class="dropdown-menu" aria-labelledby="dropdownMenuButton">`;

            for (let i = 0; i < filterBox["filters"].length; i++) {
                innerHtmlContent += `<a class="dropdown-item" onclick="AddProductFilter.addToFilterList('${filterBox["filters"][i]["id"]}', '${filterBox["filters"][i]["filterTitle"]}')" style="cursor:pointer">
                            ${filterBox["filters"][i]["filterTitle"]}</a>`;
            }
            innerHtmlContent += `</div></div>`;
        })
        searchResults.innerHTML = innerHtmlContent;
        this.originalSearchResult = `<div class="input-group mb-3"> <input id="searchFilterInput" type="text" placeholder="Filtrelerde Ara" class="form-control"> <div class="input-group-append" style="height:40px"> <span class="input-group-text" id="searchfilterres"> <a onclick="adminFilterSearch()" style="padding:1rem;cursor:pointer"> <i class="mdi mdi-magnify"></i> </a> </span> </div> </div><h3 class="border-bottom" style="padding:.75rem 0">Seçilen Kategori Filtreleri</h3><div class="search-results mt-1">` + searchResults.innerHTML + "</div>";
        this.setFiltersToDOM();
    }

    setFilterSearchErrorResponse() {
        const parentModal = document.getElementById("filterModal");
        const searchResults = parentModal.querySelector(".search-results");
        searchResults.innerHTML = "";
        searchResults.innerHTML = `<div style="width:100%;height:100%;display:flex;align-items:center;flex-direction:column;justify-content:center" class="mt-4"><i class="fa-solid fa-magnifying-glass" style="font-size:4rem;color:#9a9a9a;"></i><span style="color:#9a9a9a;max-width:300px;text-align:center;margin-top:1rem">Maalesef aradığınız kelimeler ile eşleşen bir filtre bulamadık</span></div>`
        this.setFiltersToDOM();
    }

    setFiltersToDOM() {
        var innerHtml = "";
        const dialog = document.querySelector("#filterModal .modal-body");
        this.filterList.forEach(filter => {
            innerHtml += `<span class="btn btn-sm btn-danger text-white" style="margin: 0 0.25rem 0.25rem 0;">${filter["filterName"]} <i class="mdi mdi-close" style="margin-left:.5rem;cursor:pointer;" onclick="AddProductFilter.removeFromFilterList('${filter["filterId"]}')"></i></span>`;
        });
        dialog.innerHTML = innerHtml + this.originalSearchResult;
    }


}
const AddProductFilter = new Filter("filterEdit", "form");