class DiscountCodeDOMManager
{
    constructor(numberOrDateId, numberOrDateDiv, rateOrPriceId, rateOrPriceDiv)
    {
        this.rateOrPrice = document.getElementById(rateOrPriceId);
        this.numberOrDate = document.getElementById(numberOrDateId);
        this.numberOrDateDiv = document.getElementById(numberOrDateDiv);
        this.rateOrPriceDiv = document.getElementById(rateOrPriceDiv);
        this.setEvents();
    }


    setEvents()
    {
        this.rateOrPrice.addEventListener("change", () => { this.changeRateOrPrice() });
        this.numberOrDate.addEventListener("change", () => { this.changeNumberOrDate() });
    }

    changeRateOrPrice()
    {
        var selectedValue = this.rateOrPrice.options[this.rateOrPrice.selectedIndex].value;
        if (selectedValue == "price")
        {
            this.rateOrPriceDiv.innerHTML = `
                <div class="form-group mt-3">
                    <label for="discountNumber" class="col-md-12">İndirim Miktarı (Fiyat)</label>
                    <div class="col-md-12">
                        <input type="number" name="DiscountNumber" placeholder="50" step="0.25" id="discountNumber" class="form-control form-control-line">
                    </div>
                </div>`;
        }
        else if (selectedValue == "rate") 
        {
            this.rateOrPriceDiv.innerHTML = `
                <div class="form-group mt-3">
                    <label for="discountRate" class="col-md-12">İndirim Oranı (Yüzde)</label>
                    <div class="col-md-12">
                        <input type="number" name="DiscountRate" placeholder="10" step="1" id="discountRate" class="form-control form-control-line">
                    </div>
                </div>`;
        }
    }

    changeNumberOrDate()
    {
        var selectedValue = this.numberOrDate.options[this.numberOrDate.selectedIndex].value;
        if (selectedValue == "number")
        {
            this.numberOrDateDiv.innerHTML = `
                <div class="form-group mt-3">
                    <label for="codeLimitNumber" class="col-md-12">Kupon Kullanım Limiti (Adet)</label>
                    <div class="col-md-12">
                        <input type="number" name="CodeLimitNumber" placeholder="50" step="1" id="codeLimitNumber" class="form-control form-control-line">
                    </div>
                </div>`;
        }
        else if (selectedValue == "date")
        {
            this.numberOrDateDiv.innerHTML = `
                <div class="row mt-3">
                    <div class="col form-group mx-2">
                        <label class="col-md-12">Başlangıç Tarihi</label>
                        <div class="col-md-12">
                            <input name="CodeStartDate" type="datetime-local" id="code" class="form-control form-control-line">
                        </div>
                    </div>
                    <div class="col form-group mx-2">
                        <label class="col-md-12">Bitiş Tarihi</label>
                        <div class="col-md-12">
                            <input name="CodeEndDate" type="datetime-local" id="code" class="form-control form-control-line">
                        </div>
                    </div>
                </div>`;
        }
    }
}