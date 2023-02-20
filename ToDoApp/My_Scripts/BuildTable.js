$(document).ready(function () {

    $.ajax({
        url: '/ToDoItems/BuildToDoTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });
});