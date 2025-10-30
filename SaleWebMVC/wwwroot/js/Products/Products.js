$(document).ready(function () {
    applyPlugins();
 
})

function applyPlugins() {
    $('#productsTable').DataTable({
        dom: '<"d-flex justify-content-between mb-6"lfB>rtip',
        buttons: [
            {
                extend: 'excel',
                text: 'Export to Excel',
                className: 'btn btn-success btn-sm'
            },
            {
                extend: 'pdf',
                text: 'Export to PDF',
                className: 'btn btn-danger btn-sm'
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-secondary btn-sm'
            }
        ],
        columnDefs: [
            { orderable: false, targets: 5 }
        ],
        language: {
            search: "",
            searchPlaceholder: "🔍 Search...",
            lengthMenu: "Show _MENU_ records",
            paginate: {
                previous: "← Prev",
                next: "Next →"
            },
            info: "Showing _START_ to _END_ of _TOTAL_ records"
        },
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        lengthChange: true,
        searching: true,
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
                .addClass('btn btn-sm btn-outline-info mx-1');

            $('.dataTables_paginate .paginate_button.current')
                .removeClass('btn-outline-info')
                .addClass('btn-primary text-white');
        }
    });
};