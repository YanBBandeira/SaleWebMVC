$(document).ready(function () {
    $('#sellersTable').DataTable({
        dom: '<"d-flex justify-content-between mb-3"lfB>rtip', // layout customizado
        buttons: [
            {
                extend: 'excel',
                text: 'Export to Excel',
                className: 'btn btn-info btn-sm',
                exportOptions: {
                    columns: [0, 1, 2, 3]  // índices das colunas a exportar
                }
            },
            {
                extend: 'pdfHtml5',
                text: 'Export to PDF',
                className: 'btn btn-warning btn-sm',
                title: 'Sellers',
                exportOptions: {
                    columns: [0, 1, 2, 3]  // índices das colunas a exportar
                },
                customize: function (doc) {
                    doc.styles.title = {
                        fontSize: 20,
                        bold: true,
                        alignment: 'center',
                        color: '#4CAF50' // Verde, por exemplo
                    };
                    doc.content[doc.content.length - 1].layout = {
                        hLineWidth: function () { return 0.5; },
                        vLineWidth: function () { return 0.5; },
                        hLineColor: function () { return '#ccc'; },
                        vLineColor: function () { return '#ccc'; },
                        paddingLeft: function () { return 6; },
                        paddingRight: function () { return 6; },
                        paddingTop: function () { return 4; },
                        paddingBottom: function () { return 4; },
                    };
                    const header = doc.content[doc.content.length - 1].table.body[0];
                    header.forEach(cell => {
                        cell.fillColor = '#0d6efd'; // cor do header
                        cell.color = '#fff';
                        cell.bold = true;
                        cell.alignment = 'center';
                    });
                },
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-secondary btn-sm',
                exportOptions: {
                    columns: [0, 1, 2, 3]  // índices das colunas a exportar
                }
            }
        ],
        columnDefs: [
            { orderable: false, targets: 4 }
        ],
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        lengthChange: true,
        info: true,
        rowReorder: true,
        initComplete: function () {
            $('.dataTables_filter input')
                .addClass('form-control form-control-sm d-inline-block ms-2')
                .attr('placeholder', '🔍 Search...');

            $('.dataTables_length select')
                .addClass('form-select form-select-sm d-inline-block ms-2');
        },
        drawCallback: function () {
            $('.dataTables_paginate .paginate_button')
                .addClass('btn btn-sm btn-outline-primary mx-1');

            $('.dataTables_paginate .paginate_button.current')
                .removeClass('btn-outline-primary')
                .addClass('btn-primary text-white');
        }
    });
});