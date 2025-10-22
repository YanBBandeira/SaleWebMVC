document.addEventListener('DOMContentLoaded', function () {
    const dataElement = document.getElementById('seller-data');
    //#region Chart for Total Sales per Seller

    // Dados para o gráfico de vendas por vendedor
    const sellerLabels = JSON.parse(dataElement.dataset.labels);
    const sellerSales = JSON.parse(dataElement.dataset.sales);


    // Gera uma cor aleatória para cada vendedor
    const backgroundColors = sellerLabels.map(() => {
        const r = Math.floor(Math.random() * 255);
        const g = Math.floor(Math.random() * 255);
        const b = Math.floor(Math.random() * 255);
        return `rgba(${r}, ${g}, ${b}, 0.5)`;
    });

    const borderColors = backgroundColors.map(color => color.replace("0.5", "1"));

    const ctx2 = document.getElementById('sellerChart').getContext('2d');

    new Chart(ctx2, {
        type: 'bar',
        data: {
            labels: sellerLabels, // nomes dos vendedores const definido anteriormente
            datasets: [{
                label: 'Total Sales per Seller',
                data: sellerSales, // vendas totais por vendedor const definido anteriormente
                backgroundColor: backgroundColors,
                borderColor: borderColors,
                borderWidth: 1
            }]
        },
        options: {
            indexAxis: 'x', // horizontal bar chart
            responsive: true,
            plugins: {
                legend: { display: false },
                title: { display: true, text: 'Sales by Seller (Top Performers)' },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let value = context.parsed.y;
                            return 'R$ ' + value.toLocaleString();
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return 'R$ ' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });

    //#endregion Chart for Total Sales per Seller


    //#region Chart for Total Sales Over Time
    const labels = JSON.parse(dataElement.dataset.datelabels);
    const data = JSON.parse(dataElement.dataset.datesales);

    const ctx = document.getElementById('salesChart').getContext('2d');

    new Chart(ctx, {
        type: 'line', // ou 'bar', 'pie', etc.
        data: {
            labels: labels,
            datasets: [{
                label: 'Total Sales',
                data: data,
                borderColor: 'rgba(75, 192, 192, 1)', 
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                fill: true,
                tension: 0 // suaviza a linha
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { position: 'top' },
                title: { display: true, text: 'Monthly Sales Overview' }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return 'R$ ' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });

    //#endregion Chart for Total Sales Over Time


});