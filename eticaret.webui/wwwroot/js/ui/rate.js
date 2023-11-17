const rates_div = document.querySelector('.rates');
const rates = rates_div.querySelectorAll('.rate');

var clickRate = (i) => {
    for (let j = 0; j < i + 1; j++) {
        rates[j].classList = "fa-solid fa-star";
    }
    if (i < 4) {
        for (let k = i + 1; k < 5; k++) {
            rates[k].classList = "fa-regular fa-star";
        }
    }
    leaveRate = (i) => { };
    enterRate = (i) => { };
    document.querySelector('#product-rate').setAttribute("value", i + 1);
}

for (let i = 0; i < rates.length; i++) {
    rates[i].addEventListener('click', () => { clickRate(i) })
}