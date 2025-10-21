$(document).ready(function () {
    // Inicializa o datepicker
    $('#birthDate').datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy'
    });

    const userRole = $('.card').data('role');
    console.log('Role do user', userRole);

    if (userRole === 'Admin') {
        $('#password').removeAttr('disabled');
        $('#confirmPassword').removeAttr('disabled');
    };

    if (userRole === 'Admin' || userRole === 'Manager') {
        $('#roleSelector').removeAttr('disabled');
    };
});




