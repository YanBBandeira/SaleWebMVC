$(document).ready(function () {
    $('#usersTable').DataTable({
        dom: '<"d-flex justify-content-between mb-3"lfB>rtip', // layout customizado
        columnDefs: [
            { orderable: false, targets: 3 }
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


