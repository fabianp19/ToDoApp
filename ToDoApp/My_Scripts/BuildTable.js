$(document).ready(function () {

    $.ajax({
        url: '/ToDoItems/BuildToDoTable',
        success: function (result) {
            $('#tablediv').html(result);
        }
    });
});