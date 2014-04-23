$(function () {
    $("#submitButton").click(function () {
        $("#insertForm").validate({
            rules: {
                required: true
            },
            messages: {
                required: 'Please enter a value'
            }
        });
    });
});