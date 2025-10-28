$(document).ready(function () {
    applyPlugins();

    $('.select2').select2({
        allowClear: true,
        width: '150%'
    });
    $('#bDate').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true
    });
});


$(document).on('submit', '#filter-form', function (e) {
    e.preventDefault();

    const form = $(this);
    const url = form.attr('action') || window.location.pathname + '/Filter';

    const data = form.serialize();

    $.ajax({
        url: url,
        type: 'GET',
        data: data,
        success: function (partialView) {
            $('#table-container').html(partialView);
            applyPlugins();
        },
        error: function () {
            alert('Error at acess the users data!')
        }
    });

});


$(document).on('click', '#limparCampos', function () {
    LimparCampos();
});

function LimparCampos() {
    $('.select2').val(null).trigger('change');
    $('#userName').val('');
    $('#userEmail').val('');
}

function applyPlugins() {
    $('#usersTable').DataTable({
        dom: '<"d-flex justify-content-between mb-3"lfB>rtip', // layout customizado
        buttons: [],
        columnDefs: [
            { orderable: false, targets: 4 }
        ],
        pageLength: 5,
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
};