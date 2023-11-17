function selectAddress(htmlObject) {
    var allObjects = document.querySelectorAll(".address");
    for (let i = 0; i < allObjects.length; i++) {
        if (allObjects[i] == htmlObject) {
            htmlObject.setAttribute("style", "border:1px solid #000");
            var input = htmlObject.querySelector("input");
            input.checked = true;
        }
        else {
            allObjects[i].setAttribute("style", "border:1px solid transparent");
        }
    }
}
document.querySelector("#cvccode").addEventListener('focusin', () => {
    document.querySelector('.card__front').setAttribute('style',`-webkit-transform: rotateY(180deg);-moz-transform: rotateY(180deg);`);
    document.querySelector('.card__back').setAttribute('style',`-webkit-transform: rotateY(0deg);-moz-transform: rotateY(0deg);`);

});
document.querySelector("#cvccode").addEventListener('focusout', () => {
    document.querySelector('.card__front').setAttribute('style',``);
    document.querySelector('.card__back').setAttribute('style',``);
});