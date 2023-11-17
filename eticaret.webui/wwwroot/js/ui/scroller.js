class Scroller {

    constructor(mainDiv) {
        this.mainDiv = mainDiv;
        this.prev = mainDiv.querySelector('.prev');
        this.next = mainDiv.querySelector('.next');
        this.productsDiv = mainDiv.querySelector('.similar-products_products');
        this.setEvents();
    }

    setEvents() {
        this.prev.addEventListener('click', () => {
            this.goPrev();
        });

        this.next.addEventListener('click', () => {
            this.goNext();
        });
    }

    goPrev() {
        let width = this.mainDiv.offsetWidth;
        this.productsDiv.scrollBy(-1 * width, 0);
    }

    goNext() {
        let width = this.mainDiv.offsetWidth;
        this.productsDiv.scrollBy(width, 0);
    }

}

window.addEventListener('load', () => {
    const scrollers = document.querySelectorAll('.similar-products');
    for (let i = 0; i < scrollers.length; i++) {
        new Scroller(scrollers[i]);
    };
})