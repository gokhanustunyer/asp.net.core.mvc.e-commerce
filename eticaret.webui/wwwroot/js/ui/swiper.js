class Swiper {

    constructor(mainDiv) {
        this.mainDiv = mainDiv;
        this.prev = mainDiv.querySelector('.prev');
        this.next = mainDiv.querySelector('.next');
        this.productsDiv = mainDiv.querySelector('.color-images');
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

const color_swiper = document.querySelector('.swiper');
var swiper = new Swiper(color_swiper);