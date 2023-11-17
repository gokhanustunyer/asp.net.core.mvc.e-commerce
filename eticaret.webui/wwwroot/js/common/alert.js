class Alert {
    alerts = document.getElementById("alerts");

    Error(title, message) {
        alerts.innerHTML = `
        <div class="alert-box">
            <div class="alert-icon-box-error">
                <i class="fa-solid fa-circle-xmark"></i>
            </div>
            <div class="alert-box-message">
                <p class="alert-box-title">
                    ${title}
                </p>
                <p>
                    ${message}
                </p>
            </div>
        </div>`;
        alerts.classList.add('active');
        setTimeout(() => {
            alerts.classList.remove('active');
        }, 5000);
    }

    Success(title, message) {
        alerts.innerHTML = `
        <div class="alert-box">
            <div class="alert-icon-box-success">
                <i class="fa-solid fa-circle-check"></i>
            </div>
            <div class="alert-box-message">
                <p class="alert-box-title">
                    ${title}
                </p>
                <p>
                    ${message}
                </p>
            </div>
        </div>`;
        alerts.classList.add('active');
        setTimeout(() => {
            alerts.classList.remove('active');
        }, 5000);
    }

    Warning(title, message) {
        alerts.innerHTML = `
        <div class="alert-box">
            <div class="alert-icon-box-warning">
                <i class="fa-solid fa-triangle-exclamation"></i>
            </div>
            <div class="alert-box-message">
                <p class="alert-box-title">
                    ${title}
                </p>
                <p>
                    ${message}
                </p>
            </div>
        </div>`;
        alerts.classList.add('active');
        setTimeout(() => {
            alerts.classList.remove('active');
        }, 5000);
    }

    static CloseEvent() {
        alerts.classList.remove('active');
    }
}

const alertController = new Alert();
alerts.addEventListener('click', () => {
    Alert.CloseEvent();
})

function CreateAlert(jsonData) {
    if (jsonData == 'null' || jsonData == '' || jsonData == null) { return; }
    jsonData = JSON.parse(jsonData);
    title = jsonData["Title"];
    message = jsonData["Message"];
    type = jsonData["MessageType"];
    if (type == 0) {
        alertController.Error(title, message);
    }
    else if (type == 2) {
        alertController.Warning(title, message);
    }
    else if (type == 1) {
        alertController.Success(title, message);
    }
}