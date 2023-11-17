function drawBarChartByJson(title, jsonData, canvasId) {
    const ctx = document.getElementById(canvasId);
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: Object.keys(jsonData),
            datasets: [{
                label: title,
                data: Object.values(jsonData),
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

function drawPieChartByJson(title, jsonData, canvasId) {
    const ctx = document.getElementById(canvasId);
    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: Object.keys(jsonData),
            datasets: [{
                label: title,
                data: Object.values(jsonData),
                borderWidth: 1,
            }],
            hoverOffset: 4,
        },
    });
}