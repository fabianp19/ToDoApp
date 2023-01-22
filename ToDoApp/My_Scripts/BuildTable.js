$(document).ready(function () {

    $.ajax({
        url: '/ToDoItemsController/BuildToDoTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });
});