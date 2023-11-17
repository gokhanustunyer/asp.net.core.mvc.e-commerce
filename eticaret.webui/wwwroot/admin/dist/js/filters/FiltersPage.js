class FiltersPage
{
    constructor(getButtonId, inputBoxId, filterSerachResultId)
    {
        this.getButton = document.getElementById(getButtonId);
        this.inputBox = document.getElementById(inputBoxId);
        this.filterSerachResult = document.getElementById(filterSerachResultId);
        this.selectedCategoryIds = [];
        this.setEvents();
    }

    setEvents()
    {
        this.getButton.addEventListener("click", () => { this.getFiltersByCategory() });
    }

    getFiltersByCategory()
    {
        this.selectedCategoryIds = [];
        var category_inputs = this.inputBox.querySelectorAll("input");
        category_inputs.forEach(input => {
            if (input.checked)
            {
                this.selectedCategoryIds.push(input.value);
            }
        });
        const formData = new FormData();
        formData.append("CategoryIds", this.selectedCategoryIds);
        this.sendPost("/admin/getFiltersByCategoryId", formData, (response) => { this.setCategoriesToDOM(response) });
    }

    setCategoriesToDOM(datas)
    {
        this.filterSerachResult.innerHTML = "";
        datas.forEach(data => {
            this.filterSerachResult.innerHTML += `
                <a href="/admin/editFilter/${data["id"]}" class="btn btn-sm btn-danger text-white">
                    <i class="mdi mdi-tooltip-edit"></i>
                    <span class="text-white">${data["filterBoxTitle"]}</span>
                </div>
`;
        })
    }

    sendPost(url, data, successCallbackFunc, errorCallbackFunc)
    {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            contentType: false,
            processData: false,
            success: successCallbackFunc,
            error: errorCallbackFunc
        });
    }
}

