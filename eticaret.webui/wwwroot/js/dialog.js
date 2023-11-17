function openDialog() {
    let element = document.querySelector(".dialog");
    if (!element.style.display || element.style.display == "none") {
        element.style.display = "flex";
    }
    else {
        element.style.display = "none";
    }

}

function openDialogById(id) {
    let element = document.getElementById(id);
    if (!element.style.display || element.style.display == "none") {
        element.style.display = "flex";
    }
    else {
        element.style.display = "none";
    }

}