function loadCharts() {
    var date = new Date();
    var month = date.getMonth();
    var year = date.getFullYear();
    var formData = new FormData();
    formData.append("month", month);
    formData.append("year", year);
    $.ajax({
        type: "POST",
        data: formData,
        url: "/admin/GetStatistics",
        contentType: false,
        processData: false,
        success: (response) => {
            drawBarChartByJson("Toplam Satış Tutarı", response.totalSalesPriceByDay,"salePrices");
            drawBarChartByJson("Toplam Satış Miktari", response.totalSalesCountByDay,"saleCounts");
            drawPieChartByJson("Toplam Satın Alım (Adet)",
                response.orderCountByCategoryName, "salesByCategoryCount");
            drawPieChartByJson("Toplam Satın Alım (Ciro)",
                response.orderPricesByCategoryName, "salesByCategoryPrice");
        }
    });
};

loadCharts();