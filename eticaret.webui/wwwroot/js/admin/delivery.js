function saveDelivery(orderId, counter) {
    const isConfirmed = document.querySelector(`#isConfirmed${counter}`).checked;
    const deliveryStatus = document.querySelector(`#deliveryStatus${counter}`).checked;
    let notyf = new Notyf({
        duration: 3000,
        dismissible: true,
        position: { x: 'right', y: 'bottom' }
    });
    $.ajax({
        type: "POST",
        url: "/Admin/ChangeOrderStatus",
        dataType: "json",
        data: { "orderId": orderId, "isConfirmed": isConfirmed, "deliveryStatus": deliveryStatus },
        success: () => { notyf.success("Sipariş Durumu Başarıyla Güncellendi"); },
        error: () => { notyf.error("Beklenmedik Bir Sorun Oluştu"); }
    });
}