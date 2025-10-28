$(document).ready(function () {
    $('.select2').select2({
        allowClear: true,
        width: '150%'
    });
    $('#minDate, #maxDate').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true
    });
});

$(document).on('click', '#limparCampos', function () {
    LimparCampos();
});

function LimparCampos() {
    $('#departmentIds').val(null).trigger('change');
    $('#sellerIds').val(null).trigger('change');
    $('#minDate').val('');
    $('#maxDate').val('');
}
